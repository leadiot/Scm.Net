import config from "@/config"
import http from "@/utils/request"
export default {
    page: {
        url: `${config.API_URL}/scmloguser/pages`,
        name: "查询",
        get: async function(params){
            return await http.get(this.url,params);
        }
    },
    list: {
        url: `${config.API_URL}/scmloguser/list`,
        name: "列表",
        get: async function(params){
            return await http.get(this.url + params);
        }
    },
    get: {
        url: `${config.API_URL}/scmloguser/`,
        name: "查看",
        get: async function(params){
            return await http.get(this.url + params);
        }
    }
}