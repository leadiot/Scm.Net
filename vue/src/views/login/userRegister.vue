<template>
	<common-page title="用户注册">
		<el-steps :active="stepActive" simple finish-status="success">
			<el-step title="基础信息" />
			<el-step title="详细信息" />
			<el-step title="完成注册" />
		</el-steps>
		<el-form v-if="stepActive == 0" ref="stepForm_0" :model="formData" :rules="rules" :label-width="120">
			<el-form-item label="登录账号" prop="user">
				<el-input v-model="formData.user" placeholder="请输入登录账号"></el-input>
				<div class="el-form-item-msg">登录账号将作为当前企业下管理员账户，建议使用英文字母或数字。</div>
			</el-form-item>
			<el-form-item label="登录密码" prop="password1">
				<el-input v-model="formData.password1" type="password" show-password placeholder="请输入登录密码"></el-input>
				<sc-password-strength v-model="formData.password1"></sc-password-strength>
				<div class="el-form-item-msg">请输入包含英文、数字的8位以上密码</div>
			</el-form-item>
			<el-form-item label="确认密码" prop="password2">
				<el-input v-model="formData.password2" type="password" show-password
					placeholder="请再一次输入登录密码"></el-input>
			</el-form-item>
			<el-form-item label="" prop="agree">
				<el-checkbox v-model="formData.agree" label="">已阅读并同意</el-checkbox><span class="link"
					@click="showAgree = true">《平台服务协议》</span>
			</el-form-item>
		</el-form>
		<el-form v-if="stepActive == 1" ref="stepForm_1" :model="formData" :rules="rules" :label-width="120">
			<el-form-item label="真实姓名" prop="user_name">
				<el-input v-model="formData.user_name" placeholder="请输入真实姓名"></el-input>
			</el-form-item>
			<el-form-item label="手机号码" prop="phone">
				<el-input v-model="formData.phone" placeholder="请输入手机号码"></el-input>
			</el-form-item>
			<el-form-item label="电子邮件" prop="email">
				<el-input v-model="formData.email" placeholder="请输入电子邮件"></el-input>
			</el-form-item>
			<el-form-item label="开通服务" prop="open">
				<el-checkbox-group v-model="formData.open">
					<el-checkbox label="1">订单管理系统</el-checkbox>
					<el-checkbox label="2">仓储管理系统</el-checkbox>
					<el-checkbox label="3">园区管理系统</el-checkbox>
				</el-checkbox-group>
			</el-form-item>
		</el-form>
		<div v-if="stepActive == 2">
			<el-result icon="success" title="注册成功">
				<template #extra>
					<div style="margin-bottom: var(--el-result-extra-margin-top);">
						您可以使用 <el-text type="success">{{ signInName }}</el-text> 登录系统！
					</div>
					<el-button type="primary" @click="goLogin">前去登录</el-button>
				</template>
			</el-result>
		</div>
		<el-form style="text-align: center;" v-if="userRegister">
			<el-button v-if="stepActive > 0 && stepActive < 2" @click="pre()">上一步</el-button>
			<el-button v-if="stepActive < 1" type="primary" @click="next()">下一步</el-button>
			<el-button v-if="stepActive == 1" type="primary" @click="save()">提交</el-button>
		</el-form>
		<el-dialog v-model="showAgree" title="平台服务协议" :width="800" destroy-on-close>
			平台服务协议
			<template #footer>
				<el-button @click="showAgree = false">取消</el-button>
				<el-button type="primary" @click="showAgree = false; formData.agree = true;">我已阅读并同意</el-button>
			</template>
		</el-dialog>
	</common-page>
</template>

<script>
import scPasswordStrength from '@/components/scPasswordStrength';
import commonPage from './components/commonPage'

export default {
	name: 'user_register',
	components: {
		commonPage,
		scPasswordStrength
	},
	data() {
		return {
			stepActive: 0,
			showAgree: false,
			formData: this.def_data(),
			rules: {
				unit_name: [{ required: true, message: '请输入企业全称' }],
				unit: [{ required: true, message: '请输入企业代码' }],
				user_name: [{ required: true, message: '请输入真实姓名' }],
				user: [{ required: true, message: '请输入登录用户' }],
				password1: [{ required: true, message: '请输入登录密码' }],
				password2: [
					{ required: true, message: '请输入确认密码' },
					{
						validator: (rule, value, callback) => {
							if (value !== this.formData.password1) {
								callback(new Error('两次输入密码不一致'));
							} else {
								callback();
							}
						}
					}
				],
				agree: [
					{
						validator: (rule, value, callback) => {
							if (!value) {
								callback(new Error('请阅读并同意协议'));
							} else {
								callback();
							}
						}
					}
				],
				email: [{ required: true, message: '请输入电子邮件' }],
				open: [
					{ required: true, message: '请选择开通类别' }
				]
			},
			signInName: 'user@unit',
			userRegister: this.$CONFIG.USER_REGISTER_ENABLED
		}
	},
	mounted() {
		this.formData.user = this.getUser();
	},
	methods: {
		def_data() {
			return {
				type: this.$CONFIG.DEF_LOGIN_TYPE,
				mode: 10,//口令登录
				unit: this.$CONFIG.DEF_LOGIN_UNIT,
				user: '',
				pass: '',
				password1: '',
				password2: '',
				agree: false,
				unit_name: '',
				user_name: '',
				email: '',
				phone: '',
				open: []
			}
		},
		pre() {
			this.stepActive -= 1
		},
		next() {
			const formName = `stepForm_${this.stepActive}`
			this.$refs[formName].validate((valid) => {
				if (valid) {
					this.stepActive += 1
				} else {
					return false
				}
			})
		},
		save() {
			const formName = `stepForm_${this.stepActive}`
			this.$refs[formName].validate((valid) => {
				if (valid) {
					this.signon();
				}
			});
		},
		async signon() {
			var form = {
				type: this.formData.type,
				mode: this.formData.mode,
				unit: this.formData.unit,
				user: this.formData.user,
				pass: this.$CRYPTO.SHA(this.formData.password1),
				user_name: this.formData.user_name,
				email: this.formData.email,
				phone: this.formData.phone,
				open: []
			};
			var res = await this.$API.operator.signon.post(form);
			if (res.code != 200) {
				this.$message.warning(res.message);
				return false;
			}
			var data = res.data;
			if (!data.success) {
				this.$message.warning(data.message);
				return false;
			}

			this.stepActive += 1;
			this.signInName = this.getFullUser();
		},
		/** 获取推荐登录用户名称 */
		getUser() {
			return 'admin';
		},
		/** 获取完整登录用户名称 */
		getFullUser() {
			return this.formData.user;
		},
		goLogin() {
			this.$router.push({
				path: '/login'
			})
		}
	}
}
</script>
