<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>PageHeader 页面标题</h1>
                    <p>PageHeader 组件用于页面标题。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Page Header 组件默认使用示例。">
                        <sc-page-header ref="pageHeader" icon="sc-user-line" title="页面标题" description="这是一段描述文本" />
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
import scPageHeader from '@/components/scPageHeader';

export default {
    name: 'scui_page_header',
    components: {
        scCode,
        scPageHeader,
    },
    data() {
        return {
            example1: {
                code: `<sc-page-header ref="pageHeader"
icon="sc-user-line"
title="页面标题"
description="这是一段描述文本"
/>`,
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