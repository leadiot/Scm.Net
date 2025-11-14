<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>TableSelect 表格选择组件</h1>
                    <p>TableSelect 组件用于选择表格数据。</p>
                    <sc-code lang="html" :code="code1" title="基础用法" desc="TableSelect 组件默认使用示例。">
                        <sc-table-select ref="tableSelect" :api-obj="apiObj" />
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scTableSelect from '@/components/scTableSelect';

export default {
    name: 'scui_table_select',
    components: {
        scCode,
        scTableSelect,
    },
    data() {
        return {
            apiObj: this.$API.scmurgroup.list,
            code1: `<sc-table-select ref="tableSelect" :api-obj="apiObj"/>`,
        };
    },
    methods: {
    }
}
</script>