const APP_CONFIG = {
	/** 应用代码，默认不需要修改 */
	APP_CODE: "Scm.Net",
	/** 应用名称，可以根据需要修改 */
	APP_NAME: "AppName",
	/** 产品描述，可以根据需要修改 */
	APP_DESC: "这是应用的简单介绍。",

	/** 默认登录模式，可以修改，支持10，20，30，40四种方式 */
	DEF_LOGIN_MODE: [10, 40],
	/** 默认登录用户，便于开发时减少输入，可以修改或置空 */
	DEF_LOGIN_USER: "admin",
	/** 默认登录口令，便于开发时减少输入，可以修改或置空 */
	DEF_LOGIN_PASS: "123456",

	/** OIDC KEY，可以修改为您的应用KEY */
	OIDC_KEY: "08dc965832db7248",
	/** OIDC 服务列表，不能修改，否则三方登录可能无法使用 */
	OIDC_OSP: "https://oidc.org.cn/oauth/apposp/",
	/** OIDC 应用图标，不能修改，否则服务图标显示异常 */
	OIDC_LOGO: "https://oidc.org.cn/data/logo/",
	/** OIDC 授权路径，不能修改，否则外部授权页面无法打开 */
	OIDC_AUTH:
		"https://oidc.org.cn/oauth/login/{osp}?client_id={key}&state={state}",
	/** OIDC 绑定路径，不能修改，否则外部授权页面无法打开 */
	OIDC_BIND: "https://oidc.org.cn/oauth/index?client_id={key}&state={state}",

	/** 系统预定义颜色列表，可以根据需要修改 */
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

	/** 商务部备案信息，可以使用HTML标签，例如：<a href="https://www.miitbeian.gov.cn/">粤ICP备2023030233号</a> */
	BEIAN_MIIT: "",
	/** 公安部备案信息，可以使用HTML标签，例如：<a href="https://www.beian.gov.cn/portal/registerSystemInfo?recordcode=粤ICP备2023030233号">粤ICP备2023030233号</a> */
	BEIAN_MPS: "",
};
