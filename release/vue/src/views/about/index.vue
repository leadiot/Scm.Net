<template>
    <el-container>
        <el-main>
            <el-card>
                <div v-html="html" v-if="html"></div>
                <div v-else>
                    <div style="padding: 20px">
                        <span>暂时还没有来得及写，如果您有好意见或建议，请先通QQ交流群沟通。</span>
                    </div>
                    <img :src="img" class="image" />
                </div>
            </el-card>
        </el-main>
    </el-container>
</template>

<script>
export default {
    name: 'about',
    data() {
        return {
            img: this.$CONFIG.SERVER_URL + '/data/qq.jpg',
            html: ''
        }
    },
    mounted() {
    },
    methods: {
    }
}
</script>