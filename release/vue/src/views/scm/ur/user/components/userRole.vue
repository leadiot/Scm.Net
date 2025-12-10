<template>
	<sc-dialog v-model="visible" show-fullscreen title="授权角色" width="500px" @close="close">
		<el-scrollbar max-height="400px">
			<el-tree ref="tree" :data="data" node-key="id" default-expand-all show-checkbox />
		</el-scrollbar>
		<template #footer>
			<el-button @click="close">取 消</el-button>
			<el-button :loading="isSaveing" type="primary" @click="save">
				确 定
			</el-button>
		</template></sc-dialog>
</template>
<script>
export default {
	data() {
		return {
			isSaveing: false,
			visible: false,
			data: [],
			adminIdArr: [],
		};
	},
	mounted() {
		this.initRole();
	},
	methods: {
		async initRole() {
			const roleRes = await this.$API.scmurrole.list.get();
			let roleArr = [];
			roleRes.data.forEach(function (m) {
				roleArr.push({
					id: m.id,
					value: m.id,
					label: m.namec,
					parentId: m.parentId,
				});
			});
			this.data = this.$TOOL.changeTree(roleArr);
		},
		async save() {
			var nodes = this.$refs.tree.getCheckedNodes();
			if (nodes.length == 0) {
				this.$message.warning("请选择要授权的角色");
				return;
			}
			let role = [];
			nodes.forEach((item) => {
				role.push(item.id);
			});
			this.isSaveing = true;
			let res = await this.$API.scmurroleauth.plusAdmin.post({
				roleArr: role,
				adminArr: this.adminIdArr,
			});
			this.isSaveing = false;
			if (res.code == 200) {
				this.$message.success("授权成功");
				this.visible = false;
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
		open(id) {
			this.adminIdArr = id;
			this.visible = true;
		},
		close() {
			this.$refs.tree.setCheckedNodes([]);
			this.visible = false;
		},
	},
};
</script>
