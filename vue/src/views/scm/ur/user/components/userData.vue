<template>
    <sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="title" width="450px" @close="close">
        <el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
            <el-form-item label="权限类型" prop="data">
                <sc-select v-model="formData.data" placeholder="请输入权限类型" :data="data_list"></sc-select>
            </el-form-item>
        </el-form>

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
            title: '数据权限',
            visible: false,
            isSaveing: false,
            formData: this.def_data(),
            rules: {
                data: [
                    { required: true, trigger: "blur", message: "请选择权限类型" },
                ],
            },
            data_list: [this.$SCM.OPTION_ONE_INT],
        };
    },
    mounted() {
        this.$SCM.list_dic(this.data_list, 'user_data_type', false);
    },
    methods: {
        def_data() {
            return {
                id: this.$SCM.DEF_ID,
                data: this.$SCM.ID_ONE_INT
            };
        },
        async open(row) {
            if (!row || !row.id) {
                return;
            }
            // row.data = '' + row.data;
            this.formData = row;
            this.visible = true;
        },
        save() {
            this.$refs.formRef.validate(async (valid) => {
                if (valid) {
                    this.isSaveing = true;
                    let res = await this.$API.scmuruser.userData.put(this.formData);
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
