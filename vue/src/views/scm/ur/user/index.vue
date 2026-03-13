<template>
	<el-container>
		<el-aside width="220px" v-loading="showGrouploading">
			<el-container>
				<el-header>
					<el-input placeholder="输入关键字进行过滤" v-model="groupFilterText" clearable></el-input>
				</el-header>
				<el-main class="nopadding">
					<el-tree ref="group" class="menu" node-key="id" default-expand-all :data="group"
						:current-node-key="''" :highlight-current="true" :expand-on-click-node="false"
						:filter-node-method="groupFilterNode" @node-click="groupClick"></el-tree>
				</el-main>
			</el-container>
		</el-aside>
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
					<el-button type="primary" plain :disabled="selection.length == 0" @click="userRole">分配角色</el-button>
					<el-button type="danger" plain :disabled="selection.length != 1" @click="pwdreset">密码重置</el-button>
					<el-button type="danger" plain @click="exportAll()">导出</el-button>
				</div>
			</el-header>
			<el-main class="nopadding">
				<sc-table ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id"
					@menu-handle="menuHandle" @selection-change="selectionChange">
					<!-- 固定列-选择列 -->
					<el-table-column fixed type="selection" width="60" />
					<el-table-column label="#" type="index" width="50"></el-table-column>
					<el-table-column align="center" fixed="right" label="操作" width="210">

						<template #default="scope">
							<el-button size="small" text type="primary" @click="open_dialog(scope.row)">
								编辑
							</el-button>
							<el-divider direction="vertical" />
							<el-button size="small" text type="primary" @click="userData(scope.row)">
								权限
							</el-button>
							<el-divider direction="vertical" />
							<el-popconfirm title="确定删除吗？" @confirm="delete_item(scope.row)">
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

					<template #avatar="{ data }">
						<el-avatar :src="$SCM.get_avatar(data.avatar)" size="small"></el-avatar>
					</template>

					<template #OrganizeObj="{ data }">
						{{ data.organizeObj?.name }}
					</template>
				</sc-table>
			</el-main>
			<edit ref="edit" @complete="complete" />
			<userRole ref="userRole" @complete="complete" />
			<userData ref="userData" @complete="complete" />
		</el-container>
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_ur_user',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
		userRole: defineAsyncComponent(() => import("./components/userRole")),
		userData: defineAsyncComponent(() => import("./components/userData")),
	},
	data() {
		return {
			tableName: 'scm_ur_user',
			apiObj: this.$API.scmuruser.page,
			showGrouploading: false,
			groupFilterText: "",
			param: {
				key: "",
			},
			selection: [],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ label: "头像", prop: "avatar", width: "60" },
				{ label: "系统编码", prop: "codes", width: "120", align: "left" },
				{ label: "员工工号", prop: "codec", width: "120", align: "left" },
				{ label: "登录账户", prop: "names", width: "120", align: "left" },
				{ label: "展示姓名", prop: "namec", width: "120", align: "left" },
				{ label: "所属部门", prop: "OrganizeObj", align: "left", width: "200" },
				{ label: "性别", prop: "sex", width: "80", formatter: this.getSexNames },
				{ label: "手机号码", prop: "cellphone", width: "120" },
				{ label: "固话", prop: "telephone", width: "120" },
				{ label: "数据状态", prop: "row_status", width: "80" },
				{ prop: "row_status", label: "数据状态", width: "80", },
				{ prop: "update_names", label: "更新人员", width: "100", },
				{ prop: "update_times", label: "更新时间", width: "160" },
				{ prop: "create_names", label: "创建人员", width: "100", },
				{ prop: "create_times", label: "创建时间", width: "160" },
			],
			group: [],
			sex_list: [],
		};
	},
	watch: {
		groupFilterText(val) {
			this.$refs.group.filter(val);
		},
	},
	mounted() {
		this.getRole();
		this.$SCM.list_sex(this.sex_list, true);
	},
	methods: {
		complete() {
			this.$refs.table.refresh();
		},
		search() {
			this.$refs.table.upData(this.param);
		},
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.scmuruser.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmuruser.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmuruser.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmuruser.delete, this.selection);
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
		pwdreset() {
			this.$confirm(
				`确定要重置 ${this.selection.length} 项的密码吗？`,
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
					var res = await this.$API.scmuruser.passreset.put(ids);
					if (res.code == 200) {
						this.$refs.table.refresh();
						loading.close();
						this.$message.success("重置成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		userRole() {
			if (this.selection.length == 0) {
				this.$message.warning("请选择授权角色的用户");
				return;
			}
			let ids = [];
			this.selection.forEach((element) => {
				ids.push(element.id);
			});
			this.$refs.userRole.open(ids);
		},
		userData(row) {
			if (!row || !row.id) {
				return;
			}
			this.$refs.userData.open(row);
		},
		async exportAll() {
			const res = await this.$API.scmuruser.exportAll.get();
			if (res == null || res.code != 200) {
				return;
			}
		},
		//加载树数据
		async getRole() {
			this.showGrouploading = true;
			const res = await this.$API.scmurrole.list.get();
			this.showGrouploading = false;
			let _tree = [{ id: "1", value: "0", label: "所有", parentId: "0" }];
			res.data.some((m) => {
				_tree.push({
					id: m.id,
					value: m.id,
					label: m.namec,
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
			var params = {
				id: data.id,
			};
			this.$refs.table.reload(params);
		},
		getSexNames(key) {
			return this.$SCM.get_dic_names(this.sex_list, key, '');
		}
	},
};
</script>
