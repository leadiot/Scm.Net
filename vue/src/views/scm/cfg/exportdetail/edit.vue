<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="显示排序" prop="od">
				<el-input v-model="formData.od" placeholder="请输入显示排序" :maxlength="11" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="字段" prop="col">
				<el-input v-model="formData.col" placeholder="请输入字段" :maxlength="256" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="标题" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入标题" :maxlength="64" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="默认值" prop="def">
				<el-input v-model="formData.def" placeholder="请输入默认值" :maxlength="256" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="表达式" prop="fun">
				<el-input v-model="formData.fun" placeholder="请输入表达式" :maxlength="256" show-word-limit clearable></el-input>
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
				col: [
					{ required: true, trigger: "blur", message: "请输入字段" },
				],
				namec: [
					{ required: true, trigger: "blur", message: "请输入标题" },
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
				export_id: '',
				od: '',
				col: '',
				namec: '',
				def: '',
				fun: '',
			}
		},
		async open(row, hid) {
			if (!row || !row.id) {
				this.formData.export_id = hid;
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmcfgexportdetail.edit.get(row.id);
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
						res = await this.$API.scmcfgexportdetail.update.put(this.formData);
					} else {
						res = await this.$API.scmcfgexportdetail.add.post(this.formData);
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