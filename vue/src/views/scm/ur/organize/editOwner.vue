<template>
	<sc-dialog v-model="visible" show-fullscreen :title="title" :destroy-on-close="true" width="450px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="组织负责人" prop="owner_id">
				<sc-select v-model="formData.owner_id" placeholder="请输入组织负责人" :data="user_list"></sc-select>
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
	emits: ['complete'],
	data() {
		return {
			title: '修改负责人',
			isSaveing: false,
			visible: false,
			formData: this.def_data(),
			rules: {
				owner_id: [
					{ required: true, trigger: "change", pattern: this.$SCM.REGEX_ID, message: "请选择主管人员", },
				]
			},
			user_list: [this.$SCM.OPTION_ONE]
		};
	},
	mounted() {

	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				owner_id: '0'
			}
		},
		async open(row) {
			if (!row || !row.id) {
				return;
			}

			this.formData.id = row.id;
			this.formData.owner_id = row.owner_id;

			this.$SCM.list_option(this.user_list, this.$API.scmuruser.option, { 'organize_id': row.id }, false);
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = await this.$API.scmurorganize.changeOwner.post(
						this.formData
					);
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
