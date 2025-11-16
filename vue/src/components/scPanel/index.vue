<template>
    <div ref="scPanel" class="sc-panel">
        <div class="sc-panel_base panel-default">
            <div class="sc-panel_header">
                <div class="title">
                    {{ title }}
                </div>
                <div>
                    <slot name="header"></slot>
                    <el-button type="default" circle @click="showFullPage()" v-show="!fullScreen && !fullPage"
                        title="全窗口显示">
                        <sc-icon name="sc-fullscreen" />
                    </el-button>
                    <el-button type="default" circle @click="exitFullPage()" v-show="!fullScreen && fullPage"
                        title="退出全窗口">
                        <sc-icon name="sc-fullscreen-exit" />
                    </el-button>
                    <el-button type="default" circle @click="showFullScreen()" v-show="!fullScreen" title="全屏幕显示">
                        <sc-icon name="sc-fullpage" />
                    </el-button>
                    <el-button type="default" circle @click="exitFullScreen()" v-show="fullScreen" title="退出全屏幕">
                        <sc-icon name="sc-fullpage-exit" />
                    </el-button>
                    <el-button type="default" circle @click="showDrawer()" title="帮助">
                        <sc-icon name="sc-info" />
                    </el-button>
                </div>
            </div>
            <div class="sc-panel_body">
                <slot></slot>
            </div>
            <div class="sc-panel_footer">
                <slot name="footer"></slot>
            </div>
        </div>
    </div>
    <el-drawer v-model="drawer" title="提示" direction="btt">
        <div class="panel-drawer">
            <span>{{ description }}</span>
        </div>
    </el-drawer>
</template>
<script>
export default {
    name: "ScPanel",
    props: {
        title: { type: String, default: '卡片标题' },
        description: { type: String, default: '这是工具说明！' },
    },
    data() {
        return {
            fullPage: false,
            fullScreen: false,
            drawer: false,
        }
    },
    methods: {
        showDrawer() {
            this.drawer = true;
        },
        showFullPage() {
            this.$TOOL.showFullPage(this.$refs.scPanel);
            this.fullPage = true;
        },
        exitFullPage() {
            this.$TOOL.exitFullPage(this.$refs.scPanel);
            this.fullPage = false;
        },
        showFullScreen() {
            this.$TOOL.screen(this.$refs.scPanel);
            this.fullScreen = true;
        },
        exitFullScreen() {
            this.$TOOL.screen(this.$refs.scPanel);
            this.fullScreen = false;
        }
    }
}
</script>

<style scoped>
.sc-panel {
    height: 100%;
    padding: 15px;
    background-color: white;
}

.sc-panel_base {
    height: 100%;
    margin: 0px;
    background-color: #fff;
    border: 1px solid transparent;
    border-radius: 4px;
    -webkit-box-shadow: 0 1px 1px rgba(0, 0, 0, .05);
    box-shadow: 0 1px 1px rgba(0, 0, 0, .05);
    display: flex;
    flex-direction: column;
    justify-content: space-between;
}

.sc-panel_header {
    margin: 0px;
    padding: 10px 15px;
    border: 0px;
    border-bottom: 1px solid transparent;
    border-radius: 0px;
    border-top-left-radius: 3px;
    border-top-right-radius: 3px;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
}

.sc-panel_body {
    flex-basis: 100%;
    margin: 15px;
}

.sc-panel_footer {
    /* display: flex;
    flex-direction: row;
    justify-content: space-between; */
}

.title {
    margin: 3px 0px;
    padding: 0px;
    display: inline-block;
    font-size: 18px;
}

.sc-panel_base .sc-body {
    padding: 15px;
}

.panel-default {
    border-color: #ddd;
}

.panel-default>.sc-panel_header {
    color: #333;
    background-color: #f5f5f5;
    border-color: #ddd;
}

.panel-drawer {
    overflow: scroll-y;
    min-height: 60px;
    margin: 0px 10px;
}
</style>