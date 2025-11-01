<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="键" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入键" :maxlength="30" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="值" prop="value">
				<el-input v-model="formData.value" placeholder="请输入值" :maxlength="30" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入名称" :maxlength="30" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="排序" prop="od" required>
				<el-slider v-model="formData.od" :max="100" :step="1"></el-slider>
			</el-form-item>
			<el-form-item label="备注">
				<el-input v-model="formData.remark" type="textarea" placeholder="请输入备注" :maxlength="100" show-word-limit
					:autosize="{ minRows: 2, maxRows: 4 }" :style="{ width: '100%' }"></el-input>
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
			isSaveing: false,
			visible: false,
			formData: this.def_data(),
			rules: {
				codec: [
					{ required: true, trigger: "blur", message: "字典键不能为空" },
				],
				value: [
					{ required: true, trigger: "blur", message: "字典值不能为空" },
					{ required: true, pattern: this.$SCM.REGEX_NUMBER, message: "字典值不是有效数字" },
				],
				namec: [
					{ required: true, trigger: "blur", message: "字典名称不能为空" },
				],
			},
		};
	},
	mounted() {
		if (this.$route.path === "/exam/setting") {
			this.formData.tag = 2;
		}
		if (this.$route.path === "/crm/config") {
			this.formData.tag = 3;
		}
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				tag: 1,
				dic_header_id: 0,
				namec: undefined,
				value: undefined,
				od: 1,
				remark: undefined,
			}
		},
		async open(row, type = "edit") {
			if (type == "add") {
				this.mode = type;
				this.formData.dic_header_id = row.id;
				this.formData.tag = row.type;
			} else {
				this.mode = type;
				var res = await this.$API.sysexportdetail.model.get(row.id);
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
						res = await this.$API.sysexportdetail.update.put(this.formData);
					} else {
						res = await this.$API.sysexportdetail.add.post(this.formData);
					}
					this.isSaveing = false;
					if (res.code == 200) {
						this.$emit("complete");
						this.visible = false;
						this.$message.success("操作成功");
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
