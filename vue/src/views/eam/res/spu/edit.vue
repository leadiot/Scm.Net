<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="商品编码" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入商品编码" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="商品全称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入商品全称" :maxlength="64" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="商品简称" prop="names">
				<el-input v-model="formData.names" placeholder="请输入商品简称" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="分类ID" prop="cat_id">
				<el-input v-model="formData.cat_id" placeholder="请输入分类ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="说明" prop="description">
				<el-input v-model="formData.description" type="textarea" placeholder="请输入说明" :maxlength="1024"
					show-word-limit clearable></el-input>
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
				codec: '',
				names: '',
				namec: '',
				cat_id: this.$SCM.DEF_CAT_ID,
				description: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.eamresspu.edit.get(row.id);
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
					res = await this.$API.eamresspu.update.put(this.formData);
				} else {
					res = await this.$API.eamresspu.add.post(this.formData);
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