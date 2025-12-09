<template>
    <div id="waterfall" ref="waterfall" @scroll="handleScroll">
        <div class="waterfall-item default-card-animation" v-for="(item, index) in objData" :key="getKey(item, index)"
            ref="scWaterfallItem" :style="{
                'top': item.top + 'px',
                'left': item.left + 'px',
                'width': colWidth + 'px',
                'padding': imgGap + 'px'
            }">
            <slot :item="item" :index="index">
                <img :data-src="item.src" :alt="item.names" :style="{ 'height': item._dim + 'px', }">
                <div>
                    <label>这是说明文字</label>
                </div>
            </slot>
        </div>
    </div>
</template>
<script>
export default {
    props: {
        apiObj: { type: Object, default: () => { } },
        list: { type: Array, default: () => [] },
        rowKey: { type: String, default: 'id', },//属性Key
        imgSrc: { type: String, default: 'src', },//图片属性
        columnWidth: { type: Number, default: 300 },//列宽度
        columnCount: { type: Number, default: 0 },//列数量
        columnGap: { type: Number, default: 10 },//列间隙
        direction: { type: Number, default: 1 }// 展示方向
    },
    data() {
        return {
            apiData: [],// 返回数据
            objData: [],
            beginIndex: 0,
            loadedCount: 0,
            colGap: 20,// 列间隙
            colCount: 0,// 列数量
            colDim: [],// 列高度
            colWidth: 300,
            imgGap: 0,//图像间隙
            imgWidth: 0,
            reachBottomDistance: 20, // 滚动触底距离，触发加载新图片
            hasMore: true,// 是否还有更多数据
            isLoading: false,
            isFirst: true,// 是否首次加载
            observer: null,// 监控对象
            element: null,// 监控对象
            viewWidth: 0,
            viewHeight: 0,
        }
    },
    mounted() {
        this.layout();
        this.load_img();
        this.listen();
    },
    beforeDestroyed() {
        // 取消监听元素
        if (this.observer) {
            this.observer.unobserve(this.element)
        }
    },
    methods: {
        // 区域调整监听
        listen() {
            var resizeTimer = null;
            this.observer = new ResizeObserver(() => {
                if (resizeTimer) {
                    clearTimeout(resizeTimer);
                }
                resizeTimer = setTimeout(this.resize, 200);
            });
            this.element = document.querySelector('#waterfall');
            // 监听元素
            this.observer.observe(this.element);
        },
        // 页面布局
        layout() {
            // 初始化列宽度
            var waterfall = this.$refs.waterfall;
            if (!waterfall) {
                return;
            }

            this.viewWidth = waterfall.clientWidth;
            this.viewHeight = waterfall.clientHeight;
            var domSize = this.direction == 2 ? this.viewHeight : this.viewWidth;

            this.colGap = this.columnGap;
            if (!this.columnWidth && this.columnCount) {
                this.colCount = this.columnCount;
                var gap = this.colGap * (this.colCount - 1);
                this.colWidth = (domSize - gap) / this.colCount;
            } else if (this.columnWidth && !this.columnCount) {
                this.colWidth = this.columnWidth;
                this.colCount = Math.floor(domSize / (this.colWidth + this.colGap))
            }

            if (this.colWidth < 200) {
                this.colWidth = 200;
                this.colCount = Math.floor(domSize / (this.colWidth + this.colGap))
            }

            this.imgWidth = this.colWidth - this.colGap * 2;

            this.beginIndex = 0;

            // 初始化列高度
            for (var i = 0; i < this.colCount; i += 1) {
                this.colDim[i] = 0;
            }
        },
        resize() {
            if (this.isFirst) {
                this.isFirst = false;
                return;
            }

            this.layout();
            this.$nextTick(() => {
                // 对每个元素进行排列
                this.waterfall();
            });
        },
        async load_img() {
            if (this.isLoading) {
                return;
            }
            if (!this.hasMore) {
                return;
            }

            this.isLoading = true;
            this.hasMore = false;

            if (this.list && this.list.length > 0) {
                this.apiData = this.apiData.concat(this.list);
            }

            // 测试数据
            var testData = this.testData();
            this.apiData = this.apiData.concat(testData);
            this.hasMore = true;

            if (this.apiObj) {
                var res = await this.apiObj.get();
                if (res && res.code == 200) {
                    this.apiData = res.data;
                    this.hasMore = false;
                }
            }

            this.preLoad();
            this.isLoading = false;
        },
        testData() {
            var imgArr = [{ 'src': 'https://vitejs.cn/vite3-cn/logo-with-shadow.png', 'width': 595, 'height': 808 },
            { 'src': 'https://fscdn.zto.com/fs8/M02/94/F3/wKhBD19OTuuAedCpAAIKGsFFPqc560.png', 'width': 595, 'height': 808 },
            { 'src': 'https://vitejs.cn/vite3-cn/logo-with-shadow.png', 'width': 188, 'height': 121 },
            { 'src': 'https://vitejs.cn/vite3-cn/logo-with-shadow.png', 'width': 1317, 'height': 845 },
            { 'src': 'https://fscdn.zto.com/fs8/M02/94/F3/wKhBD19OTuuAedCpAAIKGsFFPqc560.png', 'width': 1349, 'height': 799 }];

            var list = [];
            for (let i = 0; i < 40; i++) {
                var idx = Math.round(Math.random() * (imgArr.length - 1));
                var img = imgArr[idx];
                //var tmp = this.imgWidth * img.height / img.width;
                img.height = (Math.random() * img.height) + 100;
                list.push(img);
            }

            return list;
        },
        // 预加载 设置并保存图片宽高
        preLoad() {
            // forEach 无法通过 item 直接修改数组元素，需用数组下标修改
            this.apiData.forEach((item, index) => {
                if (!item) {
                    return;
                }

                if (index < this.loadedCount) {
                    return;
                }

                // 指定大小
                if (item.height && item.width) {
                    this.calSize(this.apiData[index], item.width, item.height, this.imgWidth);
                    return;
                }

                // 无图则把高度设置为0
                var src = this.getSrc(item);
                if (!src) {
                    this.calSize(this.apiData[index], 0, 0, 0);
                    return;
                }

                let img = new Image();
                img.src = src;
                img.onload = img.onerror = (e) => {
                    var width = 0;
                    var height = 0;
                    if (e.type === "error") {
                        this.apiData[index]._error = true;
                    }
                    else if (e.type === 'load') {
                        width = img.width;
                        height = img.height;
                    }

                    this.calSize(this.apiData[index], width, height, this.imgWidth);
                }
            })
        },
        // 计算图片大小
        calSize(item, width, height, def) {
            var tmp = def;
            if (width && height) {
                tmp = Math.round(this.imgWidth * height / width);
            }
            item._dim = tmp;
            ++this.loadedCount;
            // 当前图片都与处理完，则加载图片
            if (this.apiData.length === this.loadedCount) {
                this.preloaded();
            }
        },
        preloaded() {
            // 开始渲染
            this.objData = this.objData.concat(this.apiData);
            this.$nextTick(() => {
                // 对每个元素进行排列
                this.waterfall();
            });
        },
        // waterfall，等到整个视图都渲染完毕再执行
        waterfall() {
            // 选择所有图片
            this.divItems = this.$refs["scWaterfallItem"];
            // 若没图片，则返回
            if (!this.divItems) {
                return;
            }

            let top, left, height;
            for (let i = this.beginIndex; i < this.divItems.length; ++i) {
                var divItem = this.divItems[i];
                if (!divItem) {
                    return;
                }

                // 当前图片的高度
                height = divItem.offsetHeight + this.colGap;

                // 找到当前最低的高度和其索引
                let minIdx = this.getColumn();
                let minHeight = this.colDim[minIdx];
                // 当前图片的 top，即当前图片应所在的高度
                top = minHeight;
                // 当前图片的 left，即当前图片应该排到目前高度最低那一列下面
                left = minIdx * (this.colWidth + this.colGap);
                // 更新第 minIdx 列的高度
                this.colDim[minIdx] += height;

                // 设置 waterfall-item 位置
                divItem.style.top = top + "px";
                divItem.style.left = left + "px";
                // 当前图片在窗口内，则加载，这是用于后面的图片懒加载。viewHeight 为窗口高度
                if (top < this.viewHeight) {
                    this.showContent(divItem, this.objData[i]);
                }
            }
            // 排列完之后，之后新增图片从这个索引开始预加载图片和排列，之前排列的图片无需在处理
            this.beginIndex = this.divItems.length;
        },
        getKey(item, index) {
            return item[this.rowKey] || index;
        },
        getSrc(item) {
            return item[this.imgSrc] || item.src;
        },
        getColumn() {
            var min = this.colDim[0];
            var idx = 0;
            for (var i = 1; i < this.colDim.length; i += 1) {
                if (this.colDim[i] < min) {
                    idx = i;
                    min = this.colDim[i];
                }
            }
            return idx;
        },
        reset() {
            this.apiData.length = 0;
            this.objData.length = 0;
            this.loadedCount = 0;
            this.beginIndex = 0;
        },
        // 滚动触底事件
        handleScroll() {
            let minHeight = Math.min.apply(null, this.colDim);
            // 滚动条滚动的高度
            let scrollTop = this.$refs.waterfall.scrollTop
            // 到达最底层的高度最低的一列，则触发 handleReachBottom 方法
            if (scrollTop + this.viewHeight > minHeight - this.reachBottomDistance) {
                this.load_img();
            }

            // 图片懒加载
            this.divItems.forEach((imgBoxEl) => {
                // 当前图片所处的高度
                let top = imgBoxEl.style.top;
                top = Number.parseFloat(top.slice(0, top.length - 2));
                // 图片已到达可视范围，则加载
                if (scrollTop + this.viewHeight > top) {
                    this.showContent(imgBoxEl, null);
                }
            })
        },
        // 明细展示
        showContent(item) {
            let img = item.children[0];
            if (img) {
                if (img.loaded) {
                    return;
                }

                img.src = img.getAttribute("data-src");
                img.style.opacity = 1;
                img.style.transform = "scale(1)";
                img.loaded = true;
            }
        },
    }
}
</script>

<style scoped lang="scss">
#waterfall {
    width: 100%;
    height: 100%;
    position: relative;
    overflow: scroll;

    @keyframes show-card {
        0% {
            transform: scale(0.5);
        }

        100% {
            transform: scale(1);
        }
    }

    .waterfall-item {
        position: absolute;
        border-radius: 10px;
        padding: 5px;
        padding-left: 0;
        border: 1px solid #eaeaea;
        background-color: #ffffff;

        img {
            width: 100%;
            border-radius: 10px;
            opacity: 0;
            transform: scale(0.5);
            transition: all 0.6s;
            transition-delay: 0.1s;
        }
    }

    .default-card-animation {
        animation: show-card 0.4s;
        transition: left 0.6s top 0.6s;
        transition-delay: 0.1s;
    }
}
</style>