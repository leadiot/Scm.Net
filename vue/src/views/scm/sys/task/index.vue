<template>
	<sc-search ref="search" @search="search">
		<template #search>
			<el-form ref="formRef" label-width="80px" :model="param">
				<el-form-item label="任务类型" prop="types">
					<sc-select v-model="param.types" placeholder="请选择任务类型" :data="types_list" />
				</el-form-item>
				<el-form-item label="执行状态" prop="handle">
					<sc-select v-model="param.handle" placeholder="请选择执行状态" :data="handle_list" />
				</el-form-item>
				<el-form-item label="执行结果" prop="result">
					<sc-select v-model="param.result" placeholder="请选择执行结果" :data="result_list" />
				</el-form-item>
				<el-form-item label="数据状态" prop="row_status">
					<sc-select v-model="param.row_status" placeholder="请选择数据状态" :data="row_status_list" />
				</el-form-item>
				<el-form-item label="创建时间" prop="create_time">
					<el-date-picker v-model="param.create_time" type="datetimerange" range-separator="至"
						start-placeholder="开始日期" end-placeholder="结束日期" />
				</el-form-item>
				<el-form-item label="搜索内容">
					<el-input v-model="param.key" clearable placeholder="关键字" />
				</el-form-item>
			</el-form>
		</template>
	</sc-search>
	<el-container>
		<el-header>
			<div class="left-panel">
				<!-- <el-button type="primary" @click="open_dialog()"><sc-icon name="sc-plus"/></el-button>
	<el-divider direction="vertical"></el-divider> -->
				<el-button-group>
					<!-- <el-tooltip content="启用">
			<el-button type="primary" plain :disabled="selection.length == 0"
				@click="status_list(1)"><sc-icon name="sc-check-circle-line"/></el-button>
		</el-tooltip>
		<el-tooltip content="停用">
			<el-button type="primary" plain :disabled="selection.length == 0"
				@click="status_list(2)"><sc-icon name="sc-pause-circle-line"/></el-button>
		</el-tooltip> -->
					<el-tooltip content="删除">
						<el-button type="danger" plain :disabled="selection.length == 0" @click="delete_list"><sc-icon
								name="sc-close-circle-line" /></el-button>
					</el-tooltip>
				</el-button-group>
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :api-obj="apiObj" :column="column" row-key="id" @menu-handle="menuHandle"
				@selection-change="selectionChange">
				<el-table-column align="center" fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column label="操作" align="center" fixed="right" width="140">

					<template #default="scope">
						<el-button text type="primary" size="small" @click="open_dialog(scope.row)"
							:disabled="!canDownload(scope.row)">
							下载
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
			</scTable>
		</el-main>
		<edit ref="edit" @complete="complete" />
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_sys_task',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		return {
			apiObj: this.$API.scmsystask.page,
			list: [],
			param: {
				types: this.$SCM.ID_ALL_INT,
				handle: this.$SCM.ID_ALL_INT,
				result: this.$SCM.ID_ALL_INT,
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: 'codes', label: '系统代码', width: 160 },
				{ prop: 'names', label: '任务名称', width: 160 },
				{ prop: 'types', label: '任务类型', width: 100, formatter: this.getTypesNames },
				{ prop: 'need_time_f', label: '计划开始时间', width: 160, formatter: this.dateTimeFormat },
				{ prop: 'need_time_t', label: '计划结束时间', width: 160, formatter: this.dateTimeFormat },
				{ prop: 'exec_time_f', label: '实际开始时间', width: 160, formatter: this.dateTimeFormat },
				{ prop: 'exec_time_t', label: '实际结束时间', width: 160, formatter: this.dateTimeFormat },
				{ prop: 'handle', label: '执行状态', width: 100, formatter: this.getHandleNames },
				{ prop: 'result', label: '执行结果', width: 100, formatter: this.getResultNames },
				{ prop: 'message', label: '提示信息', align: 'left', minWidth: 160, showOverflowTooltip: true },
				{ prop: 'update_time', label: '更新时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: 'update_names', label: '更新人员', width: 100 },
				{ prop: 'create_time', label: '创建时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: 'create_names', label: '创建人员', width: 100 },
			],
			row_status_list: [this.$SCM.OPTION_ALL_INT],
			types_list: [this.$SCM.OPTION_ALL_INT],
			handle_list: [this.$SCM.OPTION_ALL_INT],
			result_list: [this.$SCM.OPTION_ALL_INT],
		};
	},
	mounted() {
		this.$SCM.list_dic(this.types_list, 'task_type', true);
		this.$SCM.list_dic(this.handle_list, 'task_handle', true);
		this.$SCM.list_dic(this.result_list, 'task_result', true);
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
			this.$SCM.status_item(this, this.$API.scmsystask.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmsystask.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmsystask.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmsystask.delete, this.selection);
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
		getTypesNames(types) {
			return this.$SCM.get_dic_names(this.types_list, types, '');
		},
		getHandleNames(handle) {
			return this.$SCM.get_dic_names(this.handle_list, handle, '');
		},
		getResultNames(result) {
			return this.$SCM.get_dic_names(this.result_list, result, '');
		},
		dateTimeFormat(time) {
			if (!time || time == '0') {
				return '';
			}
			return this.$TOOL.dateTimeFormat(time);
		},
		canDownload(row) {
			return row.types == 2 && row.result == 2 && row.file;
		},
	},
};
</script>
