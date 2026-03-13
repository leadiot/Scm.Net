<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>TableFilter 表格过滤组件</h1>
                <p>TableFilter 组件用于过滤表格数据。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="TableFilter 组件默认使用示例。">
                    <sc-table-filter ref="tableFilter" :column="example1.column" :filter-name="example1.filterName"
                        :show-operator="example1.showOperator" />
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
</template>
<script>
import scCode from '@/components/scCode';
import scTableFilter from '@/components/scTableFilter';

export default {
    name: 'scui_table_filter',
    components: {
        scCode,
        scTableFilter,
    },
    data() {
        return {
            example1: {
                filterName: "群组过滤",
                showOperator: true,
                column: [
                    { label: "id", prop: "id", hide: true, },
                    { prop: "namec", label: "群组名称", width: 200, align: "left", type: 'text', filter: true },
                    { prop: "remark", label: "备注", minWidth: 200, align: "left", type: 'text', filter: true },
                    { prop: "row_status", label: "数据状态", width: "80", },
                    { prop: "update_names", label: "更新人员", width: "100", },
                    { prop: "update_times", label: "更新时间", width: "160" },
                    { prop: "create_names", label: "创建人员", width: "100", },
                    { prop: "create_times", label: "创建时间", width: "160" },
                ],
                code: `<sc-table-filter ref="tableFilter" :column="column" :filter-name="filterName" :show-operator="showOperator" />`,
            },
            fieldTable: [
                { name: 'column', type: 'Array', desc: '表格列配置', val: '', def: '[]' },
                { name: 'filterName', type: 'String', desc: '过滤器名称', val: '', def: '' },
                { name: 'showOperator', type: 'Boolean', desc: '是否显示操作符', val: '', def: 'false' },
                { name: 'options', type: 'Array', desc: '过滤选项', val: '', def: '[]' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>