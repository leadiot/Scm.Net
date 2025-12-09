<template>
	<el-card shadow="never" header="近7天操作记录">
		<scTable ref="table" :table-name="tableName" :data="data" :column="column" height="auto" paginationLayout="total, prev, pager, next" hideDo>
			<el-table-column label="序号" type="index" width="60"></el-table-column>
			<el-table-column label="业务名称" prop="title" min-width="240"></el-table-column>
			<el-table-column label="IP" prop="ip" width="150"></el-table-column>
			<el-table-column label="结果" prop="code" width="150">
				<el-tag type="success">成功</el-tag>
			</el-table-column>
			<el-table-column label="操作时间" prop="time" width="150"></el-table-column>
		</scTable>
	</el-card>
</template>

<script>
export default {
	data() {
		return {
			tableName: 'scm_unit_logs',
			data: [
				{
					title: "修改用户",
					ip: "211.187.11.18",
					code: "成功",
					operate_time: "2022-10-10 08:41:17"
				},
				{
					title: "用户登录",
					ip: "211.187.11.18",
					code: "成功",
					operate_time: "2022-10-10 08:21:51"
				}
			],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: "title", label: "操作类型", width: 80 },
				{ prop: "ip", label: "IP", width: 120 },
				{ prop: "operate_time", label: "操作时间", width: 150, align: 'left' },
				{ prop: "url", label: "操作地址", showOverflowTooltip: true, align: 'left' },
				{ prop: "method", label: "提交类型", width: 80 },
				{ prop: "browser", label: "浏览器", width: 100 },
				{ prop: "agent", label: "用户代理", width: 100, showOverflowTooltip: true },
				{ prop: "duration", label: "执行时长(毫秒)", width: 120 },
			]
		}
	}
}
</script>
