import { defineStore } from 'pinia'
import config from "@/config"

export const useGlobalStore = defineStore('global', {
	state: () => ({
		ismobile: false,
		layout: config.LAYOUT,
		menuIsCollapse: config.MENU_IS_COLLAPSE,
		layoutTags: config.LAYOUT_TAGS,
		theme: config.THEME,
	}),
	actions: {
		SET_ismobile(value) {
			this.ismobile = value
		},
		SET_layout(value) {
			this.layout = value
		},
		SET_theme(value) {
			this.theme = value
		},
		TOGGLE_menuIsCollapse() {
			this.menuIsCollapse = !this.menuIsCollapse
		},
		TOGGLE_layoutTags() {
			this.layoutTags = !this.layoutTags
		}
	}
})
