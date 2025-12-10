<template>
	<div :style="theme.page" class="login_page">
		<div class="mask" :style="theme.mask"></div>
		<el-container class="main">
			<el-header>
				<div class="common-header-left">
					<div class="common-header-logo">
						<img :alt="$CONFIG.APP_NAME" src="img/logo.png">
						<label>{{ $CONFIG.APP_NAME }}</label>
					</div>
					<div class="common-header-title">{{ $t('login.signInTitle') }}</div>
				</div>
				<div class="common-header-right">
					<el-button :icon="config.dark ? 'el-icon-sunny' : 'el-icon-moon'" circle type="info"
						@click="configDark" :title="$t('login.themeTitle')">
					</el-button>
					<el-dropdown trigger="click" placement="bottom-end" @command="configLang"
						:title="$t('login.langTitle')">
						<el-button circle>
							<sc-icon name="sc-translate" />
						</el-button>
						<template #dropdown>
							<el-dropdown-menu>
								<el-dropdown-item v-for="item in lang_list" :key="item.value" :command="item"
									:class="{ selected: config.lang == item.value }">{{ item.name }}</el-dropdown-item>
							</el-dropdown-menu>
						</template>
					</el-dropdown>
				</div>
			</el-header>
			<el-main>
				<div class="login_card">
					<el-card class="login_main">
						<el-container>
							<el-header class="login-header">
								<div class="logo">
									<label>{{ $CONFIG.APP_NAME }}</label>
								</div>
							</el-header>
							<el-container>
								<el-aside class="login_aside">
									<div class="login_note">
										<div class="body">
											<div class="login_note__content">
												<div v-html="info.content"></div>
											</div>
										</div>
									</div>
								</el-aside>
								<el-main>
									<div class="login_user">
										<div class="body">
											<el-tabs>
												<el-tab-pane :label="$t('login.accountLogin')" lazy v-if="hasMode(10)">
													<pass-form></pass-form>
												</el-tab-pane>
												<el-tab-pane :label="$t('login.otpLogin')" lazy v-if="hasMode(20)">
													<otp-form></otp-form>
												</el-tab-pane>
												<el-tab-pane :label="$t('login.phoneLogin')" lazy v-if="hasMode(21)">
													<phone-form></phone-form>
												</el-tab-pane>
												<el-tab-pane :label="$t('login.emailLogin')" lazy v-if="hasMode(22)">
													<email-form></email-form>
												</el-tab-pane>
												<el-tab-pane :label="$t('login.oauthLogin')" lazy v-if="hasMode(30)">
													<oidc-form></oidc-form>
												</el-tab-pane>
											</el-tabs>
										</div>
									</div>
								</el-main>
							</el-container>
						</el-container>
					</el-card>
				</div>
			</el-main>
		</el-container>
	</div>
	<div class="login_foot">
		© {{ $CONFIG.APP_NAME }} {{ $CONFIG.APP_VER }} BUILD {{ $CONFIG.APP_BUILD }}
		<a href="https://beian.miit.gov.cn/" target="_blank" title="备案信息查询" v-if="$CONFIG.BEIAN_MIIT">
			{{ $CONFIG.BEIAN_MIIT }}
		</a>
		<a href="https://beian.mps.gov.cn/" target="_blank" title="备案信息查询" v-if="$CONFIG.BEIAN_MPS">
			{{ $CONFIG.BEIAN_MPS }}
		</a>
	</div>
</template>

<script>
import passForm from "./components/passForm";
import otpForm from "./components/otpForm";
import phoneForm from "./components/phoneForm";
import emailForm from "./components/emailForm";
import oidcForm from "./components/oidcForm";

