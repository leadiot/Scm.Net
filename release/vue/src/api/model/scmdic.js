import config from "@/config"
import http from "@/utils/request"
export default {
    option: {
        url: `${config.API_URL}/scmdic/option/`,
        name: "列表",
        get: async function (params) {
            return await http.get(this.url + params);
        }
    }
}