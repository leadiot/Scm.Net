<template>
    <el-container>
        <el-header>
            <div class="left-panel">
                <span>语言：</span>
                <sc-select v-model="lang" :data="lang_list" style="width: 120px;" />
            </div>
            <div class="right-panel">
                <el-button type="primary" @click="format">格式化</el-button>
            </div>
        </el-header>
        <el-main>
            <div class="main-panel">
                <div class="editor">
                    <textarea v-model="input" />
                </div>
                <div class="format">
                    <pre><code class="hljs" :class="lang" v-html="code"></code></pre>
                </div>
            </div>
        </el-main>
    </el-container>
</template>
<script>
import vkbeautify from "vkbeautify";

export default {
    name: 'tools_code',
    data() {
        return {
            hljs: null,
            lang: 'json',
            input: '',
            code: '',
            lang_list: [
                // { label: '纯文本', value: 'plaintext', },
                // { label: 'C', value: 'c', },
                // { label: 'C++', value: 'cpp', },
                // { label: 'C#', value: 'c#', },
                { label: 'CSS', value: 'css', },
                // { label: 'F#', value: 'fsharp', },
                // { label: 'Go', value: 'go', },
                // { label: 'Gradle', value: 'gradle', },
                // { label: 'INI', value: 'ini', },
                // { label: 'Java', value: 'java', },
                { label: 'JSON', value: 'json', },
                // { label: 'Kotlin', value: 'kotlin', },
                // { label: 'Lua', value: 'lua', },
                // { label: 'Makefile', value: 'makefile', },
                // { label: 'Matlab', value: 'matlab', },
                // { label: 'Perl', value: 'perl', },
                // { label: 'PHP', value: 'php', },
                // { label: 'Python', value: 'python', },
                // { label: 'Ruby', value: 'ruby', },
                // { label: 'Rust', value: 'rust', },
                // { label: 'Shell', value: 'shell', },
                { label: 'SQL', value: 'sql', },
                // { label: 'Swift', value: 'swift', },
                // { label: 'Tcl', value: 'tcl', },
                // { label: 'Vbscript', value: 'vbscript', },
                { label: 'XML', value: 'xml', },
            ],
            isString: false,
        };
    },
    mounted() {
        this.loadHighlight();
    },
    methods: {
        async loadHighlight() {
            try {
                // 动态加载highlight.js
                const modual = await import('highlight.js/lib/core');
                console.log(modual);

                this.hljs = modual.default;

                // 按需加载需要的语言
                const css = await import('highlight.js/lib/languages/css');
                const json = await import('highlight.js/lib/languages/json');
                const sql = await import('highlight.js/lib/languages/sql');
                const xml = await import('highlight.js/lib/languages/xml');

                // 注册语言
                this.hljs.registerLanguage('css', css.default);
                this.hljs.registerLanguage('json', json.default);
                this.hljs.registerLanguage('sql', sql.default);
                this.hljs.registerLanguage('xml', xml.default);

                // 加载样式（可选）
                import('highlight.js/styles/github.css');

                // 使用highlight方法
                // const codeBlocks = document.querySelectorAll('pre code');
                // codeBlocks.forEach(block => {
                //     hljs.highlightElement(block);
                // });
            } catch (error) {
                console.error('Failed to load highlight.js:', error);
            }
        },
        format() {
            if (!this.input) {
                this.code = '';
                return;
            }

            var temp = this.input;
            try {
                if (this.lang == 'json') {
                    temp = this.jsonFormat(temp);
                } else if (this.lang == 'xml') {
                    temp = vkbeautify.xml(temp);
                } else if (this.lang == 'sql') {
                    temp = vkbeautify.sql(temp);
                } else if (this.lang == 'css') {
                    temp = vkbeautify.css(temp);
                }
            } catch (e) {
                this.isString = true;
            }

            //this.code = hljs.highlightAuto(this.code).value;
            this.code = this.hljs.highlight(temp, { language: this.lang }).value;
        },
        jsonFormat(jsonTemp) {
            // stringify 时需指定缩进否则不会显示换行。为了防止传入的string没有指定 在此统一执行一遍
            if (typeof jsonTemp == 'string') {
                jsonTemp = JSON.parse(jsonTemp);
            }

            var json = JSON.stringify(jsonTemp, undefined, 2);

            let jsonObj = JSON.parse(json);
            if (typeof jsonObj === "object") {
                this.isString = false;
                return vkbeautify.json(jsonTemp);
            } else {
                this.isString = true;
                return jsonTemp;
            }
        }
    }
}
</script>
<style scoped>
.main-panel {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    gap: 1rem;
    width: 100%;
    height: 100%;
}

.editor {
    height: 100%;
    width: 50%;

    textarea {
        height: 100%;
        width: 100%;
        padding: 0.5rem;
        border: 1px solid #e5e5e5;
    }
}

.format {
    height: 100%;
    width: 50%;
    background-color: #ffffff;
    border: 1px solid #e5e5e5;
    overflow: auto;
}
</style>