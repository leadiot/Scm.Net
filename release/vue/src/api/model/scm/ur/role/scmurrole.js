import config from "@/config";
import http from "@/utils/request";
export default {
	list: {
		url: `${config.API_URL}/scmurrole/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	tenantlist: {
		url: `${config.API_URL}/scmurrole/tenantlist`,
		name: "租户角色查询",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	add: {
		url: `${config.API_URL}/scmurrole`,
		name: "添加",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	model: {
		url: `${config.API_URL}/scmurrole/`,
		name: "查询一条",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	update: {
		url: `${config.API_URL}/scmurrole`,
		name: "修改",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	option: {
		url: `${config.API_URL}/scmurrole/option/`,
		name: "查询一条",
		get: async function () {
			return await http.get(this.url + '0');
		},
	},
	status: {
		url: `${config.API_URL}/scmurrole/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmurrole`,
		name: "删除",
		delete: async function (params) {
			return await http.delete(this.url + "?ids=" + params);
		},
	},
};
