<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>TextViewer 文本查看组件</h1>
                <p>TextViewer 组件用于查看文本内容。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="TextViewer 组件默认使用示例。">
                    <sc-text-viewer ref="textViewer" :uri="example1.uri" />
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
import scTextViewer from '@/components/scTextViewer';

export default {
    name: 'scui_text_viewer',
    components: {
        scCode,
        scTextViewer,
    },
    data() {
        return {
            example1: {
                uri: 'https://www.baidu.com',
                code: `<sc-text-viewer ref="textViewer" :uri="uri"/>`,
            },
            fieldTable: [
                { name: 'uri', type: 'String', desc: '文本 URI', val: '', def: '' },
                // { name: 'Name', type: 'String', desc: '标题', val: '', def: '' },
                // { name: 'Data', type: 'String', desc: '数据', val: '', def: '600' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>