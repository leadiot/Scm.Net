<template>
    <el-container>
        <el-main>
            <el-card>
                <div v-html="html" v-if="html"></div>
                <div v-else>
                    <el-skeleton />
                </div>
            </el-card>
        </el-main>
    </el-container>
</template>

<script>
import { useRoute } from 'vue-router';

export default {
    name: 'about',
    data() {
        return {
            html: ''
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        async init() {
            // 默认使用关于作者
            var section = "author";
            // 默认使用系统配置的代码
            var code = this.$CONFIG.APP_CODE;

            var route = useRoute();
            var path = route.path.toLowerCase();
            var idx = path.indexOf('/app/');
            if (idx > 0) {
                path = path.substring(idx + 5);
                var arr = path.split('/');
                // 如果指定了模块，从路径中获取
                if (arr.length > 0) {
                    section = arr[0];
                }

                // 如果指定代码，从路径中获取
                if (arr.length > 1) {
                    code = arr[1];
                }
            }

            var res = await this.$API.scmabout.info.get({ 'code': code, 'section': section });
            if (!res || res.code != 200) {
                return;
            }

            this.html = res.data;
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
