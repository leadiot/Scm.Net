<template>
	<el-card shadow="never" header="机构信息">
		<el-form ref="formRef" :model="form" :rules="rules" label-width="120px" style="margin-top: 20px">
			<el-form-item label="机构代码" prop="codec">
				<el-input v-model="form.codec" disabled></el-input>
				<div class="el-form-item-msg">
					机构代码用于标识登录用户的所属机构，不允许修改
				</div>
			</el-form-item>
			<el-form-item label="机构全称" prop="namec">
				<el-input v-model="form.namec"></el-input>
			</el-form-item>
			<el-form-item label="机构简称" prop="names">
				<el-input v-model="form.names"></el-input>
			</el-form-item>
			<el-form-item label="联系人" prop="contact">
				<el-input v-model="form.contact"></el-input>
			</el-form-item>
			<el-form-item label="联系手机" prop="cellphone">
				<el-input v-model="form.cellphone"></el-input>
			</el-form-item>
			<el-form-item label="固话" prop="telephone">
				<el-input v-model="form.telephone"></el-input>
			</el-form-item>
			<el-form-item>
				<el-button type="primary" @click="saveBasic">保存</el-button>
			</el-form-item>
		</el-form>
	</el-card>
</template>

<script>
export default {
	data() {
		return {
			sex_list: [],
			form: {
				id: this.$SCM.DEF_ID,
				codec: '',
				names: "",
				namec: "",
				telephone: "",
				contact: "",
				cellphone: "",
			},
			rules: {
				codec: [
					{ required: true, trigger: "blur", message: "请输入机构代码" },
				],
				namec: [
					{ required: true, trigger: "blur", message: "请输入机构全称" },
				],
				names: [
					{ required: true, trigger: "blur", message: "请输入机构简称" },
				],
				contact: [
					{ required: true, trigger: "change", message: "请输入联系人" },
				],
				cellphone: [
					{ required: true, trigger: "blur", message: "请输入联系手机" },
				],
			},
		};
	},
	mounted() {
		this.init();
	},
	methods: {
		async init() {
			const res = await this.$API.operator.unitWork.get();
			if (!res || res.code != 200) {
				return;
			}

			this.form = res.data;
		},
		async saveBasic() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					const res = await this.$API.operator.unitBasic.put(this.form);
					if (res.code == 200) {
						this.$message.success("保存成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				}
			});
		},
	},
};
</script>