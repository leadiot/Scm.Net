<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Select Filter 选择器组件</h1>
                    <p>Select Filter 组件用于选择器的展示。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Select Filter 组件默认使用示例。">
                        <sc-select-filter :data="example1.options"
                            :selectedValues="example1.selectedValues"></sc-select-filter>
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
import scSelectFilter from '@/components/scSelectFilter';

export default {
    name: 'scui_select_filter',
    components: {
        scCode,
        scSelectFilter,
    },
    data() {
        return {
            example1: {
                options: [
                    {
                        title: '选项1',
                        key: 'key1',
                        options: [
                            {
                                icon: 'sc-user-line',
                                label: '选项1-1',
                                value: '11'
                            },
                            {
                                icon: 'sc-user-line',
                                label: '选项1-2',
                                value: '12'
                            }
                        ]
                    },
                    {
                        title: '选项2',
                        key: 'key2',
                        options: [
                            {
                                icon: 'sc-user-line',
                                label: '选项2-1',
                                value: '21'
                            },
                            {
                                icon: 'sc-user-line',
                                label: '选项2-2',
                                value: '22'
                            }
                        ]
                    }
                ],
                selectedValues: {
                    'key1': ['11'],
                    'key2': ['22'],
                },
                code: `<sc-select-filter :data="options" :selectedValues="selectedValues"></sc-select-filter>`,
            },
            fieldTable: [
                { name: 'data', type: 'Array', desc: '选项列表，格式：{title: "选项组名称", key: "选项组键值", options: [{icon: "图标类名", label: "选项名称", value: "选项值"}]}', val: '', def: '[]' },
                { name: 'selectedValues', type: 'Object', desc: '选中值，格式：{key: ["value1", "value2"]}', val: '', def: '{}' },
                { name: 'labelWidth', type: 'Number', desc: '选项组标题宽度', val: '', def: '80' },
                { name: 'outputValueTypeToArray', type: 'Boolean', desc: '是否将选中值转换为数组', val: '', def: 'false' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>