<template>
    <sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="title" width="750px" @close="close">
        <el-form ref="formRef" label-width="100px" :model="formData">
            <el-form-item label="模板类型" prop="types">
                <el-input v-model="formData.types" placeholder="请选择模板类型"></el-input>
            </el-form-item>
            <el-form-item label="模板代码" prop="codec">
                <el-input v-model="formData.codec" placeholder="请输入模板代码" :maxlength="32" show-word-limit
                    clearable></el-input>
            </el-form-item>
            <el-form-item label="模板名称" prop="namec">
                <el-input v-model="formData.namec" placeholder="请输入模板名称" :maxlength="64" show-word-limit
                    clearable></el-input>
            </el-form-item>
            <el-form-item label="标题模板" prop="head">
                <el-input v-model="formData.head" placeholder="请输入标题模板" :maxlength="128" show-word-limit
                    clearable></el-input>
            </el-form-item>
            <el-form-item label="内容模板" prop="body">
                <el-input v-model="formData.body" placeholder="请输入内容模板" :maxlength="512" show-word-limit clearable
                    type="textarea"></el-input>
            </el-form-item>
            <el-form-item label="声明模板" prop="foot">
                <el-input v-model="formData.foot" placeholder="请输入声明模板" :maxlength="128" show-word-limit
                    clearable></el-input>
            </el-form-item>
            <el-form-item label="文件模板" prop="file">
                <el-input v-model="formData.file" placeholder="请输入文件模板" :maxlength="64" show-word-limit
                    clearable></el-input>
            </el-form-item>
        </el-form>

        <template #footer>
            <el-button type="primary" @click="close">
                关闭
            </el-button>
        </template>
    </sc-dialog>
</template>
<script>
export default {
    data() {
        return {
            title: '查看',
            visible: false,
            formData: this.def_data(),
        };
    },
    mounted() {
    },
    methods: {
        def_data() {
            return {
                id: this.$SCM.DEF_ID,
                types: '0',
                codec: '',
                namec: '',
                head: '',
                body: '',
                foot: '',
                file: '',
            }
        },
        async open(id) {
            var res = await this.$API.scmressms.view.get(id);
            this.formData = res.data;
            this.visible = true;
        },
        close() {
            this.formData = this.def_data();
            this.$refs.formRef.resetFields();
            this.visible = false;
        },
    },
};
</script>