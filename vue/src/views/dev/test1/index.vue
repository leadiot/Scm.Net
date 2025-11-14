<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>Dialog 对话框</h1>
                <p>Dialog 组件用于显示对话框示例。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Dialog 组件默认使用示例。">
                    <sc-echarts></sc-echarts>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';
import scEcharts from "@/components/scEcharts";

export default {
    name: 'scui_button',
    components: {
        scCode,
        scEcharts,
    },
    data() {
        return {
            visible: false,
            code1: `<sc-echarts></sc-echarts>`,
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