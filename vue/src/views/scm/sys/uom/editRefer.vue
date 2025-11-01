<template>
    <sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="title" width="450px" @close="close">
        <el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
            <el-form-item label="单位名称" prop="refer_id">
                <sc-select v-model="formData.refer_id" placeholder="请输入单位名称" :data="refer_list"></sc-select>
            </el-form-item>
            <el-form-item label="转换数量" prop="refer_qty">
                <el-input-number v-model="formData.refer_qty" placeholder="请输入转换数量"></el-input-number>
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
            mode: "add",
            title: "参照单位",
            visible: false,
            isSaveing: false,
            formData: this.def_data(),
            rules: {
                refer_id: [
                    { required: true, trigger: "change", message: "请选择以单位制式", pattern: this.$SCM.REGEX_ID },
                ],
                refer_qty: [
                    { required: true, trigger: "change", message: "请输入转换数量", pattern: this.$SCM.REGEX_INT },
                ],
            },
            types_list: [this.$SCM.OPTION_ONE_INT],
            modes_list: [this.$SCM.OPTION_ONE_INT],
            kinds_list: [this.$SCM.OPTION_ONE_INT],
            lang_list: [this.$SCM.OPTION_ONE_INT],
            refer_list: [this.$SCM.OPTION_ONE],
        };
    },
    mounted() {
        this.$SCM.list_dic(this.types_list, 'uom_types', false);
        this.$SCM.list_dic(this.modes_list, 'uom_modes', false);
        this.$SCM.list_dic(this.kinds_list, 'uom_kinds', false);
        this.$SCM.list_dic(this.lang_list, 'lang', false);
    },
    methods: {
        def_data() {
            return {
                id: '0',
                refer_id: this.$SCM.ID_ONE,
                refer_qty: 0,
            }
        },
        async open(row) {
            if (!row || !row.id) {
                return;
            }

            this.formData.id = row.id;
            var data = { 'types': row.types, 'modes': row.modes, 'kinds': row.kinds, 'lang': row.lang };
            this.$SCM.list_option(this.refer_list, this.$API.scmsysuom.option, data, true);
            this.visible = true;
        },
        save() {
            this.$refs.formRef.validate(async (valid) => {
                if (!valid) {
                    return;
                }

                this.isSaveing = true;
                let res = await this.$API.scmsysuom.changeRefer.post(this.formData);
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
            this.formData = this.def_data();
            this.$refs.formRef.resetFields();
            this.visible = false;
        },
    },
};
</script>