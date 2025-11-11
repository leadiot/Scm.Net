<template>
	<el-card shadow="never" header="修改密码">
		<el-alert title="密码更新成功后，您将被重定向到登录页面，您可以使用新密码重新登录。" type="info" show-icon style="margin-bottom: 15px;" />
		<el-form ref="form" :model="form" :rules="rules" label-width="120px" style="margin-top:20px;">
			<el-form-item label="当前密码" prop="oldPass">
				<el-input v-model="form.oldPass" type="password" show-password placeholder="请输入当前密码"></el-input>
				<div class="el-form-item-msg">必须提供当前登录用户密码才能进行更改</div>
			</el-form-item>
			<el-form-item label="新密码" prop="newPass">
				<el-input v-model="form.newPass" type="password" show-password placeholder="请输入新密码"></el-input>
				<sc-password-strength v-model="form.newPass"></sc-password-strength>
				<div class="el-form-item-msg">请输入包含英文、数字的8位以上密码</div>
			</el-form-item>
			<el-form-item label="确认新密码" prop="confirm">
				<el-input v-model="form.confirm" type="password" show-password placeholder="请再次输入新密码"></el-input>
			</el-form-item>
			<el-form-item>
				<el-button type="primary" @click="save">保存密码</el-button>
			</el-form-item>
		</el-form>
	</el-card>
</template>

<script>
import scPasswordStrength from '@/components/scPasswordStrength'

export default {
	components: {
		scPasswordStrength
	},
	data() {
		return {
			form: {
				oldPass: "",
				newPass: "",
				confirm: ""
			},
			rules: {
				oldPass: [
					{ required: true, message: '请输入当前密码' }
				],
				newPass: [
					{ required: true, message: '请输入新密码' }
				],
				confirm: [
					{ required: true, message: '请再次输入新密码' },
					{
						validator: (rule, value, callback) => {
							if (value !== this.form.newPass) {
								callback(new Error('两次输入密码不一致'));
							} else {
								callback();
							}
						}
					}
				]
			}
		}
	},
	methods: {
		save() {
			this.$refs.form.validate(async (valid) => {
				if (valid) {
					var oldPass = this.$CRYPTO.SHA(this.form.oldPass);
					var newPass = this.$CRYPTO.SHA(this.form.newPass);
					
					let res = await this.$API.operator.userPass.put({ oldPass: oldPass, newPass: newPass });
					if (res.code == 200) {
						this.$alert("密码修改成功，是否跳转至登录页使用新密码登录", "修改成功", {
							type: 'success',
							center: true
						}).then(() => {
							this.$router.replace({
								path: '/login'
							})
						}).catch(() => { })
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				} else {
					return false
				}
			})
		}
	}
}
</script>