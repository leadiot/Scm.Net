import config from "@/config";
import http from "@/utils/request";
import tool from "@/utils/tool";

export default {
	token: {
		url: `${config.API_URL}/operator/login`,
		name: "登录获取TOKEN",
		post: async function (data = {}) {
			return await http.post(this.url, data);
		},
	},
	sendSms: {
		url: `${config.API_URL}/operator/sendSms`,
		name: "发送验证码",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	getAuth: {
		url: `${config.API_URL}/operator/getAuth/`,
		name: "获取授权信息",
		get: async function (data) {
			return await http.get(this.url + data);
		},
	},
	userwork: {
		url: `${config.API_URL}/operator/userwork`,
		name: "查询登录人信息",
		get: async function (data = {}) {
			return await http.get(this.url, data);
		},
	},
	signIn: {
		url: `${config.API_URL}/operator/signin`,
		name: "登录",
		post: async function (data = {}) {
			return await http.post(this.url, data);
		},
	},
	signon: {
		url: `${config.API_URL}/operator/signon`,
		name: "注册",
		post: async function (data) {
			return await http.post(this.url, data);
		},
	},
	dateTheme: {
		url: `${config.API_URL}/operator/datetheme`,
		name: "获取登录风格",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	userTheme: {
		url: `${config.API_URL}/operator/usertheme`,
		name: "获取登录风格",
		get: async function (data) {
			return await http.get(this.url, data);
		},
	},
	userBasic: {
		url: `${config.API_URL}/operator/userbasic`,
		name: "修改登录人基本信息",
		put: async function (data = {}) {
			return await http.put(this.url, data);
		},
	},
	userPass: {
		url: `${config.API_URL}/operator/userpass`,
		name: "修改密码",
		put: async function (data) {
			return await http.put(this.url, data);
		},
	},
	unitWork: {
		url: `${config.API_URL}/operator/unitwork`,
		name: "查询机构信息",
		get: async function (data = {}) {
			return await http.get(this.url, data);
		},
	},
	unitBasic: {
		url: `${config.API_URL}/operator/unitbasic`,
		name: "修改登录机构基本信息",
		put: async function (data = {}) {
			return await http.put(this.url, data);
		},
	},
	authority: {
		url: `${config.API_URL}/operator/authoritymenu`,
		name: "获得权限",
		get: async function (data) {
			if (!data) {
				data = {};
			}
			if (!data.client) {
				data.client = 10;
			}
			if (!data.lang) {
				data.lang = tool.getCache("APP_LANG") || config.LANG;
			}
			return await http.get(this.url, data);
		},
	},
	listOsp: {
		get: async function (url) {
			return await http.get(url);
		},
	},
};
