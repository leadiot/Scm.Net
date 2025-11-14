<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>FileImport 导入数据</h1>
                <p>FileImport 组件用于导入数据文件。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="FileImport 组件默认使用示例。">
                    <sc-file-import ref="fileImport"></sc-file-import>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';
import scFileImport from "@/components/scFileImport";

export default {
    name: 'scui_file_import',
    components: {
        scCode,
        scFileImport,
    },
    data() {
        return {
            code1: `<sc-file-import ref="fileImport"></sc-file-import>`,
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