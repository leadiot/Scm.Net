<template>
	<sc-search ref="search" @search="search">
		<template #search>
			<el-form ref="formRef" label-width="80px" :model="param">
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
			<sc-table ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id"
				@menu-handle="menuHandle" @selection-change="selectionChange">
				<!-- 固定列-选择列 -->
				<el-table-column fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
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
			</sc-table>
		</el-main>
		<edit ref="edit" @complete="complete" />
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_ur_position',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		return {
			apiObj: this.$API.scmurposition.page,
			tableName: 'scm_ur_position',
			row_status_list: [this.$SCM.OPTION_ALL_INT],
			param: {
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ""
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ label: "岗位编号", prop: "codec", width: 120, align: "left" },
				{ label: "岗位名称", prop: "namec", minWidth: 200, align: "left" },
				{ label: "排序", prop: "od", width: "80" },
				{ prop: "row_status", label: "数据状态", width: "80", },
				{ prop: "update_names", label: "更新人员", width: "100", },
				{ prop: "update_times", label: "更新时间", width: "160" },
				{ prop: "create_names", label: "创建人员", width: "100", },
				{ prop: "create_times", label: "创建时间", width: "160" },
			],
		};
	},
	mounted() {
		this.$SCM.list_status(this.row_status_list, true);
	},
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search(key) {
			if (key != null) {
				this.param.key = key;
			}
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.scmurposition.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmurposition.status, this.selection, status);
		},
		//删除
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmurposition.delete, row);
		},
		//批量删除
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmurposition.delete, this.selection);
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
			if (obj.command == 'add') {
				this.open_dialog({})
			}
			if (obj.command == 'edit') {
				this.open_dialog(obj.row)
			}
			if (obj.command == 'delete') {
				this.delete_item(obj.row)
			}
		},
	},
};
</script>
