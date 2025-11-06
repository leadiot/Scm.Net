<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				{{ title }}
			</div>
			<div class="right-panel">
			</div>
		</el-header>
		<el-main>
			<div class="common-container">
				<div class="common-main el-card">
					<el-steps :active="stepActive" finish-status="success" :space="520" simple>
						<el-step title="开启服务" />
						<el-step title="开启完成" />
					</el-steps>
					<div v-if="stepActive == 0">
						<div class="block">
							<el-alert type="primary" :closable="false">
								授信登录（TOTP）是一种安全的登录方式，其采用基于时间同步的原理，用户在需要登录时输入动态生成的一次性口令，以完成用户身份验证。
								授信登录可以有效的帮助用户保护自己的账号不被他人盗用，并且让用户不再需要记住复杂的密码。
							</el-alert>
							<p>
								您可以使用第三方 TOTP 身份验证应用扫描下方二维码，开户授信登录服务。系统在您扫描成功后周期性生成一个随机口令，该口令可以让您更简单、更安全的登录系统。
							</p>
							<p>
								常见的 TOTP 身份验证应用有：
								<el-link type="primary" href="https://www.microsoft.com/en-us/account/authenticator"
									target="_blank">
									Microsoft Authenticator
								</el-link>、
								<el-link type="primary" href="https://www.google.com/intl/zh-CN/authenticator/"
									target="_blank">
									Google Authenticator
								</el-link>、
								<el-link type="primary" href="https://1password.com/" target="_blank">
									1Password
								</el-link>、
								<el-link type="primary" href="https://authy.com/" target="_blank">
									Authy
								</el-link>等。
							</p>
							<p>请使用 TOTP 身份验证应用扫描下方二维码：</p>
							<p class="center">
								<sc-qr-code ref="qrcode" :text="otp_info.token"></sc-qr-code>
							</p>
							<p>
								无法扫描？点此获取
								<el-link type="primary" @click="copyKey">TOTP 身份 KEY</el-link>
								进行手动配置。
							</p>
						</div>
						<el-form ref="otpForm" :model="formData" :rules="rules" :label-width="120">
							<el-form-item label="口令" prop="code">
								<el-input v-model="formData.code" placeholder="请输入口令" :maxlength="6"></el-input>
							</el-form-item>
						</el-form>
					</div>
					<div v-if="stepActive == 1">
						<div>
							请扫描下面二维码：
						</div>
					</div>
					<div v-if="stepActive == 2">
						<el-result icon="success" title="绑定成功">
							<template #extra>
								<div style="margin-bottom: var(--el-result-extra-margin-top);">
									一次性密码(口令)验证通过，请妥善保管你的代码！
									您后续可以使用OTP进行登录系统啦！
								</div>
								<el-button type="primary" @click="goLogin">前去登录</el-button>
							</template>
						</el-result>
					</div>
					<el-form style="text-align: center;">
						<el-button v-if="stepActive > 0 && stepActive < 2" @click="pre()">上一步</el-button>
						<el-button v-if="stepActive < 1" type="primary" @click="next()">下一步</el-button>
						<el-button v-if="stepActive == 1" type="primary" @click="save()">提交</el-button>
					</el-form>
					<el-dialog v-model="showAgree" title="平台服务协议" :width="800" destroy-on-close>
						平台服务协议
						<template #footer>
							<el-button @click="showAgree = false">取消</el-button>
							<el-button type="primary"
								@click="showAgree = false; formData.agree = true;">我已阅读并同意</el-button>
						</template>
					</el-dialog>
				</div>
			</div>
		</el-main>
	</el-container>
</template>

<script>
export default {
	name: 'user_register',
	data() {
		return {
			title: "授信登录",
			stepActive: 0,
			showAgree: false,
			app_list: [{
				id: 1,
				name: '微软Authenticator',
				icon: '/img/otp/scan.png',
				url: 'https://www.baidu.com',
			}, {
				id: 2,
				name: '谷歌Authenticator',
				icon: '/img/otp/scan.png',
				url: 'https://www.baidu.com',
			}, {
				id: 3,
				name: '苹果Authenticator',
				icon: '/img/otp/scan.png',
				url: 'https://www.baidu.com',
			}, {
				id: 4,
				name: '1Password Authenticator',
				icon: '/img/otp/scan.png',
				url: 'https://www.baidu.com',
			}],
			formData: {
				code: '',
			},
			rules: {
				code: [
					{ required: true, message: '请输入6位数字代码' },
					{
						validator: (rule, value, callback) => {
							var reg = /^\d{6}$/;
							if (!reg.test(value)) {
								callback(new Error('请输入正确的代码'));
							} else {
								callback();
							}
						}
					}
				]
			},
			otp_info: {
				otp: '',
				secret: '',
				token: '123123123123',
			}
		}
	},
	mounted() {
		///this.init();
	},
	methods: {
		async init() {
			var res = await this.$API.scmuruserotp.model.get();
			if (res.code != 200) {
				this.$message.warning(res.message);
				return false;
			}
			this.otp_info = res.data;
		},
		pre() {
			this.stepActive -= 1
		},
		next() {
			this.stepActive += 1;
		},
		save() {
			this.$refs['otpForm'].validate((valid) => {
				if (valid) {
					this.verify();
				}
			});
		},
		async verify() {
			var verifyRes = this.$API.scmuruserotp.verify.post(this.formData);
			if (verifyRes.code != 200) {
				this.$message.warning(verifyRes.message);
				return false;
			}

			this.stepActive += 1;
		},
		copyKey() {
			this.$copy(this.otp_info.secret);
			this.$message.success('TOTP 身份 KEY 已复制');
		}
	}
}
</script>
<style scoped>
.center {
	text-align: center;
}

.block {
	width: 500px;
	margin: 30px auto;

	p {
		padding: 1rem;
	}
}
</style>