const APP_CONFIG = {
	/** 应用代码，默认不需要修改 */
	APP_CODE: "scm.net",
	/** 应用名称，可以根据需要修改 */
	APP_NAME: "Scm.Net",
	/** 产品描述，可以根据需要修改 */
	APP_DESC: "这是应用的简单介绍。",

	/** 服务路径(含端口)，可以根据需要修改 */
	//SERVER_URL: '',

	/** API调用路径(含端口)，可以根据需要修改 */
	//API_URL: '/api',

	/** 默认登录模式，可以修改，支持10，20，30，40四种方式 */
	DEF_LOGIN_MODE: [10, 30],
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

	PLATFORM_LIST: [
		{ id: 1, code: 'server', name: '桌面端应用', visible: false },
		{ id: 2, code: 'desktop', name: '桌面端应用', visible: false },
		{ id: 3, code: 'mobile', name: '移动端应用', visible: false },
		{ id: 4, code: 'browser', name: '网页端应用', visible: false },
		{ id: 5, code: 'quickapp', name: '小程序应用', visible: false }
	],

	DOWNLOAD_LIST: [
		{ id: 'window', client: 20, platform: 'desktop', method: 'file', visible: true, name: 'Windows', desc: '适用于Windows 10及以上版本', url: '/download/app_v1.0.0.zip' },
		{ id: 'macos', client: 30, platform: 'desktop', method: 'appstore', visible: true, name: 'MacOS', desc: '适用于macOS 10.15及以上版本', url: '/download/app_v1.0.0.zip' },
		{ id: 'linux', client: 40, platform: 'desktop', method: 'qrcode', visible: true, name: 'Linux', desc: '适用于大多数Linux发行版', url: '/download/app_v1.0.0.zip' },
		{ id: 'android', client: 50, name: 'Android', desc: '适用于Android 5.0及以上版本', url: '/download/app_v1.0.0.zip' },
		{ id: 'ios', client: 60, name: 'iOS', desc: '适用于iOS 11及以上版本', url: '/download/app_v1.0.0.zip' },
		{ id: 'harmony', client: 70, name: '鸿蒙', desc: '适用于鸿蒙系统', url: '/download/app_v1.0.0.zip' },
		{ id: 'chrome', client: 11, name: 'Google Chrome', desc: '适用于Google Chrome 80及以上版本', url: '/download/app_v1.0.0.zip' },
		{ id: 'edge', client: 12, name: 'Microsoft Edge', desc: '适用于Microsoft Edge 80及以上版本', url: '/download/app_v1.0.0.zip' },
		{ id: 'firefox', client: 13, name: 'Mozilla Firefox', desc: '适用于Mozilla Firefox 78及以上版本', url: '/download/app_v1.0.0.zip' },
		{ id: 'wechat', client: 81, name: '微信小程序', desc: '适用于微信小程序', url: '/download/app_v1.0.0.zip' },
		{ id: 'alipay', client: 82, name: '支付宝小程序', desc: '适用于支付宝小程序', url: '/download/app_v1.0.0.zip' }
	],
};
