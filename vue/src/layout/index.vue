<template>

	<!-- 桌面布局 -->
	<template v-if="layout == 'none'">
		<section class="scmui-wrapper">
			<div class="scmui-body el-container">
				<div class="scmui-main" id="scmui-main">
					<router-view v-slot="{ Component }">
						<keep-alive :include="this.$store.state.keepAlive.keepLiveRoute">
							<component :is="Component" :key="$route.fullPath" v-if="$store.state.keepAlive.routeShow" />
						</keep-alive>
					</router-view>
					<iframe-view></iframe-view>
				</div>
			</div>
		</section>
	</template>

	<!-- 通栏布局 -->
	<template v-else-if="layout == 'header'">
		<header class="scmui-header">
			<div class="scmui-header-left">
				<div class="logo-bar">
					<img class="logo" src="img/logo.png">
					<span>{{ $CONFIG.APP_NAME }}</span>
				</div>
				<ul v-if="!ismobile" class="nav">
					<li v-for="item in menu" :key="item" :class="pmenu.path == item.path ? 'active' : ''"
						@click="showMenu(item)">
						<sc-icon :name="item.meta.icon || 'sc-menu-fill'"></sc-icon>
						<span>{{ item.meta.title }}</span>
					</li>
				</ul>
			</div>
			<div class="scmui-header-right">
				<userbar></userbar>
			</div>
		</header>
		<section class="scmui-wrapper">
			<div v-if="!ismobile && nextMenu.length > 0 || !pmenu.component"
				:class="menuIsCollapse ? 'scmui-side isCollapse' : 'scmui-side'">
				<div v-if="!menuIsCollapse" class="scmui-side-top">
					<h2>{{ pmenu.meta.title }}</h2>
				</div>
				<div class="scmui-side-scroll">
					<el-scrollbar>
						<el-menu :default-active="active" router :collapse="menuIsCollapse"
							:unique-opened="$CONFIG.MENU_UNIQUE_OPENED">
							<NavMenu :navMenus="nextMenu"></NavMenu>
						</el-menu>
					</el-scrollbar>
				</div>
				<div class="scmui-side-bottom" @click="$store.commit('TOGGLE_menuIsCollapse')">
					<sc-icon :name="menuIsCollapse ? 'sc-menu-show' : 'sc-menu-hide'"></sc-icon>
				</div>
			</div>
			<Side-m v-if="ismobile"></Side-m>
			<div class="scmui-body el-container">
				<Topbar v-if="!ismobile"></Topbar>
				<Tags v-if="!ismobile && layoutTags"></Tags>
				<div class="scmui-main" id="scmui-main">
					<router-view v-slot="{ Component }">
						<keep-alive :include="this.$store.state.keepAlive.keepLiveRoute">
							<component :is="Component" :key="$route.fullPath" v-if="$store.state.keepAlive.routeShow" />
						</keep-alive>
					</router-view>
					<iframe-view></iframe-view>
				</div>
			</div>
		</section>
	</template>

	<!-- 经典布局 -->
	<template v-else-if="layout == 'menu'">
		<header class="scmui-header">
			<div class="scmui-header-left">
				<div class="logo-bar">
					<img class="logo" src="img/logo.png">
					<span>{{ $CONFIG.APP_NAME }}</span>
				</div>
			</div>
			<div class="scmui-header-right">
				<userbar></userbar>
			</div>
		</header>
		<section class="scmui-wrapper">
			<div v-if="!ismobile" :class="menuIsCollapse ? 'scmui-side isCollapse' : 'scmui-side'">
				<div class="scmui-side-scroll">
					<el-scrollbar>
						<el-menu :default-active="active" router :collapse="menuIsCollapse"
							:unique-opened="$CONFIG.MENU_UNIQUE_OPENED">
							<NavMenu :navMenus="menu"></NavMenu>
						</el-menu>
					</el-scrollbar>
				</div>
				<div class="scmui-side-bottom" @click="$store.commit('TOGGLE_menuIsCollapse')">
					<sc-icon :name="menuIsCollapse ? 'sc-menu-show' : 'sc-menu-hide'"></sc-icon>
				</div>
			</div>
			<Side-m v-if="ismobile"></Side-m>
			<div class="scmui-body el-container">
				<Topbar v-if="!ismobile"></Topbar>
				<Tags v-if="!ismobile && layoutTags"></Tags>
				<div class="scmui-main" id="scmui-main">
					<router-view v-slot="{ Component }">
						<keep-alive :include="this.$store.state.keepAlive.keepLiveRoute">
							<component :is="Component" :key="$route.fullPath" v-if="$store.state.keepAlive.routeShow" />
						</keep-alive>
					</router-view>
					<iframe-view></iframe-view>
				</div>
			</div>
		</section>
	</template>

	<!-- 功能坞布局 -->
	<template v-else-if="layout == 'dock'">
		<header class="scmui-header">
			<div class="scmui-header-left">
				<div class="logo-bar">
					<img class="logo" src="img/logo.png">
					<span>{{ $CONFIG.APP_NAME }}</span>
				</div>
			</div>
			<div class="scmui-header-right">
				<div v-if="!ismobile" class="scmui-header-menu">
					<el-menu mode="horizontal" :default-active="active" router background-color="#222b45"
						text-color="#fff" active-text-color="var(--el-color-primary)">
						<NavMenu :navMenus="menu"></NavMenu>
					</el-menu>
				</div>
				<Side-m v-if="ismobile"></Side-m>
				<userbar></userbar>
			</div>
		</header>
		<section class="scmui-wrapper">
			<div class="scmui-body el-container">
				<Tags v-if="!ismobile && layoutTags"></Tags>
				<div class="scmui-main" id="scmui-main">
					<router-view v-slot="{ Component }">
						<keep-alive :include="this.$store.state.keepAlive.keepLiveRoute">
							<component :is="Component" :key="$route.fullPath" v-if="$store.state.keepAlive.routeShow" />
						</keep-alive>
					</router-view>
					<iframe-view></iframe-view>
				</div>
			</div>
		</section>
	</template>

	<!-- 默认布局 -->
	<template v-else>
		<section class="scmui-wrapper">
			<div v-if="!ismobile" class="scmui-side-split">
				<div class="scmui-side-split-top">
					<router-link :to="$CONFIG.DASHBOARD_URL">
						<img class="logo" :title="$CONFIG.APP_NAME" src="img/logo-r.png">
					</router-link>
				</div>
				<div class="scmui-side-split-scroll">
					<el-scrollbar>
						<ul>
							<li v-for="item in menu" :key="item" :class="pmenu.path == item.path ? 'active' : ''"
								@click="showMenu(item)">
								<sc-icon :name="item.meta.icon || 'sc-menu-fill'"></sc-icon>
								<p>{{ item.meta.title }}</p>
							</li>
						</ul>
					</el-scrollbar>
				</div>
			</div>
			<div v-if="!ismobile && nextMenu.length > 0 || !pmenu.component"
				:class="menuIsCollapse ? 'scmui-side isCollapse' : 'scmui-side'">
				<div v-if="!menuIsCollapse" class="scmui-side-top">
					<h2>{{ pmenu.meta.title }}</h2>
				</div>
				<div class="scmui-side-scroll">
					<el-scrollbar>
						<el-menu :default-active="active" router :collapse="menuIsCollapse"
							:unique-opened="$CONFIG.MENU_UNIQUE_OPENED">
							<NavMenu :navMenus="nextMenu"></NavMenu>
						</el-menu>
					</el-scrollbar>
				</div>
				<div class="scmui-side-bottom" @click="$store.commit('TOGGLE_menuIsCollapse')">
					<sc-icon :name="menuIsCollapse ? 'sc-menu-show' : 'sc-menu-hide'"></sc-icon>
				</div>
			</div>
			<Side-m v-if="ismobile"></Side-m>
			<div class="scmui-body el-container">
				<Topbar>
					<userbar></userbar>
				</Topbar>
				<Tags v-if="!ismobile && layoutTags"></Tags>
				<div class="scmui-main" id="scmui-main">
					<router-view v-slot="{ Component }">
						<keep-alive :include="this.$store.state.keepAlive.keepLiveRoute">
							<component :is="Component" :key="$route.fullPath" v-if="$store.state.keepAlive.routeShow" />
						</keep-alive>
					</router-view>
					<iframe-view></iframe-view>
				</div>
			</div>
		</section>
	</template>

	<div class="main-maximize-exit" @click="exitMaximize"><sc-icon title="退出全屏" name="close" /></div>

	<div class="layout-setting" @click="openFeedback"><sc-icon name="sc-feedback-line" title="用户反馈" /></div>
	<feedback ref="feedback"></feedback>
