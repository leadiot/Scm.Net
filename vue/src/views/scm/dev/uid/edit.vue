<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="420px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="键" prop="k">
				<el-input v-model="formData.k" placeholder="请输入键" :maxlength="32" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="值" prop="v">
				<el-input v-model="formData.v" placeholder="请输入值" :maxlength="20" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="预取数量" prop="c">
				<el-input-number v-model="formData.c" :min="1" :max="1024" placeholder="请输入预取数量" />
			</el-form-item>
			<el-form-item label="缓冲大小" prop="b">
				<el-input-number v-model="formData.b" :min="0" :max="1024" placeholder="请输入缓冲大小" />
			</el-form-item>
			<el-form-item label="数字位数" prop="l">
				<el-input-number v-model="formData.l" :min="0" :max="32" placeholder="请输入数字位数" />
			</el-form-item>
			<el-form-item label="前置掩码" prop="m">
				<el-input v-model="formData.m" placeholder="请输入前置掩码" :maxlength="8" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="后置掩码" prop="p">
				<el-input v-model="formData.p" placeholder="请输入后置掩码" :maxlength="8" show-word-limit
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
			rules: {
				k: [{ required: true, trigger: "blur", message: "请输入键" }],
				v: [{ required: true, trigger: "blur", message: "请输入值" }]
			},
		};
	},
	mounted() {
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				k: '',
				v: 0,
				c: 0,
				b: 0,
				l: 0,
				m: '',
				p: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmdevuid.edit.get(row.id);
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
						res = await this.$API.scmdevuid.update.put(this.formData);
					} else {
						res = await this.$API.scmdevuid.add.post(this.formData);
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

<style>
.el-input-number {
	width: 100%;
}
</style>