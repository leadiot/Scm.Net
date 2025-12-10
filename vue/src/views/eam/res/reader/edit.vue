<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="450px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="设备代码" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入设备代码" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="设备全称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入设备全称" :maxlength="64" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="设备简称" prop="names">
				<el-input v-model="formData.names" placeholder="请输入设备简称" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="行动方向" prop="dir">
				<sc-select v-model="formData.dir" :data="dir_list" placeholder="请选择行动方向">
				</sc-select>
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
					{ required: true, trigger: "blur", message: "设备编码不能为空" },
					{ required: true, trigger: "blur", message: "设备编码应4至32个字符", pattern: this.$SCM.REGEX_CODEC },
				],
				namec: [
					{ required: true, trigger: "blur", message: "设备名称不能为空" },
					{ required: true, trigger: "blur", message: "设备名称应4至64个字符", pattern: this.$SCM.REGEX_NAMEC },
				],
				dir: [
					{ required: true, trigger: "change", message: "行动方向不能为空" }
				],
			},
			dir_list: [this.$SCM.OPTION_ONE_INT],
		};
	},
	mounted() {
		this.$SCM.list_dic(this.dir_list, 'eam_reader_dir', false);
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				codec: '',
				names: '',
				namec: '',
				dir: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.eamresreader.edit.get(row.id);
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
					res = await this.$API.eamresreader.update.put(this.formData);
				} else {
					res = await this.$API.eamresreader.add.post(this.formData);
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