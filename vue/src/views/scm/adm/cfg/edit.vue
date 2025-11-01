<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="终端类型" prop="client">
				<sc-select v-model="formData.client" :data="client_list" />
			</el-form-item>
			<el-form-item label="键" prop="key">
				<el-input v-model="formData.key" placeholder="请输入键" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="值" prop="value">
				<el-input v-model="formData.value" placeholder="请输入值" :maxlength="256" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="数据类型" prop="data">
				<sc-select v-model="formData.data" :data="data_list" />
			</el-form-item>
			<el-form-item label="备注" prop="remark">
				<el-input v-model="formData.remark" type="textarea" placeholder="请输入备注" :maxlength="100" show-word-limit
					:autosize="{ minRows: 2, maxRows: 4 }"></el-input>
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
			client_list: [this.$SCM.OPTION_ONE_INT],
			data_list: [this.$SCM.OPTION_ONE_INT],
			rules: {
				client: [
					{ required: true, trigger: "blur", message: "请选择终端类型" },
				],
				key: [
					{ required: true, trigger: "blur", message: "配置键不能为空" },
				],
				value: [
					{ required: true, trigger: "blur", message: "配置值不能为空" },
				],
			},
		};
	},
	mounted() {
		this.$SCM.list_dic(this.client_list, 'client_type', false);
		this.$SCM.list_dic(this.data_list, 'data_type', false);
	},
	methods: {
		def_data() {
			return {
				id: '0',
				cat_id: '0',
				user_id: '0',
				client: this.$SCM.INT_ONE_INT,
				data: this.$SCM.INT_ONE_INT,
				key: undefined,
				value: undefined,
				remark: undefined,
			}
		},
		async open(row, type = "edit") {
			if (type == "add") {
				this.mode = type;
				this.formData.cat_id = row.id;
			} else {
				this.mode = type;
				var res = await this.$API.scmadmconfig.model.get(row.id);
				this.formData = res.data;
				// this.formData.client = '' + this.formData.client;
				// this.formData.data = '' + this.formData.data;
			}
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = null;
					if (this.formData.id === '0') {
						res = await this.$API.scmadmconfig.add.post(this.formData);
					} else {
						res = await this.$API.scmadmconfig.update.put(this.formData);
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
