<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="应用类型" prop="types">
				<sc-select v-model="formData.types" :data="app_types_list"></sc-select>
			</el-form-item>
			<el-form-item label="应用代码" prop="code">
				<el-input v-model="formData.code" placeholder="请输入应用代码" :maxlength="16" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="应用名称" prop="name">
				<el-input v-model="formData.name" placeholder="请输入应用标题" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="应用简介" prop="content">
				<el-input v-model="formData.content" placeholder="请输入应用说明" :maxlength="1024" show-word-limit clearable
					type="textarea" rows="6"></el-input>
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
					{ required: true, trigger: "change", message: "请选择应用类型", },
				],
				code: [
					{ required: true, trigger: "blur", message: "请输入应用代码", },
				],
				name: [
					{ required: true, trigger: "blur", message: "请输入应用名称", },
				],
				content: [
					{ required: true, trigger: "blur", message: "请输入应用简介", },
				],
			},
			app_types_list: [this.$SCM.OPTION_ONE_INT],
		};
	},
	mounted() {
		this.$SCM.list_dic(this.app_types_list, 'app_types', false);
	},
	methods: {
		def_data() {
			return {
				id: '0',
				types: this.$SCM.ID_ONE_INT,
				code: '',
				name: '',
				content: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmdevapp.edit.get(row.id);
				this.formData = res.data;
			}
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = null;
					if (this.formData.id === '0') {
						res = await this.$API.scmdevapp.add.post(this.formData);
					} else {
						res = await this.$API.scmdevapp.update.put(this.formData);
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