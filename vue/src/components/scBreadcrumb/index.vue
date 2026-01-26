<template>
    <el-input v-model="path" clearable @blur="isInput = false" v-if="isInput" />
    <div class="sc-breadcrumb" @click="copyText" v-else>
        <el-button type="primary" link size="small" @click.stop="changeRoot()">{{ root.name }}</el-button>
        <div v-for="(item, index) in itemList" :key="item">
            <label>{{ separatorChar }}</label>
            <el-button type="primary" link size="small" @click.stop="changeNode(index)">{{ item.name }}</el-button>
        </div>
    </div>
</template>
<script>
export default {
    emits: ['click'],
    props: {
        separator: { type: String, default: '/' },
        root: { type: Object, default: () => ({ id: '0', name: 'nas:' }) },
        items: { type: Array, default: () => [] },
    },
    data() {
        return {
            separatorChar: this.separator,
            itemList: this.items || [],
            isInput: false,
            path: ''
        }
    },
    methods: {
        addItem(item) {
            this.itemList.push(item);
            this.showPath();
        },
        copyText() {
            //this.$copyText(this.path);
        },
        changeRoot() {
            this.itemList.splice(0, this.itemList.length);
            this.showPath();
            this.$emit('click', this.root);
        },
        changeNode(index) {
            this.itemList.splice(index + 1, this.itemList.length - index - 1);
            this.showPath();
            this.$emit('click', this.itemList[index]);
        },
        showPath() {
            this.path = this.root.name;
            this.itemList.forEach((item) => {
                this.path += this.separator + item.name;
            });
        }
    },
}
</script>
<style>
.sc-breadcrumb {
    display: flex;
    flex-direction: row;
    align-items: start;
    justify-content: flex-start;
    vertical-align: middle;
    border: 1px solid var(--el-border-color);
    background-color: var(--el-bg-color);
    padding: 4px 8px;
    border-radius: 4px;
    min-height: 32px;
    width: 100%;
}
</style>