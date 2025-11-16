<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>WaterMark 水印组件</h1>
                    <p>WaterMark 组件用于添加水印。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="WaterMark 组件默认使用示例。">
                        <sc-watermark ref="watermark" text="水印文字">
                            <div style="width: 100%; height: 400px;"></div>
                        </sc-watermark>
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
import scCode from '@/components/scCode';
import scWatermark from '@/components/scWatermark';

export default {
    name: 'scui_watermark',
    components: {
        scCode,
        scWatermark,
    },
    data() {
        return {
            example1: {
                code: `<sc-watermark ref="watermark" text="水印文字">
    <div style="width: 100%; height: 400px;"></div>
</sc-watermark>`,
            },
            fieldTable: [
                { name: 'text', type: 'String', desc: '水印文字', val: '', def: '' },
                { name: 'subtext', type: 'String', desc: '水印子文字', val: '', def: '' },
                { name: 'angle', type: 'Number', desc: '水印旋转角度', val: '', def: '-30' },
                { name: 'color', type: 'String', desc: '水印字体颜色', val: '', def: 'rgba(128,128,128,0.2)' },
                { name: 'fontSize', type: 'Number', desc: '水印字体大小', val: '', def: '20' },
                { name: 'subtextFontSize', type: 'Number', desc: '水印子字体大小', val: '', def: '14' }, 
                { name: 'fontFamily', type: 'String', desc: '水印字体', val: '', def: 'SimHei' },
                { name: 'subtextFontFamily', type: 'String', desc: '水印子字体', val: '', def: 'Microsoft YaHei' },
                // { name: 'fontWeight', type: 'String', desc: '水印字体粗细', val: '', def: 'normal' },
                // { name: 'fontStyle', type: 'String', desc: '水印字体样式', val: '', def: 'normal' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>