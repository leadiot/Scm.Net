<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="终端ID" prop="terminal_id">
				<el-input v-model="formData.terminal_id" placeholder="请输入终端ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="驱动ID" prop="drive_id">
				<el-input v-model="formData.drive_id" placeholder="请输入驱动ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="目录ID" prop="dir_id">
				<el-input v-model="formData.dir_id" placeholder="请输入目录ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="名称" prop="name">
				<el-input v-model="formData.name" placeholder="请输入名称" :maxlength="256" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="路径" prop="path">
				<el-input v-model="formData.path" placeholder="请输入路径" :maxlength="2048" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="文档大小" prop="size">
				<el-input v-model="formData.size" placeholder="请输入文档大小" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="文档摘要" prop="hash">
				<el-input v-model="formData.hash" placeholder="请输入文档摘要" :maxlength="64" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="版本" prop="ver">
				<el-input v-model="formData.ver" placeholder="请输入版本" :maxlength="20" show-word-limit
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
					{ required: true, trigger: "blur", message: "编码应4至32个字符", pattern: this.$SCM.REGEX_CODEC },
				],
				namec: [
					{ required: true, trigger: "blur", message: "名称不能为空" },
					{ required: true, trigger: "blur", message: "名称应1至250个字符", pattern: this.$NAS.REGEX_NAMEC },
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
				ver: '0',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.nasressecret.edit.get(row.id);
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
					res = await this.$API.nasressecret.update.put(this.formData);
				} else {
					res = await this.$API.nasressecret.add.post(this.formData);
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