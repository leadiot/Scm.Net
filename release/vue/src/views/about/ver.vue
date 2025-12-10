<template>
    <el-container>
        <el-main>
            <el-card>
                <el-timeline>
                    <el-timeline-item placement="top" v-for="item in ver_list" :key="item.id" :timestamp="item.date">
                        <el-card shadow="never">
                            <h4>版本：{{ item.ver }}（BUILD:{{ item.build }}）</h4>
                            <p v-html="toHtml(item.remark)"></p>
                        </el-card>
                    </el-timeline-item>
                </el-timeline>
            </el-card>
        </el-main>
    </el-container>
</template>

<script>
import { useRoute } from "vue-router";

export default {
    name: 'about_version',
    data() {
        return {
            app_code: 'scm.net',
            ver_list: []
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        async init() {
            var route = useRoute();
            var path = route.path.toLowerCase();
            var idx = path.indexOf('/ver/');
            if (idx < 0) {
                return;
            }

            path = path.substring(idx + 1);
            var arr = path.split('/');
            if (arr.length < 2) {
                return;
            }

            this.app_code = arr[1];
            var res = await this.$API.scmdevversion.list.get({ 'code': this.app_code });
            if (res == null || res.code != 200) {
                return;
            }

            this.ver_list = res.data;
        },
        toHtml(html) {
            if (!html) {
                return '';
            }
            return html.replace(/\n/g, '<br/>');
        }
    }
}
</script>

<style scoped>
.el-card {
    max-width: 800px;
    margin: 0 auto;
}

@media (max-width: 1000px) {
    .el-card {
        width: 100%;
    }
}
</style>
