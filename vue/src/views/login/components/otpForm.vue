<template>
    <el-form ref="loginForm" :model="form" :rules="rules" label-width="0" size="large">
        <el-form-item prop="type">
            <el-segmented v-model="form.mode" :options="type_list" block style="width: 100%;"></el-segmented>
        </el-form-item>
        <el-form-item prop="phone" v-if="form.mode == 21">
            <el-input v-model="form.phone" prefix-icon="el-icon-iphone" clearable
                :placeholder="$t('login.phonePlaceholder')">
                <!-- <template #prepend>+86</template> -->
            </el-input>
        </el-form-item>
        <el-form-item prop="code" v-if="form.mode == 21">
            <div class="login-msg-yzm">
                <el-input v-model="form.code" prefix-icon="el-icon-unlock" clearable
                    :placeholder="$t('login.smsPlaceholder')"></el-input>
                <el-button @click="getSms" :disabled="disabled">
                    {{ this.$t('login.smsGet') }}<span v-if="disabled">({{ time }})</span>
                </el-button>
            </div>
        </el-form-item>
        <el-form-item prop="email" v-if="form.mode == 22">
            <el-input v-model="form.email" prefix-icon="el-icon-message" clearable
                :placeholder="$t('login.emailPlaceholder')">
            </el-input>
        </el-form-item>
        <el-form-item prop="code" v-if="form.mode == 22">
            <div class="login-msg-yzm">
                <el-input v-model="form.code" prefix-icon="el-icon-unlock" clearable
                    :placeholder="$t('login.msgPlaceholder')"></el-input>
                <el-button @click="getMsg" :disabled="disabled">
                    {{ this.$t('login.msgGet') }}<span v-if="disabled">({{ time }})</span>
                </el-button>
            </div>
        </el-form-item>
        <el-form-item prop="user" v-if="form.mode == 23">
            <el-input v-model="form.user" prefix-icon="el-icon-user" clearable
                :placeholder="$t('login.userPlaceholder')">
            </el-input>
        </el-form-item>
        <el-form-item prop="code" v-if="form.mode == 23">
            <el-input v-model="form.code" prefix-icon="el-icon-unlock" clearable
                :placeholder="$t('login.otpPlaceholder')"></el-input>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" style="width: 100%;" :loading="islogin" round @click="login">
                {{ $t('login.signIn') }}
            </el-button>
        </el-form-item>
        <div class="login-reg" v-if="userRegister">
            {{ $t('login.noAccount') }} <router-link to="/user_register">{{ $t('login.createAccount') }}</router-link>
        </div>
    </el-form>
</template>

