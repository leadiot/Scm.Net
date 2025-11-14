<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>QrCode 二维码组件</h1>
                <p>QrCode 组件用于二维码的展示。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="QrCode 组件默认使用示例。">
                    <sc-qr-code text="https://www.baidu.com" :size="200"></sc-qr-code>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
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

<style scoped>
.el-card {
    max-width: 960px;
    margin: 0 auto;
}
</style>