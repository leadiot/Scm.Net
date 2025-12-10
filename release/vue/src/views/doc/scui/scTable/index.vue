<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Table 表格组件</h1>
                <p>Table 组件用于展示列表数据。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Table 组件默认使用示例。">
                    <sc-table ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" hide-pagination
                        is-tree row-key="id" @menu-handle="menuHandle" @selection-change="selectionChange">
                        <el-table-column fixed type="selection" width="60" />

                        <el-table-column align="center" fixed="right" label="操作" width="140">
                            <template #default="scope">
                                <el-button size="small" text type="primary" @click="open_dialog(scope.row)">
                                    编辑
                                </el-button>
                                <el-divider direction="vertical" />
                                <el-popconfirm title="确定删除吗？" @confirm="delete_item(scope.row, scope.$index)">
                                    <template #reference>
                                        <el-button text type="primary" size="small">删除</el-button>
                                    </template>
                                </el-popconfirm>
                            </template>
                        </el-table-column>

                        <template #row_status="scope">
                            <el-tooltip :content="scope.row.row_status ? '正常' : '停用'" placement="right">
                                <el-switch v-model="scope.row.row_status" :active-value="1" :inactive-value="2"
                                    @change="status_item($event, scope.row)">
                                </el-switch>
                            </el-tooltip>
                        </template>

                        <template #isSystem="{ data }">
                            <el-tag :type="data.isSystem ? 'success' : 'danger'">
                                {{ data.isSystem ? "是" : "否" }}
                            </el-tag>
                        </template>
                    </sc-table>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';

export default {
    name: 'scui_table',
    components: {
        scCode,
    },
    data() {
        return {
            tableName: 'scui_table',
            apiObj: this.$API.scmurgroup.list,
            param: {
                key: '',
            },
            selection: [],
            column: [
                { label: "id", prop: "id", hide: true, },
                { prop: "namec", label: "群组名称", width: 200, align: "left", },
                { prop: "remark", label: "备注", minWidth: 200, align: "left", },
                { prop: "row_status", label: "数据状态", width: "80", },
                { prop: "update_names", label: "更新人员", width: "100", },
                { prop: "update_time", label: "更新时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
                { prop: "create_names", label: "创建人员", width: "100", },
                { prop: "create_time", label: "创建时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
            ],
            code1: `<sc-table ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" hide-pagination
    is-tree row-key="id" @menu-handle="menuHandle" @selection-change="selectionChange">
    <el-table-column fixed type="selection" width="60" />

    <el-table-column align="center" fixed="right" label="操作" width="140">
        <template #default="scope">
            <el-button size="small" text type="primary" @click="open_dialog(scope.row)">
                编辑
            </el-button>
            <el-divider direction="vertical" />
            <el-popconfirm title="确定删除吗？" @confirm="delete_item(scope.row, scope.$index)">
                <template #reference>
                    <el-button text type="primary" size="small">删除</el-button>
                </template>
            </el-popconfirm>
        </template>
    </el-table-column>

    <template #row_status="scope">
        <el-tooltip :content="scope.row.row_status ? '正常' : '停用'" placement="right">
            <el-switch v-model="scope.row.row_status" :active-value="1" :inactive-value="2"
                @change="status_item($event, scope.row)">
            </el-switch>
        </el-tooltip>
    </template>

    <template #isSystem="{ data }">
        <el-tag :type="data.isSystem ? 'success' : 'danger'">
            {{ data.isSystem ? "是" : "否" }}
        </el-tag>
    </template>
</sc-table>`,
        };
    },
    methods: {
        search(key) {
            if (key != null) {
                this.param.key = key;
            }
            this.$refs.table.upData(this.param);
        },
        async status_item(e, row) {
            this.$SCM.status_item(this, this.$API.scmurgroup.status, row, row.row_status);
        },
        status_list(status) {
            this.$SCM.status_list(this, this.$API.scmurgroup.status, this.selection, status);
        },
        async delete_item(row) {
            this.$SCM.delete_item(this, this.$API.scmurgroup.delete, row);
        },
        delete_list() {
            this.$SCM.delete_list(this, this.$API.scmurgroup.delete, this.selection);
        },
        open_dialog(row) {
            this.$refs.edit.open(row);
        },
        selectionChange(selection) {
            this.selection = selection;
        },
        menuHandle(obj) {
            if (obj.command == "add") {
                this.open_dialog({});
            }
            if (obj.command == "edit") {
                this.open_dialog(obj.row);
            }
            if (obj.command == "delete") {
                this.delete_item(obj.row);
            }
        },
    }
}
</script>