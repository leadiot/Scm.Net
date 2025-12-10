<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Cropper 图片裁剪组件</h1>
                <p>Cropper 组件用于图片裁剪示例。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Cropper 组件默认使用示例。">
                    <sc-cropper ref="cropper" src="/img/logo.png"></sc-cropper>
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
import scCropper from "@/components/scCropper";

export default {
    name: 'scui_cropper',
    components: {
        scCode,
        scCropper,
    },
    data() {
        return {
            example1: {
                code: `<sc-cropper ref="cropper" src="/img/logo.png"></sc-cropper>`,
            },
            fieldTable: [
                { name: 'src', type: 'String', desc: '图片路径', val: '', def: '' },
                { name: 'compress', type: 'Number', desc: '压缩比例', val: '', def: '1' },
                { name: 'aspectRatio', type: 'Number', desc: '裁剪比例', val: '', def: '' },
            ],
            eventTable: [
            ]
        };
    }
}
</script>