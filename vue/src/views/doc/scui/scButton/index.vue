<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Button 按钮</h1>
                <p>sc-button 组件默认用法</p>
                <sc-code lang="html" :code="code1" title="默认用法" desc="默认用法">
                    <sc-button>Default</sc-button>
                    <sc-button type="primary">Primary</sc-button>
                    <sc-button type="info">Info</sc-button>
                    <sc-button type="success">Success</sc-button>
                    <sc-button type="warning">Warning</sc-button>
                    <sc-button type="danger">Danger</sc-button>
                </sc-code>

                <sc-code lang="html" :code="code2" title="外观风格" desc="使用plain、round、circle属性">
                    <div>
                        <sc-button plain>Default</sc-button>
                        <sc-button type="primary" plain>Primary</sc-button>
                        <sc-button type="info" plain>Info</sc-button>
                        <sc-button type="success" plain>Success</sc-button>
                        <sc-button type="warning" plain>Warning</sc-button>
                        <sc-button type="danger" plain>Danger</sc-button>
                    </div>
                    <div class="row">
                        <sc-button round>Default</sc-button>
                        <sc-button type="primary" round>Primary</sc-button>
                        <sc-button type="info" round>Info</sc-button>
                        <sc-button type="success" round>Success</sc-button>
                        <sc-button type="warning" round>Warning</sc-button>
                        <sc-button type="danger" round>Danger</sc-button>
                    </div>
                    <div class="row">
                        <sc-button icon="sc-user-line" circle></sc-button>
                        <sc-button icon="sc-user-line" type="primary" circle></sc-button>
                        <sc-button icon="sc-user-line" type="info" circle></sc-button>
                        <sc-button icon="sc-user-line" type="success" circle></sc-button>
                        <sc-button icon="sc-user-line" type="warning" circle></sc-button>
                        <sc-button icon="sc-user-line" type="danger" circle></sc-button>
                    </div>
                </sc-code>

                <sc-code lang="html" :code="code3" title="禁用状态" desc="使用disabled属性">
                    <sc-button icon="sc-user-line" type="default" disabled>Default</sc-button>
                    <sc-button icon="sc-user-line" type="primary" disabled>Primary</sc-button>
                    <sc-button icon="sc-user-line" type="info" disabled>Info</sc-button>
                    <sc-button icon="sc-user-line" type="success" plain disabled>Success</sc-button>
                    <sc-button icon="sc-user-line" type="warning" round disabled>Warning</sc-button>
                    <sc-button icon="sc-user-line" type="danger" circle disabled></sc-button>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scButton from '@/components/scButton';
import scCode from '@/components/scCode';

export default {
    name: 'scui_button',
    components: {
        scButton,
        scCode,
    },
    data() {
        return {
            code1: `<sc-button>Default</sc-button>
<sc-button type="primary">Primary</sc-button>
<sc-button type="info">Info</sc-button>
<sc-button type="success">Success</sc-button>
<sc-button type="warning">Warning</sc-button>
<sc-button type="danger">Danger</sc-button>`,
            code2: `<sc-button plain>Default</sc-button>
<sc-button type="primary" plain>Primary</sc-button>
<sc-button type="info" plain>Info</sc-button>
<sc-button type="success" plain>Success</sc-button>
<sc-button type="warning" plain>Warning</sc-button>
<sc-button type="danger" plain>Danger</sc-button>

<sc-button round>Default</sc-button>
<sc-button type="primary" round>Primary</sc-button>
<sc-button type="info" round>Info</sc-button>
<sc-button type="success" round>Success</sc-button>
<sc-button type="warning" round>Warning</sc-button>
<sc-button type="danger" round>Danger</sc-button>

<sc-button icon="sc-user-line" circle></sc-button>
<sc-button icon="sc-user-line" type="primary" circle></sc-button>
<sc-button icon="sc-user-line" type="info" circle></sc-button>
<sc-button icon="sc-user-line" type="success" circle></sc-button>
<sc-button icon="sc-user-line" type="warning" circle></sc-button>
<sc-button icon="sc-user-line" type="danger" circle></sc-button>`,
            code3: `<sc-button icon="sc-user-line" type="default" disabled>Default</sc-button>
<sc-button icon="sc-user-line" type="primary" disabled>Primary</sc-button>
<sc-button icon="sc-user-line" type="info" disabled>Info</sc-button>
<sc-button icon="sc-user-line" type="success" plain disabled>Success</sc-button>
<sc-button icon="sc-user-line" type="warning" round disabled>Warning</sc-button>
<sc-button icon="sc-user-line" type="danger" circle disabled></sc-button>`,
        };
    }
}
</script>

<style scoped>
.row {
    margin-top: 1rem;
}
</style>