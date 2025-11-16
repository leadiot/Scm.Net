<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>FileSelect 选择文件</h1>
                <p>FileSelect 组件用于选择文件。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="FileSelect 组件默认使用示例。">
                    <el-button type="primary" @click="handleClick">选择文件</el-button>
                    <el-dialog title="选择文件" v-model="example1.visible" width="640px" height="480px">
                        <sc-file-select ref="fileSelect"></sc-file-select>
                    </el-dialog>
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
import scFileSelect from "@/components/scFileSelect";

export default {
    name: 'scui_file_select',
    components: {
        scCode,
        scFileSelect,
    },
    data() {
        return {
            example1: {
                visible: false,
                code: `<sc-file-select ref="fileSelect"></sc-file-select>`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'Object', desc: '选中的文件路径', val: '', def: '' },
                { name: 'hideUpload', type: 'Boolean', desc: '是否隐藏上传按钮', val: '', def: 'false' },
                { name: 'multiple', type: 'Boolean', desc: '是否支持多选', val: '', def: 'false' },
                { name: 'max', type: 'Number', desc: '最大选择文件数', val: '', def: '' },
                { name: 'onlyImage', type: 'Boolean', desc: '是否只接受图片文件', val: '', def: 'false' },
                { name: 'maxSize', type: 'Number', desc: '最大文件大小（单位：字节）', val: '', def: '' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
        handleClick() {
            this.example1.visible = true;
        }
    }
}
</script>