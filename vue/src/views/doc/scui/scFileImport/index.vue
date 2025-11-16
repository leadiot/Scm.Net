<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>FileImport 导入数据组件</h1>
                <p>FileImport 组件用于导入数据文件。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="FileImport 组件默认使用示例。">
                    <sc-file-import ref="fileImport"></sc-file-import>
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
            fieldTable: [
                { name: 'apiObj', type: 'Object', desc: '导入数据的 API 对象', val: '', def: '{}' },
                { name: 'data', type: 'Object', desc: '导入数据', val: '', def: '{}' },
                { name: 'accept', type: 'String', desc: '导入文件类型', val: '', def: 'xls,xlsx' },
                { name: 'maxSize', type: 'Number', desc: '导入文件最大大小（单位：字节）', val: '', def: '10MB' },
                { name: 'maxRows', type: 'Number', desc: '导入文件最大行数', val: '', def: '2000' },
                { name: 'tip', type: 'String', desc: '导入文件提示信息', val: '', def: '' },
                { name: 'templateUrl', type: 'String', desc: '导入数据模板 URL', val: '', def: '' },
                { name: 'errorUrl', type: 'String', desc: '导入数据错误提示 URL', val: '', def: '' },
                { name: 'autoUpload', type: 'Boolean', desc: '是否自动上传导入数据', val: '', def: 'true' },
                { name: 'text', type: 'String', desc: '导入按钮文本', val: '', def: '导入' },
                { name: 'title', type: 'String', desc: '导入窗口标题', val: '', def: '导入' },
            ],
            eventTable: [
            ]
        };
    }
}
</script>