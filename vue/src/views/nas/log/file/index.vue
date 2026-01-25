<template>
	<sc-search ref="search" @search="search">
		<template #search>
			<el-form ref="formRef" label-width="80px" :model="param">
				<el-form-item label="终端" prop="terminal_id">
					<sc-select v-model="param.terminal_id" placeholder="请选择" :data="terminal_list" />
				</el-form-item>
				<el-form-item label="文件类型" prop="type">
					<sc-select v-model="param.type" placeholder="请选择" :data="type_list" />
				</el-form-item>
				<el-form-item label="同步操作" prop="opt">
					<sc-select v-model="param.opt" placeholder="请选择" :data="opt_list" />
				</el-form-item>
				<el-form-item label="同步方向" prop="dir">
					<sc-select v-model="param.dir" placeholder="请选择" :data="dir_list" />
				</el-form-item>
				<el-form-item label="创建时间" prop="create_time">
					<el-date-picker v-model="param.create_time" type="datetimerange" range-separator="至"
						start-placeholder="开始日期" end-placeholder="结束日期" />
				</el-form-item>
			</el-form>
		</template>
	</sc-search>
	<el-container>
		<el-header>
			<div class="left-panel"></div>
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="关键字">
					<template #append>
						<el-button type="primary" @click="search()"><sc-icon name="sc-search" /></el-button>
					</template>
				</el-input>
				<el-button @click="show_search">高级</el-button>
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id"
				@menu-handle="menuHandle" @selection-change="selectionChange">
				<el-table-column align="center" fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column label="操作" align="center" fixed="right" width="80">
					<template #default="scope">
						<el-button text type="primary" size="small" @click="open_dialog(scope.row)">
							详情
						</el-button>
					</template>
				</el-table-column>
			</scTable>
		</el-main>
		<info ref="info" @complete="complete" />
	</el-container>
</template>
<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'naslogfile',
	components: {
		info: defineAsyncComponent(() => import("./info")),
	},
	data() {
		return {
			tableName: 'nas_log_file',
			apiObj: this.$API.naslogfile.page,
			param: {
				terminal_id: this.$SCM.ID_ALL,
				type: this.$SCM.ID_ALL_INT,
				opt: this.$SCM.ID_ALL_INT,
				dir: this.$SCM.ID_ALL_INT,
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: 'terminal_id', label: '终端', width: 100, align: 'left', formatter: this.getTerminalNames },
				{ prop: 'type', label: '文件类型', width: 100, formatter: this.getTypeNames },
				{ prop: 'opt', label: '操作类型', width: 100, formatter: this.getOptNames },
				{ prop: 'dir', label: '同步方向', width: 100, formatter: this.getDirNames },
				{ prop: 'name', label: '文件名称', minWidth: 140, align: 'left' },
				{ prop: "update_time", label: "更新时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_time", label: "创建时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
			],
			terminal_list: [this.$SCM.OPTION_ALL],
			type_list: [this.$SCM.OPTION_ONE_INT],
			opt_list: [this.$SCM.OPTION_ONE_INT],
			dir_list: [this.$SCM.OPTION_ONE_INT],
		};
	},
	mounted() {
		this.$SCM.list_option(this.terminal_list, this.$API.scmurterminal.option, {}, true);
		this.$SCM.list_dic(this.type_list, 'nas_type', true);
		this.$SCM.list_dic(this.opt_list, 'nas_opt', true);
		this.$SCM.list_dic(this.dir_list, 'nas_dir', true);
	},
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.naslogfile.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.naslogfile.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.naslogfile.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.naslogfile.delete, this.selection);
		},
		show_search() {
			this.$refs.search.open(this.param.key);
		},
		open_dialog(row) {
			if (!row) {
				return;
			}
			row.terminal_names = this.getTerminalNames(row.terminal_id);
			row.type_names = this.getTypeNames(row.type);
			row.opt_names = this.getOptNames(row.opt);
			row.dir_names = this.getDirNames(row.dir);
			this.$refs.info.open(row);
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
		getTerminalNames(id) {
			return this.$SCM.get_option_names(this.terminal_list, id, '');
		},
		getTypeNames(id) {
			return this.$SCM.get_dic_names(this.type_list, id, '');
		},
		getOptNames(id) {
			return this.$SCM.get_dic_names(this.opt_list, id, '');
		},
		getDirNames(id) {
			return this.$SCM.get_dic_names(this.dir_list, id, '');
		}
	},
};
</script>