export default {
	name: 'login',
	components: {
		passForm,
		otpForm,
		phoneForm,
		emailForm,
		oidcForm,
	},
	data() {
		var lang = this.$TOOL.getCache("APP_LANG") || this.$CONFIG.LANG;
		this.$i18n.locale = lang;

		var dark = this.$TOOL.getCache("APP_THEME") == "dark";
		if (dark) {
			document.documentElement.classList.add("dark");
		} else {
			document.documentElement.classList.remove("dark");
		}

		return {
			config: {
				lang: lang,
				dark: dark,
			},
			info: {},
			theme: {
				page: {
					"backgroundColor": "",
					"backgroundSize": "cover",
					"backgroundPosition": "center center",
					"backgroundRepeat": "no-repeat"
				},
				mask: {
					"backgroundColor": "rgba(0,0,0,0.5)"
				}
			},
			mode: [10],
			lang_list: [
				{ name: "简体中文", value: "zh-cn", },
				{ name: "English", value: "en", },
			],
		};
	},
	watch: {
		"config.dark"(val) {
			if (val) {
				document.documentElement.classList.add("dark");
				this.$TOOL.setCache("APP_THEME", "dark");
			} else {
				document.documentElement.classList.remove("dark");
				this.$TOOL.removeCache("APP_THEME");
			}
		},
		"config.lang"(val) {
			this.$i18n.locale = val;
			this.$TOOL.setCache("APP_LANG", val);
		},
	},
	created: function () {
		this.$TOOL.cookie.remove("TOKEN");
		this.$TOOL.data.remove("USER_INFO");
		this.$TOOL.data.remove("MENU");
		this.$TOOL.data.remove("PERMISSIONS");
		this.$TOOL.data.remove("grid");
		this.$TOOL.data.remove("ISLONINEND");
		this.$store.commit("clearViewTags");
		this.$store.commit("clearKeepLive");
		this.$store.commit("clearIframeList");
	},
	mounted() {
		this.mode = this.$CONFIG.DEF_LOGIN_MODE;

		this.loadInfo();
		this.loadTheme();
	},
	methods: {
		async loadInfo() {
			var res = await this.$API.scmsysapp.model.get(this.$CONFIG.APP_CODE);
			if (res == null || res.code != 200) {
				return;
			}

			this.info = res.data;
		},
		async loadTheme() {
			var res = await this.$API.operator.dateTheme.get({});
			if (res == null || res.code != 200) {
				return;
			}

			this.theme = res.data;
		},
		configDark() {
			this.config.dark = this.config.dark ? false : true;
		},
		configLang(command) {
			this.config.lang = command.value;
		},
		hasMode(mode) {
			if (this.mode && this.mode.length > 0) {
				for (var i = 0; i < this.mode.length; i += 1) {
					if (this.mode[i] == mode) {
						return true;
					}
				}
			}
			return false;
		}
	},
};
</script>

<style scoped>
.login_page {
	width: 100%;
	height: 100%;
	position: relative;
}

.login_page .mask {
	position: absolute;
	top: 0px;
	left: 0px;
	right: 0px;
	bottom: 0px;
	z-index: 1;
}

.login_page .main {
	position: absolute;
	top: 0px;
	left: 0px;
	right: 0px;
	bottom: 0px;
	z-index: 2;
}

.login_card {
	height: 100%;
	display: flex;
	justify-content: center;
	align-content: center;
}

.login_main {
	width: 100%;
	max-width: 800px;
	margin: auto;
}

.login_aside {
	width: 50%;
	padding: 15px;
}

.login_note {
	/* padding: 10px; */
}

.login_note .body {
	border-radius: 10px;
	min-height: 390px;
}

.login_note__title {
	padding: 40px;
}

.login_note__title h2 {
	font-size: 40px;
}

.login_note__title h4 {
	font-size: 18px;
	margin-top: 10px;
	font-weight: normal;
}

.login_note__title p {
	font-size: 14px;
	margin-top: 10px;
	line-height: 1.8;
	color: rgba(255, 255, 255, 0.6);
}

.login_note__title div {
	margin-top: 10px;
	display: flex;
	align-items: center;
}

.login_note__title div span {
	margin-right: 15px;
}

.login_note__title div i {
	font-size: 40px;
}

.login_note__title div i.add {
	font-size: 20px;
	color: rgba(255, 255, 255, 0.6);
}

.login_note__content {
	/* color: #666; */
}

.login-header {
	border: 0px;
}

.login-header .logo {
	display: flex;
	align-items: center;
}

.login-header .logo img {
	width: 40px;
	height: 40px;
	vertical-align: bottom;
	margin-right: 10px;
}

.login-header .logo label {
	font-size: 26px;
	font-weight: bold;
}

.login_foot {
	left: 0px;
	right: 0px;
	bottom: 0px;
	padding: 10px;
	position: absolute;
	text-align: center;
	color: #fff;
	z-index: 3;
}

.login_foot a {
	color: #fff;
}

.login_user {}

.login_user .body {
	border-radius: 10px;
}

.login_user .el-divider {
	margin-top: 40px;
}

.login_user:deep(.el-tabs) .el-tabs__header {
	margin-bottom: 25px;
}

.login_user:deep(.el-tabs) .el-tabs__header .el-tabs__item {
	font-size: 14px;
}

.login_user:deep(.login-forgot) {
	text-align: right;
}

.login_user:deep(.login-forgot) a {
	color: var(--el-color-primary);
}

.login_user:deep(.login-forgot) a:hover {
	color: var(--el-color-primary-light-3);
}

.login_user:deep(.login-reg) {
	font-size: 14px;
	color: var(--el-text-color-primary);
}

.login_user:deep(.login-reg) a {
	color: var(--el-color-primary);
}

.login_user:deep(.login-reg) a:hover {
	color: var(--el-color-primary-light-3);
}

.login_user:deep(.login-msg-yzm) {
	display: flex;
	width: 100%;
}

.login_user:deep(.login-msg-yzm) .el-button {
	margin-left: 10px;
	--el-button-size: 42px;
}

@media (max-width: 1200px) {
	.login_user {
		width: 340px;
	}
}

@media (max-width: 1000px) {
	.login_main {
		display: block;
	}

	.login_main .login_config {
		position: static;
		padding: 20px 20px 0 20px;
		text-align: right;
	}

	.login_user {
		width: 100%;
	}

	.login_aside {
		display: none;
	}
}
</style>
