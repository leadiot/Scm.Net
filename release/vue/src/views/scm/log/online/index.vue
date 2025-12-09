<template>
	<el-container>
		<el-main class="nopadding">
			<scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" :hidePagination="true"
				:hide-context-menu="false" row-key="id">
				<!-- 固定列-选择列 -->
				<el-table-column fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column align="center" fixed="right" label="操作" width="140">
					<template #default="scope">
						<el-popconfirm title="确定踢出当前登录的用户吗？" @confirm="kickout(scope.row, scope.$index)">
							<template #reference>
								<el-button text type="primary" size="small">踢出</el-button>
							</template>
						</el-popconfirm>
					</template>
				</el-table-column>
			</scTable>
		</el-main>
	</el-container>
</template>

<script>
import socket from "@/utils/socket"

export default {
	name: 'scm_log_online',
	data() {
		return {
			tableName: 'scm_log_online',
			apiObj: this.$API.scmlogonline.list,
			param: {
				key: "",
			},
			selection: [],
			connection: null,
			column: [
				{ label: "id", prop: "id", width: "200", sortable: true, hide: true },
				{ label: "用户名", prop: "name", width: 200, },
				{ label: "连接编号", prop: "connectionId", align: "left", },
				{ label: "登录时间", prop: "time", width: "180", sortable: true, },
			],
		};
	},
	created() {
		this.init();
	},
	methods: {
		init() {
			socket.open_wss("ReceiveKickout", (out, user) => {
				const userInfo = this.$TOOL.data.get("USER_INFO");
				if (userInfo != null && out == "out" && userInfo.id == user) {
					this.$TOOL.data.clear();
					this.$router.replace({
						path: "/login",
					});
				}
			});
		},
		async kickout(row) {
			const userInfo = this.$TOOL.data.get("USER_INFO");
			if (userInfo.id == row.id) {
				this.$alert("自己不能踢自己", "提示", { type: "error" });
				return;
			}
			await socket.send_msg("SendKickOut", row.id);
			const that = this;
			setTimeout(() => {
				that.$refs.table.refresh();
			}, 1000);
		},
	},
};
</script>
