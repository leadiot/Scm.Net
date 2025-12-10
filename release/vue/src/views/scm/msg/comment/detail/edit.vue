<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="主题ID" prop="comment_id">
				<el-input v-model="formData.comment_id" placeholder="请输入主题ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="评分" prop="score">
				<el-input v-model="formData.score" placeholder="请输入评分" :maxlength="11" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="评论" prop="comment">
				<el-input v-model="formData.comment" placeholder="请输入评论" :maxlength="1024" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="回复ID" prop="rid">
				<el-input v-model="formData.rid" placeholder="请输入回复ID" :maxlength="20" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="引用ID" prop="pid">
				<el-input v-model="formData.pid" placeholder="请输入引用ID" :maxlength="20" show-word-limit
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
				comment_id: '',
				score: '',
				comment: '',
				rid: '',
				pid: '',

			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmmsgcommentdetail.edit.get(row.id);
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
						res = await this.$API.scmmsgcommentdetail.update.put(this.formData);
					} else {
						res = await this.$API.scmmsgcommentdetail.add.post(this.formData);
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