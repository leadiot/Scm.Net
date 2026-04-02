<template>
	<div class="picture-app">
		<div class="picture-grid">
			<div
				v-for="picture in pictures"
				:key="picture.id"
				class="picture-item"
				@click="viewPicture(picture)"
			>
				<div class="picture-thumbnail">🖼️</div>
				<span class="picture-name">{{ picture.name }}</span>
			</div>
		</div>
	</div>
</template>

<script>
import { ref } from 'vue';

export default {
	name: 'Picture',
	setup() {
		const pictures = ref([
			{ id: 1, name: '风景照片.jpg' },
			{ id: 2, name: '人物照片.png' },
			{ id: 3, name: '产品图片.jpg' },
			{ id: 4, name: '设计稿.png' },
			{ id: 5, name: '截图.png' },
			{ id: 6, name: '壁纸.jpg' },
		]);

		const viewPicture = (picture) => {
			alert(`查看图片: ${picture.name}`);
		};

		return { pictures, viewPicture };
	},
};
</script>

<style scoped>
.picture-app {
	height: 100%;
	padding: 20px;
	overflow-y: auto;
}

.picture-grid {
	display: grid;
	grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
	gap: 20px;
}

.picture-item {
	cursor: pointer;
	transition: transform 0.3s;
}

.picture-item:hover {
	transform: scale(1.05);
}

.picture-thumbnail {
	width: 100%;
	height: 120px;
	background-color: #f0f0f0;
	border-radius: 8px;
	display: flex;
	align-items: center;
	justify-content: center;
	font-size: 48px;
}

.picture-name {
	display: block;
	margin-top: 8px;
	font-size: 12px;
	color: #666;
	text-align: center;
	overflow: hidden;
	text-overflow: ellipsis;
	white-space: nowrap;
}
</style>
