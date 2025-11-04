<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="700px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="上级群组" prop="pid">
				<el-tree-select v-model="formData.pid" placeholder="请选择上级群组" :data="pid_list" collapse-tags check-strictly
					default-expand-all />
			</el-form-item>
			<el-form-item label="群组编码" prop="codec">
				<el-input v-model="formData.codec" clearable :maxlength="30" placeholder="请输入群组编码" show-word-limit />
			</el-form-item>
			<el-form-item label="群组名称" prop="namec">
				<el-input v-model="formData.namec" clearable :maxlength="30" placeholder="请输入群组名称" show-word-limit />
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
			titleMap: { add: "新增", edit: "编辑", },
			isSaveing: false,
			visible: false,
			formData: this.def_data(),
			rules: {
				pid: [
					{ required: true, trigger: "change", message: "请选择所属群组", },
				],
				namec: [
					{ required: true, trigger: "blur", message: "请输入群组名称", },
				],
				remark: [],
			},
			pid_list: [],
			pidProps: {
				multiple: false,
				checkStrictly: true,
				expandTrigger: "hover",
			},
		};
	},
	mounted() { },
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				pid: '0',
				namec: '',
				remark: '',
			}
		},
		async initTree() {
			const t = await this.$API.scmurgroup.list.get();
			let _tree = [
				{ id: '1', value: '0', label: "（默认）", parentId: "0" },
			];
			t.data.some((m) => {
				_tree.push({
					id: m.id,
					value: m.id,
					label: m.namec,
					parentId: m.pid,
				});
			});
			this.pid_list = this.$TOOL.changeTree(_tree);
		},
		async open(row) {
			this.initTree();
			if (!row) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmurgroup.model.get(row.id);
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
						res = await this.$API.scmurgroup.update.put(this.formData);
					} else {
						res = await this.$API.scmurgroup.add.post(this.formData);
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