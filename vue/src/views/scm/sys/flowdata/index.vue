<template>
	<sc-search ref="search" @search="search">
		<template #search>
			<el-form ref="formRef" label-width="80px" :model="param">
				<el-form-item label="查询选项" prop="option_id">
					<sc-select v-model="param.option_id" placeholder="请选择" :data="option_list" />
				</el-form-item>
				<el-form-item label="数据状态" prop="row_status">
					<sc-select v-model="param.row_status" placeholder="请选择" :data="row_status_list" />
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
			<div class="left-panel">
				<el-button icon="el-icon-plus" type="primary" @click="open_dialog()" />
				<el-divider direction="vertical"></el-divider>
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
				<el-divider direction="vertical"></el-divider>
				<el-radio-group v-model="param.filter">
					<el-radio-button label="我发起的" :value="1" />
					<el-radio-button label="我收到的" :value="2" />
				</el-radio-group>
			</div>
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="关键字" @keyup.enter="search()">
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
				<template #order_codes="scope">
					<el-button type="primary" plain @click="open_tab(scope.row)">{{ scope.row.order_codes }}</el-button>
				</template>
			</scTable>
		</el-main>
	</el-container>
</template>
<script>
export default {
	name: 'scm_sys_flowdata',
	data() {
		return {
			tableName: 'scm_sys_flowdata',
			apiObj: this.$API.scmsysflowdata.page,
			list: [],
			param: {
				option_id: '0',
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: '',
				filter: 1
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: 'flow_id', label: '流程信息', width: 180, align: 'left' },
				{ prop: 'order_codes', label: '单据信息', width: 180, align: 'left' },
				{ prop: 'remark', label: '审批说明', minWidth: 140, align: 'left' },
				{ prop: "update_names", label: "更新人员", width: 100, },
				{ prop: "update_time", label: "更新时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_names", label: "创建人员", width: 100, },
				{ prop: "create_time", label: "创建时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
			],
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
			this.$SCM.status_item(this, this.$API.scmsysflowdata.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmsysflowdata.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmsysflowdata.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmsysflowdata.delete, this.selection);
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
		open_tab(row) {
			if (!row.id || !row.url) {
				return;
			}

			this.$router.push({ path: row.url, query: { 'id': row.id } });
		}
	},
};
</script>
