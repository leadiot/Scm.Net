import config from "@/config";
import http from "@/utils/request";
export default {
	option: {
		url: `${config.API_URL}/scmsysregion/option/`,
		name: "列表",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	list: {
		url: `${config.API_URL}/scmsysregion/listbypid/`,
		name: "查询一条",
		get: async function (params) {
			return await http.get(this.url + params);
		},
	},
	DEF_PID: "1000000000000000002",
};
