import config from "@/config";

//系统路由
const routes = [
	{
		id: '1000',
		name: "layout",
		path: "/",
		component: () => import(/* webpackChunkName: "layout" */ "@/layout"),
		redirect: config.DASHBOARD_URL || "/dashboard",
		children: [],
	},
	{
		id: '1001',
		path: "/about",
		component: () => import(/* webpackChunkName: "about" */ "@/views/about"),
		meta: { title: "关于" },
	},
	{
		id: '1002',
		path: "/login",
		component: () => import(/* webpackChunkName: "login" */ "@/views/login"),
		meta: { title: "登录" },
	},
	{
		id: '1003',
		path: "/user_register",
		component: () => import(/* webpackChunkName: "userRegister" */ "@/views/login/userRegister"),
		meta: { title: "用户注册" },
	},
	{
		id: '1004',
		path: "/reset_password",
		component: () => import(/* webpackChunkName: "resetPassword" */ "@/views/login/resetPassword"),
		meta: { title: "重置密码" },
	},
	{
		id: '1005',
		path: "/oauth",
		component: () => import(/* webpackChunkName: "userRegister" */ "@/views/oauth"),
		meta: { title: "联合登录" },
	},
	{
		id: '1006',
		path: "/scm/sys/workflow/design",
		component: () => import(/* webpackChunkName: "userRegister" */ "@/views/scm/sys/flowinfo/design"),
		meta: { title: "流程设计" },
	},
];

export default routes;
