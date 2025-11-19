<template>
    <sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="600px" @close="close">
        <div class="icon_panel">
            <div class="icon">
                <sc-icon :name="formData.name" :size="64" />
            </div>
            <el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
                <el-form-item label="图标名称" prop="name">
                    <el-input v-model="formData.name" placeholder="请输入图标名称" :maxlength="32" show-word-limit
                        clearable></el-input>
                </el-form-item>
                <el-form-item label="图标说明" prop="desc">
                    <el-input v-model="formData.desc" placeholder="请输入图标说明" type="textarea" :rows="3" :maxlength="256"
                        show-word-limit clearable></el-input>
                </el-form-item>
                <el-form-item label="显示排序" prop="od">
                    <el-input-number v-model="formData.od" :min="1" :max="1024" placeholder="请输入显示排序" />
                </el-form-item>
            </el-form>
        </div>

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
    data() {
        return {
            mode: "add",
            titleMap: { add: "新增", edit: "编辑" },
            visible: false,
            isSaveing: false,
            formData: this.def_data(),
            rules: {
                name: [{ required: true, trigger: "blur", message: "请输入图标名称" }]
            },
        };
    },
    mounted() {
    },
    methods: {
        def_data() {
            return {
                id: this.$SCM.DEF_ID,
                name: '',
                desc: '',
                od: 0,
            }
        },
        async open(row) {
            if (!row || !row.id) {
                this.mode = "add";
            } else {
                this.mode = "edit";
                // var res = await this.$API.scmdevuid.edit.get(row.id);
                // this.formData = res.data;
            }
            this.visible = true;
        },
        save() {
            this.$refs.formRef.validate(async (valid) => {
                if (valid) {
                    this.isSaveing = true;
                    let res = null;
                    if (this.formData.id === '0') {
                        res = await this.$API.scmdevuid.add.post(this.formData);
                    } else {
                        res = await this.$API.scmdevuid.update.put(this.formData);
                    }
                    this.isSaveing = false;
                    if (res.code == 200) {
                        this.$emit("complete");
                        this.visible = false;
                        this.$message.success("保存成功");
                    } else {
                        this.$alert(res.message, "提示", { type: "error" });
                    }
                }
            });
        },
        close() {
            this.formData = this.def_data();
            this.$refs.formRef.resetFields();
            this.visible = false;
        },
    },
};
</script>
<style scoped>
.el-input-number {
    width: 100%;
}

.icon_panel {
    display: flex;
    flex-direction: row;
    justify-content: center;
}

.icon {
    border: 1px solid #ccc;
    margin-bottom: 20px;
    display: flex;
    text-align: center;
    align-items: center;
    width: 148px;
    height: 148px;
    justify-content: center;
}
</style>