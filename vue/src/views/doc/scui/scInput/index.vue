<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Input 输入框</h1>
                    <p>Input 组件用于输入文本。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Input 组件默认使用示例。">
                        <sc-input ref="input" :data="example1.list"></sc-input>
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
import scInput from '@/components/scInput';

export default {
    name: 'scui_input',
    components: {
        scCode,
        scInput,
    },
    data() {
        return {
            example1: {
                list: [{ value: 'vue', link: 'https://github.com/vuejs/vue' }, { value: '22', link: 'https://github.com/vuejs/vue' }],
                code: `<sc-input ref="input" :data="list"></sc-input>`,
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