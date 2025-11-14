<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>Charts 图表</h1>
                <p>Charts 组件用于显示图表示例。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Charts 组件默认使用示例。">
                    <sc-charts :option="optionCharts" height="400px"></sc-charts>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';
import scCharts from "@/components/scCharts";

export default {
    name: 'scui_charts',
    components: {
        scCode,
        scCharts,
    },
    data() {
        return {
            optionCharts: {
                tooltip: {
                    trigger: 'axis',
                    axisPointer: { type: 'cross' }
                },
                legend: {},
                xAxis: [
                    {
                        type: 'category',
                        axisTick: {
                            alignWithLabel: true
                        },
                        data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: 'PV数量',
                        position: 'right',
                        axisLabel: {
                            formatter: '{value}'
                        }
                    },
                    {
                        type: 'value',
                        name: 'UV数量',
                        position: 'left',
                        axisLabel: {
                            formatter: '{value}'
                        }
                    }
                ],
                series: [
                    {
                        name: 'PV数量',
                        type: 'bar',
                        yAxisIndex: 0,
                        data: [20, 32, 33, 40, 200, 323, 422, 450]
                    },
                    {
                        name: 'UV数量',
                        type: 'line',
                        smooth: true,
                        yAxisIndex: 1,
                        data: [17, 22, 23, 30, 32, 33, 42, 45]
                    }
                ]
            },
            code1: `<sc-charts :option="optionCharts" height="400px"></sc-charts>`,
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