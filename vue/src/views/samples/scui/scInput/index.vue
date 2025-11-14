<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Input 输入框</h1>
                <p>Input 组件用于输入文本。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Input 组件默认使用示例。">
                    <sc-input ref="input" :data="list"></sc-input>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scInput from '@/components/scInput';

export default {
    name: 'scui_icon_select',
    components: {
        scCode,
        scInput,
    },
    data() {
        return {
            list: [{ value: 'vue', link: 'https://github.com/vuejs/vue' }, { value: '22', link: 'https://github.com/vuejs/vue' }],
            code1: `<sc-icon-select ref="iconSelect"></sc-icon-select>`,
        };
    },
    methods: {
    }
}
</script>