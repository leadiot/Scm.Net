import config from "@/config";
import http from "@/utils/request";
export default {
	page: {
		url: `${config.API_URL}/scmmsgchatmessage/pages`,
		name: "查询",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	list: {
		url: `${config.API_URL}/scmmsgchatmessage/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	view: {
		url: `${config.API_URL}/scmmsgchatmessage/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	edit: {
		url: `${config.API_URL}/scmmsgchatmessage/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	get: {
		url: `${config.API_URL}/scmmsgchatmessage`,
		name: "新增",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	add: {
		url: `${config.API_URL}/scmmsgchatmessage`,
		name: "新增",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmmsgchatmessage/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmmsgchatmessage`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		},
	},
	list_header: {
		url: `${config.API_URL}/scmmsgchatmessage/listheader`,
		name: "会话列表",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	list_detail: {
		url: `${config.API_URL}/scmmsgchatmessage/listdetail`,
		name: "会话明细",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	create: {
		url: `${config.API_URL}/scmmsgchatmessage`,
		name: "创建会话",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	chat: {
		url: `${config.API_URL}/scmmsgchatmessage/chat`,
		name: "聊天",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
};
