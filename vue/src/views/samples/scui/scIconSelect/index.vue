<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>IconSelect 图标选择器</h1>
                <p>IconSelect 组件用于选择图标。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="IconSelect 组件默认使用示例。">
                    <sc-icon-select ref="iconSelect"></sc-icon-select>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scIconSelect from '@/components/scIconSelect';

export default {
    name: 'scui_icon_select',
    components: {
        scCode,
        scIconSelect,
    },
    data() {
        return {
            code1: `<sc-icon-select ref="iconSelect"></sc-icon-select>`,
        };
    },
    methods: {
    }
}
</script>