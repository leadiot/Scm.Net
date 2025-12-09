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
import 'highlight.js/styles/default.css';
// import hljs from 'highlight.js/lib/common';
import hljs from 'highlight.js/lib/core';
import css from 'highlight.js/lib/languages/css';
import json from 'highlight.js/lib/languages/json';
import sql from 'highlight.js/lib/languages/sql';
import xml from 'highlight.js/lib/languages/xml';

hljs.registerLanguage('css', css);
hljs.registerLanguage('json', json);
hljs.registerLanguage('sql', sql);
hljs.registerLanguage('xml', xml);

import vkbeautify from "vkbeautify";

export default {
    name: 'tools_code',
    data() {
        return {
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
    methods: {
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
            this.code = hljs.highlight(temp, { language: this.lang }).value;
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
    background-color: #f5f5f5;
    border: 1px solid #e5e5e5;
}
</style>