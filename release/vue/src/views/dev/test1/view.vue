<template>
    <el-drawer title="短信详情" v-model="visible" width="750px" :show-close="false">
        <el-form ref="formRef" label-width="100px" :model="formData">
            <el-form-item label="模板类型" prop="types">
                <sc-select v-model="formData.types" disabled :data="types_list"></sc-select>
            </el-form-item>
            <el-form-item label="终端号码" prop="code">
                <el-input v-model="formData.code" disabled></el-input>
            </el-form-item>
            <el-form-item label="校验码" prop="sms">
                <el-input v-model="formData.sms" disabled></el-input>
            </el-form-item>
            <el-form-item label="发送次数" prop="send_qty">
                <el-input v-model="formData.send_qty" disabled></el-input>
            </el-form-item>
            <el-form-item label="发送时间" prop="send_time">
                <el-input v-model="formData.send_time" disabled></el-input>
            </el-form-item>
            <el-form-item label="模板标题" prop="head">
                <el-input v-model="formData.head" disabled></el-input>
            </el-form-item>
            <el-form-item label="模板内容" prop="body">
                <el-input v-model="formData.body" disabled></el-input>
            </el-form-item>
            <el-form-item label="模板声明" prop="foot">
                <el-input v-model="formData.foot" disabled></el-input>
            </el-form-item>
            <el-form-item label="文件模板" prop="file">
                <el-input v-model="formData.file" disabled></el-input>
            </el-form-item>
        </el-form>

        <template #footer>
            <el-button type="primary" @click="close">
                关闭
            </el-button>
        </template>
    </el-drawer>
</template>
<script>
export default {
    data() {
        return {
            title: '查看',
            visible: false,
            formData: this.def_data(),
            types_list: [this.$SCM.OPTION_ONE_INT],
        };
    },
    mounted() {
        this.$SCM.list_dic(this.types_list, 'sms_types', false);
    },
    methods: {
        def_data() {
            return {
                id: this.$SCM.DEF_ID,
                types: 0,
                codec: '',
                namec: '',
                head: '',
                body: '',
                foot: '',
                file: '',
            }
        },
        async open(row) {
            if (!row) {
                return;
            }

            var res = await this.$API.scmlogotp.view.get(row.id);
            if (res.code != 200) {
                return;
            }

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