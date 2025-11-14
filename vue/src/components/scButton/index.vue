<template>
    <el-button v-bind="$attrs">
        <sc-icon v-if="icon" :name="icon" :size="size"></sc-icon>
        <slot />
    </el-button>
</template>

<script>
export default {
    props: {
        icon: { type: String, default: null },
        size: { type: Number, default: 32 }
    },
    data() {
        return {
        }
    }
}
</script>
