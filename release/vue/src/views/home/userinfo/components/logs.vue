<template>
	<el-card shadow="never" header="近7天操作记录">
		<scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" height="auto"
			paginationLayout="total, prev, pager, next">
			<el-table-column label="#" type="index" width="50"></el-table-column>
			<template #result="scope">
				<el-tag type="success" v-if="scope.row.result">成功</el-tag>
				<el-tag type="danger" v-else>失败</el-tag>
			</template>
		</scTable>
	</el-card>
</template>

<script>
export default {
	name: 'scm_user_logs',
	data() {
		return {
			tableName: 'scm_user_logs',
			apiObj: this.$API.scmloguser.page,
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: "client_names", label: "终端类型", width: 80 },
				{ prop: "type_names", label: "登录类型", width: 80 },
				{ prop: "mode_names", label: "登录方式", width: 80 },
				{ prop: "time", label: "操作时间", width: 160, align: 'left', formatter: this.$TOOL.dateTimeFormat },
				{ prop: "ip", label: "IP", width: 140, align: 'right' },
				{ prop: "result", label: "执行结果", width: 80 },
				{ prop: "remark", label: "说明", minWidth: 160, align: 'left' },
			]
		}
	},
	mounted() {
	},
	methods: {
		getTag(result) {
			return result ? 'success' : 'error';
		}
	}
}
</script>
