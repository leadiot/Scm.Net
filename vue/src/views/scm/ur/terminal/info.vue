<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="title" width="450px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData">
			<el-form-item label="终端名称" prop="names">
				<el-input v-model="formData.names" readonly />
			</el-form-item>
			<el-form-item label="服务地址" prop="url">
				<el-input v-model="formData.url" readonly />
			</el-form-item>
			<el-form-item label="终端代码" prop="codes">
				<el-input v-model="formData.codes" readonly />
			</el-form-item>
			<el-form-item label="终端口令" prop="pass">
				<el-input v-model="formData.pass" readonly />
			</el-form-item>
		</el-form>

		<template #footer>
			<el-button type="primary" @click="close">确 定</el-button>
		</template>
	</sc-dialog>
</template>
<script>
export default {
	data() {
		return {
			title: '详情',
			visible: false,
			formData: {},
		};
	},
	mounted() {
		this.$SCM.list_dic(this.types_list, 'client_type', false);
	},
	methods: {
		async open(row) {
			this.formData = row;
			this.formData.url = this.$CONFIG.SERVER_URL;
			this.visible = true;
		},
		close() {
			this.formData = {};
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