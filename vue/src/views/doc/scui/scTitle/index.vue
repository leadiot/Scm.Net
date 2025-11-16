<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Title 标题组件</h1>
                    <p>Title 组件用于显示标题。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Title 组件默认使用示例。">
                        <sc-title ref="title" title="这是一个标题" />
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scTitle from '@/components/scTitle';

export default {
    name: 'scui_text_viewer',
    components: {
        scCode,
        scTitle,
    },
    data() {
        return {
            example1: {
                code: `<sc-title ref="title" title="这是一个标题" />`,
            },
        };
    },
    methods: {
    }
}
</script>