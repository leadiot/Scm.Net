<template>
	<div class="documents-app">
		<div class="app-header">
			<el-input v-model="searchQuery" placeholder="搜索文档..." prefix-icon="Search" />
		</div>
		<div class="app-content">
			<div class="folder-list">
				<div v-for="folder in folders" :key="folder.id" class="folder-item">
					<el-icon :size="24"><Folder /></el-icon>
					<span>{{ folder.name }}</span>
				</div>
			</div>
			<div class="file-list">
				<div v-for="file in files" :key="file.id" class="file-item">
					<el-icon :size="24"><Document /></el-icon>
					<div class="file-info">
						<span class="file-name">{{ file.name }}</span>
						<span class="file-size">{{ file.size }}</span>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
import { ref } from 'vue';
import { Folder, Document } from '@element-plus/icons-vue';

export default {
	name: 'Documents',
	components: { Folder, Document },
	setup() {
		const searchQuery = ref('');
		const folders = ref([
			{ id: 1, name: '我的文档' },
			{ id: 2, name: '桌面' },
			{ id: 3, name: '下载' },
			{ id: 4, name: '图片' },
			{ id: 5, name: '音乐' },
			{ id: 6, name: '视频' },
		]);
		const files = ref([
			{ id: 1, name: '项目文档.docx', size: '2.3 MB' },
			{ id: 2, name: '会议记录.pdf', size: '1.1 MB' },
			{ id: 3, name: '数据表格.xlsx', size: '856 KB' },
			{ id: 4, name: '演示文稿.pptx', size: '5.2 MB' },
		]);

		return { searchQuery, folders, files };
	},
};
</script>

<style scoped>
.documents-app {
	height: 100%;
	display: flex;
	flex-direction: column;
}

.app-header {
	padding: 15px;
	border-bottom: 1px solid #e0e0e0;
}

.app-content {
	flex: 1;
	display: flex;
	overflow: hidden;
}

.folder-list {
	width: 200px;
	border-right: 1px solid #e0e0e0;
	padding: 10px;
	overflow-y: auto;
}

.folder-item {
	display: flex;
	align-items: center;
	gap: 10px;
	padding: 10px;
	cursor: pointer;
	border-radius: 4px;
	transition: background-color 0.3s;
}

.folder-item:hover {
	background-color: #f5f5f5;
}

.file-list {
	flex: 1;
	padding: 10px;
	overflow-y: auto;
}

.file-item {
	display: flex;
	align-items: center;
	gap: 10px;
	padding: 10px;
	cursor: pointer;
	border-radius: 4px;
	transition: background-color 0.3s;
}

.file-item:hover {
	background-color: #f5f5f5;
}

.file-info {
	display: flex;
	flex-direction: column;
	gap: 4px;
}

.file-name {
	font-size: 14px;
	color: #333;
}

.file-size {
	font-size: 12px;
	color: #999;
}
</style>
