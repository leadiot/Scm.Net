<template>
    <el-card shadow="never" header="数据报表">
        <scCharts height="300px" :option="option"></scCharts>
    </el-card>
</template>

<script>
import scCharts from "@/components/scCharts";

export default {
    components: {
        scCharts
    },
    props: {
        title: { type: String, default: "我的卡片" },
    },
    data() {
        return {
            option: {
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
                        name: '降水量',
                        min: 0,
                        max: 250,
                        position: 'right',
                        axisLabel: {
                            formatter: '{value} ml'
                        }
                    },
                    {
                        type: 'value',
                        name: '温度',
                        min: 0,
                        max: 25,
                        position: 'left',
                        axisLabel: {
                            formatter: '{value} °C'
                        }
                    }
                ],
                series: [
                    {
                        name: '降水量',
                        type: 'bar',
                        yAxisIndex: 0,
                        data: []
                    },
                    {
                        name: '温度',
                        type: 'line',
                        smooth: true,
                        yAxisIndex: 1,
                        data: []
                    }
                ]
            }
        }
    },
    methods: {
        formatTime(row) {
            return this.$TOOL.dateTimeFormat(row.create_time);
        }
    }
}
</script>

<style></style>