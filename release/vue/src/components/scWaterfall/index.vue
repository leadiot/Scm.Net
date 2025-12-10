<template>
    <div id="scWaterfall" ref="scWaterfall" class="sc-waterfall" @scroll="handleScroll">
        <div class="sc-waterfall-head">
            <slot name="head"></slot>
        </div>
        <div class="sc-waterfall-body" :class="this.align" :style="{ width: bodyWidth, height: maxHeight + colGap + 'px' }">
            <div class="sc-waterfall-item default-card-animation" v-for="(item, index) in objData"
                :key="getKey(item, index)" ref="scWaterfallItem" :style="{
                    top: item.top + 'px',
                    left: item.left + 'px',
                    width: colWidth + 'px',
                    padding: imgGap + 'px',
                }">
                <slot name="item" :item="item" :index="index">
                    <img :data-src="item.src" :alt="item.names" />
                    <div>
                        <label>这是说明文字</label>
                    </div>
                </slot>
            </div>
        </div>
        <div class="sc-waterfall-foot" v-if="!hasMore">
            <slot name="foot">
                <el-button @click="reload" v-if="hasMore && !autoLoad">加载更多</el-button>
                <div v-if="!hasMore">没有更多数据了！</div>
            </slot>
        </div>
    </div>
</template>
<script>
export default {
    emits: ['preload', 'showContent'],
    props: {
        apiObj: { type: Object, default: () => { } },
        data: { type: Array, default: () => [] },
        rowKey: { type: String, default: "id" }, //属性Key
        columnWidth: { type: Number, default: 0 }, //列宽度
        columnCount: { type: Number, default: 2 }, //列数量
        columnGap: { type: Number, default: 15 }, //列间隙
        direction: { type: Number, default: 1 }, // 展示方向
        align: { type: String, default: "center" }, // 对齐方式
        autoLoad: { type: Boolean, default: true }, //是否自动加载
    },
    watch: {
        //监听从props里拿到值了
        data() {
            this.apiData = this.data;
            this.refresh();
        },
        apiObj() {
            this.refresh();
        },
    },
    data() {
        return {
            apiData: [], // 返回数据
            objData: [],
            beginIndex: 0,
            loadedCount: 0,
            colGap: 20, // 列间隙
            colCount: 0, // 列数量
            colDim: [], // 列高度
            colWidth: 300,
            imgGap: 0, //图像间隙
            imgWidth: 0,
            bodyWidth: "100%",
            reachBottomDistance: 20, // 滚动触底距离，触发加载新图片
            hasMore: true, // 是否还有更多数据
            isLoading: false,
            isFirst: true, // 是否首次加载
            observer: null, // 监控对象
            element: null, // 监控对象
            viewWidth: 0,
            viewHeight: 0,
            leftPosition: 0,
            maxHeight: 0,
        };
    },
    mounted() {
        // this.layout();
        // this.reload();
        this.listen();
    },
    beforeDestroyed() {
        // 取消监听元素
        if (this.observer) {
            this.observer.unobserve(this.element);
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
            this.element = document.querySelector("#scWaterfall");
            // 监听元素
            this.observer.observe(this.element);
        },
        refresh() {
            this.layout();
            this.reload();
        },
        // 页面布局
        layout() {
            // 初始化列宽度
            var waterfall = this.$refs.scWaterfall;
            if (!waterfall) {
                return;
            }

            this.viewWidth = waterfall.clientWidth;
            this.viewHeight = waterfall.clientHeight;
            var domSize =
                this.direction == 2 ? this.viewHeight : this.viewWidth;

            this.colGap = this.columnGap;
            if (!this.columnWidth && this.columnCount) {
                this.colCount = this.columnCount;
                var gap = this.colGap * (this.colCount - 1);
                this.colWidth = (domSize - gap) / this.colCount;
            } else if (this.columnWidth && !this.columnCount) {
                this.colWidth = this.columnWidth;
                this.colCount = Math.floor(
                    domSize / (this.colWidth + this.colGap)
                );
            }

            if (this.colWidth < 200) {
                this.colWidth = 200;
                this.colCount = Math.floor(
                    domSize / (this.colWidth + this.colGap)
                );
            }

            this.imgWidth = this.colWidth - this.colGap * 2;
            this.bodyWidth =
                (this.colWidth + this.colGap) * this.colCount -
                this.colGap +
                "px";

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
        async reload() {
            if (this.isLoading) {
                return;
            }
            if (!this.hasMore) {
                return;
            }

            this.isLoading = true;
            this.hasMore = false;

            // if (this.data && this.data.length > 0) {
            //     this.apiData = this.apiData.concat(this.data);
            // }

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

                this.$emit("preload", item);
                this.calSize();
            });
        },
        // 计算图片大小
        calSize() {
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
                this.setColumn(minIdx, height);
                //this.colDim[minIdx] += height;

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
        setColumn(idx, height) {
            var tmp = this.colDim[idx] + height;
            this.colDim[idx] = tmp;
            if (tmp > this.maxHeight) {
                this.maxHeight = tmp;
            }
        },
        getColumn() {
            var min = this.colDim[0];
            var idx = 0;
            for (var i = 1; i < this.colDim.length; i += 1) {
                var dim = this.colDim[i];
                if (dim < min) {
                    min = dim;
                    idx = i;
                }
                // if (dim > this.maxHeight) {
                // 	this.maxHeight = dim;
                // }
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
            let scrollTop = this.$refs.scWaterfall.scrollTop;
            // 到达最底层的高度最低的一列，则触发 handleReachBottom 方法
            if (scrollTop + this.viewHeight > minHeight - this.reachBottomDistance) {
                if (this.autoLoad) {
                    this.reload();
                }
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
            });
        },
        showContent(div, dat) {
            this.$emit("showContent", div, dat);
        }
    },
};
</script>

<style scoped lang="scss">
.sc-waterfall {
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

    .sc-waterfall-body {
        position: relative;
        top: 0px;
    }

    .left {
        left: 0px;
    }

    .center {
        margin: 0 auto;
    }

    .right {
        right: 0px;
    }

    .sc-waterfall-item {
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

    .sc-waterfall-foot {
        background-color: #eaeaea;
        border: 1px solid #eaeaea;
        text-align: center;
        padding: 10px;
    }

    .default-card-animation {
        animation: show-card 0.4s;
        transition: left 0.6s top 0.6s;
        transition-delay: 0.1s;
    }
}
</style>
