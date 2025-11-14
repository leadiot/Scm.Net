<template>
	<el-container>
		<scSearch>
			<template #search>
				<el-tabs v-model="editableTabsValue" type="card" closable @tab-change="changeTab" @tab-remove="removeTab"
					style="padding-left: 18px;padding-right: 18px;padding-bottom: 0px;">
					<el-tab-pane v-for="item in editableTabs" :key="item.name" :label="item.title" :name="item.name" />
				</el-tabs>
			</template>
		</scSearch>
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
			</div>
			<div class="right-panel">
				<div class="right-panel-search">
					<el-input v-model="param.key" clearable placeholder="关键字" />
					<el-button icon="el-icon-search" type="primary" @click="search" />
				</div>
			</div>
		</el-header>
		<el-main class="nopadding">
			<scTable ref="table" :data="list" :column="column" row-key="id" hide-pagination @menu-handle="menuHandle"
				@selection-change="selectionChange">
				<!-- 固定列-选择列 -->
				<el-table-column align="center" fixed type="selection" width="60" />
				<el-table-column label="#" type="index" width="50" />
				<el-table-column align="center" fixed="right" label="操作" width="210">
					<template #default="scope">
						<el-button size="small" text type="primary" @click="open_dialog(scope.row)">
							编辑
						</el-button>
						<el-divider direction="vertical" />
						<el-button size="small" text type="primary" @click="open_newtab(scope.row)">
							下级
						</el-button>
						<el-divider direction="vertical" />
						<el-popconfirm title="确定删除吗？" @confirm="delete_item(scope.row, scope.$index)">
							<template #reference>
								<el-button text type="primary" size="small">删除</el-button>
							</template>
						</el-popconfirm>
					</template>
				</el-table-column>
				<template #od="{ data }">
					<el-link style="color: #5cd29d" :underline="false"
						@click="sort({ parent: data.parentId, id: data.id, type: 0, })">
						<el-icon><el-icon-top /></el-icon>
					</el-link>
					<el-divider direction="vertical" />
					<el-link style="color: #5cd29d" :underline="false"
						@click="sort({ parent: data.parentId, id: data.id, type: 1, })">
						<el-icon><el-icon-bottom /></el-icon>
					</el-link>
				</template>
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
	</el-container>
</template>
<script>
import scTableFilter from "@/components/scTableFilter";
import { defineAsyncComponent } from "vue";
export default {
	components: {
		scTableFilter,
		edit: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		let defTab = { title: '国别', name: '1000000000000000002', content: { 'id': '1000000000000000002' } };
		return {
			list: [],
			param: {
				key: "",
			},
			selection: [],
			editableTabsValue: '0',
			editableTabs: [defTab],
			currTab: defTab,
			column: [
				{ label: "id", prop: "id", hide: true },
				{ label: "区域简称", prop: "names", filter: true, type: "text" },
				{ label: "区域全称", prop: "namef", filter: true, type: "text" },
				{ label: "区域编号", prop: "codec", filter: true, type: "text", width: 100 },
				{
					label: "排序",
					prop: "od",
					width: 100,
					filter: true,
					type: "select",
					extend: {
						request: () => {
							return [
								{ label: "选项1", value: "1", },
								{ label: "选项2", value: "2", },
							];
						},
					},
				},
				{ label: "深度", prop: "lv", width: 100, },
				{ prop: "lng", label: "经度", },
				{ prop: "lat", label: "维度" },
				{ label: "状态", prop: "row_status", width: "80", },
				{ label: "创建时间", prop: "create_time", width: "150", sortable: true, },
			],
		};
	},
	mounted() {
		this.listRegion(this.currTab.content.id);
	},
	methods: {
		complete() {
			//this.$refs.table.refresh();
			this.listRegion(this.currTab.content.id);
		},
		search() {
			//this.$refs.table.upData(this.param);
			this.listRegion(this.currTab.content.id);
		},
		async sort(m) {
			await this.$API.sysregion.sort.post(m);
			this.$refs.table.refresh();
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.sysregion.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.sysregion.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.sysregion.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.sysregion.delete, this.selection);
		},
		open_dialog(row) {
			this.$refs.edit.open(this.currTab, row);
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
		selectionChange(selection) {
			this.selection = selection;
		},
		async changeTab(targetName) {
			if (!targetName) {
				return;
			}

			var pid = targetName;
			this.listRegion(pid);
		},
		removeTab(targetName) {
			let tabs = this.editableTabs;
			let activeName = this.editableTabsValue;
			if (activeName === targetName) {
				tabs.forEach((tab, index) => {
					if (tab.name === targetName) {
						const nextTab = tabs[index + 1] || tabs[index - 1]
						if (nextTab) {
							activeName = nextTab.name
						}
					}
				})
			}
			this.editableTabsValue = activeName
			this.editableTabs = tabs.filter((tab) => tab.name !== targetName)

			//this.listRegion(activeName);
		},
		async listRegion(pid) {
			var res = await this.$API.sysregion.list.get({ "id": pid });
			this.list = res.data;
		},
		open_newtab(row) {
			const newTabName = row.id;
			let tab = {
				title: row.names,
				name: newTabName,
				content: row
			};
			this.currTab = tab;
			this.editableTabs.push(tab);
			this.editableTabsValue = newTabName;

			//this.listRegion(newTabName);
		},
		filterOk(data) {
			console.log("data", data);
		},
	},
};
</script>
