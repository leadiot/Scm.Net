<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-row>
				<el-col :span="12">
					<el-form-item label="单位类型" prop="types">
						<sc-select v-model="formData.types" placeholder="请输入单位类型" :data="types_list"></sc-select>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item label="单位模式" prop="modes">
						<sc-select v-model="formData.modes" placeholder="请输入单位模式" :data="modes_list"></sc-select>
					</el-form-item>
				</el-col>
			</el-row>
			<el-row>
				<el-col :span="12">
					<el-form-item label="单位制式" prop="kinds">
						<sc-select v-model="formData.kinds" placeholder="请输入单位制式" :data="kinds_list"></sc-select>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item label="显示语言" prop="lang">
						<sc-select v-model="formData.lang" placeholder="请输入显示语言" :data="lang_list"></sc-select>
					</el-form-item>
				</el-col>
			</el-row>
			<el-row>
				<el-col :span="12">
					<el-form-item label="单位编码" prop="codec">
						<el-input v-model="formData.codec" placeholder="请输入单位编码" :maxlength="32" show-word-limit
							clearable></el-input>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item label="单位名称" prop="namec">
						<el-input v-model="formData.namec" placeholder="请输入单位名称" :maxlength="32" show-word-limit
							clearable></el-input>
					</el-form-item>
				</el-col>
			</el-row>
			<el-row>
				<el-col :span="12">
					<el-form-item label="单位符号" prop="symbol">
						<el-input v-model="formData.symbol" placeholder="请输入单位符号" :maxlength="8" show-word-limit
							clearable></el-input>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item label="显示排序" prop="od">
						<el-input-number v-model="formData.od" placeholder="请输入显示排序"></el-input-number>
					</el-form-item>
				</el-col>
			</el-row>
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
				types: [
					{ required: true, trigger: "change", message: "请选择以单位类型", pattern: this.$SCM.REGEX_INT },
				],
				modes: [
					{ required: true, trigger: "change", message: "请选择以单位模式", pattern: this.$SCM.REGEX_INT },
				],
				kinds: [
					{ required: true, trigger: "change", message: "请选择以单位制式", pattern: this.$SCM.REGEX_INT },
				],
				lang: [
					{ required: true, trigger: "change", message: "请选择以显示语言", pattern: this.$SCM.REGEX_INT },
				],
				codec: [
					{ required: true, trigger: "blur", message: "单位编码不能为空" }
				],
				namec: [
					{ required: true, trigger: "blur", message: "单位名称不能为空" }
				],
			},
			types_list: [this.$SCM.OPTION_ONE_INT],
			modes_list: [this.$SCM.OPTION_ONE_INT],
			kinds_list: [this.$SCM.OPTION_ONE_INT],
			lang_list: [this.$SCM.OPTION_ONE_INT],
		};
	},
	mounted() {
		this.$SCM.list_dic(this.types_list, 'uom_types', false);
		this.$SCM.list_dic(this.modes_list, 'uom_modes', false);
		this.$SCM.list_dic(this.kinds_list, 'uom_kinds', false);
		this.$SCM.list_dic(this.lang_list, 'lang', false);
	},
	methods: {
		def_data() {
			return {
				id: '0',
				types: this.$SCM.ID_ALL_INT,
				modes: this.$SCM.ID_ALL_INT,
				kinds: this.$SCM.ID_ALL_INT,
				od: 0,
				lang: this.$SCM.ID_ALL_INT,
				codec: '',
				namec: '',
				symbol: '',
			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmsysuom.edit.get(row.id);
				this.formData = res.data;
			}
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (!valid) {
					return;
				}

				this.isSaveing = true;
				let res = null;
				if (this.formData.id === '0') {
					res = await this.$API.scmsysuom.add.post(this.formData);
				} else {
					res = await this.$API.scmsysuom.update.put(this.formData);
				}
				this.isSaveing = false;

				if (res.code == 200) {
					this.$emit("complete");
					this.visible = false;
					this.$message.success("保存成功");
				} else {
					this.$alert(res.message, "提示", { type: "error" });
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