</template>

<script>
import SideM from './components/sideMenu.vue';
import Topbar from './components/topbar.vue';
import Tags from './components/tags.vue';
import NavMenu from './components/navMenu.vue';
import userbar from './components/userbar.vue';
import setting from './components/setting.vue';
import feedback from './components/feedback.vue';
import iframeView from './components/iframeView.vue';

export default {
	name: 'index',
	components: {
		SideM,
		Topbar,
		Tags,
		NavMenu,
		userbar,
		setting,
		feedback,
		iframeView
	},
	data() {
		return {
			menu: [],
			nextMenu: [],
			pmenu: {},
			active: ''
		}
	},
	computed: {
		ismobile() {
			return this.$store.state.global.ismobile
		},
		layout() {
			return this.$store.state.global.layout
		},
		layoutTags() {
			return this.$store.state.global.layoutTags
		},
		menuIsCollapse() {
			return this.$store.state.global.menuIsCollapse
		}
	},
	created() {
		this.onLayoutResize();
		window.addEventListener('resize', this.onLayoutResize);
		var menu = this.$router.sc_getMenu();
		this.menu = this.filterUrl(menu);
		this.showThis()
	},
	watch: {
		$route() {
			this.showThis()
		},
		layout: {
			handler(val) {
				document.body.setAttribute('data-layout', val)
			},
			immediate: true,
		}
	},
	methods: {
		openFeedback() {
			this.$refs.feedback.open();
		},
		onLayoutResize() {
			this.$store.commit("SET_ismobile", document.body.clientWidth < 992)
		},
		//路由监听高亮
		showThis() {
			this.pmenu = this.$route.meta.breadcrumb ? this.$route.meta.breadcrumb[0] : {}
			this.nextMenu = this.filterUrl(this.pmenu.children);
			this.$nextTick(() => {
				this.active = this.$route.meta.active || this.$route.fullPath;
			})
		},
		//点击显示
		showMenu(route) {
			this.pmenu = route;
			this.nextMenu = this.filterUrl(route.children);
			if ((!route.children || route.children.length == 0) && route.component) {
				this.$router.push({ path: route.path })
			}
		},
		//转换外部链接的路由
		filterUrl(map) {
			var newMap = []
			map && map.forEach(item => {
				if (!item.id) {
					return false;
				}
				item.meta = item.meta ? item.meta : {};
				//处理隐藏
				if (item.meta.hidden) {
					return false
				}
				//处理http
				if (item.meta.type == 'iframe') {
					item.path = `/i/${item.name}`;
				}
				//递归循环
				if (item.children && item.children.length > 0) {
					item.children = this.filterUrl(item.children)
				}
				newMap.push(item)
			})
			return newMap;
		},
		//退出最大化
		exitMaximize() {
			document.getElementById('app').classList.remove('main-maximize')
		}
	}
}
</script>
