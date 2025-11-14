<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Cropper 图片裁剪</h1>
                <p>Cropper 组件用于图片裁剪示例。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Cropper 组件默认使用示例。">
                    <sc-cropper ref="cropper" src="/img/logo.png"></sc-cropper>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scCropper from "@/components/scCropper";

export default {
    name: 'scui_cropper',
    components: {
        scCode,
        scCropper,
    },
    data() {
        return {
            code1: `<sc-cropper ref="cropper" src="/img/logo.png"></sc-cropper>`,
        };
    }
}
</script>