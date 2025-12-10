import config from "@/config"
import http from "@/utils/request"
export default {
	page: {
		url: `${config.API_URL}/scmcfgmenu/pages`,
		name: "查询",
		get: async function (params) {
			return await http.get(this.url, params);
		}
	},
	list: {
		url: `${config.API_URL}/scmcfgmenu/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	view: {
		url: `${config.API_URL}/scmcfgmenu/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	edit: {
		url: `${config.API_URL}/scmcfgmenu/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	add: {
		url: `${config.API_URL}/scmcfgmenu`,
		name: "新增",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	update: {
		url: `${config.API_URL}/scmcfgmenu`,
		name: "更新",
		put: async function (data) {
			return await http.put(this.url, data);
		}
	},
	save: {
		url: `${config.API_URL}/scmcfgmenu/save`,
		name: "保存",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	status: {
		url: `${config.API_URL}/scmcfgmenu/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	delete: {
		url: `${config.API_URL}/scmcfgmenu`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		}
	},
	option: {
		url: `${config.API_URL}/scmcfgmenu/option`,
		name: "删除",
		get: async function (data) {
			return await http.get(this.url, data);
		}
	},
}