<template>
	<sc-search ref="search" @search="search">
		<template #search>
			<el-form ref="formRef" label-width="80px" :model="param">
				<el-form-item label="单位类型" prop="types">
					<sc-select v-model="param.types" placeholder="请选择" :data="types_list" />
				</el-form-item>
				<el-form-item label="单位模式" prop="modes">
					<sc-select v-model="param.modes" placeholder="请选择" :data="modes_list" />
				</el-form-item>
				<el-form-item label="单位制式" prop="kinds">
					<sc-select v-model="param.kinds" placeholder="请选择" :data="kinds_list" />
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
				<el-button type="danger" icon="el-icon-delete" plain :disabled="selection.length != 1"
					@click="show_refer"></el-button>
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
	</el-container>
	<edit ref="edit" @complete="complete" />
	<editRefer ref="editRefer" @complete="complete" />
</template>
<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_sys_uom',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
		editRefer: defineAsyncComponent(() => import("./editRefer")),
	},
	data() {
		return {
			tableName: 'scm_sys_uom',
			apiObj: this.$API.scmsysuom.page,
			param: {
				types: this.$SCM.ID_ALL_INT,
				modes: this.$SCM.ID_ALL_INT,
				kinds: this.$SCM.ID_ALL_INT,
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: 'od', label: '显示排序', width: 80 },
				{ prop: 'lang', label: '显示语言', width: 100, formatter: this.getLangNames },
				{ prop: 'codec', label: '单位编码', width: 100, align: 'left' },
				{ prop: 'namec', label: '单位名称', minWidth: 140, align: 'left' },
				{ prop: 'symbol', label: '单位符号', width: 100 },
				{ prop: 'types', label: '单位类型', width: 80, formatter: this.getTypesNames },
				{ prop: 'modes', label: '单位模式', width: 80, formatter: this.getModesNames },
				{ prop: 'kinds', label: '单位制式', width: 80, formatter: this.getKindsNames },
				{ prop: 'refer_names', label: '参照单位', width: 140, align: 'left' },
				{ prop: 'refer_qty', label: '参照数量', width: 100, align: 'right' },
				{ prop: "row_status", label: "数据状态", width: 80, },
				{ prop: "update_names", label: "更新人员", width: 100, },
				{ prop: "update_time", label: "更新时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_names", label: "创建人员", width: 100, },
				{ prop: "create_time", label: "创建时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
			],
			row_status_list: [this.$SCM.OPTION_ALL_INT],
			types_list: [this.$SCM.OPTION_ALL_INT],
			modes_list: [this.$SCM.OPTION_ALL_INT],
			kinds_list: [this.$SCM.OPTION_ALL_INT],
			lang_list: [this.$SCM.OPTION_ALL_INT],
		};
	},
	mounted() {
		this.$SCM.list_status(this.row_status_list, true);
		this.$SCM.list_dic(this.types_list, 'uom_types', true);
		this.$SCM.list_dic(this.modes_list, 'uom_modes', true);
		this.$SCM.list_dic(this.kinds_list, 'uom_kinds', true);
		this.$SCM.list_dic(this.lang_list, 'lang', true);
	},
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.scmsysuom.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmsysuom.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmsysuom.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmsysuom.delete, this.selection);
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
		getModesNames(modes) {
			return this.$SCM.get_dic_names(this.modes_list, modes, '');
		},
		getKindsNames(kinds) {
			return this.$SCM.get_dic_names(this.kinds_list, kinds, '');
		},
		getLangNames(lang) {
			return this.$SCM.get_dic_names(this.lang_list, lang, '');
		},
		show_refer() {
			if (this.selection.length != 1) {
				return;
			}

			this.$refs.editRefer.open(this.selection[0]);
		}
	},
};
</script>
