<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Upload 上传组件</h1>
                    <p>Upload 组件用于上传文件。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Upload 组件默认使用示例。">
                        <el-button type="primary" @click="uploadClick">点击上传</el-button>
                        <sc-upload ref="upload" @change="uploadChange" />
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scUpload from '@/components/scUpload';

export default {
    name: 'scui_upload',
    components: {
        scCode,
        scUpload,
    },
    data() {
        return {
            example1: {
                code: `<el-button type="primary" @click="uploadClick">点击上传</el-button>
<sc-upload ref="upload" @change="uploadChange" />`,
            },
        };
    },
    methods: {
        uploadClick() {
            this.$refs.upload.open();
        },
        uploadChange(file) {
            console.log(file);
        }
    }
}
</script>