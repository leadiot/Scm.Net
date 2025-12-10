import config from "@/config"
import http from "@/utils/request"

export default {
	summary: {
		url: `${config.API_URL}/workbench/summary`,
		name: "登录获取TOKEN",
		get: async function (data = {}) {
			return await http.get(this.url, data);
		}
	},
	report: {
		url: `${config.API_URL}/workbench/report`,
		name: "查询登录人信息",
		get: async function (data = {}) {
			return await http.get(this.url, data);
		}
	},
	todo: {
		url: `${config.API_URL}/workbench/todo`,
		name: "登录",
		get: async function (data = {}) {
			return await http.get(this.url, data);
		}
	},
}
