<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>FileExport 导出数据组件</h1>
                <p>FileExport 组件用于导出数据文件。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="FileExport 组件默认使用示例。">
                    <sc-file-export ref="fileExport"></sc-file-export>
                </sc-code>

                <h2>属性</h2>
                <el-table :data="fieldTable" style="width: 100%">
                    <el-table-column prop="name" label="属性名" width="120" />
                    <el-table-column prop="desc" label="说明" />
                    <el-table-column prop="type" label="类型" width="80" />
                    <el-table-column prop="val" label="可选值" width="120" />
                    <el-table-column prop="def" label="默认值" width="120" />
                </el-table>
                <h2>事件</h2>
                <el-table :data="eventTable" style="width: 100%">
                    <el-table-column prop="name" label="事件名" width="120" />
                    <el-table-column prop="desc" label="说明" />
                    <el-table-column prop="args" label="回调参数" width="120" />
                </el-table>
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
            fieldTable: [
                { name: 'apiObj', type: 'Object', desc: '导出数据的 API 对象', val: '', def: '{}' },
                { name: 'fileName', type: 'String', desc: '导出文件名称', val: '', def: '' },
                { name: 'fileType', type: 'String', desc: '导出文件类型', val: '', def: 'xlsx' },
                { name: 'data', type: 'Object', desc: '导出数据', val: '', def: '{}' },
                { name: 'showData', type: 'Boolean', desc: '是否显示导出数据', val: '', def: 'false' },
                { name: 'async', type: 'Boolean', desc: '是否异步导出数据', val: '', def: 'false' },
                { name: 'column', type: 'Array', desc: '导出数据列配置数组', val: '', def: '[]' },
                { name: 'blob', type: 'Boolean', desc: '是否使用 Blob 导出数据', val: '', def: 'false' },
                { name: 'progress', type: 'Boolean', desc: '是否显示进度条', val: '', def: 'true' },
                { name: 'text', type: 'String', desc: '导出按钮文本', val: '', def: '导出' },
                { name: 'title', type: 'String', desc: '导出窗口标题', val: '', def: '导出' },
            ],
            eventTable: [
            ]
        };
    }
}
</script>