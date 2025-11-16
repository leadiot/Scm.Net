<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>FileImport 导入数据</h1>
                <p>FileImport 组件用于导入数据文件。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="FileImport 组件默认使用示例。">
                    <sc-file-import ref="fileImport"></sc-file-import>
                </sc-code>
            </el-card>
        </div>
    </div>
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
            example1: {
                code: `<sc-file-import ref="fileImport"></sc-file-import>`,
            },
        };
    }
}
</script>