import config from "@/config";
import http from "@/utils/request";
export default {
	DEF_PID: "1000000000000000002",
	option: {
		url: `${config.API_URL}/scmmgrregion/option/`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	list: {
		url: `${config.API_URL}/scmmgrregion/listbypid/`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	add: {
		url: `${config.API_URL}/scmmgrregion`,
		name: "添加",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	model: {
		url: `${config.API_URL}/scmmgrregion/`,
		name: "查询一条",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	update: {
		url: `${config.API_URL}/scmmgrregion`,
		name: "修改",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmmgrregion/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmmgrregion`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		},
	},
	changeOwner: {
		url: `${config.API_URL}/scmmgrregion/changeOwner`,
		name: "更新主管",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
};
