import config from "@/config"
import http from "@/utils/request"
export default {
	page: {
		url: `${config.API_URL}/samplespoheader/pages`,
		name: "分页",
		get: async function (params) {
			return await http.get(this.url, params);
		}
	},
	list: {
		url: `${config.API_URL}/samplespoheader/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	view: {
		url: `${config.API_URL}/samplespoheader/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	edit: {
		url: `${config.API_URL}/samplespoheader/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	add: {
		url: `${config.API_URL}/samplespoheader`,
		name: "新增",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	update: {
		url: `${config.API_URL}/samplespoheader`,
		name: "更新",
		put: async function (data) {
			return await http.put(this.url, data);
		}
	},
	save: {
		url: `${config.API_URL}/samplespoheader`,
		name: "保存",
		put: async function (data) {
			return await http.put(this.url, data);
		}
	},
	status: {
		url: `${config.API_URL}/samplespoheader/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	delete: {
		url: `${config.API_URL}/samplespoheader`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		}
	},
	option: {
		url: `${config.API_URL}/samplespoheader/option`,
		name: "选择",
		get: async function (data) {
			return await http.get(this.url, data);
		}
	},
	submit: {
		url: `${config.API_URL}/samplespoflow/submit/`,
		name: "提交审批",
		post: async function (data) {
			return await http.post(this.url + data);
		}
	},
	accept: {
		url: `${config.API_URL}/samplespoflow/accept`,
		name: "审批通过",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	reject: {
		url: `${config.API_URL}/samplespoflow/reject`,
		name: "审批拒绝",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
}