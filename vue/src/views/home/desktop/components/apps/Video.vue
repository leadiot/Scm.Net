<template>
	<div class="video-app">
		<div class="video-list">
			<div v-for="video in videos" :key="video.id" class="video-item">
				<div class="video-thumbnail">🎬</div>
				<div class="video-info">
					<span class="video-title">{{ video.title }}</span>
					<span class="video-duration">{{ video.duration }}</span>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
import { ref } from 'vue';

export default {
	name: 'Video',
	setup() {
		const videos = ref([
			{ id: 1, title: '项目演示视频', duration: '15:30' },
			{ id: 2, title: '会议录像', duration: '45:20' },
			{ id: 3, title: '教学视频', duration: '20:15' },
			{ id: 4, title: '产品介绍', duration: '10:45' },
		]);

		return { videos };
	},
};
</script>

<style scoped>
.video-app {
	height: 100%;
	padding: 20px;
	overflow-y: auto;
}

.video-list {
	display: grid;
	grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
	gap: 20px;
}

.video-item {
	cursor: pointer;
	transition: transform 0.3s;
}

.video-item:hover {
	transform: translateY(-5px);
}

.video-thumbnail {
	width: 100%;
	height: 120px;
	background-color: #f0f0f0;
	border-radius: 8px;
	display: flex;
	align-items: center;
	justify-content: center;
	font-size: 48px;
}

.video-info {
	margin-top: 10px;
	display: flex;
	flex-direction: column;
	gap: 4px;
}

.video-title {
	font-size: 14px;
	color: #333;
}

.video-duration {
	font-size: 12px;
	color: #999;
}
</style>
