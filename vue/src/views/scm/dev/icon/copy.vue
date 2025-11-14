<template>
	<sc-dialog v-model="visible" :title="title" width="600px" @close="close">
		<div class="body">
			<div class="icon_panel">
				<div class="icon">
					<sc-icon :name="name" :style="{ color: color }" :size="size" />
				</div>
				<el-form class="args">
					<el-form-item label="图标大小：">
						<el-slider v-model="size" :min="16" :max="128" :step="4" style="width: 120px;"></el-slider>
					</el-form-item>
					<el-form-item label="图标颜色：">
						<el-color-picker v-model="color" :predefine="predefineColors" />
					</el-form-item>
				</el-form>
			</div>
			<el-input v-model="copyText" readonly>
				<template #append>
					<el-button v-copy="copyText" title="复制代码">
						<sc-icon name="content_copy" />
					</el-button>
				</template>
			</el-input>
		</div>
	</sc-dialog>
</template>
<script>
export default {
	emits: ['complete'],
	data() {
		return {
			mode: "add",
			title: "复制",
			isSaveing: false,
			visible: false,
			icon: {},
			color: '',
			size: 24,
			predefineColors: this.$CONFIG.PREDEFINE_COLORS,
		};
	},
	mounted() { },
	computed: {
		copyText() {
			return '<sc-icon name="' + this.name + '" color="' + this.color + '" size="' + this.size + '"/>';
		}
	},
	methods: {
		open(name, color, size) {
			this.name = name;
			this.color = color;
			this.size = size;
			this.visible = true;
		},
		close() {
			this.visible = false;
		}
	},
};
</script>
<style scoped>
.body {
	text-align: center;
}

.icon_panel {
	display: flex;
	flex-direction: row;
	justify-content: center;
}

.icon {
	border: 1px solid #ccc;
	margin-bottom: 20px;
	display: flex;
	text-align: center;
	align-items: center;
	width: 148px;
	height: 148px;
	justify-content: center;
}

.args {
	margin-left: 20px;
}
</style>