import config from "@/config";
import http from "@/utils/request";
export default {
	list: {
		url: `${config.API_URL}/scmurorganize/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	add: {
		url: `${config.API_URL}/scmurorganize`,
		name: "添加",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	model: {
		url: `${config.API_URL}/scmurorganize/`,
		name: "查询一条",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	update: {
		url: `${config.API_URL}/scmurorganize`,
		name: "修改",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmurorganize/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmurorganize`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		},
	},
	changeOwner: {
		url: `${config.API_URL}/scmurorganize/changeOwner`,
		name: "更新主管",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
};
