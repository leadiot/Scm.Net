<template>
	<sc-dialog v-model="visible" :title="titleMap[mode]" width="600px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="投票标题" prop="title">
				<el-input v-model="formData.title" placeholder="请输入投票标题" :maxlength="128" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="投票类型" prop="type">
				<el-radio-group v-model="formData.type">
					<el-radio v-for="(item, index) in type_list" :key="index" :label="item.value"
						:disabled="item.disabled">{{ item.label }}</el-radio>
				</el-radio-group>
			</el-form-item>
			<el-form-item label="开始时间" prop="start_time">
				<el-col :span="11">
					<el-date-picker v-model="formData.start_time" type="date" placeholder="开始时间" style="width: 100%" />
				</el-col>
				<el-col :span="2" style="text-align: center;">
					<span class="text-gray-500">-</span>
				</el-col>
				<el-col :span="11">
					<el-date-picker v-model="formData.end_time" type="date" placeholder="结束时间" style="width: 100%" />
				</el-col>
			</el-form-item>
			<el-form-item label="防刷规则" prop="swipe_rule">
				<el-switch v-model="formData.swipe_rule"></el-switch>
				<div class="alert">
					开启后，一个IP只能投票一次
				</div>
			</el-form-item>
			<el-form-item label="规则说明" prop="summary">
				<el-input v-model="formData.summary" type="textarea" placeholder="请输入投票规则" :maxlength="1024"
					show-word-limit :autosize="{ minRows: 5, maxRows: 8 }"></el-input>
			</el-form-item>
		</el-form>

		<template #footer>
			<el-button @click="close">取 消</el-button>
			<el-button :loading="isSaveing" type="primary" @click="save">
				提 交
			</el-button>
		</template>
	</sc-dialog>
</template>

<script>
export default {
	emits: ["complete"],
	data() {
		return {
			mode: "add",
			titleMap: { add: "新增", edit: "编辑" },
			isSaveing: false,
			visible: false,
			formData: this.def_data(),
			type_list: [
				{ label: "单选", value: 1 },
				{ label: "多选", value: 2 },
			],
			rules: {
				title: [
					{ required: true, trigger: "blur", message: "请输入投票标题" },
				],
				start_time: [
					{ required: true, trigger: "blur", message: "请选择开始时间" },
				],
				end_time: [
					{ required: true, trigger: "blur", message: "请选择结束时间" },
				],
				type: [
					{ required: true, trigger: "change", message: "投票类型不能为空" },
				]
			},
		};
	},
	mounted() {

	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				title: "",
				type: 1,
				start_time: "",
				end_time: "",
				tick_rule: 1,
				swipe_rule: true,
				summary: "",
			}
		},
		async open(row) {
			if (!row) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmsysvoteheader.model.get(row.id);
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
						res = await this.$API.scmsysvoteheader.update.put(this.formData);
					} else {
						res = await this.$API.scmsysvoteheader.add.post(this.formData);
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
<style>
.alert {
	padding-left: 20px;
	font-size: 12px;
	color: #999;
}
</style>
