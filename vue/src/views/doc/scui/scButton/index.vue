<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Button 按钮</h1>
                    <p>sc-button 组件默认用法</p>
                    <sc-code lang="html" :code="example1.code" title="默认用法" desc="默认用法">
                        <sc-button>Default</sc-button>
                        <sc-button type="primary">Primary</sc-button>
                        <sc-button type="info">Info</sc-button>
                        <sc-button type="success">Success</sc-button>
                        <sc-button type="warning">Warning</sc-button>
                        <sc-button type="danger">Danger</sc-button>
                    </sc-code>

                    <sc-code lang="html" :code="example2.code" title="外观风格" desc="使用plain、round、circle属性">
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

                    <sc-code lang="html" :code="example3.code" title="禁用状态" desc="使用disabled属性">
                        <sc-button icon="sc-user-line" type="default" disabled>Default</sc-button>
                        <sc-button icon="sc-user-line" type="primary" disabled>Primary</sc-button>
                        <sc-button icon="sc-user-line" type="info" disabled>Info</sc-button>
                        <sc-button icon="sc-user-line" type="success" plain disabled>Success</sc-button>
                        <sc-button icon="sc-user-line" type="warning" round disabled>Warning</sc-button>
                        <sc-button icon="sc-user-line" type="danger" circle disabled></sc-button>
                    </sc-code>

                    <h2>属性</h2>
                    <el-table :data="fieldTable" style="width: 100%">
                        <el-table-column prop="name" label="属性名" width="120" />
                        <el-table-column prop="desc" label="说明" />
                        <el-table-column prop="type" label="类型" width="80" />
                        <el-table-column prop="val" label="可选值" width="120" />
                        <el-table-column prop="def" label="默认值" width="120" />
                    </el-table>
                    <h2>事件</h2>
                    <el-table :data="eventTable" style="width: 100%">
                        <el-table-column prop="name" label="事件名" width="120" />
                        <el-table-column prop="desc" label="说明" />
                        <el-table-column prop="args" label="回调参数" width="120" />
                    </el-table>
                </el-card>
            </div>
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
            example1: {
                code: `<sc-button>Default</sc-button>
<sc-button type="primary">Primary</sc-button>
<sc-button type="info">Info</sc-button>
<sc-button type="success">Success</sc-button>
<sc-button type="warning">Warning</sc-button>
<sc-button type="danger">Danger</sc-button>`,
            },
            example2: {
                code: `<sc-button plain>Default</sc-button>
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
            },
            example3: {
                code: `<sc-button icon="sc-user-line" type="default" disabled>Default</sc-button>
<sc-button icon="sc-user-line" type="primary" disabled>Primary</sc-button>
<sc-button icon="sc-user-line" type="info" disabled>Info</sc-button>
<sc-button icon="sc-user-line" type="success" plain disabled>Success</sc-button>
<sc-button icon="sc-user-line" type="warning" round disabled>Warning</sc-button>
<sc-button icon="sc-user-line" type="danger" circle disabled></sc-button>`,
            },
            fieldTable: [
                { name: 'icon', type: 'String', desc: '图标', val: '', def: '' },
                { name: 'size', type: 'Number', desc: '图标大小', val: '', def: '32' },
            ],
            eventTable: [
            ]
        };
    }
}
</script>

<style scoped>
.row {
    margin-top: 1rem;
}
</style>