import config from "@/config";
import http from "@/utils/request";
export default {
	list: {
		url: `${config.API_URL}/scmsysfile/folders`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	files: {
		url: `${config.API_URL}/scmsysfile/files`,
		name: "查询文件列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	view: {
		url: `${config.API_URL}/scmsysfile/view`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	upload: {
		url: `${config.API_URL}/upload/upload`,
		name: "文件上传",
		post: async function (data, config) {
			return await http.post(this.url, data, config);
		},
	},
	delFile: {
		url: `${config.API_URL}/scmsysfile/file`,
		name: "删除文件",
		delete: async function (params) {
			return await http.delete(this.url + "?path=" + params);
		},
	},
	delFolder: {
		url: `${config.API_URL}/scmsysfile/folder`,
		name: "删除目录",
		delete: async function (params) {
			return await http.delete(this.url + "?path=" + params);
		},
	},
	adv: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传广告位",
		post: async function (data) {
			return await http.post(this.url + "?path=/upload/adv/", data);
		},
	},
	site: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传站点Logo以及二维码",
		post: async function (data) {
			return await http.post(this.url + "?path=/upload/site/", data);
		},
	},
	vote: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传投票",
		post: async function (data) {
			return await http.post(this.url + "?path=/upload/vote/", data);
		},
	},
	avatar: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传头像",
		post: async function (data) {
			return await http.post(this.url + "?path=/upload/avatar/", data);
		},
	},
	artice: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传文章图片",
		post: async function (data, config = {}) {
			return await http.post(
				this.url + "?path=/upload/artice/",
				data,
				config
			);
		},
	},
	shop: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传商城图片",
		post: async function (data) {
			return await http.post(this.url + "?path=/upload/shop/", data);
		},
	},
	knowledge: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传知识库图片",
		post: async function (data) {
			return await http.post(this.url + "?path=/upload/knowledge/", data);
		},
	},
	knowledgeCover: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传课件封面",
		post: async function (data) {
			return await http.post(
				this.url + "?path=/upload/knowledge/cover/",
				data
			);
		},
	},
	medal: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "上传勋章",
		post: async function (data) {
			return await http.post(
				this.url + "?path=/upload/community/medal/",
				data
			);
		},
	},
	notice: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "通知附件",
		post: async function (data) {
			return await http.post(this.url + "?path=/upload/notice/", data);
		},
	},
	wocorder: {
		url: `${config.API_URL}/scmsysfile/upload`,
		name: "工单附件",
		post: async function (data) {
			return await http.post(this.url + "?path=/upload/woc-order/", data);
		},
	},
};
