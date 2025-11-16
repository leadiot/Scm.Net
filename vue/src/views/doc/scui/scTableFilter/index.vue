<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>TableFilter 表格过滤组件</h1>
                <p>TableFilter 组件用于过滤表格数据。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="TableFilter 组件默认使用示例。">
                    <sc-table-filter ref="tableFilter" :column="example1.column" :filter-name="example1.filterName"
                        :show-operator="example1.showOperator" />
                </sc-code>
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
                    { prop: "update_time", label: "更新时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
                    { prop: "create_names", label: "创建人员", width: "100", },
                    { prop: "create_time", label: "创建时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
                ],
                code: `<sc-table-filter ref="tableFilter" :column="column" :filter-name="filterName" :show-operator="showOperator" />`,
            },
        };
    },
    methods: {
    }
}
</script>