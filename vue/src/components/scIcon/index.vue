<template>
    <i :class="getScClass()" :style="{ 'fontSize': size + 'px', 'color': color }" v-if="isSc()"></i>
    <i :class="getMsClass()" :style="{ 'fontSize': size + 'px', 'color': color }" v-else>{{ getMsName() }}</i>
</template>

<script>
export default {
    props: {
        name: { type: String, default: '' },//图标名称
        size: { type: Number, default: 16 },//图标大小
        color: { type: String, default: '' },//颜色
        title: { type: String, default: '' },//提示
        type: { type: String, default: 'Outlined' },//风格：Filled、Outlined、Round、Sharp
    },
    data() {
        return {
            set_list: ['vs', 'sc', 'ms', 'mi'],//图标系列列表
        }
    },
    methods: {
        isSc() {
            var tmp = this.name.toLowerCase();
            return tmp.startsWith('sc-');
        },
        getScClass() {
            return 'scicon scfont ' + this.name;
        },
        getMsClass() {
            return 'material-symbols-' + (this.type || 'outlined').toLowerCase();
        },
        getMsName() {
            return this.name.substring(3);
        },
        getMiClass() {
            return 'material-icons-' + (this.type || 'outlined').toLowerCase();
        }
    }
}
</script>
