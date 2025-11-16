<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>UploadAvatar 上传头像组件</h1>
                <p>UploadAvatar 组件用于上传头像。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="UploadAvatar 组件默认使用示例。">
                    <sc-upload-avatar ref="uploadAvatar" />
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
import scUploadAvatar from '@/components/scUploadAvatar';

export default {
    name: 'scui_upload_avatar',
    components: {
        scCode,
        scUploadAvatar,
    },
    data() {
        return {
            example1: {
                code: `<sc-upload-avatar ref="uploadAvatar" />`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'String', desc: '上传文件路径', val: '', def: '' },
                { name: 'tip', type: 'String', desc: '上传提示', val: '', def: '' },
                { name: 'action', type: 'String', desc: '上传地址', val: '', def: '' },
                { name: 'apiObj', type: 'Object', desc: '上传接口对象', val: '', def: '{}' },
                { name: 'name', type: 'String', desc: '上传文件参数名', val: '', def: '' },
                { name: 'data', type: 'Object', desc: '上传额外参数', val: '', def: '{}' },
                { name: 'accept', type: 'String', desc: '上传文件类型', val: '', def: 'image/gif, image/jpeg, image/png' },
                { name: 'maxSize', type: 'Number', desc: '上传文件最大大小（字节）', val: '', def: '' },
                { name: 'limit', type: 'Number', desc: '上传文件数量限制', val: '', def: '' },
                { name: 'autoUpload', type: 'Boolean', desc: '是否自动上传', val: 'true/false', def: 'true' },
                { name: 'showFileList', type: 'Boolean', desc: '是否显示上传文件列表', val: 'true/false', def: 'true' },
                { name: 'drag', type: 'Boolean', desc: '是否开启拖拽上传', val: 'true/false', def: 'false' },
                { name: 'multiple', type: 'Boolean', desc: '是否开启多文件上传', val: 'true/false', def: 'false' },
                { name: 'disabled', type: 'Boolean', desc: '是否禁用上传', val: 'true/false', def: 'false' },
                { name: 'width', type: 'Number', desc: '头像宽度', val: '', def: '148' },
                { name: 'height', type: 'Number', desc: '头像高度', val: '', def: '148' },
                { name: 'title', type: 'String', desc: '上传提示', val: '', def: '' },
                { name: 'icon', type: 'String', desc: '上传图标', val: '', def: 'el-icon-plus' },
            ],
            eventTable: [
                { name: 'change', type: 'Function', desc: '头像上传成功事件', val: '', def: '' },
            ]
        };
    },
    methods: {
    }
}
</script>