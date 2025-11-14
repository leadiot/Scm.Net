<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Code 代码</h1>
                <p>Code 组件用于显示代码示例。</p>
                <sc-code lang="html" title="基础用法" desc="Code 组件默认显示代码示例。">
                    <el-button>测试</el-button>
                </sc-code>
            </el-card>
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
        };
    }
}
</script>