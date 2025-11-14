import config from "@/config";
import http from "@/utils/request";
export default {
	page: {
		url: `${config.API_URL}/scmmsgnotice/pages`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	add: {
		url: `${config.API_URL}/scmmsgnotice`,
		name: "添加",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	view: {
		url: `${config.API_URL}/scmmsgnotice/view/`,
		name: "查询一条",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	edit: {
		url: `${config.API_URL}/scmmsgnotice/edit/`,
		name: "查询一条",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	update: {
		url: `${config.API_URL}/scmmsgnotice`,
		name: "修改",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	status: {
		url: `${config.API_URL}/scmmsgnotice/status`,
		name: "修改状态",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmmsgnotice`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		},
	},
	summary: {
		url: `${config.API_URL}/scmmsgnotice/summary`,
		name: "统计数量",
		get: async function () {
			return await http.get(this.url);
		},
	},
	readed: {
		url: `${config.API_URL}/scmmsgnotice/changereaded`,
		name: "已读",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	unread: {
		url: `${config.API_URL}/scmmsgnotice/changeunread`,
		name: "取消已读",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	arced: {
		url: `${config.API_URL}/scmmsgnotice/ChangeArchive`,
		name: "取消已读",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	unarc: {
		url: `${config.API_URL}/scmmsgnotice/CancelArchive`,
		name: "取消已读",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
};
