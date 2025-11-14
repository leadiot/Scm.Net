<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>Summary 统计组件</h1>
                <p>Summary 组件用于展示统计数据。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Summary 组件默认使用示例。">
                    <sc-summary ref="summary" image="/img/logo.png" title="统计" summary="这是一个统计组件"></sc-summary>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';
import scSummary from '@/components/scSummary';

export default {
    name: 'scui_summary',
    components: {
        scCode,
        scSummary,
    },
    data() {
        return {
            code1: `<sc-summary ref="summary" image="/img/logo.png" title="统计" summary="这是一个统计组件"></sc-summary>`,
        };
    },
    methods: {
    }
}
</script>

<style scoped>
.el-card {
    max-width: 960px;
    margin: 0 auto;
}
</style>