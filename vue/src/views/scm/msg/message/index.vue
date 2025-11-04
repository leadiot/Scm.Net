<template>
	<el-container>
		<el-aside width="240px">
			<el-tree ref="category" class="menu" node-key="value" default-expand-all :data="category_list"
				:current-node-key="param.cat" :highlight-current="true" :expand-on-click-node="false"
				@node-click="typeClick">
				<template #default="{ node, data }">
					<span class="scmui-tree-node msg-tag">
						<span>
							<component class="msg-icon" :is="data.icon"></component>
							{{ node.label }}
						</span>
						<span v-if="data.sum > 0">
							<el-tag type="danger" effect="dark" size="small">{{ data.sum }}</el-tag>
						</span>
					</span>
				</template>
			</el-tree>
		</el-aside>
		<el-container>
			<el-header>
				<div class="left-panel">
					<el-button type="primary" @click="open_dialog">
						<sc-icon name="sc-plus" />
					</el-button>
					<el-button plain type="danger" :disabled="selection.length == 0" @click="delete_list" title="删除">
						<sc-icon name="sc-close-circle-line" />
					</el-button>
					<el-button plain type="danger" :disabled="selection.length == 0" @click="recycle_del" title="移到回收站">
						<sc-icon name="sc-delete-line" />
					</el-button>
					<el-button plain type="success" @click="all_read">
						<sc-icon name="sc-close-circle-line" />全部已读
					</el-button>
					<el-button plain type="success" :disabled="selection.length == 0" @click="set_read">
						<sc-icon name="sc-close-circle-line" />设为已读
					</el-button>
				</div>
				<div class="right-panel">
					<div class="right-panel-search">
						<el-input v-model="param.key" clearable placeholder="关键字" />
						<el-button type="primary" @click="search"><sc-icon name="sc-search" /></el-button>
					</div>
				</div>
			</el-header>
			<el-main class="nopadding">
				<scTable :table-name="tableName" ref="table" :api-obj="apiObj" :column="column" row-key="id"
					:menu-default="['add']" @menu-handle="menuHandle" @selection-change="selectionChange">
					<el-table-column fixed type="selection" width="60" />
					<el-table-column label="#" type="index" width="50"></el-table-column>

					<template #title="{ data }">
						<el-link type="primary" @click="read(data)">{{ data.title }}</el-link>
					</template>

					<template #isread="{ data }">
						<el-tag disable-transitions :type="data.isread ? 'success' : 'danger'">
							{{ data.isread ? "是" : "否" }}
						</el-tag>
					</template>
				</scTable>
			</el-main>
			<edit ref="edit" @complete="complete" />
		</el-container>
	</el-container>
	<el-drawer v-model="viewVisible" title="消息详情">
		<el-container>
			<el-main>
				<el-form ref="viewRef" label-width="100px" :model="selectedItem">
					<el-form-item label="消息标题" prop="title">
						<el-input v-model="selectedItem.title" readonly></el-input>
					</el-form-item>
					<el-form-item label="消息内容" prop="remark">
						<el-input v-model="selectedItem.remark" type="textarea" :rows="15" readonly></el-input>
					</el-form-item>
				</el-form>
			</el-main>
		</el-container>
	</el-drawer>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_msg_message',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		return {
			tableName: 'scm_msg_message',
			apiObj: this.$API.scmmsgmessage.page,
			list: [],
			param: {
				cat: 'msg10',
				key: '',
			},
			selection: [],
			category_list: [
				{
					label: "消息状态",
					value: 'msg',
					children: [
						{
							label: "所有消息",
							type: "level",
							value: 'msg10',
							sum: 0,
							icon: "sc-bell-line",
						},
						{
							label: "已读消息",
							type: "level",
							value: 'msg11',
							sum: 0,
							icon: "sc-mail-check-line",
						},
						{
							label: "未读消息",
							type: "level",
							value: 'msg12',
							sum: 0,
							icon: "sc-mail-unread-line",
						},
						{
							label: "回收站",
							type: "level",
							value: 'msg13',
							sum: 0,
							icon: "sc-delete-line",
						},
					],
				},
				{
					label: "消息标签",
					value: 'tag',
					children: [],
				},
			],
			column: [
				{ prop: "id", label: "id", hide: true },
				{ prop: "title", label: "消息标题", width: 200, align: "left", showOverflowTooltip: true },
				{ prop: "types", label: "类型", width: 100, formatter: this.getTypesNames },
				{ prop: "email", label: "邮箱信息", width: 180 },
				{ prop: "phone", label: "电话号码", width: 130 },
				{ prop: "tags", label: "消息标签", width: 200 },
				{ prop: "remark", label: "消息内容", minWidth: 200, showOverflowTooltip: true },
				{ prop: "isread", label: "是否已读", width: 100 },
				{ prop: "create_names", label: "发送人员", width: 100 },
				{ prop: "create_time", label: "发送时间", width: 180, formatter: this.$TOOL.dateTimeFormat }
			],
			selectedItem: null,
			viewVisible: false,
			types_list: [this.$SCM.OPTION_ALL_INT],
		};
	},
	mounted() {
		this.$SCM.list_dic(this.types_list, 'message_type', true);
		this.initTags();
		this.initTotal();
	},
	methods: {
		async initTags() {
			var res = await this.$API.scmrestag.option.get(this.$API.scmmsgmessage.SYS_ID);
			if (!res || res.code != 200) {
				return;
			}

			var list = [{ 'id': 1, label: '全部', value: 'tag10', sum: 0, icon: 'sc-star-line' }];
			res.data.forEach(element => {
				element.sum = 0;
				element.icon = 'sc-star-line';
				list.push(element);
			});

			if (this.category_list.length > 1) {
				this.category_list[1].children = list;
			}
		},
		async initTotal() {
			var res = await this.$API.scmmsgmessage.total.get();
			this.category_list[0].children[0].sum = res.data.allCount;
			this.category_list[0].children[2].sum = res.data.unReadCount;
			this.category_list[0].children[3].sum = res.data.recycleCount;
		},
		typeClick(data) {
			if (data.value == 'msg' || data.value == 'tag') {
				return;
			}

			this.param.cat = data.value;
			this.$refs.table.upData(this.param);
		},
		complete() {
			this.initTotal();
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.scmmsgmessage.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmmsgmessage.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmmsgmessage.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmmsgmessage.delete, this.selection);
		},
		open_dialog(row) {
			if (row.id) {
				this.$refs.edit.open(row);
			} else {
				this.$refs.edit.open();
			}
		},
		selectionChange(selection) {
			this.selection = selection;
		},
		menuHandle(obj) {
			if (obj.command == 'add') {
				this.open_dialog({})
			}
			if (obj.command == 'delete') {
				this.table_del(obj.row)
			}
		},
		recycle_del() {
			this.$confirm(
				`确定移到回收站选中的 ${this.selection.length} 项吗？`,
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
					var res = await this.$API.scmmsgmessage.recycle.put(ids);
					loading.close();
					if (res.code == 200) {
						this.initTotal();
						this.$refs.table.refresh();
						this.$message.success("删除成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		async read(data) {
			this.selectedItem = data;
			this.viewVisible = true;

			if (data.isread) {
				return;
			}

			var ids = [];
			ids.push(data.id);
			var res = await this.$API.scmmsgmessage.read.put(ids);
			if (!res || res.code != 200) {
				return;
			}

			data.isread = true;
			this.category_list[0].children[2].sum -= 1;
		},
		all_read() {
			this.$confirm(`确定要设为全部已读吗？`, "提示", {
				type: "warning",
				confirmButtonText: "确定",
				cancelButtonText: "取消",
			})
				.then(async () => {
					const loading = this.$loading();
					var res = await this.$API.scmmsgmessage.read_all.put();
					loading.close();
					if (res.code == 200) {
						this.initTotal();
						this.$refs.table.refresh();
						this.$message.success("操作成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		set_read() {
			this.$confirm(`确定设置已读选中的 ${this.selection.length} 项吗？`, "提示",
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
					var res = await this.$API.scmmsgmessage.read.put(ids);
					loading.close();
					if (res.code == 200) {
						this.initTotal();
						this.$refs.table.refresh();
						this.$message.success("操作成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		getTypesNames(types) {
			return this.$SCM.get_dic_names(this.types_list, types, '-');
		}
	},
};
</script>

<style>
.msg-tag .el-tag {
	height: 18px;
	padding: 0 5px;
	margin-left: 5px;
}

.msg-icon {
	width: 16px;
	height: 16px;
	vertical-align: middle;
	position: relative;
	top: -2px;
}
</style>
