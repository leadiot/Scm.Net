<template>
	<slot :open="open">
		<el-button type="primary" plain @click="open">{{ text }}</el-button>
	</slot>
	<el-dialog v-model="dialog" :title="title" :width="480" :close-on-click-modal="false" append-to-body
		destroy-on-close>
		<el-progress v-if="loading" :text-inside="true" :stroke-width="20" :percentage="percentage"
			style="margin-bottom: 15px;" />
		<div v-loading="loading" style="margin-top: -1rem;">
			<div class="scm-info" v-if="templateUrl">
				<div class="sc-download">
					<div>下载导入模版，根据导入模版填写并上传文件</div>
					<div>
						<el-link :href="templateUrl" target="_blank" type="primary" :underline="false">下载导入模板</el-link>
					</div>
				</div>
			</div>
			<div class="scm-info sc-import">
				<div class="head">导入说明：</div>
				<div class="item tips" v-if="tip">{{ tip }}</div>
				<div class="item tips" v-else>
					<p>1、导入文件支持 {{ accept }} 格式；</p>
					<p v-if="maxRows > 0">2、导入文件大小不超过 {{ maxSize }}MB，行数不超过 {{ maxRows }} 行；</p>
					<p v-else>2、导入文件大小不超过 {{ maxSize }}MB；</p>
					<p>3、导入文件中带星号标识（*）的列为必填列，请认真填写；</p>
					<p>4、导入文件中不能存在【合并单元格】，否则无法导入；</p>
					<p>5、导入时只要有一行数据不正确，则导入失败。</p>
				</div>
				<div class="item">
					<el-upload ref="uploader" drag v-model="file_list" :accept="accept" :maxSize="maxSize" :limit="1"
						:data="data" :show-file-list="false" :http-request="request" :before-upload="before"
						:on-progress="progress" :on-success="success" :on-error="error" :auto-upload="autoUpload">
						<slot name="uploader">
							<sc-icon name="sc-file-upload-line" />
							<div class="el-upload__text">
								将文件拖到此处或 <em>点击选择文件上传</em>
							</div>
						</slot>
					</el-upload>
				</div>
				<div class="item" v-if="errorCode > 0 && errorUrl">
					<el-link :href="errorUrl" target="_blank" type="primary" :underline="false">下载错误提示</el-link>
				</div>
				<div class="item" v-if="$slots.form">
					<el-form inline label-width="100px" label-position="left">
						<slot name="form" :formData="formData"></slot>
					</el-form>
				</div>
			</div>
		</div>
		<template #footer>
			<div class="sc-import_footer">
				<div class="left-panel">
					<el-text v-if="errorMsg" :type="result ? 'success' : 'danger'">{{ errorMsg }}</el-text>
				</div>
				<div class="right-panel">
					<el-button type="default" @click="close">取消</el-button>
					<el-button type="primary" @click="submit" v-if="!autoUpload">导入</el-button>
				</div>
			</div>
		</template>
	</el-dialog>
</template>

<script>
export default {
	emits: ['success'],
	props: {
		apiObj: { type: Object, default: () => { } },
		data: { type: Object, default: () => { } },
		accept: { type: String, default: ".xls, .xlsx" },
		maxSize: { type: Number, default: 10 },
		maxRows: { type: Number, default: 2000 },
		tip: { type: String, default: "" },
		templateUrl: { type: String, default: "" },
		text: { type: String, default: "导入" },
		title: { type: String, default: "导入" },
		errorUrl: { type: String, default: "" },
		autoUpload: { type: Boolean, default: false },
	},
	data() {
		return {
			dialog: false,
			loading: false,
			percentage: 0,
			result: false,
			errorCode: 0,
			errorMsg: "",
			file_list: [],
			formData: {}
		}
	},
	mounted() {
	},
	methods: {
		open() {
			this.dialog = true
			this.formData = {}
		},
		close() {
			this.dialog = false
		},
		submit() {
			// if (this.file_list.length == 0) {
			// 	this.$message.warning(`请选择文件！`);
			// 	return false;
			// }
			this.$refs.uploader.submit()
		},
		before(file) {
			const maxSize = file.size / 1024 / 1024 < this.maxSize;
			if (!maxSize) {
				this.$message.warning(`上传文件大小不能超过 ${this.maxSize}MB!`);
				return false;
			}
			this.loading = true
		},
		progress(e) {
			this.percentage = e.percent
		},
		success(res, file) {
			this.$refs.uploader.handleRemove(file);
			this.$refs.uploader.clearFiles();
			this.loading = false;
			this.percentage = 0;
			this.result = true;
			this.errorMsg = "导入成功！";
			this.$emit('success', res, this.close);
		},
		error(err) {
			this.loading = false;
			this.percentage = 0;
			this.result = false;
			this.errorCode = 1;
			this.errorMsg = "导入失败！";
			this.$notify.error({
				title: '上传文件未成功',
				message: err
			})
		},
		request(param) {
			Object.assign(param.data, this.formData)
			const data = new FormData();
			data.append('filename', param.filename);
			data.append('file', param.file);
			for (const key in param.data) {
				data.append(key, param.data[key]);
			}
			this.apiObj.post(data, {
				onUploadProgress: e => {
					const complete = parseInt(((e.loaded / e.total) * 100) | 0, 10);
					param.onProgress({ percent: complete });
				}
			}).then(res => {
				if (res.code == 200) {
					param.onSuccess(res);
				}
				else {
					param.onError(res);
				}
			}).catch(err => {
				param.onError(err);
			})
		}
	}
}
</script>
<style scoped>
.scm-info {
	width: 100%;
	padding: 1rem;
	color: var(--el-color-info);
	background-color: var(--el-color-info-light-9);
	border: 1px solid var(--el-border-color);
	border-radius: var(--el-border-radius-base);
	margin: 1rem 0 0 0;
	/* box-sizing: border-box;
	box-shadow: var(--el-box-shadow-light); */
}

.sc-download {
	display: flex;
	align-items: center;
	flex-direction: row;
	justify-content: space-between;
}

.sc-import {
	.head {
		font-size: 1.2rem;
		font-weight: bold;
	}

	.tips {
		font-size: 1rem;
		padding: 0 0.5rem;
	}

	.item {
		margin: 1rem 0 0 0;
	}
}

.sc-import_footer {
	display: flex;
	align-items: center;
	flex-direction: row;
	justify-content: space-between;
}
</style>