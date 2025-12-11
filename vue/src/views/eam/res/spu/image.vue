<template>
    <sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="title" width="450px" @close="close">
        <scUploadAvatar v-model="formData.avatar" :apiObj="uploadApi" :width="320" :height="320" :onSuccess="upSuccess"
            :data="formData" />
    </sc-dialog>
</template>
<script>
import scUploadAvatar from "@/components/scUploadAvatar";
export default {
    components: {
        scUploadAvatar: scUploadAvatar,
    },
    data() {
        return {
            title: '新增商品图片',
            uploadApi: this.$API.eamresspu.upload,
            visible: false,
            isSaveing: false,
            formData: this.def_data(),
        };
    },
    mounted() {
    },
    methods: {
        def_data() {
            return {
                id: this.$SCM.DEF_ID,
                image: this.$SCM.DEF_CAT_ID
            }
        },
        async open(row) {
            if (!row || !row.id) {
                return;
            }

            this.formData.id = row.id;
            this.formData.image = row.image;
            this.visible = true;
        },
        close() {
            this.visible = false;
        },
        upSuccess(res) {
            this.formData.avatar = res.data.path;
            if (res.code == 200) {
                this.$message.success("上传成功~");
            } else {
                this.$message.warning(res.message);
            }
        },
    },
};
</script>