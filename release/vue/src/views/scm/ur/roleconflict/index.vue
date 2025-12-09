<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button type="primary" @click="open_dialog">
					<sc-icon name="sc-plus" />
				</el-button>
				<el-button plain type="danger" :disabled="selection.length == 0" @click="batch_del">
					<sc-icon name="sc-close" />
				</el-button>
			</div>
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="关键字" @keyup.enter="search()">
					<template #append>
						<el-button type="primary" @click="search()"><sc-icon name="sc-search" /></el-button>
					</template>
				</el-input>
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id" @menu-handle="menuHandle"
				@selection-change="selectionChange">
				<!-- 固定列-选择列 -->
				<el-table-column fixed type="selection" align="center" width="60" />
				<el-table-column align="center" fixed="right" label="操作" width="140">
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

				<template #status="{ data }">
					<el-tag disable-transitions :type="data.status ? 'success' : 'danger'">
						{{ data.status ? "正常" : "停用" }}
					</el-tag>
				</template>

				<template #roleA="{ data }">
					{{ data.roleAObj.name }}
				</template>

				<template #roleB="{ data }">
					{{ data.roleBObj.name }}
				</template>
			</scTable>
		</el-main>
		<modify ref="modify" @complete="complete" />
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_ur_roleconflict',
	components: {
		modify: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		return {
			tableName: 'scm_ur_roleconflict',
			apiObj: this.$API.scmurroleconflict.page,
			param: {
				key: "",
			},
			selection: [],
			column: [
				{ prop: "id", label: "id", hide: true },
				{ prop: "rolea_names", label: "角色A", width: 100, align: "left" },
				{ prop: "roleb_names", label: "角色B", width: 100, align: "left" },
				{ prop: "remark", label: "互斥说明", minWidth: 200, align: "left" },
				{ prop: "row_status", label: "数据状态", width: "80", },
				{ prop: "update_names", label: "更新人员", width: "100", },
				{ prop: "update_time", label: "更新时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_names", label: "创建人员", width: "100", },
				{ prop: "create_time", label: "创建时间", width: "160", formatter: this.$TOOL.dateTimeFormat },
			],
		};
	},
	mounted() { },
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		//删除
		async table_del(row) {
			var res = await this.$API.scmurroleconflict.delete.delete([row.id]);
			if (res.code == 200) {
				this.$refs.table.refresh();
				this.$message.success("删除成功");
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
		//批量删除
		batch_del() {
			this.$confirm(
				`确定删除选中的 ${this.selection.length} 项吗？`,
				"提示",
				{
					type: "warning",
					confirmButtonText: "确定",
					cancelButtonText: "取消",
				}
			)
				.then(async () => {
					const loading = this.$loading();
					let ids = [];
					this.selection.forEach((element) => {
						ids.push(element.id);
					});
					var res = await this.$API.scmurroleconflict.delete.delete(
						ids
					);
					if (res.code == 200) {
						this.$refs.table.refresh();
						loading.close();
						this.$message.success("删除成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		open_dialog(row) {
			if (row.id) {
				this.$refs.modify.open(row);
			} else {
				this.$refs.modify.open();
			}
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
				this.table_del(obj.row);
			}
		},
	},
};
</script>
