<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
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
                { name: 'modelValue', type: 'String', desc: '上传头像路径', val: '', def: '' },
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