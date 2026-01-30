<template>
	<el-container>
		<el-aside width="260px" v-loading="menuloading">
			<el-container>
				<el-header>
					<el-input placeholder="输入关键字进行过滤" v-model="menuFilterText" clearable></el-input>
					<el-button type="primary" round icon="el-icon-plus" class="add-column" @click="add()"></el-button>
					<el-button type="danger" class="add-column" round plain icon="el-icon-delete"
						@click="delMenu"></el-button>
				</el-header>
				<el-main class="nopadding">
					<el-scrollbar>
						<el-tree ref="menu" class="menu" node-key="id" :data="menuList" draggable highlight-current
							:expand-on-click-node="false" check-strictly show-checkbox
							:filter-node-method="menuFilterNode" @node-click="menuClick" @node-drop="nodeDrop">
							<template #default="{ node, data }">
								<span class="scmui-item-node el-tree-node__label">
									<span class="label">{{ node.label }}</span>
									<span class="opt">
										<el-icon @click.stop="add(node, data)"><el-icon-plus /></el-icon>
									</span>
								</span>
							</template>
						</el-tree>
					</el-scrollbar>
				</el-main>
			</el-container>
		</el-aside>
		<el-container>
			<el-main class="nopadding" style="padding: 20px" ref="main">
				<edit ref="edit" :menu="sourceLst" @complete="getMenu"></edit>
			</el-main>
		</el-container>
	</el-container>
</template>

<script>
let newMenuIndex = 1;
import edit from "./edit";

export default {
	name: 'dev_menu',
	components: {
		edit,
	},
	data() {
		return {
			menuloading: false,
			menuList: [],
			sourceLst: [],
			menuFilterText: "",
		};
	},
	watch: {
		menuFilterText(val) {
			this.$refs.menu.filter(val);
		},
	},
	mounted() {
		this.getMenu();
	},
	methods: {
		//加载树数据
		async getMenu() {
			this.menuloading = true;
			var res = await this.$API.scmadmmenu.list.get();
			this.menuloading = false;
			let _tree = [];
			res.data.some((m) => {
				_tree.push({
					id: m.id,
					value: m.id,
					label: m.namec,
					parentId: m.pid,
				});
			});
			this.sourceLst = res.data;
			this.menuList = this.$TOOL.changeTree(_tree);
		},
		//树点击
		menuClick(data, node) {
			var pid = node.level == 1 ? undefined : node.parent.data.id;
			this.$refs.edit.setData(data, pid);
			this.$refs.main.$el.scrollTop = 0;
		},
		//树过滤
		menuFilterNode(value, data) {
			if (!value) return true;
			var targetText = data.label;
			return targetText.indexOf(value) !== -1;
		},
		//树拖拽
		async nodeDrop(dragNode, dropNode, dropType) {
			this.menuloading = true;
			var res = await this.$API.scmadmmenu.dragging.put({
				dragNode: dragNode.data,
				dropNode: dropNode.data,
				sortType: dropType,
			});
			if (res.code == 200) {
				this.$message.success("操作成功");
				this.getMenu();
			} else {
				this.$message.warning(res.message);
			}
			this.menuloading = false;
		},
		//增加
		async add(node, data) {
			var newMenuName = "未命名" + newMenuIndex++;
			var newMenuData = {
				pid: data ? data.id : "0",
				lang: 'zh-cn',
				client: 10,
				name: newMenuName,
				label: newMenuName
			};
			this.menuloading = true;
			var res = await this.$API.scmadmmenu.temp.post(newMenuData);
			this.menuloading = false;
			newMenuData.id = res.data.id;

			this.$refs.menu.append(newMenuData, node);
			this.$refs.menu.setCurrentKey(newMenuData.id);
			var pid = node ? node.data.id : "0";
			this.$refs.edit.setData(newMenuData, pid);
		},
		//删除菜单
		async delMenu() {
			var CheckedNodes = this.$refs.menu.getCheckedNodes();
			if (CheckedNodes.length == 0) {
				this.$message.warning("请选择需要删除的项");
				return false;
			}

			var confirm = await this.$confirm(
				"确认删除已选择的菜单吗？",
				"提示",
				{
					type: "warning",
					confirmButtonText: "删除",
					confirmButtonClass: "el-button--danger",
				}
			).catch(() => { });
			if (confirm != "confirm") {
				return false;
			}

			this.menuloading = true;
			var reqData = {
				ids: CheckedNodes.map((item) => item.id),
			};
			var res = await this.$API.scmadmmenu.delete.delete(
				reqData.ids.join(",")
			);
			this.menuloading = false;

			if (res.code == 200) {
				CheckedNodes.forEach((item) => {
					var node = this.$refs.menu.getNode(item);
					if (node.isCurrent) {
						this.$refs.edit.setData({ id: "0" });
					}
					this.$refs.menu.remove(item);
				});
			} else {
				this.$message.warning(res.message);
			}
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
