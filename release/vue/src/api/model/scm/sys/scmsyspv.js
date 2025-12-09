import config from "@/config";
import http from "@/utils/request";
export default {
	save: {
		url: `${config.API_URL}/scmsyspv`,
		name: "更新",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
};
