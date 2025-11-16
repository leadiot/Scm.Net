<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Form 动态表单</h1>
                <p>Form 组件用于动态构建表单。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Form 组件默认使用示例。">
                    <sc-dynamic-form ref="form" v-model="example1.form" :config="example1.config"></sc-dynamic-form>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scDynamicForm from "@/components/scDynamicForm";

export default {
    name: 'scui_form',
    components: {
        scCode,
        scDynamicForm,
    },
    data() {
        return {
            example1: {
                config: {
                    labelWidth: '100px',
                    labelPosition: 'left',
                    formItems:
                        [
                            { component: 'title', label: '这是标题', message: '这是message' },
                            { component: 'input', label: '用户名', tips: '这是Tips', name: 'input', message: '这是message', options: { placeholder: '这是placeholder', maxlength: 20 } },
                            { component: 'number', label: '数字选择框', tips: '这是Tips', name: 'number', message: '这是message', options: { placeholder: '这是placeholder', type: 'number', min: 0, max: 100, step: 1 } },
                            { component: 'radio', label: '单选', tips: '这是Tips', name: 'radio1', message: '这是message', options: { items: [{ label: '选项1', value: '1' }, { label: '选项2', value: '2' }, { label: '选项3', value: '3' }] } },
                            { component: 'switch', label: '开关', tips: '这是Tips', name: 'switch', message: '这是message' },
                            { component: 'checkbox', label: '多选', tips: '这是Tips', name: 'list1', message: '这是message', options: { items: [{ label: '选项1', value: '1', name: 'checkbox1' }, { label: '选项2', value: '2', name: 'checkbox2' }] } },
                            { component: 'checkboxGroup', label: '多选组合', tips: '这是Tips', name: 'list2', message: '这是message', options: { items: [{ label: '选项1', value: '1', name: 'checkboxGroup1' }, { label: '选项2', value: '2', name: 'checkboxGroup2' }, { label: '选项3', value: '3', name: 'checkboxGroup3' }] } },
                            { component: 'select', label: '选择框', tips: '这是Tips', name: 'select', message: '这是message', options: { placeholder: '这是placeholder', items: [{ label: '选项1', value: '1' }, { label: '选项2', value: '2' }, { label: '选项3', value: '3' }] } },
                            { component: 'cascader', label: '级联选择框', tips: '这是Tips', name: 'cascader', message: '这是message', options: { placeholder: '这是placeholder', items: [{ label: '选项1', value: '1' }, { label: '选项2', value: '2' }, { label: '选项3', value: '3' }] } },
                            { component: 'date', label: '日期选择框', tips: '这是Tips', name: 'date', message: '这是message', options: { placeholder: '这是placeholder', type: 'date', shortcuts: [{ text: '今天', value: new Date() }], defaultTime: new Date(), valueFormat: 'yyyy-MM-dd' } },
                            { component: 'color', label: '颜色选择框', tips: '这是Tips', name: 'color', message: '这是message', options: { placeholder: '这是placeholder', } },
                            { component: 'rate', label: '评分选择框', tips: '这是Tips', name: 'rate', message: '这是message', options: { placeholder: '这是placeholder', } },
                            { component: 'slider', label: '滑块选择框', tips: '这是Tips', name: 'slider', message: '这是message', options: { placeholder: '这是placeholder', marks: { 0: '0', 100: '100' } } },
                            { component: 'tableselect', label: '表格选择框', tips: '这是Tips', name: 'tableselect', message: '这是message', options: { placeholder: '这是placeholder', } },
                            { component: 'upload', label: '文件上传', tips: '这是Tips', name: 'files', message: '这是message', options: { items: [{ label: '文件1', name: 'file1' }, { label: '文件2', name: 'file2' }, { label: '文件3', name: 'file3' }] } },
                            { component: 'rate', label: '评分选择框', tips: '这是Tips', name: 'rate', message: '这是message', options: { placeholder: '这是placeholder', } },
                            { component: 'editor', label: '编辑器', tips: '这是Tips', name: 'editor', message: '这是message', options: { placeholder: '这是placeholder', } },
                        ]
                },
                form: {
                    input: '',
                    number: 50,
                    radio1: '1',
                    switch: false,
                    list1: [],
                    list2: [],
                    select: '1',
                    cascader: ['1', '2'],
                    date: '2023-01-01',
                    color: '#000000',
                    rate: 5,
                    slider: 50,
                    tableselect: '1',
                    editor: '这是编辑器内容',
                    file1: null,
                    file2: null,
                    file3: null,
                },
                code: `<sc-dynamic-form ref="form" v-model="form" :config="config"></sc-dynamic-form>`,
            },
        };
    },
    methods: {
    }
}
</script>