<script>
export default {
    data() {
        return {
            form: {
                type: this.$CONFIG.DEF_LOGIN_TYPE,
                mode: 21,
                unit: this.$CONFIG.DEF_LOGIN_UNIT,
                phone: '',
                email: '',
                code: '',
                key: '',
                req: '',
                auto: true
            },
            type_list: [
                { label: "手机号码", value: 21, },
                { label: "电子邮件", value: 22, },
                { label: "随机口令", value: 23, },
            ],
            rules: {
                phone: [{ required: true, message: this.$t('login.phoneError') }],
                email: [{ required: true, message: this.$t('login.emailError') }],
                code: [{ required: true, message: this.$t('login.msgError') }]
            },
            disabled: false,
            time: 0,
            islogin: false,
            userRegister: this.$CONFIG.USER_REGISTER_ENABLED,
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        init() {
            var req = this.$SCM.read_cache('scm_login_otp', '');
            if (!req) {
                req = this.$TOOL.uuid();
            }
            this.form.req = req;
        },
        async getSms() {
            this.form.key = '';

            var validate = await this.$refs.loginForm.validateField("phone").catch(() => { })
            if (!validate) { return false }

            this.$message.success(this.$t('login.smsSent'))
            this.disabled = true
            this.time = 60
            var t = setInterval(() => {
                this.time -= 1
                if (this.time < 1) {
                    clearInterval(t)
                    this.disabled = false
                    this.time = 0
                }
            }, 1000);

            var data = {
                mode: this.form.mode,
                code: this.form.phone,
                req: this.form.req,
            };
            var userRes = await this.$API.operator.sendOtp.post(data);
            if (userRes.code != 200) {
                this.$message.warning(userRes.message);
                return false;
            }
            data = userRes.data;
            if (!data.success) {
                {
                    this.$message.warning(data.message);
                    return false;
                }
            }
            this.form.key = data.key;
        },
        async getMsg() {
            this.form.key = '';

            var validate = await this.$refs.loginForm.validateField("email").catch(() => { })
            if (!validate) { return false }

            this.$message.success(this.$t('login.msgSent'))
            this.disabled = true
            this.time = 60
            var t = setInterval(() => {
                this.time -= 1
                if (this.time < 1) {
                    clearInterval(t)
                    this.disabled = false
                    this.time = 0
                }
            }, 1000);

            var data = {
                mode: this.form.mode,
                code: this.form.email,
                req: this.form.req,
            };
            var userRes = await this.$API.operator.sendOtp.post(data);
            if (userRes.code != 200) {
                this.$message.warning(userRes.message);
                return false;
            }
            data = userRes.data;
            if (!data.success) {
                {
                    this.$message.warning(data.message);
                    return false;
                }
            }
            this.form.key = data.key;
        },
        async login() {
            var validate = await this.$refs.loginForm.validate().catch(() => { })
            if (!validate) { return false }

            if (!this.form.key || this.form.key.length != 32) {
                if (this.form.mode == 23) {
                    this.form.key = '00000000000000000000000000000000';
                } else {
                    this.$message.warning('请先获取验证码！');
                    return false;
                }
            }

            this.islogin = true;
            this.checkAuth();
            this.islogin = false;
        },
        async checkAuth() {
            var userRes = await this.$API.operator.signIn.post(this.form);
            if (userRes.code != 200) {
                this.$message.warning(userRes.message);
                return false;
            }
            var data = userRes.data;
            if (!data.success) {
                this.$message.warning(data.message);
                return false;
            }

            this.$SCM.save_cache('scm_login_pone', '');
            this.$TOOL.data.set("TOKEN", userRes.data.accessToken);
            this.$TOOL.data.set("USER_INFO", userRes.data.userInfo);
            this.$TOOL.data.set("USER_THEME", userRes.data.userTheme);

            //获取菜单
            var menuRes = await this.$API.operator.authority.get();
            if (menuRes.code != 200) {
                this.$message.warning(menuRes.message);
                return false;
            }
            if (menuRes.data.length == 0) {
                this.$alert(
                    "当前用户无任何菜单权限，请联系系统管理员",
                    "无权限访问",
                    { type: "error", center: true }
                );
                return false;
            }
            var menuList = this.$SCM.recursive_menu(menuRes.data, this.$SCM.SYS_ID);
            this.$TOOL.data.set("MENU", menuList);
            this.$TOOL.data.set("PERMISSIONS", []);

            this.loadCfg();

            let path = "/";
            this.$router.replace({ path: path });
            this.$message.success("Login Success 登录成功");
        },
        async loadCfg() {
            var cfgRes = await this.$API.scmsysconfig.list.get({ 'types': 10 });
            cfgRes.data.forEach((item) => {
                if ("app_theme" == item.key) {
                    if (item.value == "true") {
                        document.documentElement.classList.add("dark")
                    } else {
                        document.documentElement.classList.remove("dark")
                    }
                    return;
                }
                if ("app_color" == item.key) {
                    //this.config.colorPrimary = item.val;
                    return;
                }
                if ("app_lang" == item.key) {
                    //this.config.lang = item.value;
                }
            });
        }
    }
}
</script>
