<template>
    <el-drawer title="用户反馈" v-model="visible" :size="400" append-to-body destroy-on-close>
        <el-form ref="formRef" :model="formData" :rules="rules" label-width="80px" style="padding:0 20px;">
            <el-form-item label="网页地址" prop="url">
                <el-input v-model="formData.url" show-word-limit clearable disabled></el-input>
            </el-form-item>
            <el-form-item label="问题类型" prop="types">
                <sc-select v-model="formData.types" :data="types_list" placeholder="请选择问题类型" collapse-tags></sc-select>
            </el-form-item>
            <el-form-item label="回馈事项" prop="title">
                <el-input v-model="formData.title" placeholder="请输入回馈事项" :maxlength="32" show-word-limit
                    clearable></el-input>
            </el-form-item>
            <el-form-item label="详细说明" prop="remark">
                <el-input v-model="formData.remark" type="textarea" placeholder="请输入详细说明" :maxlength="1024" :rows="5"
                    show-word-limit clearable></el-input>
            </el-form-item>
            <el-form-item>
                <el-button @click="close">取 消</el-button>
                <el-button :loading="isSaveing" type="primary" @click="save">
                    确 定
                </el-button>
            </el-form-item>
        </el-form>
    </el-drawer>
</template>

<script>
export default {
    emits: ["complete"],
    data() {
        return {
            visible: false,
            isSaveing: false,
            formData: {
                url: this.$route.path,
                types: this.$SCM.ID_ONE_INT,
                title: '',
                remark: ''
            },
            rules: {
                types: [
                    { required: true, trigger: "change", type: 'number', min: 1, message: "请选择问题类型" },
                ],
                title: [
                    { required: true, trigger: "blur", message: "请输入回馈事项" },
                ],
            },
            types_list: [this.$SCM.OPTION_ONE_INT],
        }
    },
    mounted() {
        this.$SCM.list_dic(this.types_list, 'feedback_type', false);
    },
    methods: {
        open() {
            this.visible = true;
        },
        close() {
            this.visible = false;
        },
        save() {
            this.$refs.formRef.validate(async (valid) => {
                if (valid) {
                    this.isSaveing = true;
                    var res = await this.$API.scmsysfeedbackheader.add.post(this.formData);
                    this.isSaveing = false;
                    if (res.code == 200) {
                        this.$emit("complete");
                        this.visible = false;
                        this.$message.success("您的反馈已提交，我们会尽快跟进处理！");
                    } else {
                        this.$alert(res.message, "提示", { type: "error" });
                    }
                }
            });
        }
    }
}
</script>
<style scoped>
.el-select {
    width: 100%;
}
</style>