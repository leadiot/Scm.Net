<template>
	<el-container>
		<el-header style="height: auto;display: block;">
			<div>
				<el-row>
					<el-col :span="8">
						123:{{ header.codes }}
					</el-col>
					<el-col :span="8">
						123
					</el-col>
					<el-col :span="8">
						123
					</el-col>
					<el-col :span="8">
						123
					</el-col>
					<el-col :span="8">
						123
					</el-col>
					<el-col :span="8">
						123
					</el-col>
				</el-row>
			</div>
		</el-header>
		<el-main class="nopadding">
			<el-container>
				<el-header>
					<div class="left-panel" v-show="editable">
						<el-button icon="el-icon-plus" type="primary" @click="open_selector()" />
						<el-divider direction="vertical"></el-divider>
						<el-button type="danger" icon="el-icon-delete" plain :disabled="selection.length == 0"
							@click="delete_list"></el-button>
					</div>
					<div class="right-panel" v-show="editable">
						<el-button type="primary" @click="save()">保存草稿</el-button>
						<el-button type="danger" @click="submit()">提交审批</el-button>
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
						<template #need_qty="scope">
							<el-input-number v-model="scope.row.need_qty" :min="1" controls-position="right" />
						</template>
					</scTable>
				</el-main>
			</el-container>
		</el-main>
	</el-container>
	<bookSelector ref="bookSelector" @complete="complete" />
</template>
<script>
import { defineAsyncComponent } from "vue";
import { useRoute } from "vue-router";

export default {
	name: 'samples_po_detail',
	components: {
		bookSelector: defineAsyncComponent(() => import("../book/selector")),
	},
	data() {
		return {
			tableName: 'samples_po_detail',
			apiObj: this.$API.samplespodetail.page,
			param: {
				id: '0',
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: 'book_codes', label: '书籍编码', width: 100 },
				{ prop: 'book_names', label: '书籍名称', width: 140, align: 'left' },
				{ prop: 'need_qty', label: '需求数量', width: 120, align: 'right' },
				{ prop: 'real_qty', label: '实际数量', width: 100, align: 'right' },
				{ prop: 'remark', label: '备注', minWidth: 140, align: 'left' },
				{ prop: "update_names", label: "更新人员", width: 100, },
				{ prop: "update_time", label: "更新时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
				{ prop: "create_names", label: "创建人员", width: 100, },
				{ prop: "create_time", label: "创建时间", width: 160, formatter: this.$TOOL.dateTimeFormat },
			],
			editable: false,
			header: {}
		};
	},
	mounted() {
		this.init();
	},
	methods: {
		async init() {
			var route = useRoute();
			var id = route.query.id;
			console.log(id);
			if (!this.$SCM.is_valid_id(id)) {
				console.log('无效的id');
				return;
			}

			this.param.id = id;

			var res = await this.$API.samplespoheader.view.get(id);
			this.header = res.data;
			this.editable = res.data.wfa_status < 1;

			this.search();
		},
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.samplespodetail.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.samplespodetail.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.samplespodetail.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.samplespodetail.delete, this.selection);
		},
		open_dialog() {
			this.$refs.bookSelector.open(this.param.id, this.$API.samplespodetail.batch);
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
		open_selector() {
			this.$refs.bookSelector.open(this.param.id, this.$API.samplespodetail.batch);
		},
		async save() {
			var tableData = this.$refs.table.getData();
			if (!tableData || tableData.length < 1) {
				return;
			}

			var details = [];
			tableData.forEach((ele) => {
				details.push({ "id": ele.id, "qty": ele.need_qty });
			});

			var data = { "id": this.param.id, "items": details };
			this.isSaving = true;
			var res = await this.$API.samplespodetail.batchupdate.post(data);
			this.isSaving = false;
			if (res.code == 200) {
				this.$message.success("保存成功");
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
		async submit() {
			this.isSaving = true;
			var res = await this.$API.samplespoheader.submit.post(this.param.id);
			this.isSaving = false;
			if (res.code == 200) {
				this.editable = false;
				this.$message.success("提交成功");
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		}
	},
};
</script>
<style scoped>
.el-input-number {
	width: 100%;
}
</style>