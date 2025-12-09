<template>
    <sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="600px" @close="close">
        <el-form ref="formRef" label-width="auto" label-position="top" :model="formData" :rules="rules">
            <el-form-item label="上传目录" prop="path">
                <el-input v-model="formData.path" show-word-limit clearable></el-input>
                <div class="el-form-item-msg">
                    可自定义添加目录，系统会自动创建，例如/upload/{video}
                </div>
            </el-form-item>
            <el-upload ref="upload" class="upload-demo" :autoUpload="false" drag :multiple="multiple" :limit="limit"
                :accept="accept" :httpRequest="upload" :on-success="uploadSuccess" :on-error="uploadError">
                <sc-icon class="el-icon--upload" name="sc-file-upload-line" />
                <div class="el-upload__text">
                    你可以将文件拖拽到特定区域以进行上传 <em>点击上传</em>
                </div>
            </el-upload>
            <div class="el-form-item-msg">大小不超过100MB</div>
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
    props: {
        apiObj: { type: Object, default: () => { } },
        path: { type: String, default: '/' },
        multiple: { type: Boolean, default: false },
        limit: { type: Number, default: 0 },
        accept: { type: String, default: "image/gif, image/jpeg, image/png" },
        draggable: { type: Boolean, default: false },
    },
    data() {
        return {
            mode: "add",
            titleMap: { add: "上传文件", },
            isSaveing: false,
            visible: false,
            formData: {
                path: '',
                file: '',
            },
            rules: {
                path: [
                    { required: true, trigger: "blur", message: "上传目录不能为空", },
                ],
            },
            formDic: {},
        };
    },
    mounted() {
        this.formData.path = this.path;
    },
    methods: {
        async open(data) {
            this.visible = true;
            this.formDic = data;
        },
        save() {
            this.$refs.formRef.validate(async (valid) => {
                if (valid) {
                    this.$refs.upload.submit();
                }
            });
        },
        upload(param) {
            Object.assign(param.data, this.formDic);

            const data = new FormData();
            data.append('path', this.formData.path);
            data.append('file', param.file);
            data.append('file_name', param.filename);
            for (const key in param.data) {
                data.append(key, param.data[key]);
            }
            this.apiObj.post(data, {
                onUploadProgress: e => {
                    const complete = parseInt(((e.loaded / e.total) * 100) | 0, 10);
                    param.onProgress({ percent: complete });
                }
            }).then(res => {
                if (res.code == 200) {
                    param.onSuccess(res);
                }
                else {
                    param.onError(res);
                }
            }).catch(err => {
                param.onError(err);
            })
        },
        uploadSuccess(res) {
            this.$emit("complete", res);
            this.visible = false;
        },
        uploadError() {
            // this.$emit("complete");
            // this.visible = false;
        },
        close() {
            this.visible = false;
        },
    },
};
</script>