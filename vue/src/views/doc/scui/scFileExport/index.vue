<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>FileExport 导出数据</h1>
                <p>FileExport 组件用于导出数据文件。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="FileExport 组件默认使用示例。">
                    <sc-file-export ref="fileExport"></sc-file-export>
                </sc-code>
            </el-card>
        </div>
    </div>
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
            example1: {
                code: `<sc-file-export ref="fileExport"></sc-file-export>`,
            },
        };
    }
}
</script>