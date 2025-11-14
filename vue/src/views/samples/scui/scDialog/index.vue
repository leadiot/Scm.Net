<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>Dialog 对话框</h1>
                <p>Dialog 组件用于显示对话框示例。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Dialog 组件默认使用示例。">
                    <el-button type="danger" @click="visible = true">显示对话框</el-button>
                    <sc-dialog ref="dialog" v-model="visible" title="窗口标题" width="480px">
                        <p>这里是您的个性化代码</p>
                        <template #footer>
                            <el-button type="default" @click="visible = false">取消</el-button>
                            <el-button type="primary" @click="visible = false">确认</el-button>
                        </template>
                    </sc-dialog>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';
import scDialog from "@/components/scDialog";

export default {
    name: 'scui_button',
    components: {
        scCode,
        scDialog,
    },
    data() {
        return {
            visible: false,
            code1: `<sc-dialog ref="dialog" v-model="visible" title="窗口标题" width="480px">
    <p>这里是您的个性化代码</p>
    <template #footer>
        <el-button type="default" @click="visible = false">取消</el-button>
        <el-button type="primary" @click="visible = false">确认</el-button>
    </template>
</sc-dialog>`,
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