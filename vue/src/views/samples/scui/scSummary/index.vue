<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Summary 统计组件</h1>
                <p>Summary 组件用于展示统计数据。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Summary 组件默认使用示例。">
                    <sc-summary ref="summary" image="/img/logo.png" title="统计" summary="这是一个统计组件"></sc-summary>
                </sc-code>
            </el-card>
        </div>
    </div>
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