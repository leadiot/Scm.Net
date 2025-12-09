<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="480px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="流程类型" prop="types">
				<sc-select v-model="formData.types" placeholder="请选择流程类型"></sc-select>
			</el-form-item>
			<el-form-item label="流程名称" prop="title">
				<el-input v-model="formData.title" placeholder="请输入流程名称" :maxlength="128" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="图标" prop="icon">
				<el-input v-model="formData.icon" placeholder="请输入图标" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="拒绝流转" prop="refused">
				<sc-select v-model="formData.refused" placeholder="请选择审批被拒后重新提交" clearable :style="{ width: '100%' }"
					:data="refusedOptions">
				</sc-select>
			</el-form-item>
			<el-form-item label="流程说明" prop="summary">
				<el-input v-model="formData.summary" type="textarea" placeholder="请输入流程说明 " :maxlength="200"
					show-word-limit :autosize="{ minRows: 4, maxRows: 4 }" :style="{ width: '100%' }"></el-input>
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
				title: [
					{ required: true, trigger: "blur", message: "请输入审批流名称" },
				],
				refused: [
					{ required: true, trigger: "change", message: "请选择审批被拒后重新提交" },
				],
				icon: [
					{ required: true, trigger: "blur", message: "请输入图标" },
				],
				summary: [
					{ required: true, trigger: "blur", message: "请输入流程说明 " },
				],
			},
			type_list: [{ value: 1, label: "默认" }],
			refusedOptions: [
				{ value: 1, label: "返回审批流初始层级" },
				{ value: 2, label: "返回到上一级" },
			],
		};
	},
	mounted() {
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				app_id: '',
				types: 0,
				title: '',
				icon: '',
				refused: 1,
				summary: ''
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmsysflowinfo.edit.get(row.id);
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
					res = await this.$API.scmsysflowinfo.update.put(this.formData);
				} else {
					res = await this.$API.scmsysflowinfo.add.post(this.formData);
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