<template>
	<div class="desktop" :style="desktopStyle" @click="clearSelection">
		<!-- 桌面图标区域 -->
		<div class="desktop-icons">
			<div v-for="app in desktopApps" :key="app.id" class="desktop-icon"
				:class="{ selected: selectedApp === app.id }" @click="selectApp(app.id)"
				@dblclick.stop="openApp(app)">
				<div class="icon-image">
					<sc-icon name="sc-heart-3-line" :size="48" />
				</div>
				<div class="icon-text">{{ app.name }}</div>
			</div>
		</div>

		<!-- 窗口容器 -->
		<div class="windows-container">
			<Window v-for="window in windows" :key="window.id" :window="window" @close="closeWindow"
				@minimize="minimizeWindow" @maximize="maximizeWindow" @focus="focusWindow" />
		</div>

		<!-- 任务栏 -->
		<div class="taskbar">
			<div class="taskbar-start" @click="toggleStartMenu">
				<sc-icon name="sc-heart-3-line" :size="24" />
				<span>开始</span>
			</div>
			<div class="taskbar-apps">
				<div v-for="window in windows" :key="window.id" class="taskbar-app"
					:class="{ active: window.focused, minimized: window.minimized }" @click="toggleWindow(window.id)">
					<el-icon :size="20">
						<component :is="window.icon" />
					</el-icon>
					<span>{{ window.title }}</span>
				</div>
			</div>
			<div class="taskbar-tray">
				<span class="time">{{ currentTime }}</span>
			</div>
		</div>

		<!-- 开始菜单 -->
		<div v-if="showStartMenu" class="start-menu">
			<div class="start-menu-header">
				<sc-icon name="sc-heart-3-line" :size="32" />
				<span>用户名</span>
			</div>
			<div class="start-menu-apps">
				<div v-for="app in allApps" :key="app.id" class="start-menu-app" @click="openApp(app)">
					<sc-icon name="sc-heart-3-line" />
					<span>{{ app.name }}</span>
				</div>
			</div>
			<div class="start-menu-footer">
				<div class="start-menu-action" @click="openSettings">
					<sc-icon name="sc-heart-3-line" />
					<span>设置</span>
				</div>
				<div class="start-menu-action">
					<sc-icon name="sc-heart-3-line" />
					<span>关机</span>
				</div>
			</div>
		</div>

		<!-- 设置窗口 -->
		<el-dialog v-model="showSettings" title="桌面设置" width="500px" :modal-append-to-body="false">
			<el-tabs>
				<el-tab-pane label="背景设置">
					<div class="settings-section">
						<h4>背景类型</h4>
						<el-radio-group v-model="backgroundType">
							<el-radio label="color">纯色</el-radio>
							<el-radio label="image">图片</el-radio>
							<el-radio label="gradient">渐变</el-radio>
						</el-radio-group>
					</div>

					<div v-if="backgroundType === 'color'" class="settings-section">
						<h4>选择颜色</h4>
						<el-color-picker v-model="backgroundColor" />
					</div>

					<div v-if="backgroundType === 'image'" class="settings-section">
						<h4>背景图片</h4>
						<el-input v-model="backgroundImage" placeholder="输入图片URL或选择预设" />
						<div class="preset-images">
							<div v-for="(img, index) in presetImages" :key="index" class="preset-image"
								:style="{ backgroundImage: `url(${img})` }" @click="backgroundImage = img" />
						</div>
					</div>

					<div v-if="backgroundType === 'gradient'" class="settings-section">
						<h4>渐变颜色</h4>
						<el-color-picker v-model="gradientColor1" />
						<el-color-picker v-model="gradientColor2" />
						<el-select v-model="gradientDirection" style="margin-left: 10px;">
							<el-option label="从左到右" value="to right" />
							<el-option label="从上到下" value="to bottom" />
							<el-option label="对角线" value="to bottom right" />
						</el-select>
					</div>
				</el-tab-pane>
			</el-tabs>
			<template #footer>
				<el-button @click="showSettings = false">取消</el-button>
				<el-button type="primary" @click="applySettings">应用</el-button>
			</template>
		</el-dialog>
	</div>
</template>

<script>
import { ref, computed, onMounted, onUnmounted } from 'vue';
import Window from './components/Window.vue';

