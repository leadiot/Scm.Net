<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Cron 表达式</h1>
                    <p>Cron 表达式用于定时任务表达式示例。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Cron 组件默认显示上下文菜单示例。">
                        <sc-cron ref="cron" v-model="example1.myCronValue" style="width: 200px"></sc-cron>
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
import scCron from "@/components/scCron";

export default {
    name: 'scui_cron',
    components: {
        scCode,
        scCron,
    },
    data() {
        return {
            example1: {
                myCronValue: '* * * * * ?',
                code: `<sc-cron ref="cron" v-model="myCronValue"></sc-cron>`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'String', desc: 'Cron 表达式', val: '', def: '* * * * * ?' },
                { name: 'shortcuts', type: 'Array', desc: '快捷选项，格式：{name: "快捷选项名称", value: "快捷选项值"}', val: '', def: '[]' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>