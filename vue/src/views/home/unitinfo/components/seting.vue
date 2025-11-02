<template>
	<el-card shadow="never" header="语言主题">
		<el-form ref="form" label-width="120px" style="margin-top:20px;">
			<el-form-item :label="$t('user.nightmode')">
				<el-switch v-model="dark" inline-prompt active-icon="el-icon-moon"
					inactive-icon="el-icon-sunny"></el-switch>
				<div class="el-form-item-msg">{{ $t('user.nightmode_msg') }}</div>
			</el-form-item>
			<el-form-item :label="$t('user.language')">
				<el-select v-model="lang">
					<el-option label="简体中文" value="zh-cn"></el-option>
					<el-option label="English" value="en"></el-option>
				</el-select>
				<div class="el-form-item-msg">{{ $t('user.language_msg') }}</div>
			</el-form-item>
			<el-divider></el-divider>
			<el-form-item label="主题颜色">
				<el-color-picker v-model="colorPrimary" :predefine="colorList">></el-color-picker>
			</el-form-item>
			<el-divider></el-divider>
			<el-form-item label="框架布局">
				<el-select v-model="layout" placeholder="请选择">
					<el-option label="默认" value="default"></el-option>
					<el-option label="通栏" value="header"></el-option>
					<el-option label="经典" value="menu"></el-option>
					<el-option label="功能坞" value="dock"></el-option>
				</el-select>
			</el-form-item>
			<el-form-item label="折叠菜单">
				<el-switch v-model="menuIsCollapse"></el-switch>
			</el-form-item>
			<el-form-item label="标签栏">
				<el-switch v-model="layoutTags"></el-switch>
			</el-form-item>
			<el-divider></el-divider>
		</el-form>
	</el-card>
</template>

<script>
import colorTool from '@/utils/color'

export default {
	data() {
		return {
			layout: this.$store.state.global.layout,
			menuIsCollapse: this.$store.state.global.menuIsCollapse,
			layoutTags: this.$store.state.global.layoutTags,
			lang: this.$TOOL.data.get('APP_LANG') || this.$CONFIG.LANG,
			dark: this.$TOOL.data.get('APP_THEME') == 'dark',
			colorList: this.$CONFIG.PREDEFINE_COLORS,
			colorPrimary: this.$TOOL.data.get('APP_COLOR') || this.$CONFIG.COLOR || '#409EFF'
		}
	},
	watch: {
		layout(val) {
			this.$store.commit("SET_layout", val)
		},
		menuIsCollapse() {
			this.$store.commit("TOGGLE_menuIsCollapse")
		},
		layoutTags() {
			this.$store.commit("TOGGLE_layoutTags")
		},
		dark(val) {
			if (val) {
				document.documentElement.classList.add("dark")
				this.$TOOL.data.set("APP_THEME", "dark")
			} else {
				document.documentElement.classList.remove("dark")
				this.$TOOL.data.remove("APP_THEME")
			}
		},
		lang(val) {
			this.$i18n.locale = val
			this.$TOOL.data.set("APP_LANG", val);
		},
		colorPrimary(val) {
			document.documentElement.style.setProperty('--el-color-primary', val);
			for (let i = 1; i <= 9; i++) {
				document.documentElement.style.setProperty(`--el-color-primary-light-${i}`, colorTool.lighten(val, i / 10));
			}
			for (let i = 1; i <= 9; i++) {
				document.documentElement.style.setProperty(`--el-color-primary-dark-${i}`, colorTool.darken(val, i / 10));
			}
			this.$TOOL.data.set("APP_COLOR", val);
		}
	},
	mounted() {
		//this.init();
	},
	methods: {
		async init() {
			var cfgRes = await this.$API.scmsysconfig.list.get(10);
			cfgRes.data.forEach((item) => {
				if ("app_lang" == item.key) {
					this.lang = item.value;
					return;
				}
				if ("app_theme" == item.key) {
					this.dark = item.value == "true";
					return;
				}
				if ("app_color" == item.key) {
					this.colorPrimary = item.val;
					return;
				}
			});
		},
		async save(key, val) {
			let data = {};
			data.key = key;
			data.value = val;
			data.types = 10;
			data.data = 0;
			var res = await this.$API.scmsysconfig.save.post(data);
			if (res.code == 200) {
				this.$message.success("保存成功");
				this.visible = false;
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
	},
}
</script>