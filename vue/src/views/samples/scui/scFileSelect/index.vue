<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>FileSelect 选择文件</h1>
                <p>FileSelect 组件用于选择文件。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="FileSelect 组件默认使用示例。">
                    <el-button type="primary" @click="handleClick">选择文件</el-button>
                    <el-dialog title="选择文件" v-model="visible" width="640px" height="480px">
                        <sc-file-select ref="fileSelect"></sc-file-select>
                    </el-dialog>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scFileSelect from "@/components/scFileSelect";

export default {
    name: 'scui_file_select',
    components: {
        scCode,
        scFileSelect,
    },
    data() {
        return {
            visible: false,
            code1: `<sc-file-select ref="fileSelect"></sc-file-select>`,
        };
    },
    methods: {
        handleClick() {
            this.visible = true;
        }
    }
}
</script>