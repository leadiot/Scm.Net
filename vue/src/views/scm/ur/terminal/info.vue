<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="title" width="670px" @close="close">
		<el-row>
			<el-col :span="12">
				<div style="text-align: center;">
					<scQrcode ref="qrcode" :text="qrCode" :size="200"></scQrcode>
				</div>
			</el-col>
			<el-col :span="12">
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
			</el-col>
		</el-row>

		<template #footer>
			<el-button type="primary" @click="close">确 定</el-button>
		</template>
	</sc-dialog>
</template>
<script>
import scQrcode from '@/components/scQrcode';
export default {
	components: {
		scQrcode,
	},
	data() {
		return {
			title: '详情',
			visible: false,
			formData: {},
			qrCode: ''
		};
	},
	mounted() {
		this.$SCM.list_dic(this.types_list, 'client_type', false);
	},
	methods: {
		async open(row) {
			this.formData = row;
			this.formData.url = this.$CONFIG.SERVER_URL;
			if (!this.formData.url) {
				this.formData.url = window.location.host;
			}
			this.visible = true;
			this.qrCode = this.formData.url + ';' + this.formData.codes + ';' + this.formData.pass;
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