<template>
    <sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="title" width="750px" @close="close" style="">
        <el-container>
            <el-header>
                <div class="left-panel">
                </div>
                <div class="right-panel">
                    <el-input v-model="param.key" clearable placeholder="关键字">
                        <template #append>
                            <el-button type="primary" @click="search()"><sc-icon name="sc-search" /></el-button>
                        </template>
                    </el-input>
                </div>
            </el-header>
            <el-main class="nopadding">
                <scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id"
                    @selection-change="selectionChange" :hideDo="true">
                    <el-table-column align="center" fixed type="selection" width="60" />
                    <el-table-column label="#" type="index" width="50"></el-table-column>
                </scTable>
            </el-main>
        </el-container>

        <template #footer>
            <el-button @click="close">取 消</el-button>
            <el-button :loading="isSaveing" type="primary" @click="save">
                确 定
            </el-button>
        </template>
    </sc-dialog>
</template>
<script>
export default {
    emits: ['complete'],
    data() {
        return {
            title: "书籍选择",
            tableName: 'samples_book_selector',
            apiObj: this.$API.samplesbook.page,
            visible: false,
            isSaveing: false,
            param: {
                row_status: this.$SCM.DEF_STATUS,
                key: ''
            },
            selection: [],
            column: [
                { label: "id", prop: "id", hide: true },
                { prop: 'codes', label: '系统代码', width: 140 },
                { prop: 'codec', label: '书籍编码', width: 100 },
                { prop: 'names', label: '系统简称', width: 160 },
                { prop: 'namec', label: '书籍全称', minWidth: 200, align: 'left' },
                { prop: 'barcode', label: '条码', width: 100 },
                { prop: 'update_time', label: '更新时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
                { prop: 'update_names', label: '更新人员', width: 100 },
                { prop: 'create_time', label: '创建时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
                { prop: 'create_names', label: '创建人员', width: 100 },
            ],
            id: '0',
            api: null,
        };
    },
    mounted() {
    },
    methods: {
        complete() {
            this.$refs.table.refresh();
        },
        search() {
            this.$refs.table.upData(this.param);
        },
        selectionChange(selection) {
            this.selection = selection;
        },
        async open(id, api) {
            this.id = id;
            this.api = api;
            this.visible = true;
        },
        async save() {
            if (!this.api) {
                this.$message.success("调用API为空！");
                return;
            }
            if (!this.selection || this.selection.length < 1) {
                this.$message.success("请选择要添加的书籍！");
                return;
            }

            var items = [];
            this.selection.forEach((ele) => {
                items.push(ele.id);
            });

            this.isSaveing = true;
            var data = { 'id': this.id, 'items': items }
            let res = await this.api.post(data);
            this.isSaveing = false;

            if (res.code == 200) {
                this.$emit("complete");
                this.visible = false;
                this.$message.success("保存成功");
            } else {
                this.$alert(res.message, "提示", { type: "error" });
            }
        },
        close() {
            this.visible = false;
        }
    },
};
</script>
<style scoped>
.el-dialog__body {
    padding: 0px;
}
</style>
