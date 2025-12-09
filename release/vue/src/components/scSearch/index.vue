<template>
    <el-drawer v-model="drawer" title="查询">
        <div class="sc_search">
            <slot name="search">
                <el-form ref="formRef" label-width="80px">
                    <el-form-item label="搜索内容">
                        <el-input v-model="key" clearable placeholder="关键字" />
                    </el-form-item>
                </el-form>
            </slot>
        </div>
        <template #footer>
            <el-button type="primary" @click="doSearch">
                <sc-icon name="sc-search" />查询
            </el-button>
        </template>
    </el-drawer>
</template>

<script>
export default {
    emits: ['search'],
    data() {
        return {
            key: '',
            drawer: false
        }
    },
    methods: {
        doSearch() {
            this.$emit('search', this.key);
        },
        open(key) {
            this.key = key;
            this.drawer = true;
        }
    }
}
</script>

<style scoped>
.sc_search {
    padding: 0px var(--el-drawer-padding-primary);
}
</style>