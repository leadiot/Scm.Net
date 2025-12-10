import config from "@/config";
import http from "@/utils/request";
export default {
	page: {
		url: `${config.API_URL}/scmresiconcat/pages`,
		name: "查询",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	list: {
		url: `${config.API_URL}/scmresiconcat/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	option: {
		url: `${config.API_URL}/scmresiconcat/option/`,
		name: "列表",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	model: {
		url: `${config.API_URL}/scmresiconcat/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	add: {
		url: `${config.API_URL}/scmresiconcat`,
		name: "新增",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	update: {
		url: `${config.API_URL}/scmresiconcat`,
		name: "更新",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmresiconcat/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmresiconcat`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		},
	},
};
