<template>
    <div class="page">
        <el-card>
            <div v-html="html" v-if="html"></div>
            <div v-else>
                <div style="padding: 20px">
                    <span>暂时还没有来得及写，如果您有好意见或建议，请先通QQ交流群沟通。</span>
                </div>
                <img :src="img" class="image" />
            </div>
        </el-card>
    </div>
</template>

<script>
import { useRoute } from 'vue-router';

export default {
    name: 'about',
    data() {
        return {
            img: this.$CONFIG.SERVER_URL + '/data/qq.jpg',
            html: ''
        }
    },
    mounted() {
        this.readData();
    },
    methods: {
        init() {
            var route = useRoute();
            var path = route.path;
            var idx = path.lastIndexOf('/');
            var code = idx > 0 ? path.substring(idx + 1) : '';
        },
        async readData() {
            var res = await this.$API.scmabout.model.get({ info: 'index' });
            if (!res || res.code != 200) {
                return;
            }

            this.html = res.data;
        }
    }
}
</script>

<style scoped>
.page {
    padding: 20px;
}

.el-card {
    width: 600px;
    margin: 0 auto;
}

.image {
    width: 100%;
}

@media (max-width: 1000px) {
    .el-card {
        width: 100%;
    }
}
</style>
