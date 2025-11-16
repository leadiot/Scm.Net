<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Dialog 对话框</h1>
                    <p>Dialog 组件用于显示对话框示例。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Dialog 组件默认使用示例。">
                        <el-button type="danger" @click="example1.visible = true">显示对话框</el-button>
                        <sc-dialog ref="dialog" v-model="example1.visible" title="窗口标题" width="480px">
                            <p>这里是您的个性化代码</p>
                            <template #footer>
                                <el-button type="default" @click="example1.visible = false">取消</el-button>
                                <el-button type="primary" @click="example1.visible = false">确认</el-button>
                            </template>
                        </sc-dialog>
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
import scDialog from "@/components/scDialog";

export default {
    name: 'scui_dialog',
    components: {
        scCode,
        scDialog,
    },
    data() {
        return {
            example1: {
                visible: false,
                code: `<sc-dialog ref="dialog" v-model="visible" title="窗口标题" width="480px">
    <p>这里是您的个性化代码</p>
    <template #footer>
        <el-button type="default" @click="visible = false">取消</el-button>
        <el-button type="primary" @click="visible = false">确认</el-button>
    </template>
</sc-dialog>`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'Boolean', desc: '对话框是否可见', val: '', def: 'false' },
                { name: 'title', type: 'String', desc: '对话框标题', val: '', def: '' },
                // { name: 'width', type: 'String', desc: '对话框宽度', val: '', def: '480px' },
                { name: 'showClose', type: 'Boolean', desc: '对话框是否显示关闭按钮', val: '', def: 'true' },
                { name: 'showFullscreen', type: 'Boolean', desc: '对话框是否显示全屏按钮', val: '', def: 'true' },
                { name: 'loading', type: 'Boolean', desc: '对话框是否显示加载状态', val: '', def: 'false' },
            ],
            eventTable: [
            ]
        };
    }
}
</script>