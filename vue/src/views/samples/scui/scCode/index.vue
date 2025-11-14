<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>Code 代码</h1>
                <p>Code 组件用于显示代码示例。</p>
                <sc-code lang="html" title="基础用法" desc="Code 组件默认显示代码示例。">
                    <el-button>测试</el-button>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';

export default {
    name: 'scui_button',
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
        };
    }
}
</script>

<style scoped>
.el-card {
    max-width: 960px;
    margin: 0 auto;
}
</style>