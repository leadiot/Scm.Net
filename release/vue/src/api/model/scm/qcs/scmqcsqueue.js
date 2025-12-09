import config from "@/config";
import http from "@/utils/request";
export default {
	page: {
		url: `${config.API_URL}/scmqcsqueue/pages`,
		name: "查询",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	list: {
		url: `${config.API_URL}/scmqcsqueue/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	view: {
		url: `${config.API_URL}/scmqcsqueue/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	edit: {
		url: `${config.API_URL}/scmqcsqueue/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	queuing: {
		url: `${config.API_URL}/scmqcsqueue/queuing`,
		name: "取号",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	queuing_list: {
		url: `${config.API_URL}/scmqcsqueue/queuinglist`,
		name: "取号列表",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	calling: {
		url: `${config.API_URL}/scmqcsqueue/calling`,
		name: "叫号",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	calling_list: {
		url: `${config.API_URL}/scmqcsqueue/callinglist`,
		name: "叫号列表",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	save: {
		url: `${config.API_URL}/scmqcsqueue`,
		name: "保存",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	update: {
		url: `${config.API_URL}/scmqcsqueue`,
		name: "保存",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmqcsqueue/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmqcsqueue`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		},
	},
	option: {
		url: `${config.API_URL}/scmqcsqueue/option`,
		name: "删除",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
};
