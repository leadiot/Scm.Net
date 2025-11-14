<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>PageHeader 页面标题</h1>
                <p>PageHeader 组件用于页面标题。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Page Header 组件默认使用示例。">
                    <sc-page-header ref="pageHeader" icon="sc-user-line" title="页面标题" description="这是一段描述文本" />
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
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
            data: 10,
            code1: `<sc-page-header ref="pageHeader"
icon="sc-user-line"
title="页面标题"
description="这是一段描述文本"
/>`,
        };
    },
    methods: {
    }
}
</script>

<style scoped>
.el-card {
    max-width: 960px;
    margin: 0 auto;
}
</style>