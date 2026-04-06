<template>
	<el-config-provider :locale="locale" :size="config.size" :zIndex="config.zIndex" :button="config.button">
		<router-view></router-view>
	</el-config-provider>
</template>

<script>
import colorTool from "@/utils/color";
import { useI18n } from "vue-i18n";
import { computed } from "vue";

export default {
	name: "App",
	setup() {
		const { locale, messages } = useI18n()
		const elLocale = computed(() => messages.value[locale.value].el)
		return { elLocale }
	},
	data() {
		return {
			config: {
				size: "default",
				zIndex: 2000,
				button: {
					autoInsertSpace: false,
				},
			},
		};
	},
	computed: {
		locale() {
			return this.elLocale;
		},
	},
	created() {
		//设置主题颜色
		const app_color = this.$CONFIG.COLOR || this.$TOOL.data.get("APP_COLOR");
		if (app_color) {
			document.documentElement.style.setProperty("--el-color-primary", app_color);
			for (let i = 1; i <= 9; i++) {
				document.documentElement.style.setProperty(
					`--el-color-primary-light-${i}`,
					colorTool.lighten(app_color, i / 10)
				);
			}
			for (let i = 1; i <= 9; i++) {
				document.documentElement.style.setProperty(
					`--el-color-primary-dark-${i}`,
					colorTool.darken(app_color, i / 10)
				);
			}
		}
	},
};
</script>

<style lang="scss">
@use "@/style/style.scss";
</style>
