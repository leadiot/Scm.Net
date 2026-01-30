<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="450px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="目录名称" prop="name">
				<el-input v-model="formData.name" placeholder="请输入目录名称" :maxlength="256" show-word-limit
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
				name: [
					{ required: true, trigger: "blur", message: "名称不能为空" },
					{ required: true, trigger: "blur", message: "名称应4至64个字符", pattern: this.$SCM.REGEX_NAMEC },
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
				terminal_id: this.$SCM.DEF_ID,
				drive_id: this.$SCM.DEF_ID,
				dir_id: this.$SCM.DEF_ID,
				name: '',
				path: '',
				size: '',
				hash: '',
				ver: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.nasrespublic.edit.get(row.id);
				this.formData = res.data;
			}
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (!valid) {
					return;
				}

				this.isSaveing = true;
				let res = null;
				if (this.$SCM.is_valid_id(this.formData.id)) {
					res = await this.$API.nasrespublic.update.put(this.formData);
				} else {
					res = await this.$API.nasrespublic.add.post(this.formData);
				}
				this.isSaveing = false;

				if (res.code == 200) {
					this.$emit("complete");
					this.visible = false;
					this.$message.success("保存成功");
				} else {
					this.$alert(res.message, "提示", { type: "error" });
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

<style scoped>
.el-select {
	width: 100%;
}
</style>