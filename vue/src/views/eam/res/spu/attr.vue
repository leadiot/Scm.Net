<template>
    <sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="title" width="750px" @close="close">
        <sc-dynamic-table ref="formTable" v-model="list" :addTemplate="addTemplate">
            <el-table-column label="属性类型" prop="type">
                <template #default="scope">
                    <el-select v-model="scope.row.type" placeholder="请选择属性类型">
                        <el-option label="文本" value="1"></el-option>
                        <el-option label="数字" value="2"></el-option>
                        <el-option label="选项" value="3"></el-option>
                    </el-select>
                </template>
            </el-table-column>
            <el-table-column label="属性代码" prop="codec">
                <template #default="scope">
                    <el-input v-model="scope.row.codec" placeholder="请输入属性代码"></el-input>
                </template>
            </el-table-column>
            <el-table-column label="属性名称" prop="namec">
                <template #default="scope">
                    <el-input v-model="scope.row.namec" placeholder="请输入属性名称"></el-input>
                </template>
            </el-table-column>
            <el-table-column label="默认数据" prop="default_value">
                <template #default="scope">
                    <el-input v-model="scope.row.default_value" placeholder="请输入默认数据"></el-input>
                </template>
            </el-table-column>
        </sc-dynamic-table>

        <template #footer>
            <el-button @click="close">取 消</el-button>
            <el-button :loading="isSaveing" type="primary" @click="save">
                确 定
            </el-button>
        </template>
    </sc-dialog>
</template>
<script>
import scDynamicTable from "@/components/scDynamicTable";
export default {
    components: {
        scDynamicTable: scDynamicTable,
    },
    data() {
        return {
            title: '属性管理',
            visible: false,
            isSaveing: false,
            list: [],
            addTemplate:
            {
                codec: '',
                namec: '',
                type: '1',
                default_value: '',
            },
            rules: {
                codec: [
                    { required: true, trigger: "blur", message: "商品编码不能为空" },
                    { required: true, trigger: "blur", message: "商品编码应4至32个字符", pattern: this.$SCM.REGEX_CODEC },
                ],
                namec: [
                    { required: true, trigger: "blur", message: "商品名称不能为空" },
                    { required: true, trigger: "blur", message: "商品名称应4至64个字符", pattern: this.$SCM.REGEX_NAMEC },
                ],
                cat_id: [
                    { required: true, trigger: "change", message: "请选择商品分类" },
                ],
            },
        };
    },
    mounted() {
    },
    methods: {
        async open(row) {
            if (!row || !row.id) {
                this.mode = "add";
            } else {
                this.mode = "edit";
                var res = await this.$API.eamresspu.edit.get(row.id);
                this.formData = res.data;
            }
            this.visible = true;
        },
        save() {
            this.$refs.formRef.validate(async (valid) => {
                if (!valid) {
                    return;
                }

                this.isSaveing = true;
                let res = null;
                if (this.$SCM.is_valid_id(this.formData.id)) {
                    res = await this.$API.eamresspu.update.put(this.formData);
                } else {
                    res = await this.$API.eamresspu.add.post(this.formData);
                }
                this.isSaveing = false;

                if (res.code == 200) {
                    this.$emit("complete");
                    this.visible = false;
                    this.$message.success("保存成功");
                } else {
                    this.$alert(res.message, "提示", { type: "error" });
                }
            });
        },
        close() {
            this.visible = false;
        },
    },
};
</script>