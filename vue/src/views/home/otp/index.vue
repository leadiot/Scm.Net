<template>
	<common-page title="用户注册">
		<el-steps :active="stepActive" simple finish-status="success">
			<el-step title="准备事项" />
			<el-step title="扫码采集" />
			<el-step title="完成认证" />
		</el-steps>
		<div v-if="stepActive == 0">
			<sc-list :list="app_list" :show-icon="true" :show-url="true"></sc-list>
		</div>
		<div v-if="stepActive == 1">
			<div>
				请扫描下面二维码：
			</div>
			<sc-qrcode ref="qrcode" :text="otp_info.token"></sc-qrcode>
			<el-form ref="otpForm" :model="formData" :rules="rules" :label-width="120">
				<el-form-item label="代码" prop="code">
					<el-input v-model="formData.code" placeholder="请输入代码" :maxlength="6"></el-input>
				</el-form-item>
			</el-form>
		</div>
		<div v-if="stepActive == 2">
			<el-result icon="success" title="绑定成功">
				<template #extra>
					<div style="margin-bottom: var(--el-result-extra-margin-top);">
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
				<el-button type="primary" @click="showAgree = false; formData.agree = true;">我已阅读并同意</el-button>
			</template>
		</el-dialog>
	</common-page>
</template>

<script>
export default {
	name: 'user_register',
	data() {
		return {
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
				token: '',
			}
		}
	},
	mounted() {
		this.init();
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
		}
	}
}
</script>
