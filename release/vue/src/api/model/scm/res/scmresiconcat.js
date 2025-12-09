import config from "@/config";
import http from "@/utils/request";
export default {
	list: {
		url: `${config.API_URL}/scmresiconcat/list`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	option: {
		url: `${config.API_URL}/scmresiconcat/option/`,
		name: "列表",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	}
};
