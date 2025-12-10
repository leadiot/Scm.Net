import config from "@/config"
import http from "@/utils/request"
export default {
	page: {
		url: `${config.API_URL}/scmcfgexportheader/pages`,
		name: "列表",
		get: async function(params){
			return await http.get(this.url,params);
		}
	},
	list: {
		url: `${config.API_URL}/scmcfgexportheader/list`,
		name: "列表",
		get: async function(params){
			return await http.get(this.url,params);
		}
	},
	view: {
		url: `${config.API_URL}/scmcfgexportheader/view/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	edit: {
		url: `${config.API_URL}/scmcfgexportheader/edit/`,
		name: "编辑",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	},
	add: {
		url: `${config.API_URL}/scmcfgexportheader`,
		name: "添加",
		post: async function(data){
			return await http.post(this.url, data);
		}
	},
	model: {
		url: `${config.API_URL}/scmcfgexportheader/`,
		name: "查询一条",
		get: async function(params){
			return await http.get(this.url + params);
		}
	},
	update: {
		url: `${config.API_URL}/scmcfgexportheader`,
		name: "修改",
		put: async function(data){
			return await http.put(this.url,data);
		}
	},
	delete: {
		url: `${config.API_URL}/scmcfgexportheader`,
		name: "删除",
		delete: async function(params){
			return await http.delete(this.url+'?ids='+params);
		}
	}
}