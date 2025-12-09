import config from "@/config"
import http from "@/utils/request"
export default {
	page: {
		url: `${config.API_URL}/samplesbook/page`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		}
	},
	view: {
		url: `${config.API_URL}/samplesbook/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	edit: {
		url: `${config.API_URL}/samplesbook/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	add: {
		url: `${config.API_URL}/samplesbook`,
		name: "添加",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	save: {
		url: `${config.API_URL}/samplesbook`,
		name: "保存",
		put: async function (data) {
			return await http.put(this.url, data);
		}
	},
	update: {
		url: `${config.API_URL}/samplesbook/update`,
		name: "修改",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	status: {
		url: `${config.API_URL}/samplesbook/status`,
		name: "状态",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	delete: {
		url: `${config.API_URL}/samplesbook`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		}
	},
	upload: {
		url: `${config.API_URL}/samplesbook/upload`,
		name: "上传",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	import: {
		url: `${config.API_URL}/samplesbook/import`,
		name: "上传",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	file: {
		url: `${config.API_URL}/samplesbook/file`,
		name: "查看文件",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	}
}