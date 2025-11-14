<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>TextViewer 文本查看组件</h1>
                    <p>TextViewer 组件用于查看文本内容。</p>
                    <sc-code lang="html" :code="code1" title="基础用法" desc="TextViewer 组件默认使用示例。">
                        <sc-text-viewer ref="textViewer" :uri="uri" />
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scTextViewer from '@/components/scTextViewer';

export default {
    name: 'scui_text_viewer',
    components: {
        scCode,
        scTextViewer,
    },
    data() {
        return {
            uri: 'https://www.baidu.com',
            code1: `<sc-text-viewer ref="textViewer" :uri="uri"/>`,
        };
    },
    methods: {
    }
}
</script>