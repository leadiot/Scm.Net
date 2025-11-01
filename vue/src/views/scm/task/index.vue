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
				<el-button-group>
					<el-tooltip content="暂停">
						<el-button type="primary" plain :disabled="selection.length == 0" @click="pause_job">
							<sc-icon name="sc-pause-circle-line" />
						</el-button>
					</el-tooltip>
					<el-tooltip content="执行">
						<el-button type="primary" plain :disabled="selection.length == 0" @click="run_job">
							<sc-icon name="sc-pause-circle-line" />
						</el-button>
					</el-tooltip>
					<el-tooltip content="日志">
						<el-button type="primary" plain :disabled="selection.length == 0" @click="show_log">
							<sc-icon name="sc-close-circle-line" />
						</el-button>
					</el-tooltip>
				</el-button-group>
			</div>
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="群组名称">
					<template #append>
						<el-button type="primary" @click="search()"><sc-icon name="sc-search" /></el-button>
					</template>
				</el-input>
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :tableName="tableName" :api-obj="apiObj" :column="column" row-key="id"
				@menu-handle="menuHandle" @selection-change="selectionChange">
				<!-- 固定列-选择列 -->
				<el-table-column fixed type="selection" width="60" />
				<el-table-column align="center" fixed="right" label="操作" width="140">
					<template #default="scope">
						<el-button size="small" text type="primary" @click="open_dialog(scope.row)">
							编辑
						</el-button>
						<el-divider direction="vertical" />
						<el-popconfirm title="确定删除吗？" @confirm="delete_item(scope.row, scope.$index)">
							<template #reference>
								<el-button text :disabled="scope.row.isSystem" type="primary" size="small">删除</el-button>
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
				<template #maxLength="{ data }">
					<el-tag type="info">
						{{ data.maxLength == 0 ? "不限制" : data.maxLength }}
					</el-tag>
				</template>
			</scTable>
		</el-main>
		<edit ref="edit" @complete="complete" />
		<logs ref="logs"></logs>
	</el-container>
</template>
<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_task',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
		logs: defineAsyncComponent(() => import("./logs")),
	},
	data() {
		return {
			tableName: 'scm_task',
			apiObj: this.$API.scmsysquartz.page,
			param: {
				key: '',
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true, },
				{ prop: "names", label: "任务名称", align: "left", },
				{ prop: "group", label: "任务分组", align: "left", },
				{ prop: "types", label: "任务类型", align: "left", },
				{ prop: "status", label: "执行状态", align: "left", },
				{ prop: "remark", label: "备注", align: "left", },
				{ prop: "row_status", label: "数据状态", width: "80", },
				{ prop: "update_time", label: "更新时间", width: 160, sortable: true, },
				{ prop: "create_time", label: "创建时间", width: 160, sortable: true, },
			],
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
			this.$SCM.status_item(this, this.$API.scmsysquartz.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmsysquartz.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmsysquartz.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmsysquartz.delete, this.selection);
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
				this.delete_item(obj.row);
			}
		},
		async pause_job() {
			var row = this.selection[0];
			var res = await this.$API.scmsysquartz.pause.put(row);
			if (res) {
				if (res.code == 200 && res.data.handle) {
					this.$message.success("操作成功");
					this.init();
				} else {
					this.$alert(res.data.message, "提示", { type: "error" });
				}
			}
		},
		async run_job() {
			var row = this.selection[0];
			var res = await this.$API.scmsysquartz.run.put(row);
			if (res) {
				if (res.code == 200 && res.data.handle) {
					this.$message.success("操作成功");
					this.init();
				} else {
					this.$alert(res.data.message, "提示", { type: "error" });
				}
			}
		},
		async show_log(row) {
			this.$refs.logs.opens(row);
		},
	},
};
</script>
