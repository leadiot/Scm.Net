<template>
    <i :class="getScClass()" :style="{ 'fontSize': size + 'px', 'color': color }" v-if="isSc()"></i>
    <i :class="getMsClass()" :style="{ 'fontSize': size + 'px', 'color': color }" v-else-if="isMs()">{{ getMsName()
        }}</i>
    <i :class="getMiClass()" :style="{ 'fontSize': size + 'px', 'color': color }" v-else-if="isMi()">{{ getMiName()
        }}</i>
    <i :class="getMdiClass()" :style="{ 'fontSize': size + 'px', 'color': color }" v-else-if="isMdi()"></i>
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
            set_list: ['vs', 'sc', 'mi', 'ms', 'mdi'],//图标系列列表
        }
    },
    methods: {
        /**
         * 判断是否为Scm.Net图标
         * @returns {boolean} 是否为Scm.Net图标
         */
        isSc() {
            var tmp = this.name.toLowerCase();
            return tmp.startsWith('sc-');
        },
        isMs() {
            var tmp = this.name.toLowerCase();
            return tmp.startsWith('ms-');
        },
        isMi() {
            var tmp = this.name.toLowerCase();
            return tmp.startsWith('mi-');
        },
        isMdi() {
            var tmp = this.name.toLowerCase();
            return tmp.startsWith('mdi-');
        },
        /**
         * 获取Scm.Net图标类名
         * @returns {string} 图标类名
         */
        getScClass() {
            return 'scicon scfont ' + this.name;
        },
        /**
         * 获取Material Symbols图标类名
         * @returns {string} 图标类名
         */
        getMsClass() {
            return 'material-symbols-' + (this.type || 'outlined').toLowerCase();
        },
        /**
         * 获取Material Icons图标类名
         * @returns {string} 图标类名
         */
        getMiClass() {
            return 'material-icons-' + (this.type || 'outlined').toLowerCase();
        },
        /**
         * 获取Material Design Icons图标类名
         * @returns {string} 图标类名
         */
        getMdiClass() {
            return 'mdi ' + this.name;
        },
        /**
         * 获取Material Symbols图标名称
         * @returns {string} 图标名称
         */
        getMsName() {
            return this.name.substring(3);
        },
        /**
         * 获取Material Icons图标名称
         * @returns {string} 图标名称
         */
        getMiName() {
            return this.name.substring(3);
        },
        /**
         * 获取Material Design Icons图标名称
         * @returns {string} 图标名称
         */
        getMdiName() {
            return '';
        },
    }
}
</script>
