<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>UploadAvatar 上传头像组件</h1>
                    <p>UploadAvatar 组件用于上传头像。</p>
                    <sc-code lang="html" :code="code1" title="基础用法" desc="UploadAvatar 组件默认使用示例。">
                        <sc-upload-avatar ref="uploadAvatar" />
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scUploadAvatar from '@/components/scUploadAvatar';

export default {
    name: 'scui_upload_avatar',
    components: {
        scCode,
        scUploadAvatar,
    },
    data() {
        return {
            value: 1,
            code1: `<sc-upload-avatar ref="uploadAvatar" />`,
        };
    },
    methods: {
    }
}
</script>