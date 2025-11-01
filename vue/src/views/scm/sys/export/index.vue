<template>
	<el-container>
		<el-aside width="260px" v-loading="showGrouploading">
			<el-container>
				<el-header>
					<el-input placeholder="输入关键字进行过滤" v-model="groupFilterText" clearable></el-input>
					<el-button type="primary" round icon="el-icon-plus" class="add-column" @click="edit"></el-button>
				</el-header>
				<el-main class="nopadding">
					<el-tree ref="group" class="menu" node-key="id" default-expand-all :data="group"
						:filter-node-method="groupFilterNode" @node-click="groupClick">
						<template #default="{ node, data }">
							<span class="scmui-item-node">
								<span class="label">{{ node.label }}</span>
								<span class="code">{{ data.code }}</span>
								<span class="opt">
									<el-icon @click.stop="edit(data)"><el-icon-edit /></el-icon>
									<el-icon @click.stop="remove(node, data)"><el-icon-delete /></el-icon>
								</span>
							</span>
						</template>
					</el-tree>
				</el-main>
			</el-container>
		</el-aside>

		<el-container>
			<el-header>
				<div class="left-panel">
					<el-button icon="el-icon-plus" type="primary" :disabled="!selectColumn.id" @click="open_dialog" />
					<el-divider direction="vertical"></el-divider>
					<el-button-group>
						<el-tooltip content="启用">
							<el-button type="primary" icon="el-icon-circle-check" plain
								:disabled="selection.length == 0" @click="status_list(1)"></el-button>
						</el-tooltip>
						<el-tooltip content="停用">
							<el-button type="primary" icon="el-icon-circle-close" plain
								:disabled="selection.length == 0" @click="status_list(2)"></el-button>
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
						<el-button type="primary" @click="search"><sc-icon name="sc-search" /></el-button>
					</div>
				</div>
			</el-header>
			<el-main class="nopadding">
				<scTable :tableName="tableName" ref="table" :api-obj="apiObj" :column="column" :params="defaultParam"
					row-key="id" @menu-handle="menuHandle" @selection-change="selectionChange">
					<!-- 固定列-选择列 -->
					<el-table-column fixed type="selection" width="60" align="center" />
					<el-table-column label="#" type="index" width="60"></el-table-column>
					<el-table-column align="center" fixed="right" label="操作" width="140">

						<template #default="scope">
							<el-button size="small" text type="primary" @click="open_dialog(scope.row)">
								编辑
							</el-button>
							<el-divider direction="vertical" />
							<el-popconfirm title="确定删除吗？" @confirm="table_del(scope.row, scope.$index)">
								<template #reference>
									<el-button type="primary" text size="small">删除</el-button>
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
			<column ref="column" @complete="columnComplete" />
		</el-container>
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_sys_export',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
		column: defineAsyncComponent(() => import("./detail")),
	},
	data() {
		return {
			tableName: 'scm_sys_export',
			apiObj: this.$API.sysexportdetail.page,
			showGrouploading: false,
			groupFilterText: "",
			group: [],
			param: {
				key: "",
			},
			defaultParam: { type: 1 },
			selectColumn: {},
			selection: [],
			column: [
				{ prop: "id", label: "id", hide: true },
				{ prop: "codes", label: "系统编码", width: 100, align: "left", },
				{ prop: "codec", label: "用户编码", width: 140, align: "left", },
				{ prop: "names", label: "说明", width: 240, align: "left", },
				{ prop: "file", label: "文件名称", width: 240, align: "left", },
				{ prop: "row_status", label: "数据状态", width: 60 },
				{ prop: "update_time", label: "更新时间", width: 160, align: "right", formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_time", label: "创建时间", width: 160, align: "right", formatter: this.$TOOL.dateTimeFormat },
			],
		};
	},
	watch: {
		groupFilterText(val) {
			this.$refs.group.filter(val);
		},
	},
	created: function () {
		if (this.$route.path === "/exam/setting") {
			this.defaultParam.type = 2;
		}
		if (this.$route.path === "/crm/config") {
			this.defaultParam.type = 3;
		}
		if (this.$route.path === "/hr/config") {
			this.defaultParam.type = 4;
		}
	},
	mounted() {
		this.getGroup({ type: this.defaultParam.type });
	},
	methods: {
		complete() {
			this.$refs.table.refresh({ type: this.defaultParam.type });
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.sysexportdetail.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.sysexportdetail.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.sysexportdetail.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.sysexportdetail.delete, this.selection);
		},
		open_dialog(row) {
			if (row.id) {
				this.$refs.edit.open(row);
				return;
			}
			if (this.selectColumn.id) {
				this.$refs.edit.open(this.selectColumn, "add");
				return;
			}
			this.$message.warning("请选择字典栏目，在添加字典值");
		},
		selectionChange(selection) {
			this.selection = selection;
		},
		menuHandle(obj) {
			if (obj.command == "add") {
				this.open_dialog({});
				return;
			}
			if (obj.command == "edit") {
				this.open_dialog(obj.row);
				return;
			}
			if (obj.command == "delete") {
				this.table_del(obj.row);
				return;
			}
		},
		columnComplete() {
			this.getGroup({ type: this.defaultParam.type });
		},
		//加载树数据
		async getGroup(param) {
			this.showGrouploading = true;
			const res = await this.$API.sysexportheader.list.get(param);
			this.showGrouploading = false;
			let _tree = [{ id: "1", value: "0", label: "所有", parentId: "0" }];
			res.data.some((m) => {
				_tree.push({
					id: m.id,
					value: m.id,
					label: m.namec,
					code: m.codec,
					type: m.types,
					parentId: m.parentId,
				});
			});
			this.group = this.$TOOL.changeTree(_tree);
		},
		//树过滤
		groupFilterNode(value, data) {
			if (!value) return true;
			return data.label.indexOf(value) !== -1;
		},
		//树点击事件
		groupClick(data) {
			if (data.id == 1) {
				this.$refs.table.reload({ type: this.defaultParam.type });
				return;
			}
			var params = {
				id: data.id,
			};
			this.selectColumn = data;
			this.$refs.table.reload(params);
		},
		edit(row) {
			if (row.id) {
				this.$refs.column.open(row);
			} else {
				this.$refs.column.open({ type: this.defaultParam.type });
			}
		},
		remove(node, data) {
			this.$confirm(`确定要删除选中的 ${data.label} 项吗？`, "提示", {
				type: "warning",
				confirmButtonText: "确定",
				cancelButtonText: "取消",
			})
				.then(async () => {
					const loading = this.$loading();
					var res = await this.$API.sysexportheader.delete.delete(
						data.id
					);
					if (res.code == 200) {
						this.columnComplete();
						loading.close();
						this.$message.success("删除成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
	},
};
</script>

<style scoped>
.add-column {
	padding: 8px !important;
	margin: 8px;
}
</style>
