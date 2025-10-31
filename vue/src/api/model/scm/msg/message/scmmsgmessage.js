import config from "@/config";
import http from "@/utils/request";
export default {
	page: {
		url: `${config.API_URL}/scmmsgmessage/pages`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	add: {
		url: `${config.API_URL}/scmmsgmessage`,
		name: "添加",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	total: {
		url: `${config.API_URL}/scmmsgmessage/total`,
		name: "汇总消息数量",
		get: async function () {
			return await http.get(this.url);
		},
	},
	update: {
		url: `${config.API_URL}/scmmsgmessage`,
		name: "修改",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	read: {
		url: `${config.API_URL}/scmmsgmessage/read`,
		name: "设置可读",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	read_all: {
		url: `${config.API_URL}/scmmsgmessage/readall`,
		name: "设置可读",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmmsgmessage/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmmsgmessage`,
		name: "删除",
		delete: async function (params) {
			return await http.delete(this.url + "?ids=" + params);
		},
	},
	recycle: {
		url: `${config.API_URL}/scmmsgmessage/recycle`,
		name: "删除到回收站",
		put: async function (params) {
			return await http.put(this.url, params);
		},
	},
	unread: {
		url: `${config.API_URL}/scmmsgmessage/unread`,
		name: "未读",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
};
