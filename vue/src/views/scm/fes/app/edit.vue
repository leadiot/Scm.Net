<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="组织ID" prop="org_id">
				<sc-select v-model="formData.org_id" placeholder="请选择所属组织" :data="org_list"></sc-select>
			</el-form-item>
			<el-form-item label="应用代码" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入应用代码" :maxlength="16" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="应用名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入应用名称" :maxlength="64" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="应用说明" prop="remark">
				<el-input v-model="formData.remark" placeholder="请输入应用说明" type="textarea" :maxlength="128" :rows="5"
					show-word-limit></el-input>
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
				org_id: [
					{ required: true, trigger: "change", message: "请选择所属组织", pattern: this.$SCM.REGEX_ID },
				],
				codec: [
					{ required: true, trigger: "blur", message: "编码不能为空" },
					{ required: true, trigger: "blur", message: "编码应4至32个字符", pattern: this.$SCM.REGEX_CODEC },
				],
				namec: [
					{ required: true, trigger: "blur", message: "名称不能为空" },
					{ required: true, trigger: "blur", message: "名称应4至64个字符", pattern: this.$SCM.REGEX_NAMEC },
				],
			},
			org_list: [this.$SCM.OPTION_ONE],
		};
	},
	mounted() {
		this.$SCM.list_option(this.org_list, this.$API.scmfesorg.option, {}, true);
	},
	methods: {
		def_data() {
			return {
				id: '0',
				org_id: '',
				codec: '',
				namec: '',
				remark: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmfesapp.edit.get(row.id);
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
					res = await this.$API.scmfesapp.add.post(this.formData);
				} else {
					res = await this.$API.scmfesapp.update.put(this.formData);
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