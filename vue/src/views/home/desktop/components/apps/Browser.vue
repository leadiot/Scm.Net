<template>
	<div class="browser-app">
		<div class="browser-toolbar">
			<button @click="goBack" class="nav-btn" :disabled="!canGoBack">
				<el-icon>
					<ArrowLeft />
				</el-icon>
			</button>
			<button @click="goForward" class="nav-btn" :disabled="!canGoForward">
				<el-icon>
					<ArrowRight />
				</el-icon>
			</button>
			<button @click="refresh" class="nav-btn">
				<el-icon>
					<Refresh />
				</el-icon>
			</button>
			<div class="url-bar">
				<el-input v-model="url" placeholder="输入网址..." @keyup.enter="navigate">
					<template #prefix>
						<el-icon>
							<Lock />
						</el-icon>
					</template>
				</el-input>
			</div>
			<button @click="navigate" class="go-btn">前往</button>
		</div>
		<div class="browser-content">
			<div class="welcome-page">
				<h2>欢迎使用浏览器</h2>
				<div class="quick-links">
					<div v-for="link in quickLinks" :key="link.url" class="quick-link" @click="visitLink(link.url)">
						<div class="link-icon">{{ link.icon }}</div>
						<span>{{ link.name }}</span>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
import { ref } from 'vue';
import { ArrowLeft, ArrowRight, Refresh, Lock } from '@element-plus/icons-vue';

export default {
	name: 'Browser',
	components: { ArrowLeft, ArrowRight, Refresh, Lock },
	setup() {
		const url = ref('');
		const canGoBack = ref(false);
		const canGoForward = ref(false);
		const quickLinks = ref([
			{ name: '百度', url: 'https://www.baidu.com', icon: '🔍' },
			{ name: '谷歌', url: 'https://www.google.com', icon: '🌐' },
			{ name: 'GitHub', url: 'https://github.com', icon: '💻' },
			{ name: '知乎', url: 'https://www.zhihu.com', icon: '📚' },
			{ name: '微博', url: 'https://weibo.com', icon: '📱' },
			{ name: '淘宝', url: 'https://www.taobao.com', icon: '🛒' },
		]);

		const navigate = () => {
			if (url.value) {
				alert(`导航到: ${url.value}`);
			}
		};

		const goBack = () => {
			alert('返回上一页');
		};

		const goForward = () => {
			alert('前进下一页');
		};

		const refresh = () => {
			alert('刷新页面');
		};

		const visitLink = (linkUrl) => {
			url.value = linkUrl;
			alert(`访问: ${linkUrl}`);
		};

		return {
			url,
			canGoBack,
			canGoForward,
			quickLinks,
			navigate,
			goBack,
			goForward,
			refresh,
			visitLink,
		};
	},
};
</script>

<style scoped>
.browser-app {
	height: 100%;
	display: flex;
	flex-direction: column;
}

.browser-toolbar {
	display: flex;
	align-items: center;
	gap: 8px;
	padding: 10px;
	background-color: #f5f5f5;
	border-bottom: 1px solid #e0e0e0;
}

.nav-btn {
	width: 32px;
	height: 32px;
	border: none;
	border-radius: 4px;
	background-color: transparent;
	cursor: pointer;
	display: flex;
	align-items: center;
	justify-content: center;
	transition: background-color 0.3s;
}

.nav-btn:hover:not(:disabled) {
	background-color: #e0e0e0;
}

.nav-btn:disabled {
	opacity: 0.5;
	cursor: not-allowed;
}

.url-bar {
	flex: 1;
}

.go-btn {
	padding: 8px 16px;
	border: none;
	border-radius: 4px;
	background-color: #409eff;
	color: #fff;
	cursor: pointer;
	transition: background-color 0.3s;
}

.go-btn:hover {
	background-color: #66b1ff;
}

.browser-content {
	flex: 1;
	overflow: auto;
	background-color: #fff;
}

.welcome-page {
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
	height: 100%;
	padding: 40px;
}

.welcome-page h2 {
	font-size: 28px;
	color: #333;
	margin-bottom: 40px;
}

.quick-links {
	display: grid;
	grid-template-columns: repeat(3, 1fr);
	gap: 20px;
	max-width: 600px;
}

.quick-link {
	display: flex;
	flex-direction: column;
	align-items: center;
	gap: 10px;
	padding: 20px;
	cursor: pointer;
	border-radius: 8px;
	transition: background-color 0.3s;
}

.quick-link:hover {
	background-color: #f5f5f5;
}

.link-icon {
	width: 60px;
	height: 60px;
	border-radius: 12px;
	background-color: #f0f0f0;
	display: flex;
	align-items: center;
	justify-content: center;
	font-size: 32px;
}

.quick-link span {
	font-size: 14px;
	color: #666;
}
</style>
