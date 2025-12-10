import config from "@/config";
import http from "@/utils/request";

export default {
	option: {
		url: `${config.API_URL}/scmsyslang/option`,
		name: "列表",
		get: async function () {
			return await http.get(this.url);
		},
	}
};
