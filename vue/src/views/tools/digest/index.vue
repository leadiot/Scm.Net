<template>
    <sc-panel :title="title" :remark="remark">
        <textarea v-model="param.input" style="width: 100%;height:100%;"></textarea>
        <template #footer>
            <el-card shadow="hover">
                <el-row style="margin-bottom: 15px;">
                    <el-col :span="4">
                        <sc-select v-model="param.alg" :data="algList"></sc-select>
                    </el-col>
                    <el-col :span="20" style="text-align: right;">
                        <el-radio-group v-model="param.letter" v-show="param.binary == 16">
                            <el-radio-button :value="lower">小写</el-radio-button>
                            <el-radio-button :value="upper">大写</el-radio-button>
                        </el-radio-group>
                        <sc-select v-model="param.sep" :data="sepList" style="margin: 0px 10px;width: 100px;"
                            v-show="param.binary == 16"></sc-select>
                        <el-radio-group v-model="param.binary">
                            <el-radio-button :value="16">16进制</el-radio-button>
                            <el-radio-button :value="64">64进制</el-radio-button>
                        </el-radio-group>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col>
                        <el-input v-model="result" placeholder="摘要结果" readonly>
                            <template #append>
                                <el-button type="primary" @click="digest">计算</el-button>
                            </template>
                        </el-input>
                    </el-col>
                </el-row>
            </el-card>
        </template>
    </sc-panel>
</template>

<script>
import CryptoJS from "crypto-js";
export default {
    name: 'tools_digest',
    data() {
        return {
            title: '消息摘要',
            remark: '这是一个文本摘要工具',
            algList: [
                { id: 'MD5', label: 'MD5', value: 'MD5' },
                { id: 'SHA1', label: 'SHA1', value: 'SHA1' },
                { id: 'SHA2-224', label: 'SHA2-224', value: 'SHA2-224' },
                { id: 'SHA2-256', label: 'SHA2-256', value: 'SHA2-256' },
                { id: 'SHA2-384', label: 'SHA2-384', value: 'SHA2-384' },
                { id: 'SHA2-512', label: 'SHA2-512', value: 'SHA2-512' },
                { id: 'SHA3-224', label: 'SHA3-224', value: 'SHA3-224' },
                { id: 'SHA3-256', label: 'SHA3-256', value: 'SHA3-256' },
                { id: 'SHA3-384', label: 'SHA3-384', value: 'SHA3-384' },
                { id: 'SHA3-512', label: 'SHA3-512', value: 'SHA3-512' },
                { id: 'RIPEMD160', label: 'RIPEMD160', value: 'RIPEMD160' },],
            sepList: [
                { id: '1', label: '无', value: '0' },
                { id: '2', label: '空格( )', value: ' ' },
                { id: '3', label: '冒号(:)', value: ':' },
                { id: '4', label: '中横(-)', value: '-' }],
            param: {
                input: '',
                alg: 'MD5',
                binary: 16,
                sep: '0',
                letter: 'lower',
            },
            result: ''
        };
    },
    mounted() {

    },
    methods: {
        digest() {
            if (!this.param.input) {
                this.result = '';
                return;
            }

            var hash = '';
            switch (this.param.alg) {
                case 'MD5':
                    hash = CryptoJS.MD5(this.param.input);
                    break;
                case 'SHA1':
                    hash = CryptoJS.SHA1(this.param.input);
                    break;
                case 'SHA2-224':
                    hash = CryptoJS.SHA256(this.param.input);
                    break;
                case 'SHA2-256':
                    hash = CryptoJS.SHA256(this.param.input);
                    break;
                case 'SHA2-384':
                    hash = CryptoJS.SHA384(this.param.input);
                    break;
                case 'SHA2-512':
                    hash = CryptoJS.SHA512(this.param.input);
                    break;
                case 'SHA3-224':
                    hash = CryptoJS.SHA3(this.param.input, { outputLength: 224 });
                    break;
                case 'SHA3-256':
                    hash = CryptoJS.SHA3(this.param.input, { outputLength: 256 });
                    break;
                case 'SHA3-384':
                    hash = CryptoJS.SHA3(this.param.input, { outputLength: 384 });
                    break;
                case 'SHA3-512':
                    hash = CryptoJS.SHA3(this.param.input, { outputLength: 512 });
                    break;
                case 'RIPEMD':
                    hash = CryptoJS.RIPEMD160(this.param.input);
                    break;
            }

            if (this.param.binary == 64) {
                this.result = hash.toString(CryptoJS.enc.Base64);
                return;
            }

            var map = this.param.letter == 'upper'
                ? ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F']
                : ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];
            var words = hash.words;
            var sigBytes = words.length;
            var chars = [];
            for (var i = 0; i < sigBytes; i += 1) {
                var word = words[i];
                var tmp = map[(word >>> 28) & 0xf] + map[(word >>> 24) & 0xf];
                chars.push(tmp);

                tmp = map[(word >>> 20) & 0xf] + map[(word >>> 16) & 0xf];
                chars.push(tmp);

                tmp = map[(word >>> 12) & 0xf] + map[(word >>> 8) & 0xf];
                chars.push(tmp);

                tmp = map[(word >>> 4) & 0xf] + map[(word) & 0xf];
                chars.push(tmp);
            }

            var sep = this.param.sep == '0' ? '' : this.param.sep;
            this.result = chars.join(sep);
        }
    },
}
</script>

<style>
.el-radio-group {
    font-size: inherit;
}
</style>