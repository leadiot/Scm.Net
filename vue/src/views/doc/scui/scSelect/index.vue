<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Select 选择器组件</h1>
                <p>Select 组件用于选择器的展示。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Select 组件默认使用示例。">
                    <sc-select v-model="example1.value" :options="example1.options"></sc-select>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scSelect from '@/components/scSelect';

export default {
    name: 'scui_select',
    components: {
        scCode,
        scSelect,
    },
    data() {
        return {
            example1: {
                value: '1',
                options: [
                    {
                        label: '选项1',
                        value: '1'
                    },
                    {
                        label: '选项2',
                        value: '2'
                    }
                ],
                code: `<sc-select v-model="value" :options="options"></sc-select>`,
            }
        };
    },
    methods: {
    }
}
</script>