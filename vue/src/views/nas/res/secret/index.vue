<template>
	<el-container>
		<el-header>
			<scBreadcrum ref="breadcrumb" style="margin-right: 10px;" :root="root" @click="changeDir" />
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="关键字">
					<template #append>
						<el-button type="primary" @click="search()"><sc-icon name="sc-search" /></el-button>
					</template>
				</el-input>
				<el-button icon="el-icon-plus" type="primary" @click="open_dialog()" />
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id"
				@menu-handle="menuHandle" @selection-change="selectionChange">
				<el-table-column align="center" fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column label="操作" align="center" fixed="right" width="140">
					<template #default="scope">
						<el-button text type="primary" size="small" @click="open_dialog(scope.row)">
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
				<template #name="scope">
					<el-button type="text" link @click="open(scope.row)">
						{{ scope.row.name }}
					</el-button>
				</template>
			</scTable>
		</el-main>
		<edit ref="edit" @complete="complete" />
		<info ref="info" @complete="complete" />
	</el-container>
</template>
<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'nasfiledoc',
	components: {
		scBreadcrum: defineAsyncComponent(() => import("@/components/scBreadcrumb")),
		edit: defineAsyncComponent(() => import("./edit")),
		info: defineAsyncComponent(() => import("./info")),
	},
	data() {
		return {
			tableName: 'nas_res_file',
			apiObj: this.$API.nasressecret.page,
			param: {
				dir_id: '0',
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: 'name', label: '名称', minWidth: 100, align: 'left' },
				{ prop: 'size', label: '大小', width: 120, align: 'right', formatter: this.$TOOL.fileSizeFormat },
				{ prop: "update_times", label: "更新时间", width: 160 },
			],
			root: { 'id': '0', type: 10, name: '设备' },
			row_status_list: [this.$SCM.OPTION_ALL_INT],
			option_list: [this.$SCM.OPTION_ALL],
		};
	},
	mounted() {
		this.$SCM.list_status(this.row_status_list, true);
	},
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.nasressecret.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.nasressecret.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.nasressecret.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.nasressecret.delete, this.selection);
		},
		show_search() {
			this.$refs.search.open(this.param.key);
		},
		open_dialog(row) {
			this.$refs.edit.open(row);
		},
		selectionChange(selection) {
			this.selection = selection;
		},
		menuHandle(obj) {
			if (obj.command == "add") {
				this.open_dialog();
				return;
			}
			if (obj.command == "edit") {
				this.open_dialog(obj.row);
				return;
			}
			if (obj.command == "delete") {
				this.delete_item(obj.row);
				return;
			}
		},
		open(row) {
			if (!row) {
				return;
			}

			if (row.type == 10) {
				this.$refs.breadcrumb.addItem(row);
				this.param.dir_id = row.id;
				this.search();
				return;
			}

			alert(row.type);
			//window.open(row.url);
		},
		changeDir(item) {
			if (!item) {
				item = this.root;
			}

			this.param.dir_id = item.id;
			this.search();
		},
	},
};
</script>
