<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="接收人" prop="user_id">
				<sc-select v-model="formData.user_id" placeholder="请选择接收人" :data="user_list" />
			</el-form-item>
			<el-form-item label="标签" prop="tags">
				<sc-select v-model="formData.tags" :data="tags_list" multiple placeholder="请选择标签"></sc-select>
			</el-form-item>
			<el-form-item label="消息标题" prop="title">
				<el-input v-model="formData.title" placeholder="请输入消息标题" :maxlength="32" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="消息内容" prop="remark">
				<el-input v-model="formData.remark" placeholder="请输入消息内容" type="textarea" :rows="5" :maxlength="1024"
					show-word-limit clearable></el-input>
			</el-form-item>
		</el-form>

		<template #footer>
			<el-button @click="close">取 消</el-button>
			<el-button :loading="isSaveing" type="primary" @click="save">确 定</el-button>
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
			user_list: [],
			tags_list: [],
			rules: {
				user_id: [
					{ required: true, trigger: "blur", pattern: this.$SCM.REGEX_ID, message: "请选择接收人" },
				],
				title: [
					{ required: true, trigger: "blur", message: "请输入消息标题" },
				],
				remark: [
					{ required: true, trigger: "blur", message: "请输入消息内容" },
				],
			},
		};
	},
	mounted() {
		this.$SCM.list_option(this.user_list, this.$API.scmuruser.option, {}, false);
		this.$SCM.list_tag(this.tags_list, this.$API.scmmsgmessage.SYS_ID, false);
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				tags: ['0'],
				title: '',
				remark: ''
			};
		},
		async open(row) {
			if (!row) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmmsgmessage.model.get(row.id);
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
						res = await this.$API.scmmsgmessage.update.put(this.formData);
					} else {
						res = await this.$API.scmmsgmessage.add.post(this.formData);
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