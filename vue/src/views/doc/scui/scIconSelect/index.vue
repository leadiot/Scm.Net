<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>IconSelect 图标选择器</h1>
                    <p>IconSelect 组件用于选择图标。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="IconSelect 组件默认使用示例。">
                        <sc-icon-select ref="iconSelect"></sc-icon-select>
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
import scIconSelect from '@/components/scIconSelect';

export default {
    name: 'scui_icon_select',
    components: {
        scCode,
        scIconSelect,
    },
    data() {
        return {
            example1: {
                code: `<sc-icon-select ref="iconSelect"></sc-icon-select>`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'String', desc: 'Cron 表达式', val: '', def: '* * * * * ?' },
                { name: 'shortcuts', type: 'Array', desc: '快捷选项，格式：{name: "快捷选项名称", value: "快捷选项值"}', val: '', def: '[]' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>