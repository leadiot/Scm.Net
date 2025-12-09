<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Status Indicator 状态指示器组件</h1>
                <p>Status Indicator 组件用于添加状态指示器。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Status Indicator 组件默认使用示例。">
                    <sc-status-indicator type="default" :pulse="example1.pulse"></sc-status-indicator>
                    <sc-status-indicator type="primary" :pulse="example1.pulse"></sc-status-indicator>
                    <sc-status-indicator type="success" :pulse="example1.pulse"></sc-status-indicator>
                    <sc-status-indicator type="info" :pulse="example1.pulse"></sc-status-indicator>
                    <sc-status-indicator type="warning" :pulse="example1.pulse"></sc-status-indicator>
                    <sc-status-indicator type="danger" :pulse="example1.pulse"></sc-status-indicator>
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
import scStatusIndicator from '@/components/scStatusIndicator';

export default {
    name: 'scui_status_indicator',
    components: {
        scCode,
        scStatusIndicator,
    },
    data() {
        return {
            example1: {
                pulse: true,
                code: `<sc-status-indicator type="default" pulse></sc-status-indicator>
<sc-status-indicator type="primary" pulse></sc-status-indicator>
<sc-status-indicator type="success" pulse></sc-status-indicator>
<sc-status-indicator type="info" pulse></sc-status-indicator>
<sc-status-indicator type="warning" pulse></sc-status-indicator>
<sc-status-indicator type="danger" pulse></sc-status-indicator>`,
            },
            fieldTable: [
                { name: 'type', type: 'String', desc: '状态类型', val: 'default, primary, success, info, warning, danger', def: 'default' },
                { name: 'pulse', type: 'Boolean', desc: '是否脉冲', val: '', def: 'false' },
                { name: 'size', type: 'Number', desc: '状态指示器大小', val: '', def: '8' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>