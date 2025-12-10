<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="450px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="模板类型" prop="types">
				<sc-select v-model="formData.types" placeholder="请选择模板类型" :data="types_list"></sc-select>
			</el-form-item>
			<el-form-item label="模板代码" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入模板代码" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="模板名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入模板名称" :maxlength="64" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="模板标题" prop="head">
				<el-input v-model="formData.head" placeholder="请输入模板标题" :maxlength="128" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="模板内容" prop="body">
				<el-input v-model="formData.body" placeholder="请输入模板内容" :maxlength="512" show-word-limit clearable
					type="textarea"></el-input>
			</el-form-item>
			<el-form-item label="模板声明" prop="foot">
				<el-input v-model="formData.foot" placeholder="请输入模板声明" :maxlength="128" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="模板文件" prop="file">
				<el-input v-model="formData.file" placeholder="请输入模板文件" :maxlength="64" show-word-limit
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
				types: [
					{ required: true, trigger: "change", pattern: this.$SCM.REGEX_INT, message: "请选择应用", },
				],
				codec: [
					{ required: true, trigger: "blur", message: "请输入模板代码！", },
				],
				namec: [
					{ required: true, trigger: "blur", message: "请输入模板名称！", },
				],
				head: [
					{ required: true, trigger: "blur", message: "请输入模板标题！", },
				],
				body: [
					{ required: true, trigger: "blur", message: "请输入模板内容！", },
				],
			},
			types_list: [this.$SCM.OPTION_ONE_INT],
		};
	},
	mounted() {
		this.$SCM.list_dic(this.types_list, 'sms_types', false);
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				types: this.$SCM.ID_ALL_INT,
				codec: '',
				namec: '',
				head: '',
				body: '',
				foot: '',
				file: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmresotp.edit.get(row.id);
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
						res = await this.$API.scmresotp.update.put(this.formData);
					} else {
						res = await this.$API.scmresotp.add.post(this.formData);
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

<style scoped>
.el-select {
	width: 100%;
}
</style>