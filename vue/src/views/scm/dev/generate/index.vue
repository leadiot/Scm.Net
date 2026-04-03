<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button icon="el-icon-files" type="primary" :disabled="selection.length == 0"
					@click="open_save()">批量生成代码</el-button>
			</div>
			<div class="right-panel">
				<div class="right-panel-search">
					<el-input v-model="param.key" clearable placeholder="关键字" />
					<el-button icon="el-icon-search" type="primary" @click="search()" />
				</div>
			</div>
		</el-header>
		<el-main class="nopadding">
			<sc-table ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id"
				:hidePagination="true" :hideDo="true" :hide-context-menu="false" @selection-change="selectionChange">
				<!-- 固定列-选择列 -->
				<el-table-column fixed type="selection" width="60" align="center" />
				<el-table-column label="#" type="index" width="50"></el-table-column>
				<el-table-column align="center" fixed="right" label="操作" width="200">
					<template #default="scope">
						<el-button size="small" text type="primary" @click="open_save(scope.row)">
							生成代码
						</el-button>
						<el-divider direction="vertical" />
						<el-button text type="primary" size="small" @click="open_dialog(scope.row)">查看表</el-button>
					</template>
				</el-table-column>

				<template #status="{ data }">
					<el-tag disable-transitions :type="data.status ? 'success' : 'danger'">
						{{ data.status ? "正常" : "停用" }}
					</el-tag>
				</template>
			</sc-table>
		</el-main>
	</el-container>
	<column ref="column" />
	<save ref="save" />
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_dev_gen',
	components: {
		column: defineAsyncComponent(() => import("./column")),
		save: defineAsyncComponent(() => import("./save")),
	},
	data() {
		return {
			tableName: 'scm_dev_gen',
			apiObj: this.$API.scmdevgen.list,
			param: {
				key: "",
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ label: "名称", prop: "name", width: 200, align: "left" },
				{ label: "描述", prop: "description", align: "left" },
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
		open_save(row) {
			this.$refs.save.open(row);
		},
		open_dialog(row) {
			this.$refs.column.open(row);
		},
		selectionChange(selection) {
			this.selection = selection;
		},
	},
};
</script>
