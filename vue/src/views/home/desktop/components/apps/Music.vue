<template>
	<div class="music-app">
		<div class="music-list">
			<div
				v-for="(song, index) in songs"
				:key="song.id"
				class="song-item"
				:class="{ playing: currentSong === index }"
				@click="playSong(index)"
			>
				<div class="song-number">{{ index + 1 }}</div>
				<div class="song-info">
					<span class="song-title">{{ song.title }}</span>
					<span class="song-artist">{{ song.artist }}</span>
				</div>
				<span class="song-duration">{{ song.duration }}</span>
			</div>
		</div>
		<div class="player-controls">
			<div class="now-playing">
				<span>{{ songs[currentSong]?.title || '未播放' }}</span>
			</div>
			<div class="controls">
				<button class="control-btn">⏮</button>
				<button class="control-btn play-btn" @click="togglePlay">
					{{ isPlaying ? '⏸' : '▶' }}
				</button>
				<button class="control-btn">⏭</button>
			</div>
		</div>
	</div>
</template>

<script>
import { ref } from 'vue';

export default {
	name: 'Music',
	setup() {
		const currentSong = ref(0);
		const isPlaying = ref(false);
		const songs = ref([
			{ id: 1, title: '示例歌曲1', artist: '艺术家1', duration: '3:45' },
			{ id: 2, title: '示例歌曲2', artist: '艺术家2', duration: '4:20' },
			{ id: 3, title: '示例歌曲3', artist: '艺术家3', duration: '3:15' },
			{ id: 4, title: '示例歌曲4', artist: '艺术家4', duration: '5:10' },
		]);

		const playSong = (index) => {
			currentSong.value = index;
			isPlaying.value = true;
		};

		const togglePlay = () => {
			isPlaying.value = !isPlaying.value;
		};

		return { currentSong, isPlaying, songs, playSong, togglePlay };
	},
};
</script>

<style scoped>
.music-app {
	height: 100%;
	display: flex;
	flex-direction: column;
}

.music-list {
	flex: 1;
	overflow-y: auto;
}

.song-item {
	display: flex;
	align-items: center;
	gap: 15px;
	padding: 12px 20px;
	cursor: pointer;
	transition: background-color 0.3s;
}

.song-item:hover {
	background-color: #f5f5f5;
}

.song-item.playing {
	background-color: #e8f4ff;
}

.song-number {
	width: 30px;
	color: #999;
}

.song-info {
	flex: 1;
	display: flex;
	flex-direction: column;
	gap: 4px;
}

.song-title {
	font-size: 14px;
	color: #333;
}

.song-artist {
	font-size: 12px;
	color: #999;
}

.song-duration {
	color: #999;
	font-size: 14px;
}

.player-controls {
	border-top: 1px solid #e0e0e0;
	padding: 15px;
	background-color: #f5f5f5;
}

.now-playing {
	text-align: center;
	margin-bottom: 10px;
	font-size: 14px;
	color: #333;
}

.controls {
	display: flex;
	justify-content: center;
	gap: 20px;
}

.control-btn {
	width: 40px;
	height: 40px;
	border: none;
	border-radius: 50%;
	background-color: #fff;
	cursor: pointer;
	font-size: 16px;
	transition: all 0.3s;
}

.control-btn:hover {
	background-color: #e0e0e0;
}

.play-btn {
	width: 50px;
	height: 50px;
	background-color: #409eff;
	color: #fff;
	font-size: 20px;
}

.play-btn:hover {
	background-color: #66b1ff;
}
</style>
