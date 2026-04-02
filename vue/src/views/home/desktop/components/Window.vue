<template>
	<div
		v-if="!window.minimized"
		class="window"
		:class="{ maximized: window.maximized, focused: window.focused }"
		:style="windowStyle"
		@mousedown="handleFocus"
	>
		<!-- 窗口标题栏 -->
		<div class="window-header" @mousedown="startDrag">
			<div class="window-title">
				<el-icon :size="16">
					<component :is="window.icon" />
				</el-icon>
				<span>{{ window.title }}</span>
			</div>
			<div class="window-controls">
				<button class="control-btn minimize" @click.stop="$emit('minimize', window.id)">
					<el-icon :size="12"><Minus /></el-icon>
				</button>
				<button class="control-btn maximize" @click.stop="$emit('maximize', window.id)">
					<el-icon :size="12">
						<component :is="window.maximized ? 'CopyDocument' : 'FullScreen'" />
					</el-icon>
				</button>
				<button class="control-btn close" @click.stop="$emit('close', window.id)">
					<el-icon :size="12"><Close /></el-icon>
				</button>
			</div>
		</div>

		<!-- 窗口内容区域 -->
		<div class="window-content">
			<component :is="getComponent(window.component)" />
		</div>

		<!-- 窗口缩放手柄 -->
		<div v-if="!window.maximized" class="resize-handle" @mousedown.stop="startResize"></div>
	</div>
</template>

<script>
import { computed, ref } from 'vue';
import {
	Minus,
	Close,
	FullScreen,
	CopyDocument,
	Folder,
	// Browser,
	// Calculator,
	// Calendar,
	// Message,
	// VideoCamera,
	// Music,
	// Terminal,
	// Pictures,
	// Download,
} from '@element-plus/icons-vue';
import Documents from './apps/Documents.vue';
// import BrowserApp from './apps/Browser.vue';
// import CalculatorApp from './apps/Calculator.vue';
// import CalendarApp from './apps/Calendar.vue';
// import MessageApp from './apps/Message.vue';
// import VideoApp from './apps/Video.vue';
// import MusicApp from './apps/Music.vue';
// import TerminalApp from './apps/Terminal.vue';
// import PictureApp from './apps/Picture.vue';
// import DownloadApp from './apps/Download.vue';

