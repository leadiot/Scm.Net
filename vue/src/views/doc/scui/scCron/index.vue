<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Cron 表达式</h1>
                <p>Cron 表达式用于定时任务表达式示例。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Cron 组件默认显示上下文菜单示例。">
                    <sc-cron ref="cron" v-model="myCronValue"></sc-cron>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scCron from "@/components/scCron";

export default {
    name: 'scui_cron',
    components: {
        scCode,
        scCron,
    },
    data() {
        return {
            myCronValue: '0 0 0 * * ?',
            code1: `<sc-cron ref="cron" v-model="myCronValue"></sc-cron>`,
        };
    },
    methods: {
    }
}
</script>