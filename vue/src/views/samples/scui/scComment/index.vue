<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>Comment 评论</h1>
                <p>Comment 评论用于显示评论示例。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Comment 组件默认显示评论示例。">
                    <sc-comment code="appId"></sc-comment>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';
import scComment from '@/components/scComment';

export default {
    name: 'scui_button',
    components: {
        scCode,
        scComment,
    },
    data() {
        return {
            title: 'sc-code',
            desc: 'sc-code 组件使用示例',
            code1: `<sc-comment code="appId"></sc-comment>`,
        };
    }
}
</script>

<style scoped>
.el-card {
    max-width: 960px;
    margin: 0 auto;
}
</style>