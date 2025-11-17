/**
* 富文本编辑器
* 基于 quill.js 实现
*/
<template>
    <div :style="{ height: height > 30 ? (height - 80) + 'px' : 'auto', width: width > 0 ? width + 'px' : '100%' }">
        <slot name="toolbar"></slot>
        <div ref="editor">{{ modelValue }}</div>
    </div>
</template>

<script>
export default {
    name: 'scEditor',
    props: {
        modelValue: { type: String, default: '' },
        placeholder: { type: String, default: '请输入文本 ...' },
        toolbar: { type: String, default: null },
        height: { type: Number, default: 0 },
        width: { type: Number, default: 0 },
        disabled: { type: Boolean, default: false },
        readOnly: { type: Boolean, default: false },
        options: { type: Object, required: false, default: () => { } },
        globalOptions: { type: Object, required: false, default: () => { } }
    },
    data() {
        return {
            quill: null,
            editorContent: this.modelValue,
            editorOptions: {},
            defaultOptions: {
                theme: 'snow',
                placeholder: this.placeholder,
                readOnly: this.readOnly,
                modules: {
                    toolbar: {
                        container: this.toolbar,
                        controls: [
                            ['bold', 'italic', 'underline', 'strike'],
                            ['blockquote', 'code-block'],
                            [{ 'header': 1 }, { 'header': 2 }],
                            [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                            [{ 'script': 'sub' }, { 'script': 'super' }],
                            [{ 'indent': '-1' }, { 'indent': '+1' }],
                            [{ 'direction': 'rtl' }],
                            [{ 'size': ['small', false, 'large', 'huge'] }],
                            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                            [{ 'color': [] }, { 'background': [] }],
                            [{ 'font': [] }],
                            [{ 'align': [] }],
                            ['clean'],
                            ['link', 'image', 'video']
                        ],
                        // handlers: {
                        //     // handlers object will be merged with default handlers object
                        //     'new': function (val) {
                        //         alert(val);
                        //     }
                        // }
                    }
                },
            }
        };
    },
    mounted() {
        this.initialize();
    },
    beforeUnmount() {
        this.quill = null;
        delete this.quill;
    },
    watch: {
        modelValue(newVal) {
            this.editorContent = newVal;
        },
        editorContent(val) {
            this.$emit('update:modelValue', val);
        },
        disabled(newVal) {
            if (this.quill) {
                this.quill.enable(!newVal);
            }
        }
    },
    methods: {
        initialize() {
            // if (this.modelValue) {
            //     this.editorContent = this.modelValue + '\n';
            //     this.$refs.editor.innerHTML = this.editorContent;
            // }
            this.editorOptions = Object.assign({}, this.defaultOptions, this.globalOptions, this.options);

            this.quill = new window.Quill(this.$refs.editor, this.editorOptions);

            if (!this.disabled) {
                this.quill.enable(true)
            }
        }
    }
}
</script>