export default {
	name: 'Desktop',
	components: {
		Window,
	},
	setup() {
		const selectedApp = ref(null);
		const showStartMenu = ref(false);
		const showSettings = ref(false);
		const currentTime = ref('');
		const windows = ref([]);
		const windowIdCounter = ref(0);
		const clickTimer = ref(null);

		const backgroundType = ref('gradient');
		const backgroundColor = ref('#FFFFFF');
		const backgroundImage = ref('');
		const gradientColor1 = ref('#667eea');
		const gradientColor2 = ref('#764ba2');
		const gradientDirection = ref('to bottom right');

		const presetImages = ref([
			'https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=1920',
			'https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1920',
			'https://images.unsplash.com/photo-1507400492013-162706c8c05e?w=1920',
			'https://images.unsplash.com/photo-1542224566-6e85f2e6772f?w=1920',
		]);

		const desktopApps = ref([
			{ id: 1, name: '我的文档', icon: 'Folder', component: 'Documents' },
			{ id: 2, name: '浏览器', icon: 'Browser', component: 'Browser' },
			{ id: 3, name: '计算器', icon: 'Calculator', component: 'Calculator' },
			{ id: 4, name: '日历', icon: 'Calendar', component: 'Calendar' },
			{ id: 5, name: '音乐', icon: 'Music', component: 'Music' },
			{ id: 6, name: '视频', icon: 'VideoCamera', component: 'Video' },
			{ id: 7, name: '消息', icon: 'Message', component: 'Message' },
			{ id: 8, name: '终端', icon: 'Terminal', component: 'Terminal' },
		]);

		const allApps = ref([
			{ id: 1, name: '我的文档', icon: 'Folder', component: 'Documents' },
			{ id: 2, name: '浏览器', icon: 'Browser', component: 'Browser' },
			{ id: 3, name: '计算器', icon: 'Calculator', component: 'Calculator' },
			{ id: 4, name: '日历', icon: 'Calendar', component: 'Calendar' },
			{ id: 5, name: '音乐', icon: 'Music', component: 'Music' },
			{ id: 6, name: '视频', icon: 'VideoCamera', component: 'Video' },
			{ id: 7, name: '消息', icon: 'Message', component: 'Message' },
			{ id: 8, name: '终端', icon: 'Terminal', component: 'Terminal' },
			{ id: 9, name: '图片', icon: 'Picture', component: 'Pictures' },
			{ id: 10, name: '下载', icon: 'Download', component: 'Download' },
		]);

		const desktopStyle = computed(() => {
			if (backgroundType.value === 'color') {
				return { backgroundColor: backgroundColor.value };
			} else if (backgroundType.value === 'image') {
				return {
					backgroundImage: `url(${backgroundImage.value})`,
					backgroundSize: 'cover',
					backgroundPosition: 'center',
				};
			} else {
				return {
					background: `linear-gradient(${gradientDirection.value}, ${gradientColor1.value}, ${gradientColor2.value})`,
				};
			}
		});

		const updateTime = () => {
			const now = new Date();
			currentTime.value = now.toLocaleTimeString('zh-CN', {
				hour: '2-digit',
				minute: '2-digit',
			});
		};

		const selectApp = (appId) => {
			console.log(appId);
			if (clickTimer.value) {
				clearTimeout(clickTimer.value);
				clickTimer.value = null;
			}
			clickTimer.value = setTimeout(() => {
				selectedApp.value = appId;
				clickTimer.value = null;
			}, 200);
		};

		const clearSelection = () => {
			selectedApp.value = null;
		};

		const openApp = (app) => {
			if (clickTimer.value) {
				clearTimeout(clickTimer.value);
				clickTimer.value = null;
			}
			showStartMenu.value = false;
			const windowId = ++windowIdCounter.value;
			windows.value.push({
				id: windowId,
				title: app.name,
				icon: app.icon,
				component: app.component,
				minimized: false,
				maximized: false,
				focused: true,
				x: 100 + (windowId % 5) * 30,
				y: 100 + (windowId % 5) * 30,
				width: 800,
				height: 600,
			});
			focusWindow(windowId);
		};

		const closeWindow = (windowId) => {
			windows.value = windows.value.filter((w) => w.id !== windowId);
		};

		const minimizeWindow = (windowId) => {
			const window = windows.value.find((w) => w.id === windowId);
			if (window) {
				window.minimized = true;
				window.focused = false;
			}
		};

		const maximizeWindow = (windowId) => {
			const window = windows.value.find((w) => w.id === windowId);
			if (window) {
				window.maximized = !window.maximized;
			}
		};

		const focusWindow = (windowId) => {
			windows.value.forEach((w) => {
				w.focused = w.id === windowId;
				if (w.id === windowId) {
					w.minimized = false;
				}
			});
		};

		const toggleWindow = (windowId) => {
			const window = windows.value.find((w) => w.id === windowId);
			if (window) {
				if (window.focused && !window.minimized) {
					minimizeWindow(windowId);
				} else {
					focusWindow(windowId);
				}
			}
		};

		const toggleStartMenu = () => {
			showStartMenu.value = !showStartMenu.value;
		};

		const openSettings = () => {
			showStartMenu.value = false;
			showSettings.value = true;
		};

		const applySettings = () => {
			showSettings.value = false;
		};

		onMounted(() => {
			updateTime();
			setInterval(updateTime, 1000);

			document.addEventListener('click', (e) => {
				if (!e.target.closest('.start-menu') && !e.target.closest('.taskbar-start')) {
					showStartMenu.value = false;
				}
			});
		});

		onUnmounted(() => {
			clearInterval(updateTime);
			if (clickTimer.value) {
				clearTimeout(clickTimer.value);
			}
		});

		return {
			selectedApp,
			showStartMenu,
			showSettings,
			currentTime,
			windows,
			desktopApps,
			allApps,
			backgroundType,
			backgroundColor,
			backgroundImage,
			gradientColor1,
			gradientColor2,
			gradientDirection,
			presetImages,
			desktopStyle,
			selectApp,
			clearSelection,
			openApp,
			closeWindow,
			minimizeWindow,
			maximizeWindow,
			focusWindow,
			toggleWindow,
			toggleStartMenu,
			openSettings,
			applySettings,
		};
	},
};
</script>

