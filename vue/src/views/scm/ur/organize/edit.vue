<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" :destroy-on-close="true" width="450px"
		@close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="所属组织" prop="pid">
				<el-tree-select v-model="formData.pid" :data="parentIdOptions" :default-expand-all="true"
					:check-strictly="true" :style="{ width: '100%' }" />
			</el-form-item>
			<el-form-item label="组织名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入组织名称" clearable></el-input>
			</el-form-item>
			<el-form-item label="组织编码">
				<el-input v-model="formData.codec" placeholder="请输入组织编码" clearable></el-input>
			</el-form-item>
			<el-form-item label="排序" prop="od" required>
				<el-input-number v-model="formData.od" :min="0" />
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
				// pid: [
				// 	{ required: true, trigger: "change", pattern: this.$SCM.REGEX_ID, message: "请选择所属组织", },
				// ],
				namec: [
					{ required: true, trigger: "blur", message: "请输入组织名称", },
				],
			},
			parentIdOptions: [],
			parentIdProps: {
				expandTrigger: "hover",
				//checkStrictly: true,
			},
		};
	},
	mounted() { },
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				pid: undefined,
				namec: "",
				od: 1,
			}
		},
		async initTree() {
			const t = await this.$API.scmurorganize.list.get();
			let _tree = [{ id: "1", value: "0", label: "（默认）", parentId: "0" }];
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
			if (!row) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmurorganize.model.get(row.id);
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
						res = await this.$API.scmurorganize.update.put(this.formData);
					} else {
						res = await this.$API.scmurorganize.add.post(this.formData);
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
