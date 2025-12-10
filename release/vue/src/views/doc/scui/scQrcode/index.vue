<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>QrCode 二维码组件</h1>
                <p>QrCode 二维码组件用于二维码的展示。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Qrcode 组件默认使用示例。">
                    <sc-qrcode text="https://www.baidu.com" :size="200"></sc-qrcode>
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
</template>
<script>
import scCode from '@/components/scCode';
import scQrcode from '@/components/scQrcode';

export default {
    name: 'scui_qrcode',
    components: {
        scCode,
        scQrcode,
    },
    data() {
        return {
            example1: {
                code: `<sc-qrcode text="https://www.baidu.com" :size="200"></sc-qrcode>`,
            },
            fieldTable: [
                { name: 'text', type: 'String', desc: '二维码内容', val: '', def: '' },
                { name: 'size', type: 'Number', desc: '二维码大小', val: '', def: '100' },
                { name: 'logo', type: 'String', desc: '二维码图标', val: '', def: '' },
                { name: 'logoSize', type: 'Number', desc: '二维码图标大小', val: '', def: '30' },
                { name: 'logoPadding', type: 'Number', desc: '二维码图标间距', val: '', def: '5' },
                { name: 'colorDark', type: 'String', desc: '二维码深色颜色', val: '', def: '#000000' },
                { name: 'colorLight', type: 'String', desc: '二维码浅色颜色', val: '', def: '#ffffff' },
                { name: 'correctLevel', type: 'Number', desc: '二维码纠错级别', val: '', def: '2' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>