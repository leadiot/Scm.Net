<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Summary 统计组件</h1>
                    <p>Summary 组件用于展示统计数据。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Summary 组件默认使用示例。">
                        <sc-summary ref="summary" image="/img/logo.png" title="统计" summary="这是一个统计组件"></sc-summary>
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
import scSummary from '@/components/scSummary';

export default {
    name: 'scui_summary',
    components: {
        scCode,
        scSummary,
    },
    data() {
        return {
            example1: {
                code: `<sc-summary ref="summary" image="/img/logo.png" title="统计" summary="这是一个统计组件"></sc-summary>`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'String', desc: '统计值', val: '', def: '' },
                { name: 'image', type: 'String', desc: '统计图标', val: '', def: '' },
                { name: 'title', type: 'String', desc: '统计标题', val: '', def: '' },
                { name: 'summary', type: 'String', desc: '统计描述', val: '', def: '' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>