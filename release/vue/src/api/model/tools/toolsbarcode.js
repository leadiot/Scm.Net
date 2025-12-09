import config from "@/config"
import http from "@/utils/request"

export default {
	option: {
		url: `${config.API_URL}/barcode/option`,
		name: "列表",
		get: async function (data) {
			return await http.get(this.url, data);
		}
	},
	barcode: {
		url: `${config.API_URL}/barcode/image`,
		name: "图片",
		get: async function (data) {
			return await http.get(this.url, data);
		}
	},
	fonts: {
		url: `${config.API_URL}/barcode/fonts`,
		name: "字体",
		get: async function (data) {
			return await http.get(this.url, data);
		}
	}
}
