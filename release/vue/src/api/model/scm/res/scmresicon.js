import config from "@/config";
import http from "@/utils/request";
export default {
	list: {
		url: `${config.API_URL}/scmresicon/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	model: {
		url: `${config.API_URL}/scmresicon/`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	}
};
