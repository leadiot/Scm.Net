import config from "@/config"
import http from "@/utils/request"
export default {
	page: {
		url: `${config.API_URL}/eamresspu/pages`,
		name: "分页",
		get: async function (params) {
			return await http.get(this.url, params);
		}
	},
	list: {
		url: `${config.API_URL}/eamresspu/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	view: {
		url: `${config.API_URL}/eamresspu/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	edit: {
		url: `${config.API_URL}/eamresspu/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	add: {
		url: `${config.API_URL}/eamresspu`,
		name: "新增",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	update: {
		url: `${config.API_URL}/eamresspu`,
		name: "更新",
		put: async function (data) {
			return await http.put(this.url, data);
		}
	},
	save: {
		url: `${config.API_URL}/eamresspu`,
		name: "保存",
		put: async function (data) {
			return await http.put(this.url, data);
		}
	},
	status: {
		url: `${config.API_URL}/eamresspu/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	delete: {
		url: `${config.API_URL}/eamresspu`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		}
	},
	option: {
		url: `${config.API_URL}/eamresspu/option`,
		name: "选择",
		get: async function (data) {
			return await http.get(this.url, data);
		}
	},
	upload: {
		url: `${config.API_URL}/eamresspu/upload`,
		name: "上传",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
}