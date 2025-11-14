<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Select Filter 选择器组件</h1>
                <p>Select Filter 组件用于选择器的展示。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Select Filter 组件默认使用示例。">
                    <sc-select-filter :data="options" :selectedValues="selectedValues"></sc-select-filter>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scSelectFilter from '@/components/scSelectFilter';

export default {
    name: 'scui_select_filter',
    components: {
        scCode,
        scSelectFilter,
    },
    data() {
        return {
            options: [
                {
                    title: '选项1',
                    key: 'key1',
                    options: [
                        {
                            icon: 'sc-user-line',
                            label: '选项1-1',
                            value: '11'
                        },
                        {
                            icon: 'sc-user-line',
                            label: '选项1-2',
                            value: '12'
                        }
                    ]
                },
                {
                    title: '选项2',
                    key: 'key2',
                    options: [
                        {
                            icon: 'sc-user-line',
                            label: '选项2-1',
                            value: '21'
                        },
                        {
                            icon: 'sc-user-line',
                            label: '选项2-2',
                            value: '22'
                        }
                    ]
                }
            ],
            selectedValues: {
                'key1': ['11'],
                'key2': ['22'],
            },
            code1: `<sc-select v-model="value" :options="options"></sc-select>`,
        };
    },
    methods: {
    }
}
</script>