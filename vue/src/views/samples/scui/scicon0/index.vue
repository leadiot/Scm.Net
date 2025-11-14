<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Icon 组件</h1>
                <p>Icon 组件用于显示图标。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Icon 组件默认使用示例。">
                    <sc-icon name="sc-user-line"></sc-icon>
                    <sc-icon name="sc-user-line" color="red"></sc-icon>
                    <sc-icon name="sc-user-line" color="red" :size="24"></sc-icon>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';

export default {
    name: 'scui_icon',
    components: {
        scCode,
    },
    data() {
        return {
            code1: `<sc-icon name="sc-user-line"></sc-icon>
<sc-icon name="sc-user-line" color="red"></sc-icon>
<sc-icon name="sc-user-line" color="red" :size="24"></sc-icon>`,
        };
    },
    methods: {
    }
}
</script>