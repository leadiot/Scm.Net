import config from "@/config"
import http from "@/utils/request"

export default {
	page: {
		url: `${config.API_URL}/scmsysvotedetail/pages`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		}
	},
	list: {
		url: `${config.API_URL}/scmsysvotedetail/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	add: {
		url: `${config.API_URL}/scmsysvotedetail`,
		name: "添加",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	model: {
		url: `${config.API_URL}/scmsysvotedetail/`,
		name: "查询一条",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	batch: {
		url: `${config.API_URL}/scmsysvotedetail/batch`,
		name: "修改",
		post: async function (data) {
			return await http.post(this.url, data);
		}
	},
	delete: {
		url: `${config.API_URL}/scmsysvotedetail`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		}
	}
}


