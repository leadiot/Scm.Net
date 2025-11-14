<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Statistic 统计组件</h1>
                <p>Statistic 组件用于展示统计数据。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Statistic 组件默认使用示例。">
                    <sc-statistic ref="statistic" title="统计" value="100" description="这是一个统计组件" prefix="人" suffix="万"
                        tips="当前的人流量统计"></sc-statistic>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scStatistic from '@/components/scStatistic';

export default {
    name: 'scui_statistic',
    components: {
        scCode,
        scStatistic,
    },
    data() {
        return {
            code1: `<sc-statistic ref="statistic" title="统计" value="100" description="这是一个统计组件" prefix="人" suffix="万"
 tips="当前的人流量统计"></sc-statistic>`,
        };
    },
    methods: {
    }
}
</script>