<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="600px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="角色A" prop="rolea_id">
				<el-tree-select v-model="formData.rolea_id" placeholder="请选择角色A" :data="roleOptions" collapse-tags
					check-strictly default-expand-all :style="{ width: '100%' }" />
			</el-form-item>
			<el-form-item label="角色B" prop="roleb_id">
				<el-tree-select v-model="formData.roleb_id" placeholder="请选择角色B" :data="roleOptions" collapse-tags
					check-strictly default-expand-all :style="{ width: '100%' }" />
			</el-form-item>
			<el-form-item label="互斥说明" prop="remark">
				<el-input v-model="formData.remark" placeholder="请输入互斥说明" :maxlength="255" show-word-limit clearable
					:autosize="{ minRows: 2, maxRows: 4 }" :style="{ width: '100%' }" type="textarea"></el-input>
			</el-form-item>
		</el-form>

		<template #footer>
			<el-button @click="close">取 消</el-button>
			<el-button :loading="isSaveing" type="primary" @click="save">
				确 定
			</el-button>
		</template>
	</sc-dialog>
</template>
<script>
export default {
	emits: ["complete"],
	data() {
		return {
			mode: "add",
			titleMap: {
				add: "新增",
				edit: "编辑",
			},
			isSaveing: false,
			visible: false,
			formData: this.def_data(),
			rules: {
				rolea_id: [{ required: true, message: "请选择角色A", trigger: "change" }],
				roleb_id: [{ required: true, message: "请选择角色B", trigger: "change" }],
				remark: [{ required: true, message: "请输入互斥说明", trigger: "blur" }],
			},
			roleOptions: [],
			roleList: [],
		};
	},
	mounted() {
		this.initTree();
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				rolea_id: "",
				roleb_id: "",
				remark: "",
			};
		},
		async initTree() {
			const t = await this.$API.scmurrole.list.get();
			this.roleList = t.data;
			let _tree = [];
			t.data.some((m) => {
				_tree.push({
					id: m.id,
					value: m.id,
					label: m.namec,
					parentId: m.pid,
				});
			});
			this.roleOptions = this.$TOOL.changeTree(_tree);
		},
		async open(row) {
			if (!row) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmurroleconflict.model.get(row.id);
				this.formData = res.data;
			}
			this.visible = true;
		},
		// 判断B是否为A的上级；
		isParentOfA(roleA, roleB) {
			if (roleA.id == roleB.id) {
				return true;
			}

			var role = {};
			while (role != null) {
				role = this.roleList.find(a => a.id == roleA.pid);
				if (!role) {
					return false;
				}
				if (role.id == roleB.id) {
					return true;
				}
			}

			return false;
		},
		save() {
			var roleA = this.roleList.find((m) => m.id == this.formData.rolea_id);
			var roleB = this.roleList.find((m) => m.id == this.formData.roleb_id);
			if (roleB.id == roleA.id) {
				this.$message.warning("角色A不能和角色B不允许相同");
				return;
			}
			if (this.isParentOfA(roleA, roleB)) {
				this.$message.warning("角色B不能和角色A存在父子关系");
				return;
			}
			if (this.isParentOfA(roleB, roleA)) {
				this.$message.warning("角色A不能和角色B存在父子关系");
				return;
			}
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = null;
					if (this.$SCM.is_valid_id(this.formData.id)) {
						res = await this.$API.scmurroleconflict.update.put(this.formData);
					} else {
						res = await this.$API.scmurroleconflict.add.post(this.formData);
					}
					this.isSaveing = false;
					if (res.code == 200) {
						this.$emit("complete");
						this.visible = false;
						this.$message.success("保存成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				}
			});
		},
		close() {
			this.formData = this.def_data();
			this.$refs.formRef.resetFields();
			this.visible = false;
		},
	},
};
</script>
