<template>
    <el-container>
        <el-aside>
            <sc-list :data="noticeList">
            </sc-list>
        </el-aside>
        <el-main class="nopadding content">
        </el-main>
    </el-container>
</template>
<script>
export default {
    props: {
        user: { type: Object, default: null },
        keywords: { type: [Number, String, Boolean], required: false }
    },
    data() {
        return {
            apiObj: '',
            noticeList: []
        }
    },
    mounted() {

    },
    methods: {
        addHandle() {
        },
        addMsg() {
        },
        refresh() {
        }
    }
}
</script>
<style lang="scss" scoped>
@import '@/assets/sass/_variable.scss';

.content {
    display: flex;
    background-color: $lightColor-2;
    width: 100%;
    height: 100%;
}
</style>