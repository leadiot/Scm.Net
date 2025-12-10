<template>
	<el-container>
		<el-header>
			123
		</el-header>
		<el-main>
			<div>
				<div class="phone-border">
					<div class="phone-screen">
						<img :src="img" />
						<div class="datetime" :style="{ color: color }">
							<div class="time">
								{{ time }}
							</div>
							<div class="date">
								{{ date }}
							</div>
						</div>
						<div class="tool">
							<div class="camera"></div>
							<div class="finger"></div>
							<el-button class="screen" @click="fullScreen"></el-button>
						</div>
					</div>
					<div ref="fullScreen" class="full-screen">
						<div>
							<img :src="img" />
						</div>
						<div class="tool">
							<el-button></el-button>
							<el-button></el-button>
						</div>
					</div>
					这里判断点击的元素是否是本身，是本身，则返回
				</div>
			</div>
			<br />
			<sc-comment code="111"></sc-comment>
		</el-main>
	</el-container>
</template>

<script>
export default {
	data() {
		return {
			img: '/img/bg1.jpg',
			time: '',//当前时间
			date: '',//当前日期
			fontName: '',
			fontSize: 72,
			color: '#FF9999',//字体颜色
		};
	},
	mounted() {
		var now = new Date();
		this.date = this.$TOOL.dateFormat(now, 'yyyy-MM-dd');
		this.time = this.$TOOL.dateFormat(now, 'HH:mm');
	},
	methods: {
		fullScreen() {
			this.$TOOL.screen(this.$refs.fullScreen);
		}
	},
};
</script>
<style scoped>
.phone-border {
	border-radius: 20px;
	width: 540px;
	height: 960px;
	margin: 0 auto;
	border: 3px solid #ccc;
	overflow: hidden;
	transition: 0.3s;
	background-color: #ffffff;
	box-shadow: 0px 0px 20px 1px rgba(0, 0, 0, 0.2);
}

.phone-screen {
	position: relative;
	height: 100%;
}

.phone-screen img {
	position: absolute;
	width: 100%;
	height: 100%;
}

.phone-screen .datetime {
	position: absolute;
	top: 120px;
	width: 100%;
}

.phone-screen .datetime .time {
	font-size: 72px;
	text-align: center;
}

.phone-screen .datetime .date {
	font-size: 36px;
	text-align: center;
}

.phone-screen .tool {
	position: absolute;
	bottom: 0px;
	text-align: center;
	align-items: center;
	height: 80px;
	width: 100%;
	background-color: rgba(255, 255, 255, 0.3);
}

.phone-screen .tool .finger {
	position: absolute;
	width: 80px;
	height: 80px;
	bottom: 40px;
	left: 230px;
	border-radius: 40px;
	background-color: aquamarine;
}

.phone-screen .tool .camera {
	position: absolute;
	width: 48px;
	height: 48px;
	bottom: 10px;
	left: 10px;
	border-radius: 24px;
	background-color: rebeccapurple;
}

.phone-screen .tool .screen {
	position: absolute;
	width: 48px;
	height: 48px;
	bottom: 10px;
	right: 10px;
	border-radius: 24px;
	background-color: bisque;
}

.full-screen {
	text-align: center;
}
</style>