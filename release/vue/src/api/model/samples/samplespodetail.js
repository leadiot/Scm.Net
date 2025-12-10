import config from "@/config"
import http from "@/utils/request"
export default {
	page: {
		url: `${config.API_URL}/samplespodetail/pages`,
		name: "分页",
		get: async function (params) {
			return await http.get(this.url, params);
		}
	},
	list: {
		url: `${config.API_URL}/samplespodetail/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		}
	},
	view: {
		url: `${config.API_URL}/samplespodetail/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	edit: {
		url: `${config.API_URL}/samplespodetail/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	add: {
		url: `${config.API_URL}/samplespodetail`,
		name: "新增",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	update: {
		url: `${config.API_URL}/samplespodetail`,
		name: "更新",
		put: async function (data) {
			return await http.put(this.url, data);
		}
	},
	save: {
		url: `${config.API_URL}/samplespodetail`,
		name: "保存",
		put: async function (data) {
			return await http.put(this.url, data);
		}
	},
	status: {
		url: `${config.API_URL}/samplespodetail/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	delete: {
		url: `${config.API_URL}/samplespodetail`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		}
	},
	option: {
		url: `${config.API_URL}/samplespodetail/option`,
		name: "选择",
		get: async function (data) {
			return await http.get(this.url, data);
		}
	},
	batchappend: {
		url: `${config.API_URL}/samplespodetail/batchappend`,
		name: "批量追加",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	batchupdate: {
		url: `${config.API_URL}/samplespodetail/batchupdate`,
		name: "批量更新",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
}