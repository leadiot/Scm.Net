<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>FileExport 导出数据</h1>
                <p>FileExport 组件用于导出数据文件。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="FileExport 组件默认使用示例。">
                    <sc-file-export ref="fileExport"></sc-file-export>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';
import scFileExport from "@/components/scFileExport";

export default {
    name: 'scui_file_export',
    components: {
        scCode,
        scFileExport,
    },
    data() {
        return {
            code1: `<sc-file-export ref="fileExport"></sc-file-export>`,
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