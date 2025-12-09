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
                    <el-steps :active="stepActive" finish-status="success" simple>
                        <el-step title="安装应用" />
                        <el-step title="账户绑定" />
                        <el-step title="完成" />
                    </el-steps>
                    <div v-if="stepActive == 0">
                        <div class="block">
                            <el-alert type="primary" :closable="false">
                                授信登录（TOTP）是一种安全的登录方式，其采用基于时间同步的原理，用户在需要登录时输入动态生成的一次性口令，以完成用户身份验证。
                                授信登录可以有效的帮助用户保护自己的账号不被他人盗用，并且让用户不再需要记住复杂的密码。
                            </el-alert>
                            <p class="text item">
                                您正在开通授信登录服务，以下是您可以使用的 <strong>TOTP身份验证应用</strong>：
                            </p>
                            <p class="text item">
                                <el-link type="primary"
                                    href="https://apps.microsoft.com/detail/xpdnx5wpg2vd9h?hl=zh-CN&gl=CN"
                                    target="_blank">
                                    腾讯身份验证器
                                </el-link>
                            </p>
                            <p class="text item">
                                <el-link type="primary" href="https://www.microsoft.com/en-us/account/authenticator"
                                    target="_blank">
                                    Microsoft Authenticator
                                </el-link>
                            </p>
                            <p class="text item">
                                <el-link type="primary" href="https://www.google.com/intl/zh-CN/authenticator/"
                                    target="_blank">
                                    Google Authenticator
                                </el-link>
                            </p>
                            <p class="text item">
                                <el-link type="primary" href="https://1password.com/" target="_blank">
                                    1Password
                                </el-link>
                            </p>
                            <p>
                                <el-link type="primary" href="https://www.lastpass.com/" target="_blank">
                                    LastPass
                                </el-link>
                            </p>
                            <p class="text item">
                                <el-link type="primary" href="https://authy.com/" target="_blank">
                                    Authy
                                </el-link>
                            </p>
                            <p class="text item">
                                您可以根据自己的喜好选择其中一个应用进行安装，并在安装完成后点击下一步，进入绑定页面。
                            </p>
                        </div>
                    </div>
                    <div v-if="stepActive == 1">
                        <div class="block">
                            <el-alert type="primary" :closable="false">
                                使用您安装的第三方 <strong>TOTP身份验证应用</strong>
                                扫描下方二维码，进行账户绑定。系统在您扫描成功后会周期性生成一个随机口令，该口令可以让您更简单、更安全的登录系统。
                            </el-alert>
                            <p>请使用 <strong>TOTP身份验证应用</strong> 扫描下方二维码，获取6位口令：</p>
                            <p class="center">
                                <sc-qr-code ref="qrcode" :text="otp_info.uri" :size="200"></sc-qr-code>
                            </p>
                            <p class="right">
                                无法扫描？点此获取
                                <el-link type="primary" @click="copyKey">TOTP 身份 KEY</el-link>
                                进行手动配置。
                            </p>
                        </div>
                        <el-form ref="otpForm" :model="formData" :rules="rules">
                            <p style="padding: 1rem;">请输入应用中获取到的6位数字代码：</p>
                            <el-form-item label="口令" prop="code">
                                <el-input v-model="formData.code" placeholder="请输入口令" :maxlength="6"></el-input>
                            </el-form-item>
                        </el-form>
                    </div>
                    <div v-if="stepActive == 2">
                        <el-result icon="success" title="绑定成功">
                            <template #extra>
                                <div style="margin-bottom: var(--el-result-extra-margin-top);">
                                    恭喜您，授权登录服务开通成功，您后续可以使用授信口令登录系统啦！
                                </div>
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
                    <el-dialog v-model="showToken" title="授信密钥" :width="400" destroy-on-close>
                        以下是您的授信密钥：<br />
                        <div style="text-align: center;font-weight: bold;padding: 1rem;">
                            {{ otp_info.secret }}
                        </div>
                        <template #footer>
                            <el-button type="success" v-copy="otp_info.secret">复制</el-button>
                            <el-button type="primary" @click="showToken = false">关闭</el-button>
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
            showToken: false,
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
                secret: '',
                uri: '',
            }
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        async init() {
            var res = await this.$API.scmuruserotp.view.get();
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
            var verifyRes = await this.$API.scmuruserotp.verify.post(this.formData);
            if (verifyRes.code != 200) {
                this.$message.warning(verifyRes.message);
                return false;
            }

            var result = verifyRes.data;
            if (!result) {
                this.$message.warning('口令验证失败，请重新输入！');
                return false;
            }

            this.stepActive += 1;
        },
        copyKey() {
            this.showToken = true;
        }
    }
}
</script>
<style scoped>
.block {
    width: 500px;
    margin: 30px auto;

    p {
        padding: 0.5rem;
    }

    .center {
        text-align: center;
    }

    .right {
        text-align: right;
    }
}
</style>