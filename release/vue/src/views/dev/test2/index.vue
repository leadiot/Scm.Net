<template>
    <el-container>
        <el-aside>
            <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
                <el-tab-pane label="相机列表" name="first">
                    <el-tree style="max-width: 600px" :data="cameraList" @node-click="handleNodeClick" />
                </el-tab-pane>
                <el-tab-pane label="我的收藏" name="second">
                    <sc-list></sc-list>
                </el-tab-pane>
            </el-tabs>
        </el-aside>
        <el-main class="nopadding">
            <el-container>
                <el-header>
                    <el-button-group>
                        <el-button type="primary" @click="splitVideo(1, 1)">
                            <sc-icon name="sc-skip-left-line" />
                        </el-button>
                        <el-button type="primary" @click="splitVideo(2, 2)"></el-button>
                        <el-button type="primary" @click="splitVideo(3, 3)"></el-button>
                        <el-button type="primary" @click="splitVideo(3, 4)"></el-button>
                        <el-button type="primary" @click="splitVideo(4, 4)">
                            <sc-icon name="sc-skip-right-line" />
                        </el-button>
                    </el-button-group>
                </el-header>
                <el-main>
                    <el-row v-for="(item, rowIndex) in rows" :key="rowIndex">
                        <el-col v-for='(item, colIndex) in cols' :key="'dv_' + rowIndex + '_' + colIndex" :span="item"
                            gutter="10" justify="center" @click="setCurrent(rowIndex, colIndex)">
                            <el-card class="ddd"
                                :style="(rowIndex == currentIndex[0] && colIndex == currentIndex[1]) ? 'background-color: #5ea6f1' : ''">
                                <template #header>
                                    <div>
                                        监控名称
                                    </div>
                                </template>
                                <div class="vedio-cover">
                                    <img :src="cover" style="width: 32px; height: 32px;" />
                                </div>
                                <template #footer>
                                    <div>
                                        <span>监控操作</span>
                                    </div>
                                </template>
                            </el-card>
                        </el-col>
                    </el-row>
                </el-main>
            </el-container>
        </el-main>
    </el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
    name: 'dev_version',
    components: {
        scVideo: defineAsyncComponent(() => import("@/components/scVideo")),
    },
    data() {
        return {
            activeName: 'first',
            rows: [1],
            cols: [12],
            currentIndex: [0, 0],
            cameraList: [],
            cover: '/img/play.png',
        };
    },
    mounted() {
        this.splitVideo(1, 1);
    },
    methods: {
        splitVideo(row, col) {
            this.rows = [];
            this.cols = [];
            for (let i = 0; i < row; i++) {
                this.rows.push(1);
            }
            for (let i = 0; i < col; i++) {
                this.cols.push(24 / col);
            }
        },
        setCurrent(row, col) {
            this.currentIndex = [row, col];
        },
        handleClick() {

        },
        handleNodeClick() {

        }
    },
};
</script>
<style scoped>
.selected {
    background-color: #5ea6f1;
}

.ddd {
    margin: 0.3rem;
    background-color: #fafafa;
}

.vedio-cover {
    display: flex;
    justify-content: center;
    align-items: center;
}
</style>