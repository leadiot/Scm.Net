<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>QrCode 二维码组件</h1>
                <p>QrCode 组件用于二维码的展示。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="QrCode 组件默认使用示例。">
                    <sc-qr-code text="https://www.baidu.com" :size="200"></sc-qr-code>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scQrCode from '@/components/scQrCode';

export default {
    name: 'scui_qr_code',
    components: {
        scCode,
        scQrCode,
    },
    data() {
        return {
            code1: `<sc-qr-code text="https://www.baidu.com" :size="200"></sc-qr-code>`,
        };
    },
    methods: {
    }
}
</script>