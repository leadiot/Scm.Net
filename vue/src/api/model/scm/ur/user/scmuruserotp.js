import config from "@/config";
import http from "@/utils/request";
export default {
	model: {
		url: `${config.API_URL}/scmuruserotp`,
		name: "查询",
		get: async function () {
			return await http.get(this.url);
		},
	},
	view: {
		url: `${config.API_URL}/scmuruserotp/view`,
		name: "查询",
		get: async function () {
			return await http.get(this.url);
		},
	},
	update: {
		url: `${config.API_URL}/scmuruserotp`,
		name: "更新",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	verify: {
		url: `${config.API_URL}/scmuruserotp/verify`,
		name: "验证",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmuruserotp/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
};
