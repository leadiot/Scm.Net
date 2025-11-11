<template>
    <el-form ref="userForm" :model="formData" :rules="rules" :label-width="120" v-if="needOptions">
        <el-form-item label="登录选项" prop="opt">
            <el-radio-group v-model="formData.opt">
                <el-radio-button label="1">注册新用户</el-radio-button>
                <el-radio-button label="2">关联已有用户</el-radio-button>
                <el-radio-button label="3" v-if="error_code == 46">选择登录用户</el-radio-button>
            </el-radio-group>
            <div class="el-form-item-msg">请选择登录选项</div>
        </el-form-item>
        <el-form-item label="登录账户" prop="user" v-if="formData.opt == 2">
            <el-input v-model="formData.user" placeholder="请输入登录账户"></el-input>
            <div class="el-form-item-msg">请输入需要进行关联的登录账户，格式为：username@unitname</div>
        </el-form-item>
        <el-form-item label="登录账户" prop="user_id" v-if="formData.opt == 3">
            <sc-select v-model="formData.user_id" placeholder="请选择登录账户"></sc-select>
            <div class="el-form-item-msg">系统检测到多个关联账户，请选择要登录的账户</div>
        </el-form-item>
        <el-form-item label="登录密码" prop="pass" v-if="formData.opt == 2 || formData.opt == 3">
            <el-input v-model="formData.pass" type="password" show-password placeholder="请输入登录密码"></el-input>
            <div class="el-form-item-msg">请输账户对应的登录密码</div>
        </el-form-item>
        <div style="text-align: center;">
            <el-button type="primary" @click="signon()">提交</el-button>
        </div>
    </el-form>
    <div class="loading" v-else>
        <div v-if="error_code == 0">
            <div class="notice">
                系统加载中……
            </div>
        </div>
        <div v-else>
            <div class="notice warning">
                {{ error_msg }}
            </div>
            <div>
                <a href="/login">返回登录</a>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    props: {
        code: { type: String, default: '' }
    },
    data() {
        return {
            loading: false,
            needOptions: false,
            formData: this.def_data(),
            error_code: 0,
            error_msg: '',
            rules: {
                type: [{ required: true, message: '请选择登录选项' }],
                user: [{ required: true, message: '请输入登录用户' }],
                user_id: [{ required: true, message: '请输入登录用户' }],
                pass: [{ required: true, message: '请输入登录密码' }]
            },
            key: '',
        }
    },
    mounted() {
        this.init(this.code);
    },
    methods: {
        def_data() {
            return {
                type: this.$CONFIG.DEF_LOGIN_TYPE,
                mode: 40,
                user: '',
                user_id: '0',
                pass: '',
                opt: 1,
            }
        },
        save() {
            this.$refs.userForm.validate((valid) => {
                if (valid) {
                    this.signon();
                }
            });
        },
        async init(code) {
            var reg = /^[0-9a-zA-Z]{32}$/
            if (!reg.test(code)) {
                this.$router.replace({ path: '/login' });
                return;
            }

            this.checkAuth(code);
        },
        async checkAuth(key) {
            this.key = key;
            var data = { type: this.formData.type, mode: this.formData.mode, key: key, code: key };
            var res = await this.$API.operator.token.post(data);
            if (res.code != 200) {
                this.$message.warning(res.message);
                return false;
            }

            var userData = res.data;
            if (userData.code != 0) {
                this.error_code = userData.code;
                this.error_msg = userData.message;
                this.needOptions = userData.code > 44;
                return false;
            }

            this.loadAuth(userData);
        },
        async loadAuth(userData) {
            this.$TOOL.data.set("TOKEN", userData.accessToken);
            this.$TOOL.data.set("USER_INFO", userData.userInfo);
            this.$TOOL.data.set("USER_THEME", userData.userTheme);

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
            var menuList = this.$SCM.recursive_menu(menuRes.data, '0');
            this.$TOOL.data.set("MENU", menuList);
            this.$TOOL.data.set("PERMISSIONS", []);

            this.loadCfg();

            let path = userData.userInfo.unitId == 0 ? "/" : "/";//TODO:区分管理员与用户
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
        },
        async signon() {
            var data = {
                type: this.formData.type,
                mode: this.formData.mode,
                user: this.formData.user,
                pass: this.$CRYPTO.SHA(this.formData.pass),
                opt: this.formData.opt,
                key: this.key
            };
            var res = await this.$API.operator.signon.post(data);
            if (res.code != 200) {
                this.$message.warning(res.message);
                return false;
            }

            var userData = res.data;
            if (userData.code != 0) {
                this.$message.warning(userData.message);
                return false;
            }

            this.loadAuth(userData);
        },
    }
}
</script>
<style type="scss" scoped>
.loading {
    min-height: 400px;
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
    text-align: center;

    .notice {
        font-size: 26px;
        margin: 20px;
    }
}

.warning {
    color: red;
}
</style>
