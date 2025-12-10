<template>
    <el-main>
        <el-row :gutter="15">
            <el-col :lg="6">
                <myCard :title="summary0.title" :tooltip="summary0.tooltip" :number="summary0.value"
                    :rate="summary0.rate">
                </myCard>
            </el-col>
            <el-col :lg="6">
                <myCard :title="summary1.title" :tooltip="summary1.tooltip" :number="summary1.value"
                    :rate="summary1.rate">
                </myCard>
            </el-col>
            <el-col :lg="6">
                <myCard :title="summary2.title" :tooltip="summary2.tooltip" :number="summary2.value"
                    :rate="summary2.rate">
                </myCard>
            </el-col>
            <el-col :lg="6">
                <myCard :title="summary3.title" :tooltip="summary3.tooltip" :number="summary3.value"
                    :rate="summary3.rate">
                </myCard>
            </el-col>
        </el-row>
        <el-row>
            <el-col>
                <el-card shadow="never" header="数据报表">
                    <scCharts height="300px" :option="optionCharts"></scCharts>
                </el-card>
            </el-col>
        </el-row>
        <el-row>
            <el-col>
                <el-card shadow="never">
                    <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
                        <el-tab-pane label="我的待办" name="todo">
                            <myTask></myTask>
                        </el-tab-pane>
                        <el-tab-pane label="系统消息" name="task">
                            <myTask></myTask>
                        </el-tab-pane>
                        <el-tab-pane label="行政公告" name="note">
                            <myTask></myTask>
                        </el-tab-pane>
                        <el-tab-pane label="新闻资讯" name="news">
                            <myTask></myTask>
                        </el-tab-pane>
                    </el-tabs>
                </el-card>
            </el-col>
        </el-row>
    </el-main>
</template>

<script>
import myCard from './components/myCard';
import myTask from './components/myTask';
import scCharts from "@/components/scCharts";

export default {
    name: "home_data",
    components: {
        myCard,
        myTask,
        scCharts
    },
    data() {
        return {
            rates: [],
            summarys: [],
            summary0: {},
            summary1: {},
            summary2: {},
            summary3: {},
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
                        data: []
                    },
                    {
                        name: 'UV数量',
                        type: 'line',
                        smooth: true,
                        yAxisIndex: 1,
                        data: []
                    }
                ]
            },
            activeName: 'todo'
        }
    },
    mounted() {
        this.$emit('on-mounted');
        this.init();
    },
    methods: {
        async init() {
            var res = await this.$API.home.summary.get();
            if (res != null && res.code == 200) {
                this.summarys = res.data.summaries;
            }
            if (!this.summarys) {
                this.summarys = this.defSummarys();
            }
            this.summary0 = this.summarys[0];
            this.summary1 = this.summarys[1];
            this.summary2 = this.summarys[2];
            this.summary3 = this.summarys[3];

            res = await this.$API.home.report.get();
            if (res != null && res.code == 200) {
                this.optionCharts.xAxis[0].data = res.data.titles;
                this.optionCharts.series[0].data = res.data.data1;
                this.optionCharts.series[1].data = res.data.data2;
            }

            res = await this.$API.home.todo.get();
            if (res != null && res.code == 200) {
                // this.optionCharts.series[0].data = res.data.data1;
                // this.optionCharts.series[1].data = res.data.data2;
            }
        },
        defSummarys() {
            return [{
                'names': '今日总单量', 'tips': '今日进入系统的所有订单数量', 'number': 122,
                'rate': { 'title': '环比变化：', 'value': 10 }
            },
            {
                'names': '今日待发货', 'tips': '今日进入单量中待发货数量', 'number': 122,
                'rate': { 'title': '环比变化：', 'value': 11 }
            },
            {
                'names': '月度活跃用户', 'tips': '本月所有活跃用户数量', 'number': 122,
                'rate': { 'title': '环比变化：', 'value': -21 }
            },
            {
                'names': '月度新增用户', 'tips': '本月所有新增用户数量', 'number': 122,
                'rate': { 'title': '环比变化：', 'value': 31 }
            }];
        },
        handleClick() {
        }
    }
}
</script>