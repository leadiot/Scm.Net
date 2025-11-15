<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Charts 图表</h1>
                    <p>Charts 组件用于显示图表示例。</p>
                    <sc-code lang="html" :code="code1" title="基础用法" desc="Charts 组件默认使用示例。">
                        <sc-charts :option="optionCharts" height="400px"></sc-charts>
                    </sc-code>

                    <h2>属性</h2>
                    <el-table :data="fieldTable" style="width: 100%">
                        <el-table-column prop="name" label="属性名" width="120" />
                        <el-table-column prop="desc" label="说明" />
                        <el-table-column prop="type" label="类型" width="80" />
                        <el-table-column prop="val" label="可选值" width="120" />
                        <el-table-column prop="def" label="默认值" width="120" />
                    </el-table>
                    <h2>事件</h2>
                    <el-table :data="eventTable" style="width: 100%">
                        <el-table-column prop="name" label="事件名" width="120" />
                        <el-table-column prop="desc" label="说明" />
                        <el-table-column prop="args" label="回调参数" width="120" />
                    </el-table>
                </el-card>
            </div>
        </div>
    </div>
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
            fieldTable: [
                { name: 'width', type: 'Number', desc: '图表宽度', val: '', def: '100%' },
                { name: 'height', type: 'Number', desc: '图表高度', val: '', def: '100%' },
                { name: 'nodata', type: 'Boolean', desc: '是否显示无数据提示', val: '', def: 'false' },
                { name: 'option', type: 'Object', desc: '图表配置项', val: '', def: '{}' },
            ],
            eventTable: [
            ]
        };
    }
}
</script>