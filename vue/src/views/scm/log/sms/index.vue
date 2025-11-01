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
				<el-form-item label="搜索内容">
					<el-input v-model="param.key" clearable placeholder="关键字" />
				</el-form-item>
			</el-form>
		</template>
	</sc-search>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button type="primary" @click="open_dialog()"><sc-icon name="sc-plus" /></el-button>
				<el-divider direction="vertical"></el-divider>
				<el-button-group>
					<el-tooltip content="启用">
						<el-button type="primary" plain :disabled="selection.length == 0"
							@click="status_list(1)"><sc-icon name="sc-check-circle-line" /></el-button>
					</el-tooltip>
					<el-tooltip content="停用">
						<el-button type="primary" plain :disabled="selection.length == 0"
							@click="status_list(2)"><sc-icon name="sc-pause-circle-line" /></el-button>
					</el-tooltip>
					<el-tooltip content="删除">
						<el-button type="danger" plain :disabled="selection.length == 0" @click="delete_list"><sc-icon
								name="sc-close-circle-line" /></el-button>
					</el-tooltip>
				</el-button-group>
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
			<scTable ref="table" :tableName="tableName" :api-obj="apiObj" :column="column" row-key="id" @menu-handle="menuHandle"
				@selection-change="selectionChange">
				<el-table-column align="center" fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<template #text="scope">
					<el-button @click="showSms(scope.row.sms_id)">
						{{ scope.row.sms_code }}
					</el-button>
				</template>
				<el-table-column label="操作" align="center" fixed="right" width="140">
					<template #default="scope">
						<el-button text type="primary" size="small" @click="open_dialog(scope.row)">
							编辑
						</el-button>
						<el-divider direction="vertical" />
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
		<smsView ref="view" />
	</el-container>
</template>
<script>
import { defineAsyncComponent } from "vue";
export default {
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
		smsView: defineAsyncComponent(() => import("./view")),
	},
	data() {
		return {
			tableName: 'scm_log_sms',
			apiObj: this.$API.scmlogsms.page,
			param: {
				option_id: 0,
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			column: [
				{ prop: "id", label: "id", hide: true },
				{ prop: 'text', label: '消息模板', width: 100 },
				{ prop: 'types', label: '终端类型', width: 100 },
				{ prop: 'code', label: '终端号码', minWidth: 160 },
				{ prop: 'sms', label: '校验码', width: 80 },
				{ prop: 'send_qty', label: '发送次数', width: 80 },
				{ prop: 'send_time', label: '发送时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: 'expired', label: '过期时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: 'handle', label: '发送状态', width: 100 },
				{ prop: 'row_status', label: '数据状态', width: 80 },
				{ prop: 'update_names', label: '更新人员', width: 100 },
				{ prop: 'update_time', label: '更新时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: 'create_names', label: '创建人员', width: 100 },
				{ prop: 'create_time', label: '创建时间', width: 160, formatter: this.$TOOL.dateTimeFormat },
			],
			row_status_list: [this.$SCM.OPTION_ALL_INT],
			option_list: [],
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
			this.$SCM.status_item(this, this.$API.scmlogsms.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmlogsms.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmlogsms.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmlogsms.delete, this.selection);
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
		showSms(id) {
			this.$refs.view.open(id);
		}
	},
};
</script>
