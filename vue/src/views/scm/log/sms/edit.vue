<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="身份标识" prop="key">
				<el-input v-model="formData.key" placeholder="请输入身份标识" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="模板ID" prop="sms_id">
				<el-input v-model="formData.sms_id" placeholder="请输入模板ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="终端类型" prop="types">
				<el-input v-model="formData.types" placeholder="请输入终端类型" :maxlength="11" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="终端号码" prop="code">
				<el-input v-model="formData.code" placeholder="请输入终端号码" :maxlength="128" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="校验码" prop="sms">
				<el-input v-model="formData.sms" placeholder="请输入校验码" :maxlength="8" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="消息内容" prop="content">
				<el-input v-model="formData.content" placeholder="请输入消息内容" :maxlength="1024" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="发送次数" prop="send_qty">
				<el-input v-model="formData.send_qty" placeholder="请输入发送次数" :maxlength="11" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="发送时间" prop="send_time">
				<el-input v-model="formData.send_time" placeholder="请输入发送时间" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="过期时间" prop="expired">
				<el-input v-model="formData.expired" placeholder="请输入过期时间" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="发送状态" prop="handle">
				<el-input v-model="formData.handle" placeholder="请输入发送状态" :maxlength="11" show-word-limit
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
				key: '',
				sms_id: '',
				types: '',
				code: '',
				sms: '',
				content: '',
				send_qty: '',
				send_time: '',
				expired: '',
				handle: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmlogsms.edit.get(row.id);
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
						res = await this.$API.scmlogsms.update.put(this.formData);
					} else {
						res = await this.$API.scmlogsms.add.post(this.formData);
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