<style scoped>
.desktop {
	position: fixed;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
	overflow: hidden;
	user-select: none;
}

.desktop-icons {
	position: absolute;
	top: 20px;
	left: 20px;
	display: grid;
	grid-template-columns: repeat(auto-fill, 100px);
	gap: 20px;
	max-width: 500px;
}

.desktop-icon {
	display: flex;
	flex-direction: column;
	align-items: center;
	padding: 10px;
	border-radius: 8px;
	border: 2px solid transparent;
	cursor: pointer;
	transition: all 0.3s;
}

.desktop-icon:hover {
	background-color: rgba(255, 255, 255, 0.1);
}

.desktop-icon.selected {
	background-color: rgba(64, 158, 255, 0.3);
	border: 2px solid rgba(64, 158, 255, 0.6);
	box-shadow: 0 0 10px rgba(64, 158, 255, 0.4);
}

.icon-image {
	display: flex;
	align-items: center;
	justify-content: center;
	width: 64px;
	height: 64px;
	color: white;
	filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.3));
}

.icon-text {
	margin-top: 5px;
	color: white;
	font-size: 12px;
	text-align: center;
	text-shadow: 0 1px 3px rgba(0, 0, 0, 0.5);
	word-break: break-word;
}

.windows-container {
	position: absolute;
	top: 0;
	left: 0;
	right: 0;
	bottom: 48px;
}

.taskbar {
	position: absolute;
	bottom: 0;
	left: 0;
	right: 0;
	height: 48px;
	background-color: rgba(0, 0, 0, 0.8);
	backdrop-filter: blur(10px);
	display: flex;
	align-items: center;
	padding: 0 10px;
	z-index: 1000;
}

.taskbar-start {
	display: flex;
	align-items: center;
	gap: 8px;
	padding: 8px 16px;
	color: white;
	cursor: pointer;
	border-radius: 4px;
	transition: background-color 0.3s;
}

.taskbar-start:hover {
	background-color: rgba(255, 255, 255, 0.1);
}

.taskbar-apps {
	flex: 1;
	display: flex;
	gap: 5px;
	margin-left: 10px;
	overflow-x: auto;
}

.taskbar-app {
	display: flex;
	align-items: center;
	gap: 8px;
	padding: 6px 12px;
	color: white;
	cursor: pointer;
	border-radius: 4px;
	background-color: rgba(255, 255, 255, 0.05);
	transition: background-color 0.3s;
	min-width: 150px;
}

.taskbar-app:hover {
	background-color: rgba(255, 255, 255, 0.1);
}

.taskbar-app.active {
	background-color: rgba(255, 255, 255, 0.2);
	border-bottom: 2px solid #409eff;
}

.taskbar-app.minimized {
	opacity: 0.6;
}

.taskbar-tray {
	display: flex;
	align-items: center;
	gap: 15px;
	padding: 0 10px;
	color: white;
}

.time {
	font-size: 12px;
}

.start-menu {
	position: absolute;
	bottom: 58px;
	left: 10px;
	width: 300px;
	background-color: rgba(0, 0, 0, 0.9);
	backdrop-filter: blur(20px);
	border-radius: 8px;
	overflow: hidden;
	z-index: 1001;
	box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
}

.start-menu-header {
	display: flex;
	align-items: center;
	gap: 15px;
	padding: 20px;
	color: white;
	border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.start-menu-apps {
	max-height: 400px;
	overflow-y: auto;
	padding: 10px;
}

.start-menu-app {
	display: flex;
	align-items: center;
	gap: 15px;
	padding: 12px;
	color: white;
	cursor: pointer;
	border-radius: 4px;
	transition: background-color 0.3s;
}

.start-menu-app:hover {
	background-color: rgba(255, 255, 255, 0.1);
}

.start-menu-footer {
	display: flex;
	border-top: 1px solid rgba(255, 255, 255, 0.1);
	padding: 10px;
}

.start-menu-action {
	flex: 1;
	display: flex;
	align-items: center;
	gap: 10px;
	padding: 12px;
	color: white;
	cursor: pointer;
	border-radius: 4px;
	transition: background-color 0.3s;
}

.start-menu-action:hover {
	background-color: rgba(255, 255, 255, 0.1);
}

.settings-section {
	margin-bottom: 20px;
}

.settings-section h4 {
	margin-bottom: 10px;
	color: #606266;
}

.preset-images {
	display: grid;
	grid-template-columns: repeat(4, 1fr);
	gap: 10px;
	margin-top: 10px;
}

.preset-image {
	width: 100%;
	height: 80px;
	background-size: cover;
	background-position: center;
	border-radius: 4px;
	cursor: pointer;
	border: 2px solid transparent;
	transition: border-color 0.3s;
}

.preset-image:hover {
	border-color: #409eff;
}
</style>
