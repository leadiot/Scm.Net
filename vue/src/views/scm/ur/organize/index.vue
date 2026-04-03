<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button type="primary" @click="open_dialog()"><sc-icon name="sc-plus" /></el-button>
				<el-divider direction="vertical"></el-divider>
				<el-button-group>
					<el-tooltip content="启用">
						<el-button type="primary" plain :disabled="selection.length == 0" @click="status_list(1)">
							<sc-icon name="sc-check-circle-line" />
						</el-button>
					</el-tooltip>
					<el-tooltip content="停用">
						<el-button type="primary" plain :disabled="selection.length == 0" @click="status_list(2)">
							<sc-icon name="sc-pause-circle-line" />
						</el-button>
					</el-tooltip>
					<el-tooltip content="删除">
						<el-button type="danger" plain :disabled="selection.length == 0" @click="delete_list">
							<sc-icon name="sc-close-circle-line" />
						</el-button>
					</el-tooltip>
				</el-button-group>
				<el-divider direction="vertical"></el-divider>
				<el-button type="primary" @click="open_owner()" :disabled="selection.length != 1">修改负责人</el-button>
			</div>
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="关键字" @keyup.enter="search()">
					<template #append>
						<el-button type="primary" @click="search"><sc-icon name="sc-search" /></el-button>
					</template>
				</el-input>
			</div>
		</el-header>
		<el-main class="nopadding">
			<sc-table ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" hide-pagination is-tree
				row-key="id" @menu-handle="menuHandle" @selection-change="selectionChange">
				<el-table-column fixed type="selection" width="60" />
				<el-table-column align="center" fixed="right" label="操作" width="140">

					<template #default="scope">
						<el-button size="small" text type="primary" @click="open_dialog(scope.row)">
							编辑
						</el-button>
						<el-divider direction="vertical" />
						<el-popconfirm title="确定删除吗？" @confirm="delete_item(scope.row, scope.$index)">
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

				<template #isSystem="{ data }">
					<el-tag :type="data.isSystem ? 'success' : 'danger'">
						{{ data.isSystem ? "是" : "否" }}
					</el-tag>
				</template>
			</sc-table>
		</el-main>
	</el-container>
	<edit ref="edit" @complete="complete" />
	<editOwner ref="editOwner" @complete="complete" />
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_ur_organize',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
		editOwner: defineAsyncComponent(() => import("./editOwner")),
	},
	data() {
		return {
			tableName: 'scm_ur_organize',
			apiObj: this.$API.scmurorganize.list,
			param: {
				key: "",
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: "namec", label: "组织名称", minWidth: "200", align: "left" },
				{ prop: "codec", label: "组织编码", width: "100" },
				{ prop: "owner_names", label: "负责人", width: "100", align: "left" },
				{ prop: "od", label: "排序", width: "80" },
				{ prop: "row_status", label: "数据状态", width: "80", },
				{ prop: "update_names", label: "更新人员", width: "100", },
				{ prop: "update_times", label: "更新时间", width: "160" },
				{ prop: "create_names", label: "创建人员", width: "100", },
				{ prop: "create_times", label: "创建时间", width: "160" },
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
			this.$SCM.status_item(this, this.$API.scmurorganize.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmurorganize.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmurorganize.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmurorganize.delete, this.selection);
		},
		open_dialog(row) {
			this.$refs.edit.open(row);
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
				this.table_del(obj.row);
			}
		},
		open_owner() {
			if (this.selection.length != 1) {
				return;
			}

			this.$refs.editOwner.open(this.selection[0]);
		},
	},
};
</script>