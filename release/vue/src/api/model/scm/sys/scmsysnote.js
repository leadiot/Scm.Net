import config from "@/config";
import http from "@/utils/request";
export default {
	page: {
		url: `${config.API_URL}/scmsysnote/pages`,
		name: "查询",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	list: {
		url: `${config.API_URL}/scmsysnote/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	model: {
		url: `${config.API_URL}/scmsysnote/`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	view: {
		url: `${config.API_URL}/scmsysnote/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	edit: {
		url: `${config.API_URL}/scmsysnote/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	add: {
		url: `${config.API_URL}/scmsysnote`,
		name: "新增",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	update: {
		url: `${config.API_URL}/scmsysnote`,
		name: "更新",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	save: {
		url: `${config.API_URL}/scmsysnote/save`,
		name: "保存",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmsysnote/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmsysnote`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		},
	},
	upload: {
		url: `${config.API_URL}/scmsysnote/upload`,
		name: "删除",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
};
