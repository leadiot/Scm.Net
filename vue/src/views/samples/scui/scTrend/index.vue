<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Trend 趋势组件</h1>
                    <p>Trend 组件用于显示趋势。</p>
                    <sc-code lang="html" :code="code1" title="基础用法" desc="Trend 组件默认使用示例。">
                        <sc-trend ref="trend" v-model="value" prefix="前缀" suffix="后缀" />
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scTrend from '@/components/scTrend';

export default {
    name: 'scui_trend',
    components: {
        scCode,
        scTrend,
    },
    data() {
        return {
            value: 1,
            code1: `<sc-trend ref="trend" v-model="value" prefix="前缀" suffix="后缀" />`,
        };
    },
    methods: {
    }
}
</script>