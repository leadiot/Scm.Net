import config from "@/config";
import http from "@/utils/request";
export default {
	view: {
		url: `${config.API_URL}/scmmsgcomment`,
		name: "查看",
		get: async function (params) {
			return await http.get(this.url, params);
		},
	},
	save: {
		url: `${config.API_URL}/scmmsgcomment`,
		name: "保存",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	delete: {
		url: `${config.API_URL}/scmmsgcomment`,
		name: "删除",
		delete: async function (data) {
			return await http.delete(this.url, data);
		},
	},
};
