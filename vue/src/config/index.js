const DEFAULT_CONFIG = {
	// 应用代码
	APP_CODE: import.meta.env.VITE_APP_CODE,

	// 应用名称
	APP_NAME: import.meta.env.VITE_APP_NAME,

	// 简介（支持HTML）
	APP_DESC: import.meta.env.VITE_APP_DESC,

	// 版本号
	APP_VER: import.meta.env.VITE_APP_VER,

	// 内核版本号
	CORE_VER: import.meta.env.VITE_APP_VER,

	// 构建日期
	APP_BUILD: import.meta.env.VITE_APP_BUILD,

	// 应用Key
	APP_KEY: "870cb6cfa286ac4cac462bc8e33b1bb5",

	// 参数加密签名Key
	SIGN_KEY: "ab517f95fab7d57",

	// 首页地址
	DASHBOARD_URL: "/dashboard",

	// 服务器地址
	SERVER_URL: import.meta.env.VITE_API_BASE,

	// 接口地址
	API_URL: import.meta.env.VITE_API_BASE + "/api",

	// SignalR地址
	SignalR_URL: import.meta.env.VITE_API_BASE + "/scmhub",

	// 请求超时
	TIMEOUT: 10000,

	// TokenName
	TOKEN_NAME: "ApiToken",

	// Token前缀，注意最后有个空格，如不需要需设置空字符串
	TOKEN_PREFIX: "",

	// 追加其他头
	HEADERS: {},

	// 请求是否开启缓存
	REQUEST_CACHE: false,

	// 布局 默认：default | 通栏：header | 经典：menu | 功能坞：dock
	// dock将关闭标签和面包屑栏
	LAYOUT: "default",

	// 菜单是否折叠
	MENU_IS_COLLAPSE: false,

	// 菜单是否启用手风琴效果
	MENU_UNIQUE_OPENED: false,

	// 是否开启多标签
	LAYOUT_TAGS: true,

	// 语言
	LANG: "zh-cn",

	// 主题颜色
	COLOR: "",

	// 首面
	HOME: "1",

	//控制台首页默认布局
	DEFAULT_GRID: {
		//默认分栏数量和宽度 例如 [24] [18,6] [8,8,8] [6,12,6]
		layout: [12, 6, 6],
		//小组件分布，com取值:views/home/components 文件名
		copmsList: [["welcome", "ver"], ["time", "progress"], ["about"]],
	},

	// 默认登录类型，不能修改
	DEF_LOGIN_TYPE: 2,
	// 默认登录模式，可以修改，支持10，20，30，40四种方式
	DEF_LOGIN_MODE: [10, 40],
	// 默认登录单位，不能修改，否则生产环境无法登录
	DEF_LOGIN_UNIT: "dev",
	// 默认登录用户，便于开发时减少输入，可以修改或置空
	DEF_LOGIN_USER: "",
	// 默认登录口令，便于开发时减少输入，可以修改或置空
	DEF_LOGIN_PASS: "",

	// OIDC KEY，可以修改为您的应用KEY
	OIDC_KEY: import.meta.env.VITE_APP_OIDC_KEY,
	// OIDC 服务列表，不能修改，否则三方登录可能无法使用
	OIDC_OSP: import.meta.env.VITE_APP_OIDC_OSP,
	// OIDC 应用图标，不能修改，否则服务图标显示异常
	OIDC_LOGO: import.meta.env.VITE_APP_OIDC_LOGO,
	// OIDC 授权路径，不能修改，否则外部授权页面无法打开
	OIDC_AUTH: import.meta.env.VITE_APP_OIDC_AUTH,
	// OIDC 绑定路径，不能修改，否则外部授权页面无法打开
	OIDC_BIND: import.meta.env.VITE_APP_OIDC_BIND,
	// OIDC 登录路径，不能修改，否则外部登录页面无法打开
	OIDC_LOGIN: import.meta.env.VITE_APP_OIDC_LOGIN,
	// OIDC 重定向URI，不能修改，否则授权回调页面无法打开
	OIDC_REDIRECT_URI: import.meta.env.VITE_APP_OIDC_REDIRECT_URI,

	// 是否开启用户注册
	USER_REGISTER_ENABLED: false,

	// 系统预定义颜色列表，可以根据需要修改
	PREDEFINE_COLORS: [
		"#ffffff",
		"#cccccc",
		"#999999",
		"#666666",
		"#333333",
		"#000000",
		"#ff0000",
		"#ff4500",
		"#ff8c00",
		"#ffd700",
		"#00ff00",
		"#90ee90",
		"#67c23a",
		"#00ced1",
		"#009688",
		"#1e90ff",
		"#409eff",
		"#536dfe",
		"#0000ff",
		"#c71585",
	],

	// 商务部备案信息，可以使用HTML标签，例如：<a href="https://www.miitbeian.gov.cn/">粤ICP备2023030233号</a>
	BEIAN_MIIT: "",
	// 公安部备案信息，可以使用HTML标签，例如：<a href="https://www.beian.gov.cn/portal/registerSystemInfo?recordcode=粤ICP备2023030233号">粤ICP备2023030233号</a>
	BEIAN_MPS: "",
};

Object.assign(DEFAULT_CONFIG, APP_CONFIG || {});

export default DEFAULT_CONFIG;
