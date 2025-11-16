<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Panel 面板组件</h1>
                    <p>Panel 组件用于页面内容的展示。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Panel 组件默认使用示例。">
                        <sc-panel ref="panel" title="面板标题" description="这是一段描述文本">
                            <p>这是一段描述文本</p>
                        </sc-panel>
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
import scPanel from '@/components/scPanel';

export default {
    name: 'scui_panel',
    components: {
        scCode,
        scPanel,
    },
    data() {
        return {
            example1: {
                code: `<sc-panel ref="panel" title="面板标题" description="这是一段描述文本">
    <p>这是一段描述文本</p>
</sc-panel>`,
            },
            fieldTable: [
                { name: 'title', type: 'String', desc: '面板标题', val: '', def: '' },
                { name: 'description', type: 'String', desc: '面板描述', val: '', def: '' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>