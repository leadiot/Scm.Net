<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button type="primary" @click="open_dialog">
					<sc-icon name="sc-plus" />
				</el-button>
				<el-button plain type="danger" :disabled="selection.length == 0" @click="delete_list">
					<sc-icon name="sc-close-circle-line" />
				</el-button>
			</div>
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="关键字" @keyup.enter="search()">
					<template #append>
						<el-button type="primary" @click="search()"><sc-icon name="sc-search" /></el-button>
					</template>
				</el-input>
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id"
				@menu-handle="menuHandle" @selection-change="selectionChange">
				<!-- 固定列-选择列 -->
				<el-table-column fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column align="center" fixed="right" label="操作" width="210">
					<template #default="scope">
						<el-button text type="primary" size="small" @click="open_dialog(scope.row)">
							编辑
						</el-button>
						<el-divider direction="vertical" />
						<el-button text type="primary" size="small" @click="open_design(scope.row)">
							设计
						</el-button>
						<el-divider direction="vertical" />
						<el-popconfirm title="确定删除吗？" @confirm="delete_item(scope.row)">
							<template #reference>
								<el-button text type="primary" size="small">删除</el-button>
							</template>
						</el-popconfirm>
					</template>
				</el-table-column>
				
				<template #row_status="scope">
					<el-tooltip :content="scope.row.row_status ? '正常' : '停用'" placement="right">
						<el-switch v-model="scope.row.row_status" :active-value="1" :inactive-value="2"
							@change="status_item($event, scope.row)">
						</el-switch>
					</el-tooltip>
				</template>

				<template #refused="{ data }">
					{{ data.refused == 1 ? "返回审批流初始层级" : "返回到上一级" }}
				</template>
			</scTable>
		</el-main>
	</el-container>
	<edit ref="edit" @complete="complete" />
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_sys_flowinfo',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		return {
			tableName: 'scm_sys_flowinfo',
			apiObj: this.$API.scmsysflowinfo.page,
			list: [],
			param: {
				key: "",
			},
			selection: [],
			column: [
				{ prop: "id", label: "id", hide: true },
				{ prop: "title", label: "流程名称", width: 150, align: "left" },
				{ prop: "icon", label: "图标", width: 100 },
				{ prop: "refused", label: "审批被拒后状态", width: 150, align: "left", showOverflowTooltip: true },
				{ prop: "row_status", label: "数据状态", width: "80", },
				{ prop: "update_names", label: "更新人员", width: "100", },
				{ prop: "update_time", label: "更新时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_names", label: "创建人员", width: "100", },
				{ prop: "create_time", label: "创建时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
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
		open_dialog(row) {
			this.$refs.edit.open(row);
			//this.$router.push({ path: '/scm/sys/workflow/design', query: { 'id': row.id } });
		},
		selectionChange(selection) {
			this.selection = selection;
		},
		menuHandle(obj) {
			if (obj.command == "add") {
				this.open_dialog({});
			}
			if (obj.command == "edit") {
				this.open_dialog(obj.row);
			}
			if (obj.command == "delete") {
				this.delete_item(obj.row);
			}
		},
		open_design(row) {
			this.$router.push({ path: '/scm/sys/workflow/design', query: { 'id': row.id } });
		},
	},
};
</script>
