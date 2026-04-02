<template>
	<div class="download-app">
		<div class="download-list">
			<div v-for="download in downloads" :key="download.id" class="download-item">
				<div class="download-icon">📥</div>
				<div class="download-info">
					<span class="download-name">{{ download.name }}</span>
					<div class="download-progress">
						<el-progress :percentage="download.progress" :status="download.status" />
					</div>
					<span class="download-size">{{ download.size }}</span>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
import { ref } from 'vue';

export default {
	name: 'Download',
	setup() {
		const downloads = ref([
			{ id: 1, name: '项目文件.zip', size: '125 MB', progress: 100, status: 'success' },
			{ id: 2, name: '安装包.exe', size: '256 MB', progress: 75, status: '' },
			{ id: 3, name: '文档.pdf', size: '5.2 MB', progress: 45, status: '' },
			{ id: 4, name: '视频.mp4', size: '1.2 GB', progress: 20, status: '' },
		]);

		return { downloads };
	},
};
</script>

<style scoped>
.download-app {
	height: 100%;
	padding: 20px;
	overflow-y: auto;
}

.download-list {
	display: flex;
	flex-direction: column;
	gap: 15px;
}

.download-item {
	display: flex;
	gap: 15px;
	padding: 15px;
	background-color: #f9f9f9;
	border-radius: 8px;
	border: 1px solid #e0e0e0;
}

.download-icon {
	font-size: 32px;
}

.download-info {
	flex: 1;
	display: flex;
	flex-direction: column;
	gap: 8px;
}

.download-name {
	font-size: 14px;
	font-weight: 500;
	color: #333;
}

.download-progress {
	width: 100%;
}

.download-size {
	font-size: 12px;
	color: #999;
}
</style>
