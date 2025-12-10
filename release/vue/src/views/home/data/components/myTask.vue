<template>
    <el-table :data="tasks" stripe style="width: 100%">
        <el-table-column prop="types" label="类型" width="180" />
        <el-table-column prop="title" label="标题"/>
        <el-table-column prop="crate_names" label="创建人员" width="120" />
        <el-table-column prop="crate_names" label="创建时间" width="160" :formatter="formatTime" />
        <el-table-column prop="crate_names" label="操作" width="80" />
    </el-table>
</template>

<script>
export default {
    props: {
        title: { type: String, default: "我的卡片" },
    },
    data() {
        return {
            tasks: [
                { types: '系统任务', title: '我的待办', create_name: 'admin', create_time: '1691048413727' },
                { types: '邮件通知', title: '我的待办', create_name: 'admin', create_time: '1691048413727' },
                { types: '下单', title: '我的待办', create_name: 'admin', create_time: '1691048413727' },
                { types: '采购', title: '我的待办', create_name: 'admin', create_time: '1691048413727' },
            ]
        }
    },
    methods: {
        formatTime(row) {
            return this.$TOOL.dateTimeFormat(row.create_time);
        }
    }
}
</script>

<style></style>