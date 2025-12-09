import config from "@/config";
import http from "@/utils/request";
export default {
	model: {
		url: `${config.API_URL}/scmsystable/`,
		name: "根据编码获取",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	save: {
		url: `${config.API_URL}/scmsystable/save`,
		name: "保存",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
};
