import config from "@/config"
import http from "@/utils/request"

export default {
	model: {
		url: `${config.API_URL}/scmsysvote/`,
		name: "查询一条",
		get: async function (params) {
			return await http.get(this.url + params);
		}
	}
}


