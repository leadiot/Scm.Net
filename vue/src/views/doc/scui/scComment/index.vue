<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Comment 评论</h1>
                    <p>Comment 评论用于显示评论示例。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Comment 组件默认显示评论示例。">
                        <sc-comment code="appId"></sc-comment>
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
import scComment from '@/components/scComment';

export default {
    name: 'scui_comment',
    components: {
        scCode,
        scComment,
    },
    data() {
        return {
            example1: {
                code: `<sc-comment code="yourCode" showReply="false"></sc-comment>`,
            },
            fieldTable: [
                { name: 'code', type: 'String', desc: '功能代码，用于展示指定功能的评论内容', val: '', def: '' },
                { name: 'showReply', type: 'Boolean', desc: '是否显示回复按钮', val: '', def: 'false' },
            ],
            eventTable: [
            ]
        };
    }
}
</script>