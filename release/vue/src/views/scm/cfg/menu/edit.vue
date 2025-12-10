<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="菜单ID" prop="menu_id">
				<el-input v-model="formData.menu_id" placeholder="请输入菜单ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="展示名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入展示名称" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="上级ID" prop="pid">
				<el-input v-model="formData.pid" placeholder="请输入上级ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="显示排序" prop="od">
				<el-input v-model="formData.od" placeholder="请输入显示排序" :maxlength="11" show-word-limit
					clearable></el-input>
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
	data() {
		return {
			mode: "add",
			titleMap: { add: "新增", edit: "编辑" },
			visible: false,
			isSaveing: false,
			formData: this.def_data(),
			rules: {
				codec: [
					{ required: true, trigger: "blur", message: "编码不能为空" },
				],
			},
		};
	},
	mounted() {
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				menu_id: '0',
				namec: '',
				pid: '0',
				od: '0',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmcfgmenu.edit.get(row.id);
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
						res = await this.$API.scmcfgmenu.update.put(this.formData);
					} else {
						res = await this.$API.scmcfgmenu.add.post(this.formData);
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