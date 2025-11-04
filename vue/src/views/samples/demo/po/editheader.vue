<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="450px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="采购类型" prop="types">
				<sc-select v-model="formData.types" placeholder="请选择采购类型" :data="types_list"></sc-select>
			</el-form-item>
			<el-form-item label="采购单号" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入采购单号" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="备注" prop="remark">
				<el-input v-model="formData.remark" type="textarea" :rows="5" placeholder="请输入备注" :maxlength="128"
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
				types: [
					{ required: true, trigger: "change", message: "请选择采购类型", pattern: this.$SCM.REGEX_INT },
				],
				codec: [
					{ required: true, trigger: "blur", message: "采购编码不能为空" },
					{ required: true, trigger: "blur", message: "采购编码应4至32个字符", pattern: this.$SCM.REGEX_CODEC },
				]
			},
			types_list: [this.$SCM.OPTION_ONE_INT]
		};
	},
	mounted() {
		// this.$SCM.list_option(this.types_list, this.$API.scmmgrunit.option, {}, false);
		this.$SCM.list_dic(this.types_list, 'samples_po_types', false);
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				types: this.$SCM.ID_ONE_INT,
				codec: '',
				remark: ''
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.samplespoheader.edit.get(row.id);
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
					res = await this.$API.samplespoheader.update.put(this.formData);
				} else {
					res = await this.$API.samplespoheader.add.post(this.formData);
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