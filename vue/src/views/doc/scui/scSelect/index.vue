<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Select 选择器组件</h1>
                <p>Select 组件用于选择器的展示。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Select 组件默认使用示例。">
                    <sc-select v-model="example1.value" :options="example1.options"></sc-select>
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
import scSelect from '@/components/scSelect';

export default {
    name: 'scui_select',
    components: {
        scCode,
        scSelect,
    },
    data() {
        return {
            example1: {
                value: '1',
                options: [
                    {
                        label: '选项1',
                        value: '1'
                    },
                    {
                        label: '选项2',
                        value: '2'
                    }
                ],
                code: `<sc-select v-model="value" :options="options"></sc-select>`,
            },
            fieldTable: [
                { name: 'apiObj', type: 'Object', desc: '数据查询 API 对象', val: '', def: '{}' },
                { name: 'data', type: 'Object', desc: '选项列表，格式：{label: "选项名称", value: "选项值"}', val: '', def: '[]' },
                { name: 'params', type: 'Object', desc: '查询参数', val: '', def: '{}' },
            ],
            eventTable: [
                { name: 'change', type: 'Function', desc: '选中值改变时触发', val: '', def: '' },
            ]
        };
    },
    methods: {
    }
}
</script>