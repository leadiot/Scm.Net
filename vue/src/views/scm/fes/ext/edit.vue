<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="450px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="文件类型" prop="types">
				<sc-select v-model="formData.types" placeholder="请选择文件类型" :data="types_list"></sc-select>
			</el-form-item>
			<el-form-item label="后缀代码" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入后缀代码" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="后缀名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入后缀名称" :maxlength="64" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="文件签名" prop="sign">
				<el-input v-model="formData.sign" placeholder="请输入文件签名" :maxlength="1" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="所属组织" prop="org_id">
				<sc-select v-model="formData.org_id" placeholder="请选择所属组织" :data="org_list"></sc-select>
			</el-form-item>
			<el-form-item label="所属应用" prop="app_id">
				<sc-select v-model="formData.app_id" placeholder="请选择所属应用" :data="app_list"></sc-select>
			</el-form-item>
			<el-form-item label="备注" prop="remark">
				<el-input v-model="formData.remark" placeholder="请输入备注" :maxlength="256" show-word-limit clearable
					type="textarea" :rows="5"></el-input>
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
					{ required: true, trigger: "change", message: "请选择文件类型", pattern: this.$SCM.REGEX_INT },
				],
				codec: [
					{ required: true, trigger: "blur", message: "编码不能为空" },
				]
			},
			types_list: [this.$SCM.OPTION_ONE_INT],
			org_list: [this.$SCM.OPTION_ONE],
			app_list: [this.$SCM.OPTION_ONE],
		};
	},
	mounted() {
		this.$SCM.list_dic(this.types_list, 'file_type', false);
		this.$SCM.list_option(this.org_list, this.$API.scmfesorg.option, {}, true);
	},
	methods: {
		def_data() {
			return {
				id: '0',
				types: this.$SCM.ID_ONE_INT,
				codec: '',
				sign: '',
				org_id: this.$SCM.ID_ALL,
				app_id: this.$SCM.ID_ALL,
				remark: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmfesext.edit.get(row.id);
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
				if (this.formData.id === '0') {
					res = await this.$API.scmfesext.add.post(this.formData);
				} else {
					res = await this.$API.scmfesext.update.put(this.formData);
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
		changeOrg() {
			this.$SCM.list_option(this.app_list, this.$API.scmfesapp.option, { 'org_id': this.formData.org_id }, true);
		}
	},
};
</script>

<style scoped>
.el-select {
	width: 100%;
}
</style>