<template>
	<el-card shadow="never" header="个人信息">
		<el-form ref="formRef" :model="form" :rules="rules" label-width="120px" style="margin-top: 20px">
			<el-form-item label="用户代码" prop="codec">
				<el-input v-model="form.codec" disabled></el-input>
				<div class="el-form-item-msg">
					用户代码用于登录系统，不允许修改
				</div>
			</el-form-item>
			<el-form-item label="用户名称" prop="names">
				<el-input v-model="form.names"></el-input>
			</el-form-item>
			<el-form-item label="展示名称" prop="namec">
				<el-input v-model="form.namec"></el-input>
			</el-form-item>
			<el-form-item label="性别" prop="sex">
				<sc-select v-model="form.sex" placeholder="请选择" :data="sex_list"></sc-select>
			</el-form-item>
			<el-form-item label="手机号码" prop="cellphone">
				<el-input v-model="form.cellphone"></el-input>
			</el-form-item>
			<el-form-item label="电子邮件" prop="email">
				<el-input v-model="form.email"></el-input>
			</el-form-item>
			<el-form-item label="个性签名" prop="remark">
				<el-input v-model="form.remark" type="textarea"></el-input>
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
			form: {
				id: '0',
				codec: "",
				names: "",
				namec: "",
				cellphone: '',
				email: '',
				avatar: '',
				sex: 1,
				remark: '',
				role: [],
				post: [],
			},
			rules: {
				codec: [
					{ required: true, trigger: "blur", message: "请输入用户代码" },
				],
				namec: [
					{ required: true, trigger: "blur", message: "请输入用户名称" },
				],
				sex: [
					{ required: true, trigger: "blur", message: "请选择性别", pattern: this.$SCM.REGEX_INT },
				],
				cellphone: [
					{ required: true, trigger: "blur", message: "请输入手机号码" },
				],
				email: [
					{ required: true, trigger: "blur", message: "请输入电子邮件" },
				],
			},
			sex_list: [this.$SCM.OPTION_ONE_INT],
		};
	},
	mounted() {
		this.$SCM.list_dic(this.sex_list, 'sex', false);
		this.init();
	},
	methods: {
		async init() {
			const res = await this.$API.login.userwork.get();
			if (!res || res.code != 200) {
				return;
			}

			this.form = res.data;
		},
		async saveBasic() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					const user = this.$TOOL.data.get("USER_INFO");
					this.form.id = user.id;
					const res = await this.$API.login.userBasic.put(this.form);
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