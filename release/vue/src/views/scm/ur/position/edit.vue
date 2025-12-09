<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="450px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="岗位编号" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入岗位编号,例如：10001" :maxlength="6" show-word-limit
					clearable :style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="岗位名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入岗位名称" :maxlength="25" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="排序" prop="od" required>
				<el-input-number v-model="formData.od" :min="0"></el-input-number>
			</el-form-item>
			<el-form-item label="备注" prop="remark">
				<el-input v-model="formData.remark" type="textarea" placeholder="请输入备注" :maxlength="500" show-word-limit
					:autosize="{ minRows: 2, maxRows: 3 }" :style="{ width: '100%' }"></el-input>
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
			titleMap: {
				add: "新增",
				edit: "编辑",
			},
			isSaveing: false,
			visible: false,
			formData: {
				id: this.$SCM.DEF_ID,
				codec: undefined,
				namec: undefined,
				od: 1,
				remark: undefined,
			},
			rules: {
				codec: [
					{
						required: true,
						message: "请输入岗位编号",
						trigger: "blur",
					},
				],
				namec: [
					{
						required: true,
						message: "请输入岗位名称",
						trigger: "blur",
					},
				],
				remark: [
					{
						required: false,
						message: "请输入备注",
						trigger: "blur",
					},
				],
			},
			statusOptions: [
				{
					label: "正常",
					value: true,
				},
				{
					label: "停用",
					value: false,
				},
			],
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
				var res = await this.$API.scmurposition.model.get(row.id);
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
						res = await this.$API.scmurposition.update.put(this.formData);
					} else {
						res = await this.$API.scmurposition.add.post(this.formData);
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
			this.formData = {
				id: this.$SCM.DEF_ID,
				codec: undefined,
				namec: undefined,
				od: 1,
				remark: undefined,
			};
			this.$refs.formRef.resetFields();
			this.visible = false;
		},
	},
};
</script>
