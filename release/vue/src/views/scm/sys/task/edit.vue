<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="系统代码" prop="codes">
				<el-input v-model="formData.codes" placeholder="请输入系统代码" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="任务名称" prop="names">
				<el-input v-model="formData.names" placeholder="请输入任务名称" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="任务类型" prop="types">
				<el-input v-model="formData.types" placeholder="请输入任务类型" :maxlength="4" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="开始时间" prop="start_time">
				<el-input v-model="formData.start_time" placeholder="请输入开始时间" :maxlength="24" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="结束时间" prop="end_time">
				<el-input v-model="formData.end_time" placeholder="请输入结束时间" :maxlength="24" show-word-limit
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
			rules: {},
		};
	},
	mounted() {
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				codes: '',
				names: '',
				types: '',
				start_time: '0',
				start_time_txt: '',
				end_time: '',
				end_time_txt: ''
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmsystask.edit.get(row.id);
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
						res = await this.$API.scmsystask.update.put(this.formData);
					} else {
						res = await this.$API.scmsystask.add.post(this.formData);
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

<style scoped>
.el-select {
	width: 100%;
}
</style>