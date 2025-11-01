<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="700px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="所属角色" prop="pid">
				<el-tree-select v-model="formData.pid" placeholder="请选择所属角色" :data="parentIdOptions" collapse-tags
					check-strictly default-expand-all />
			</el-form-item>
			<el-form-item label="角色名称" prop="namec">
				<el-input v-model="formData.namec" clearable :maxlength="30" placeholder="请输入角色名称" show-word-limit />
			</el-form-item>
			<el-form-item label="数据权限" prop="data">
				<sc-select v-model="formData.data" placeholder="请选择数据权限" :data="data_list" />
			</el-form-item>
			<el-form-item label="备注" prop="remark">
				<el-input v-model="formData.remark" :autosize="{ minRows: 2, maxRows: 4 }" :maxlength="500"
					placeholder="请输入备注" show-word-limit type="textarea" />
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
	emits: ['complete'],
	data() {
		return {
			mode: "add",
			titleMap: { add: "新增", edit: "编辑" },
			isSaveing: false,
			visible: false,
			formData: this.def_data(),
			rules: {
				// pid: [
				// 	{ required: true, trigger: "change", pattern: this.$SCM.REGEX_ID, message: "请选择所属角色" },
				// ],
				// data: [
				// 	{ required: true, trigger: "blur", pattern: this.$SCM.REGEX_INT, message: "请选择数据权限" },
				// ],
				namec: [
					{ required: true, trigger: "blur", message: "请输入角色名称" },
				],
				remark: [],
			},
			data_list: [this.$SCM.OPTION_ONE_INT],
			parentIdOptions: [],
			parentIdProps: {
				multiple: false,
				checkStrictly: true,
				expandTrigger: "hover",
			},
		};
	},
	mounted() {
		this.$SCM.list_dic(this.data_list, 'role_data', false);
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				pid: '0',
				namec: undefined,
				data: this.$SCM.ID_ONE_INT,
				remark: undefined,
				od: 1,
			}
		},
		async initTree() {
			const t = await this.$API.scmurrole.list.get();
			let _tree = [
				{ id: "1", value: "0", label: "（默认）", parentId: "0" }
			];
			t.data.some((m) => {
				_tree.push({
					id: m.id,
					value: m.id,
					label: m.namec,
					parentId: m.pid,
				});
			});
			this.parentIdOptions = this.$TOOL.changeTree(_tree);
		},
		async open(row) {
			this.initTree();
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmurrole.model.get(row.id);
				this.formData = res.data;
			}
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = null;
					if (this.$SCM.is_valid_id(this.formData.id)) {
						res = await this.$API.scmurrole.update.put(this.formData);
					} else {
						res = await this.$API.scmurrole.add.post(this.formData);
					}
					this.isSaveing = false;
					if (res.code == 200) {
						this.$emit("complete");
						this.visible = false;
						this.$message.success("操作成功");
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