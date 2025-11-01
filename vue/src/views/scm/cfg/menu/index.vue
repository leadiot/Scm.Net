<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button-group>
					<el-tooltip content="启用">
						<el-button type="primary" icon="el-icon-circle-check" plain :disabled="selection.length == 0"
							@click="status_list(1)"></el-button>
					</el-tooltip>
					<el-tooltip content="停用">
						<el-button type="primary" icon="el-icon-circle-close" plain :disabled="selection.length == 0"
							@click="status_list(2)"></el-button>
					</el-tooltip>
					<el-tooltip content="删除">
						<el-button type="danger" icon="el-icon-delete" plain :disabled="selection.length == 0"
							@click="delete_list"></el-button>
					</el-tooltip>
				</el-button-group>
			</div>
			<div class="right-panel">
				<div class="right-panel-search">
					<el-input v-model="param.key" clearable placeholder="关键字" />
					<el-button icon="el-icon-search" type="primary" @click="search" />
				</div>
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :api-obj="apiObj" :column="column" row-key="id" @menu-handle="menuHandle"
				@selection-change="selectionChange">
				<el-table-column align="center" fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column label="操作" align="center" fixed="right" width="80">
					<template #default="scope">
						<el-popconfirm title="确定删除吗？" @confirm="table_del(scope.row, scope.$index)">
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
			</scTable>
		</el-main>
		<edit ref="edit" @complete="complete" />
	</el-container>
</template>
<script>
import { defineAsyncComponent } from "vue";
export default {
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		return {
			apiObj: this.$API.scmcfgmenu.page,
			list: [],
			param: {
				option_id: '0',
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: 'namec', label: '展示名称', minWidth: 140 },
				{ prop: 'od', label: '显示排序', width: 80 },
				{ prop: "row_status", label: "数据状态", width: 80, },
				{ prop: "update_names", label: "更新人员", width: 100, },
				{ prop: "update_time", label: "更新时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_names", label: "创建人员", width: 100, },
				{ prop: "create_time", label: "创建时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
			],
			row_status_list: [this.$SCM.OPTION_ALL],
			option_list: [this.$SCM.OPTION_ALL],
		};
	},
	mounted() {
	},
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.scmcfgmenu.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmcfgmenu.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmcfgmenu.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmcfgmenu.delete, this.selection);
		},
		selectionChange(selection) {
			this.selection = selection;
		},
		menuHandle(obj) {
			if (obj.command == "add") {
				this.open_dialog();
				return;
			}
			if (obj.command == "delete") {
				this.delete_item(obj.row);
				return;
			}
		},
	},
};
</script>
