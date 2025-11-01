<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button type="primary" @click="open_header"><sc-icon name="sc-plus" /></el-button>
				<el-button type="success" plain :disabled="selection.length == 0" @click="show_info">
					<sc-icon name="sc-info" />
				</el-button>
				<el-button type="danger" plain :disabled="selection.length == 0" @click="delete_list">
					<sc-icon name="sc-close-circle-line" />
				</el-button>
			</div>
			<div class="right-panel">
				<div class="right-panel-search">
					<el-input v-model="param.key" clearable placeholder="关键字" />
					<el-button type="primary" @click="search"><sc-icon name="sc-search" /></el-button>
				</div>
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" :menu-column="menuColumn" row-key="id"
				@selection-change="selectionChange" @menu-handle="menuHandle">
				<!-- 固定列-选择列 -->
				<el-table-column fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column align="center" fixed="right" label="操作" width="210">
					<template #default="scope">
						<el-button size="small" text type="primary" @click="open_header(scope.row)">
							编辑
						</el-button>
						<el-divider direction="vertical" />
						<el-button size="small" text type="primary" @click="open_detail(scope.row)">
							明细
						</el-button>
						<el-divider direction="vertical" />
						<el-popconfirm title="确定删除吗？" @confirm="delete_item(scope.row)">
							<template #reference>
								<el-button text type="primary" size="small">删除</el-button>
							</template>
						</el-popconfirm>
					</template>
				</el-table-column>

				<template #status="{ data }">
					<el-tag disable-transitions :type="data.status ? 'success' : 'danger'">
						{{ data.status ? "正常" : "停用" }}
					</el-tag>
				</template>

				<template #type="{ data }">
					{{ data.type ? "单选" : "多选" }}
				</template>
			</scTable>
		</el-main>
		<voteHeader ref="voteHeader" @complete="complete" />
		<voteDetail ref="voteDetail" />
		<info ref="info" />
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_sys_vote',
	components: {
		voteHeader: defineAsyncComponent(() => import("./header")),
		voteDetail: defineAsyncComponent(() => import("./detail")),
		info: defineAsyncComponent(() => import("./info")),
	},
	data() {
		return {
			tableName: 'scm_sys_vote',
			apiObj: this.$API.scmsysvoteheader.page,
			param: {
				key: "",
			},
			selection: [],
			column: [
				{ prop: "id", label: "id", hide: true },
				{ prop: "title", label: "投票标题", minWidth: 200, align: 'left' },
				{ prop: "type", label: "投票类型", width: 100 },
				{ prop: "start_time", label: "开始时间", width: 160 },
				{ prop: "end_time", label: "结束时间", width: 160 },
				{ prop: "row_status", label: "数据状态", width: "80", },
				{ prop: "update_names", label: "更新人员", width: "100", },
				{ prop: "update_time", label: "更新时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_names", label: "创建人员", width: "100", },
				{ prop: "create_time", label: "创建时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
			],
			menuColumn: [
				{
					command: "look",
					title: "查看",
					icon: "sc-eye-view",
					suffix: "Look",
				}
			],
		};
	},
	mounted() { },
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.scmuruser.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmuruser.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmuruser.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmuruser.delete, this.selection);
		},
		open_header(row) {
			if (row.id) {
				this.$refs.voteHeader.open(row);
			} else {
				this.$refs.voteHeader.open();
			}
		},
		open_detail(row) {
			this.$refs.voteDetail.open(row);
		},
		open_info(row) {
			this.$refs.info.open(row);
		},
		show_info() {
			if (this.selection.length != 1) {
				return;
			}
			this.open_info(this.selection[0]);
		},
		selectionChange(selection) {
			this.selection = selection;
		},
		menuHandle(obj) {
			if (obj.command == "add") {
				this.open_header({});
			}
			if (obj.command == "edit") {
				this.open_header(obj.row);
			}
			if (obj.command == "delete") {
				this.delete_item(obj.row);
			}
			if (obj.command == "look") {
				this.open_info(obj.row);
			}
		},
	},
};
</script>
