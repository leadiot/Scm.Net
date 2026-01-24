<template>
	<el-drawer :title="title" v-model="visible" :size="400" append-to-body destroy-on-close>
		<el-form ref="formRef" label-width="100px" :model="formData">
			<el-form-item label="终端ID" prop="terminal_id">
				<el-input v-model="formData.terminal_id" placeholder="终端ID" :readonly="true" />
			</el-form-item>
			<el-form-item label="驱动ID" prop="drive_id">
				<el-input v-model="formData.drive_id" placeholder="驱动ID" :readonly="true" />
			</el-form-item>
			<el-form-item label="文件类型" prop="type">
				<el-input v-model="formData.type" placeholder="文件类型" :readonly="true" />
			</el-form-item>
			<el-form-item label="操作类型" prop="opt">
				<el-input v-model="formData.opt" placeholder="操作类型" :readonly="true" />
			</el-form-item>
			<el-form-item label="同步方向" prop="dir">
				<el-input v-model="formData.dir" placeholder="同步方向" :readonly="true" />
			</el-form-item>
			<el-form-item label="文件名称" prop="name">
				<el-input v-model="formData.name" placeholder="文件名称" :readonly="true" />
			</el-form-item>
			<el-form-item label="文件路径" prop="path">
				<el-input v-model="formData.path" placeholder="文件路径" :readonly="true" />
			</el-form-item>
			<el-form-item label="文件摘要" prop="hash">
				<el-input v-model="formData.hash" placeholder="文件摘要" :readonly="true" />
			</el-form-item>
			<el-form-item label="文件大小" prop="size">
				<el-input v-model="formData.size" placeholder="文件大小" :readonly="true" />
			</el-form-item>
			<el-form-item label="来源文件" prop="src">
				<el-input v-model="formData.src" placeholder="来源文件" :readonly="true" />
			</el-form-item>
		</el-form>

		<template #footer>
			<el-button type="primary" @click="close">关闭</el-button>
		</template>
	</el-drawer>
</template>
<script>
export default {
	data() {
		return {
			title: "详情",
			visible: false,
			isSaveing: false,
			formData: {},
		};
	},
	mounted() {
	},
	methods: {
		open(row) {
			this.formData = row;
			this.visible = true;
		},
		close() {
			this.visible = false;
		},
	},
};
</script>

<style scoped>
.el-select {
	width: 100%;
}
</style>