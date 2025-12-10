<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="700px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="书籍类型" prop="types">
				<sc-select v-model="formData.types" placeholder="请选择书籍类型" :data="types_list" />
			</el-form-item>
			<el-form-item label="书籍编码" prop="codec">
				<el-input v-model="formData.codec" clearable :maxlength="32" placeholder="请输入书籍编码" show-word-limit />
			</el-form-item>
			<el-form-item label="书籍全称" prop="namec">
				<el-input v-model="formData.namec" clearable :maxlength="128" placeholder="请输入书籍全称" show-word-limit />
			</el-form-item>
			<el-form-item label="书籍简称" prop="names">
				<el-input v-model="formData.names" clearable :maxlength="32" placeholder="请输入书籍简称" show-word-limit />
			</el-form-item>
			<el-form-item label="条码" prop="barcode">
				<el-input v-model="formData.barcode" clearable :maxlength="32" placeholder="请输入书籍条码" show-word-limit />
			</el-form-item>
			<el-form-item label="备注" prop="remark">
				<el-input v-model="formData.remark" :autosize="{ minRows: 2, maxRows: 4 }" :maxlength="500"
					placeholder="请输入备注" show-word-limit type="textarea" />
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
			mode: "add",
			titleMap: { add: "新增", edit: "编辑" },
			visible: false,
			isSaveing: false,
			formData: {
				id: this.$SCM.DEF_ID,
				codec: '',
				namec: '',
				names: '',
				barcode: '',
				remark: ''
			},
			rules: {
				codec: [
					{ required: true, trigger: "blur", message: "书籍编码不能为空", },
					{ required: true, pattern: /^[a-zA-Z0-9]{4,32}$/, message: "无效的书籍编码", },
				],
				namec: [
					{ required: true, trigger: "blur", message: "书籍全称不能为空", }
				],
			},
		};
	},
	mounted() {
	},
	methods: {
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.samplesbook.view.get(row.id);
				this.formData = res.data || {};
			}
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = null;
					if (this.$SCM.is_valid_id(this.formData.id)) {
						res = await this.$API.samplesbook.save.put(this.formData);
					} else {
						res = await this.$API.samplesbook.add.post(this.formData);
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
			this.formData = {
				id: this.$SCM.DEF_ID,
				option_id: 0,
				text: ''
			};
			this.$refs.formRef.resetFields();
			this.visible = false;
		},
	},
};
</script>
