<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Status Indicator 状态指示器组件</h1>
                    <p>Status Indicator 组件用于添加状态指示器。</p>
                    <sc-code lang="html" :code="code1" title="基础用法" desc="Status Indicator 组件默认使用示例。">
                        <sc-status-indicator type="default" pulse></sc-status-indicator>
                        <sc-status-indicator type="primary" pulse></sc-status-indicator>
                        <sc-status-indicator type="success" pulse></sc-status-indicator>
                        <sc-status-indicator type="info" pulse></sc-status-indicator>
                        <sc-status-indicator type="warning" pulse></sc-status-indicator>
                        <sc-status-indicator type="danger" pulse></sc-status-indicator>
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scStatusIndicator from '@/components/scStatusIndicator';

export default {
    name: 'scui_status_indicator',
    components: {
        scCode,
        scStatusIndicator,
    },
    data() {
        return {
            pulse: true,
            code1: `<sc-status-indicator type="default" pulse></sc-status-indicator>
<sc-status-indicator type="primary" pulse></sc-status-indicator>
<sc-status-indicator type="success" pulse></sc-status-indicator>
<sc-status-indicator type="info" pulse></sc-status-indicator>
<sc-status-indicator type="warning" pulse></sc-status-indicator>
<sc-status-indicator type="danger" pulse></sc-status-indicator>`,
        };
    },
    methods: {
    }
}
</script>