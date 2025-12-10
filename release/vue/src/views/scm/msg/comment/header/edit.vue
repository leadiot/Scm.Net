<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="评价编码" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入评价编码" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="引用ID" prop="rid">
				<el-input v-model="formData.rid" placeholder="请输入引用ID" :maxlength="20" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="主题说明" prop="remark">
				<el-input v-model="formData.remark" placeholder="请输入主题说明" :maxlength="256" show-word-limit
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
					{ required: true, trigger: "blur", message: "请输入评价编码", },
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
				codes: '',
				codec: '',
				rid: '',
				remark: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmmsgcommentheader.edit.get(row.id);
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
						res = await this.$API.scmmsgcommentheader.update.put(this.formData);
					} else {
						res = await this.$API.scmmsgcommentheader.add.post(this.formData);
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