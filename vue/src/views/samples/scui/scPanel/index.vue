<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Panel 面板组件</h1>
                <p>Panel 组件用于页面内容的展示。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Panel 组件默认使用示例。">
                    <sc-panel ref="panel" title="面板标题" description="这是一段描述文本">
                        <p>这是一段描述文本</p>
                    </sc-panel>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scPanel from '@/components/scPanel';

export default {
    name: 'scui_panel',
    components: {
        scCode,
        scPanel,
    },
    data() {
        return {
            data: 10,
            code1: `<sc-panel ref="panel" title="面板标题" description="这是一段描述文本">
    <p>这是一段描述文本</p>
</sc-panel>`,
        };
    },
    methods: {
    }
}
</script>