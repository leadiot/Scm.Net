import { createRouter, createWebHashHistory } from "vue-router";
import { ElNotification } from "element-plus";
import config from "@/config";
import NProgress from "nprogress-esm";
import "nprogress-esm/dist/style.css";
import tool from "@/utils/tool";
import systemRouter from "./systemRouter";
import userRoutes from "@/config/route";
import { beforeEach, afterEach } from "./scrollBehavior";
import http from "@/utils/request";

//系统路由
const routes = systemRouter;

//系统特殊路由
const routes_404 = {
	id: '404',
	path: "/:pathMatch(.*)*",
	hidden: true,
	component: () => import("@/layout/other/404"),
};
let routes_404_r = () => { };

const router = createRouter({
	history: createWebHashHistory(),
	routes: routes,
});

//设置标题
document.title = config.APP_NAME;

//判断是否已加载过动态/静态路由
var isGetRouter = false;

router.beforeEach(async (to, from) => {
	NProgress.start();
	//动态标题
	document.title = to.meta.title
		? `${to.meta.title} - ${config.APP_NAME}`
		: `${config.APP_NAME}`;

	if (to.path === "/login") {
		//删除路由(替换当前layout路由)
		router.addRoute(routes[0]);
		//删除路由(404)
		routes_404_r();
		isGetRouter = false;
		return;
	}

	if (routes.findIndex((r) => r.path === to.path) >= 0) {
		return;
	}

	let token = tool.data.get("TOKEN");
	if (!token) {
		return { path: "/login" };
	}

	//整页路由处理
	if (to.meta.fullpage) {
		to.matched = [to.matched[to.matched.length - 1]];
	}
	//加载动态/静态路由
	if (!isGetRouter) {
		let apiMenu = tool.data.get("MENU") || [];
		let userInfo = tool.data.get("USER_INFO");
		let userMenu = treeFilter(userRoutes, (node) => {
			return node.meta.role
				? node.meta.role.filter(
					(item) => userInfo.role.indexOf(item) > -1
				).length > 0
				: true;
		});
		let menu = [...userMenu, ...apiMenu];
		var menuRouter = filterAsyncRouter(menu);
		menuRouter = flatAsyncRoutes(menuRouter);
		menuRouter.forEach((item) => {
			router.addRoute("layout", item);
		});
		routes_404_r = router.addRoute(routes_404);
		if (to.matched.length == 0) {
			router.push(to.fullPath);
		}
		isGetRouter = true;
	}
	beforeEach(to, from);
});

router.afterEach((to, from) => {
	afterEach(to, from);
	NProgress.done();
	recordPv(to);
});

router.onError((error) => {
	NProgress.done();
	ElNotification.error({
		title: "路由错误",
		message: error.message,
	});
});

//入侵追加自定义方法、对象
router.sc_getMenu = () => {
	var apiMenu = tool.data.get("MENU") || [];
	let userInfo = tool.data.get("USER_INFO");
	let userMenu = treeFilter(userRoutes, (node) => {
		return node.meta.role
			? node.meta.role.filter((item) => userInfo.role.indexOf(item) > -1)
				.length > 0
			: true;
	});
	var menu = [...userMenu, ...apiMenu];
	return menu;
};

async function recordPv(to) {
	var url = to.path;
	if (url == '/login' || url == '/oauth' || url == '/user_register' || url == '/reset_password') {
		return;
	}

	//var title = to.meta.title;
	// 记录访问信息
	await http.post(`${config.API_URL}/scmsyspv`, { url: url });
	// if (!res || res.code != 200) {
	// 	return;
	// }
}

//转换
function filterAsyncRouter(routerMap) {
	const accessedRouters = [];
	routerMap.forEach((item) => {
		item.meta = item.meta || {};
		//处理外部链接特殊路由
		if (item.meta.type == "iframe") {
			item.meta.url = item.path;
			item.path = `/i/${item.name}`;
		}
		//MAP转路由对象
		var route = {
			id: item.id,
			path: item.path,
			name: item.name,
			meta: item.meta,
			redirect: item.redirect,
			children: item.children ? filterAsyncRouter(item.children) : null,
			component: loadComponent(item.component),
		};
		accessedRouters.push(route);
	});
	return accessedRouters;
}

function loadComponent(component) {
	if (component) {
		const modules = import.meta.glob('@/views/**/*.vue');
		const paths = [
			`/src/views/${component}.vue`,
			`/src/views/${component}/index.vue`
		];
		for (const path of paths) {
			if (modules[path]) {
				return modules[path];
			}
		}
	}

	return () => import("@/layout/other/empty");
}

//路由扁平化
function flatAsyncRoutes(routes, breadcrumb = []) {
	let res = [];
	routes.forEach((route) => {
		const tmp = { ...route };
		if (tmp.children) {
			let childrenBreadcrumb = [...breadcrumb];
			childrenBreadcrumb.push(route);
			let tmpRoute = { ...route };
			tmpRoute.meta.breadcrumb = childrenBreadcrumb;
			delete tmpRoute.children;
			res.push(tmpRoute);
			let childrenRoutes = flatAsyncRoutes(
				tmp.children,
				childrenBreadcrumb
			);
			childrenRoutes.map((item) => {
				res.push(item);
			});
		} else {
			let tmpBreadcrumb = [...breadcrumb];
			tmpBreadcrumb.push(tmp);
			tmp.meta.breadcrumb = tmpBreadcrumb;
			res.push(tmp);
		}
	});
	return res;
}

//过滤树
function treeFilter(tree, func) {
	return tree
		.map((node) => ({ ...node }))
		.filter((node) => {
			node.children = node.children && treeFilter(node.children, func);
			return func(node) || (node.children && node.children.length);
		});
}

export default router;
