<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Title 标题组件</h1>
                    <p>Title 组件用于显示标题。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Title 组件默认使用示例。">
                        <sc-title ref="title" title="这是一个标题" />
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
import scTitle from '@/components/scTitle';

export default {
    name: 'scui_text_viewer',
    components: {
        scCode,
        scTitle,
    },
    data() {
        return {
            example1: {
                code: `<sc-title ref="title" title="这是一个标题" />`,
            },
            fieldTable: [
                { name: 'title', type: 'String', desc: '标题文本', val: '', def: '' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>