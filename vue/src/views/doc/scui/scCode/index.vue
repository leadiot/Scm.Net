<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Code 代码组件</h1>
                    <p>Code 组件用于显示代码示例。</p>
                    <sc-code lang="html" title="基础用法" desc="Code 组件默认显示代码示例。">
                        <el-button>测试</el-button>
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

export default {
    name: 'scui_code',
    components: {
        scCode,
    },
    data() {
        return {
            title: 'sc-code',
            desc: 'sc-code 组件使用示例',
            code1: `<sc-code title="your title" desc="your desc" code="your code">
    <el-button>测试</el-button>
</sc-code>`,
            fieldTable: [
                { name: 'title', type: 'String', desc: '标题', val: '', def: '' },
                { name: 'desc', type: 'String', desc: '描述', val: '', def: '' },
                { name: 'lang', type: 'String', desc: '代码语言', val: '', def: 'JavaScript' },
                { name: 'code', type: 'String', desc: '代码', val: '', def: '' },
                // { name: 'warning', type: 'String', desc: '警告提示', val: '', def: '' },
                // { name: 'tips', type: 'String', desc: '提示信息', val: '', def: '' },
            ],
            eventTable: [
            ]
        };
    }
}
</script>