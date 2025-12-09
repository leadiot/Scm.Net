<template>
    <sc-panel :title="title" :remark="remark">
        <textarea v-model="text" style="height: 100%;width: 100%;" />
        <template #footer>
            <el-card shadow="hover">
                <el-descriptions style="width: 500px;">
                    <el-descriptions-item label="总计">{{ total }}</el-descriptions-item>
                    <el-descriptions-item label="数字">{{ noQty }}</el-descriptions-item>
                    <el-descriptions-item label=""></el-descriptions-item>

                    <el-descriptions-item label="英文合计">{{ enChar + enSign }}</el-descriptions-item>
                    <el-descriptions-item label="英文字符">{{ enChar }}</el-descriptions-item>
                    <el-descriptions-item label="英文符号">{{ enSign }}</el-descriptions-item>

                    <el-descriptions-item label="中文合计">{{ cnChar + cnSign }}</el-descriptions-item>
                    <el-descriptions-item label="中文字符">{{ cnChar }}</el-descriptions-item>
                    <el-descriptions-item label="中文符号">{{ cnSign }}</el-descriptions-item>
                </el-descriptions>
            </el-card>
        </template>
    </sc-panel>
</template>

<script>
export default {
    name: 'tools_text',
    data() {
        return {
            title: '字符统计',
            remark: '这是一个字符统计工具',
            text: '',
            cnChar: 0,
            cnSign: 0,
            enChar: 0,
            enSign: 0,
            noQty: 0,
            total: 0,
        };
    },
    mounted() {

    },
    methods: {
        test(val) {
            this.cnChar = 0;
            this.cnSign = 0;
            this.enChar = 0;
            this.enSign = 0;
            this.noQty = 0;
            this.total = 0;
            if (!val) {
                return;
            }

            this.total = [...val].length;
            for (var i = 0; i < val.length; i++) {
                //var c = val.charCodeAt(i);
                var c = val.charAt(i);

                // 英文
                if (c.match(/[\u0020-\u00ff]/)) {
                    if (c.match(/[a-zA-Z]/)) {
                        this.enChar++;
                        continue;
                    }
                    // 数字
                    if (c.match(/[0-9]/)) {
                        this.noQty++;
                        continue;
                    }

                    this.enSign++;
                    continue;
                }

                //基本汉字
                if (c.match(/[\u4e00-\u9fa5]/)) {
                    this.cnChar++;
                    continue;
                }
                //基本汉字补充
                if (c.match(/[\u9FA6-\u9fcb]/)) {
                    this.cnChar++;
                    continue;
                }
                if (c.match(/[。？！，、；：「」『』‘’“”（）〔〕【】—…–．《》〈〉 ]/)) {
                    this.cnSign++;
                    continue;
                }
            }
        }
    },
    watch: {
        text: {
            handler(val) {
                this.test(val);
            }
        },
    }
}
</script>