<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="审批流程" prop="flow_id">
				<sc-select v-model="formData.flow_id" placeholder="请输入单据代码" :data="flow_list"></sc-select>
			</el-form-item>
			<el-form-item label="单据代码" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入单据代码" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="单据名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入单据名称" :maxlength="64" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="单据地址" prop="url">
				<el-input v-model="formData.url" placeholder="请输入单据地址" :maxlength="256" show-word-limit
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
				flow_id: [
					{ required: true, trigger: "change", message: "请选择流程模板", pattern: this.$SCM.REGEX_ID },
				],
				codec: [
					{ required: true, trigger: "blur", message: "单据代码不能为空" },
					{ required: true, trigger: "blur", message: "单据代码应4至32个字符", pattern: this.$SCM.REGEX_CODEC },
				],
				namec: [
					{ required: true, trigger: "blur", message: "单据名称不能为空" },
					{ required: true, trigger: "blur", message: "单据名称应4至64个字符", pattern: this.$SCM.REGEX_NAMEC },
				],
				url: [
					{ required: true, trigger: "blur", message: "单据名称不能为空" }
				],
			},
			flow_list: [this.$SCM.OPTION_ALL],
		};
	},
	mounted() {
		this.$SCM.list_option(this.flow_list, this.$API.scmsysflowinfo.option, {}, false);
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				codec: '',
				namec: '',
				url: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmsysfloworder.edit.get(row.id);
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
					res = await this.$API.scmsysfloworder.update.put(this.formData);
				} else {
					res = await this.$API.scmsysfloworder.add.post(this.formData);
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