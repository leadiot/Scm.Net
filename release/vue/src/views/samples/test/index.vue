<template>
    <sc-list :data="list">
    </sc-list>
</template>
<script>
export default {
    data() {
        return {
            list: [{ id: 1, label: '红色', value: '1' },
            { id: 2, label: '黄色', value: '2' },
            { id: 3, label: '蓝色', value: '3' }
            ]
        }
    },
    mounted() { },
    methods: {
        async myShow() {
            //this.$TOOL.showNotify({ title: '测试' });
            this.$TOOL.showTitle({ title: '这是一个很长很长的测试，Hello！' })
        },
    },
};
</script>
