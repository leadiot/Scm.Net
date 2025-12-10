<template>
    <div class="sc-summary">
        <div v-if="checkBox" class="check">
            <el-checkbox v-model="isChecked" @change="handleCheckedChange"></el-checkbox>
        </div>
        <div v-if="image" class="thumb" :style="{ width: (imageSize + 6) + 'px', height: (imageSize + 6) + 'px' }">
            <el-avatar :size="imageSize" :src="image" />
        </div>
        <div>
            <div class="title">{{ title }}</div>
            <div class="summary" v-html="summary"></div>
        </div>
    </div>
</template>
<script>
export default {
    name: "ScSummary",
    data() {
        return {
            isChecked: false,
        }
    },
    //获取子组件传过来的激活tab
    props: {
        title: { type: String, default: "" },
        summary: { type: String, default: "" },
        image: { type: String, default: null },
        imageSize: { type: Number, default: 50 },
        checkBox: { type: Boolean, default: false },
        checked: { type: Boolean, default: false }
    },
    watch: {
        //监听从props里拿到值了
        checked() {
            this.isChecked = this.checked;
        },
    },
    created() {
    },
    methods: {
        handleCheckedChange() {
            this.$emit('update:checked', this.checked);
        }
    }
}
</script>

<style scoped>
.sc-summary {
    display: flex;
    flex-direction: row;
    align-items: center;
    width: 100%;
}

.sc-summary .thumb {
    background-color: lightgray;
    border-radius: var(--el-border-radius-base);
    margin-right: 10px;
    padding: 3px;
}

.sc-summary .check {
    margin-right: 10px;
    padding: 3px;
}

.sc-summary .title {
    color: var(--el-text-color-primary);
    font-size: var(--el-font-size-base);
    line-height: 1.3;
}

.sc-summary .summary {
    color: var(--el-text-color-regular);
    /* color: var(--el-text-color-primary); */
    font-size: var(--el-font-size-base);
    line-height: 1.3;
}
</style>