export default {
	name: 'Window',
	components: {
		Minus,
		Close,
		FullScreen,
		CopyDocument,
		Folder,
		// Browser,
		// Calculator,
		// Calendar,
		// Message,
		// VideoCamera,
		// Music,
		// Terminal,
		// Pictures,
		// Download,
	},
	props: {
		window: {
			type: Object,
			required: true,
		},
	},
	emits: ['close', 'minimize', 'maximize', 'focus'],
	setup(props, { emit }) {
		const isDragging = ref(false);
		const isResizing = ref(false);
		const dragStartX = ref(0);
		const dragStartY = ref(0);
		const resizeStartX = ref(0);
		const resizeStartY = ref(0);
		const resizeStartWidth = ref(0);
		const resizeStartHeight = ref(0);

		const windowStyle = computed(() => {
			if (props.window.maximized) {
				return {
					top: '0',
					left: '0',
					width: '100%',
					height: 'calc(100% - 48px)',
				};
			}
			return {
				top: `${props.window.y}px`,
				left: `${props.window.x}px`,
				width: `${props.window.width}px`,
				height: `${props.window.height}px`,
			};
		});

		const getComponent = (componentName) => {
			const components = {
				Documents,
				// Browser: BrowserApp,
				// Calculator: CalculatorApp,
				// Calendar: CalendarApp,
				// Message: MessageApp,
				// Video: VideoApp,
				// Music: MusicApp,
				// Terminal: TerminalApp,
				// Pictures: PictureApp,
				// Download: DownloadApp,
			};
			return components[componentName] || Documents;
		};

		const handleFocus = () => {
			emit('focus', props.window.id);
		};

		const startDrag = (e) => {
			if (props.window.maximized) return;
			if (e.target.closest('.window-controls')) return;

			isDragging.value = true;
			dragStartX.value = e.clientX - props.window.x;
			dragStartY.value = e.clientY - props.window.y;

			document.addEventListener('mousemove', handleDrag);
			document.addEventListener('mouseup', stopDrag);
		};

		const handleDrag = (e) => {
			if (!isDragging.value) return;

			props.window.x = e.clientX - dragStartX.value;
			props.window.y = e.clientY - dragStartY.value;

			// 限制窗口在屏幕范围内
			const maxX = window.innerWidth - props.window.width;
			const maxY = window.innerHeight - props.window.height - 48;

			props.window.x = Math.max(0, Math.min(props.window.x, maxX));
			props.window.y = Math.max(0, Math.min(props.window.y, maxY));
		};

		const stopDrag = () => {
			isDragging.value = false;
			document.removeEventListener('mousemove', handleDrag);
			document.removeEventListener('mouseup', stopDrag);
		};

		const startResize = (e) => {
			if (props.window.maximized) return;

			isResizing.value = true;
			resizeStartX.value = e.clientX;
			resizeStartY.value = e.clientY;
			resizeStartWidth.value = props.window.width;
			resizeStartHeight.value = props.window.height;

			document.addEventListener('mousemove', handleResize);
			document.addEventListener('mouseup', stopResize);
		};

		const handleResize = (e) => {
			if (!isResizing.value) return;

			const deltaX = e.clientX - resizeStartX.value;
			const deltaY = e.clientY - resizeStartY.value;

			props.window.width = Math.max(400, resizeStartWidth.value + deltaX);
			props.window.height = Math.max(300, resizeStartHeight.value + deltaY);
		};

		const stopResize = () => {
			isResizing.value = false;
			document.removeEventListener('mousemove', handleResize);
			document.removeEventListener('mouseup', stopResize);
		};

		return {
			windowStyle,
			getComponent,
			handleFocus,
			startDrag,
			startResize,
		};
	},
};
</script>

<style scoped>
.window {
	position: absolute;
	background-color: #fff;
	border-radius: 8px;
	box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
	overflow: hidden;
	display: flex;
	flex-direction: column;
	transition: box-shadow 0.3s;
}

.window.focused {
	box-shadow: 0 8px 30px rgba(0, 0, 0, 0.4);
	z-index: 100;
}

.window:not(.focused) {
	z-index: 1;
}

.window.maximized {
	border-radius: 0;
}

.window-header {
	display: flex;
	justify-content: space-between;
	align-items: center;
	padding: 8px 12px;
	background-color: #f5f5f5;
	border-bottom: 1px solid #e0e0e0;
	cursor: move;
	user-select: none;
}

.window.focused .window-header {
	background-color: #e8f4ff;
}

.window-title {
	display: flex;
	align-items: center;
	gap: 8px;
	font-size: 14px;
	font-weight: 500;
	color: #333;
}

.window-controls {
	display: flex;
	gap: 8px;
}

.control-btn {
	width: 24px;
	height: 24px;
	border: none;
	border-radius: 4px;
	cursor: pointer;
	display: flex;
	align-items: center;
	justify-content: center;
	transition: background-color 0.3s;
}

.control-btn.minimize {
	background-color: #ffbd2e;
	color: #fff;
}

.control-btn.minimize:hover {
	background-color: #ffa500;
}

.control-btn.maximize {
	background-color: #28c940;
	color: #fff;
}

.control-btn.maximize:hover {
	background-color: #00a800;
}

.control-btn.close {
	background-color: #ff5f57;
	color: #fff;
}

.control-btn.close:hover {
	background-color: #ff0000;
}

.window-content {
	flex: 1;
	overflow: auto;
	background-color: #fff;
}

.resize-handle {
	position: absolute;
	bottom: 0;
	right: 0;
	width: 16px;
	height: 16px;
	cursor: nwse-resize;
	background: linear-gradient(135deg, transparent 50%, #ccc 50%);
}
</style>
