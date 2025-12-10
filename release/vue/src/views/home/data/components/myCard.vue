<template>
    <el-card>
        <el-statistic :value="number">
            <template #title>
                <div style="display: inline-flex; align-items: center">
                    {{ title }}
                    <el-tooltip effect="dark" :content="tooltip" placement="top">
                        <el-icon style="margin-left: 4px" :size="12">
                            <Warning />
                        </el-icon>
                    </el-tooltip>
                </div>
            </template>
        </el-statistic>
        <div class="statistic-footer">
            <div class="footer-item" v-if="!!rate">
                <span>{{ rate.title }}</span>
                <span class="green" v-if="rate.value < 0">
                    {{ rate.value }} %
                    <el-icon>
                        <CaretBottom />
                    </el-icon>
                </span>
                <span class="red" v-else-if="rate.value > 0">
                    {{ rate.value }} {{ rate.units }}
                    <el-icon>
                        <CaretTop />
                    </el-icon>
                </span>
            </div>
        </div>
    </el-card>
</template>

<script>
import {
    ArrowRight,
    CaretBottom,
    CaretTop,
    Warning,
} from '@element-plus/icons-vue'

export default {
    components: {
        ArrowRight,
        CaretBottom,
        CaretTop,
        Warning
    },
    props: {
        title: { type: String, default: "我的卡片" },
        tooltip: { type: String, default: "我的卡片11" },
        number: { type: Number, default: 0 },
        rate: { type: Object, default: () => { } },
        rate0: { type: Object, default: () => { } },
        rate1: { type: Object, default: () => { } },
        rate2: { type: Object, default: () => { } },
        rate3: { type: Object, default: () => { } },
    },
    data() {
        return {
        }
    },
}
</script>

<style>
:global(h2#card-usage ~ .example .example-showcase) {
    background-color: var(--el-fill-color) !important;
}

.el-statistic {
    --el-statistic-content-font-size: 28px;
}

.statistic-card {
    height: 100%;
    padding: 20px;
    border-radius: 4px;
    background-color: var(--el-bg-color-overlay);
}

.statistic-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
    font-size: 12px;
    color: var(--el-text-color-regular);
    margin-top: 16px;
}

.statistic-footer .footer-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.statistic-footer .footer-item span:last-child {
    display: inline-flex;
    align-items: center;
    margin-left: 4px;
}

.green {
    color: var(--el-color-success);
}

.red {
    color: var(--el-color-error);
}
</style>