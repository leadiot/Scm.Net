<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="600px" @close="close">
		<el-form ref="formRef" label-width="auto" label-position="top" :model="formData" :rules="rules">
			<el-form-item label="上传目录" prop="path">
				<el-input v-model="formData.path" show-word-limit clearable :style="{ width: '100%' }"></el-input>
				<div class="el-form-item-msg">
					可自定义添加目录，系统会自动创建，例如/upload/{video}
				</div>
			</el-form-item>
			<el-upload ref="upload" class="upload-demo" drag :autoUpload="false" :httpRequest="httpRequest">
				<el-icon class="el-icon--upload"><el-icon-upload-filled /></el-icon>
				<div class="el-upload__text">
					你可以将文件拖拽到特定区域以进行上传 <em>点击上传</em>
				</div>
			</el-upload>
			<div class="el-upload__tip">大小不超过100MB</div>
		</el-form>

		<template #footer>
			<el-button @click="close">取 消</el-button>
			<el-button :loading="isSaveing" type="primary" @click="save">
				确 定
			</el-button>
		</template>
	</sc-dialog>
</template>
<script>
export default {
	data() {
		return {
			mode: "add",
			titleMap: {
				add: "上传文件",
			},
			isSaveing: false,
			visible: false,
			rules: {},
			formData: {
				path: '',
				type: 1
			},
		};
	},
	mounted() { },
	methods: {
		async open(row) {
			this.visible = true;
			this.formData.path = row;
		},
		async save() {
			this.$refs.formRef.validate(valid => {
				if (valid) {
					this.$refs.upload.submit();
				}
			})
		},
		async httpRequest(param) {
			const data = new FormData();
			data.append('file', param.file);
			data.append('filesize', param.file.size);
			data.append('filetime', param.file.lastModified);
			data.append('path', this.formData.path);
			data.append('type', 1);
			for (const key in param.data) {
				data.append(key, param.data[key]);
			}
			let config = {
				headers: {
					'Content-Type': 'multipart/form-data'
				}
			};
			await this.$API.scmsysfile.upload.post(data, config);
		},
		close() {
			this.visible = false;
		},
	},
};
</script>
