<!--
 * @Descripttion: 分类管理器
 * @version: 1.0
 * @Author:
 * @Date: 
 * @LastEditors:
 * @LastEditTime:
-->
<template>
    <el-container v-loading="loading">
        <el-header>
            <el-input placeholder="输入关键字进行过滤" v-model="key" clearable></el-input>
            <el-button type="primary" round class="add-column" @click="edit">
                <sc-icon name="sc-plus"/>
            </el-button>
        </el-header>
        <el-main class="nopadding">
            <el-tree ref="list" class="menu" node-key="value" :default-expand-all="expandAll" :data="list"
                :current-node-key="selectVal" :filter-node-method="filterNode" @node-click="nodeClick">
                <template #default="{ node, data }">
                    <span class="scmui-item-node">
                        <span class="label">{{ node.label }}</span>
                        <span class="code">{{ data.code }}</span>
                        <span class="opt">
                            <sc-icon @click.stop="edit(data)" name="sc-edit-line"></sc-icon>
                            <sc-icon @click.stop="remove(node, data)" name="sc-icon"></sc-icon>
                        </span>
                    </span>
                </template>
            </el-tree>
        </el-main>
    </el-container>
    <edit ref="edit" @complete="complete" />
</template>

<script>
import { defineAsyncComponent } from "vue";

export default {
    components: {
        edit: defineAsyncComponent(() => import("./edit")),
    },
    props: {
        appId: { type: String, default: '0' },
        showAll: { type: Boolean, default: false },
        expandAll: { type: Boolean, default: false },
        data: { type: Object, default: () => { } },
        params: { type: Object, default: () => ({}) },
        defaultValue: { type: String, default: '0' },
    },
    emits: ['change'],
    data() {
        return {
            loading: false,
            key: "",
            list: [],
            all: { id: "1", value: "0", label: "所有", parentId: "0" },
            selectVal: '',
            selectCat: null,
        }
    },
    watch: {
        key(val) {
            this.$refs.list.filter(val);
        },
    },
    created() {
        this.selectVal = this.defaultValue;
        this.getData({ app: this.appId })
    },
    methods: {
        edit(row) {
            if (row.id) {
                this.$refs.edit.open(row);
            } else {
                this.$refs.edit.open({ app: this.appId });
            }
        },
        //树过滤
        filterNode(value, data) {
            if (!value) return true;
            return data.label.indexOf(value) !== -1;
        },
        //树点击事件
        nodeClick(data) {
            if (data.value == '0') {
                this.$emit('change', { app: this.appId });
                return;
            }

            this.selectCat = data;
            this.selectVal = data.value;
            this.$emit('change', this.selectCat);
        },
        remove(node, data) {
            this.$confirm(`确定要删除选中的 ${data.label} 项吗？`, "提示", {
                type: "warning",
                confirmButtonText: "确定",
                cancelButtonText: "取消",
            })
                .then(async () => {
                    const loading = this.$loading();
                    var res = await this.$API.scmrescat.delete.delete(
                        data.id
                    );
                    if (res.code == 200) {
                        this.columnComplete();
                        loading.close();
                        this.$message.success("删除成功");
                    } else {
                        this.$alert(res.message, "提示", { type: "error" });
                    }
                })
                .catch(() => { });
        },
        //加载树数据
        async getData(param) {
            this.loading = true;
            const res = await this.$API.scmrescat.list.get(param);
            this.loading = false;
            let _tree = [];
            if (this.showAll) {
                _tree.push(this.all);
            }
            res.data.some((m) => {
                _tree.push({
                    id: m.id,
                    value: m.id,
                    label: m.namec,
                    code: m.codec,
                    app: m.app,
                    parentId: m.parentId,
                });
            });
            this.list = this.$TOOL.changeTree(_tree);
        },
        complete() {
            this.getData({ app: this.appId });
        },
    }
}
</script>
<style scoped>
.add-column {
    padding: 8px !important;
    margin: 8px;
}
</style>
