<template>
	<sc-search ref="search" @search="search">
		<template #search>
			<el-form ref="formRef" label-width="80px" :model="param">
				<el-form-item label="所属用户" prop="user_id">
					<sc-select v-model="param.user_id" placeholder="请选择" :data="user_list" />
				</el-form-item>
				<el-form-item label="反馈类型" prop="types">
					<sc-select v-model="param.types" placeholder="请选择" :data="types_list" />
				</el-form-item>
				<el-form-item label="数据状态" prop="row_status">
					<sc-select v-model="param.row_status" placeholder="请选择" :data="row_status_list" />
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
				<el-button icon="el-icon-plus" type="primary" @click="open_dialog()" />
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id"
				@menu-handle="menuHandle" @selection-change="selectionChange">
				<el-table-column align="center" fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column label="操作" align="center" fixed="right" width="70">

					<template #default="scope">
						<el-button text type="primary" size="small" @click="open_newtab(scope.row)">
							详情
						</el-button>
					</template>
				</el-table-column>
			</scTable>
		</el-main>
	</el-container>
</template>

<script>
export default {
	name: 'scm_mgr_feedback',
	data() {
		return {
			tableName: 'mgr_feedback',
			apiObj: this.$API.scmadmfeedback.page,
			param: {
				user_id: '0',
				types: this.$SCM.ID_ALL_INT,
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: 'user_id', label: '用户', width: 100, formatter: this.getUserNames },
				{ prop: 'types', label: '反馈类型', width: 100, formatter: this.getTypesNames },
				{ prop: 'title', label: '标题', width: 100 },
				{ prop: 'remark', label: '内容', width: 100 },
				{ prop: 'handle', label: '处理状态', width: 100 },
				{ prop: 'resolve', label: '解决状态', width: 100 },
				{ prop: 'row_status', label: '数据状态', width: 100 },
				{ prop: 'update_time', label: '更新时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: 'update_names', label: '更新人员', width: 100 },
				{ prop: 'create_time', label: '创建时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: 'create_names', label: '创建人员', width: 100 },
			],
			row_status_list: [this.$SCM.OPTION_ALL_INT],
			types_list: [this.$SCM.OPTION_ALL_INT],
			user_list: [],
		};
	},
	mounted() {
		this.$SCM.list_status(this.row_status_list, true);
		this.$SCM.list_dic(this.types_list, 'feedback_type', true);
		this.$SCM.list_option(this.user_list, this.$API.scmmgrunit.option, 0, true);
	},
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		open_newtab(row) {
			if (!row.id) {
				return;
			}
			this.$router.push({ path: '/scm/adm/feedback/detail', query: { 'id': row.id } });
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
		getUserNames(userId) {
			return this.$SCM.get_option_names(this.user_list, userId, '');
		},
		getTypesNames(types) {
			return this.$SCM.get_dic_names(this.types_list, types, '');
		},
		show_search() {
			this.$refs.search.open(this.param.key);
		}
	},
};
</script>
