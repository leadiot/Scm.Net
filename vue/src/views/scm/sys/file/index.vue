<template>
	<el-container>
		<el-aside width="260px" v-loading="showGrouploading">
			<el-container>
				<el-header>
					<sc-select v-model="param.kind" @change="typeChange" :data="type_list">
					</sc-select>
				</el-header>
				<el-main class="nopadding">
					<el-tree ref="group" class="menu" node-key="id" default-expand-all :data="group" :props="props"
						:current-node-key="''" :highlight-current="true" :expand-on-click-node="false"
						@node-click="groupClick">
						<template #default="{ node, data }">
							<span class="scmui-item-node">
								<span class="label">{{ node.label }}</span>
								<span class="opt" v-if="data.uri">
									<el-icon @click.stop="remove_tree(node, data)"><el-icon-delete /></el-icon>
								</span>
							</span>
						</template>
					</el-tree>
				</el-main>
			</el-container>
		</el-aside>
		<el-container>
			<el-header>
				<div class="left-panel">
					<el-input v-model="param.path" :style="{ width: '480px' }">

						<template #prepend>
							<el-icon><el-icon-folder-opened /></el-icon>
						</template>

						<template #append>
							<el-button icon="el-icon-refresh" @click="refresh" />
						</template>
					</el-input>
				</div>
				<div class="right-panel">
					<el-button icon="el-icon-copy-document" plain type="success" :disabled="!selectedFile"
						v-copy="selectedUrl">复制地址</el-button>
					<el-button icon="el-icon-view" plain type="success" :disabled="!selectedFile" @click="file_view" />
					<el-button icon="el-icon-delete" plain type="danger" :disabled="!selectedFile" @click="file_del" />
					<el-button icon="el-icon-download" plain type="primary" :disabled="!selectedFile"
						@click="file_down" />
					<el-button icon="el-icon-upload" plain type="primary" @click="open_dialog">本地上传</el-button>
				</div>
			</el-header>
			<el-main class="nopadding" style="padding: 10px">
				<el-scrollbar ref="scrollbar">
					<el-empty v-if="files.length == 0" description="无数据" :image-size="80"></el-empty>
					<div v-for="(item, index) in files" :key="index" class="sc-file-select__item"
						:class="{ active: value.includes(index + item.name) }" @click="select(index + item.name, item)"
						:title="item.name">
						<div class="sc-file-select__item__file">
							<div class="sc-file-select__item__select">
								<el-icon><el-icon-check /></el-icon>
							</div>
							<div class="sc-file-select__item__box"></div>
							<el-image v-if="item.kind == 41" :src="serverApi + item.uri"
								:preview-src-list="[serverApi + item.uri]" fit="contain" lazy></el-image>
							<div v-else class="item-file item-file-doc">
								<el-icon class="el-icon--upload">
									<el-icon-document />
								</el-icon>
							</div>
						</div>
						<p :title="item.name">{{ item.name }}</p>
					</div>
				</el-scrollbar>
			</el-main>
			<el-image-viewer @close="closeImgViewer" :url-list="previewList" v-if="showImageViewer" />
			<sc-text-viewer ref="textViewer" />
			<upload ref="upload" @complete="complete" />
		</el-container>
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_sys_file',
	components: {
		upload: defineAsyncComponent(() => import("./upload")),
		scTextViewer: defineAsyncComponent(() => import("@/components/scTextViewer")),
	},
	data() {
		return {
			serverApi: undefined,//服务器地址
			showGrouploading: false,//目录树加载标识
			group: [],//目录列表
			props: { label: "name" },
			param: {
				kind: this.$SCM.ID_ALL_INT,
				path: '/',
				key: "",
			},
			type_list: [this.$SCM.OPTION_ALL_INT],//文件类型列表
			files: [],//文件列表
			previewList: [],//预览列表
			value: [],
			selectedFile: undefined,//当前选中的文件
			selectedUrl: undefined,//文件绝对路径
			showImageViewer: false,
			showTextViewer: false,
		};
	},
	mounted() {
		this.serverApi = this.$API.scmsysfile.view.url + '?file=';
		this.init();
		this.initFiles();
		this.$SCM.list_dic(this.type_list, 'file_type', true);
	},
	methods: {
		async init() {
			const res = await this.$API.scmsysfile.list.get({ path: '/' });
			if (!res || res.code != 200) {
				return;
			}

			this.group = res.data;
		},
		async initFiles() {
			this.previewList = [];
			this.value = [];
			const res = await this.$API.scmsysfile.files.get(this.param);
			if (!res || res.code != 200) {
				return;
			}

			this.files = res.data;
		},
		refresh() {
			this.value = [];
			this.initFiles();
		},
		complete() {
			this.init();
			this.initFiles();
		},
		open_dialog() {
			this.$refs.upload.open(this.param.path);
		},
		file_view() {
			if (!this.selectedFile) {
				return;
			}
			if (this.selectedFile.kind == 30) {
				this.viewText();
				return;
			}
			if (this.selectedFile.kind == 41) {
				this.viewImage();
				return;
			}
		},
		viewImage() {
			this.showImageViewer = true;
		},
		closeImgViewer() {
			this.showImageViewer = false;
		},
		viewText() {
			this.$refs.textViewer.showUri(this.selectedUrl);
		},
		select(item, row) {
			this.previewList = [];
			this.selectedUrl = this.serverApi + row.uri;
			this.previewList = [this.selectedUrl];
			if (this.value.includes(item)) {
				this.value = "";
				this.selectedFile = undefined;
			} else {
				this.value = item;
				this.selectedFile = row;
			}
		},
		typeChange() {
			this.initFiles();
		},
		groupClick(data) {
			this.param.path = data.uri;
			this.selectedFile = undefined;
			this.initFiles();
		},
		remove_tree(node, data) {
			this.$confirm(
				`确定删除 【${data.name}】 文件夹吗，如果删除将会删除当前文件夹及子目录和文件？`,
				"提示",
				{
					type: "warning",
					confirmButtonText: "确定",
					cancelButtonText: "取消",
				}
			)
				.then(async () => {
					const loading = this.$loading();
					var res = await this.$API.scmsysfile.delFolder.delete(data.uri);
					loading.close();
					if (res.code == 200) {
						this.param.path = "/upload/";
						this.init();
						this.initFiles();
						this.$message.success("删除成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		file_del() {
			this.$confirm(
				`确定删除选中的 【${this.selectedFile.name}】 吗？`,
				"提示",
				{
					type: "warning",
					confirmButtonText: "确定",
					cancelButtonText: "取消",
				}
			)
				.then(async () => {
					const loading = this.$loading();
					var res = await this.$API.scmsysfile.delFile.delete(this.selectedFile.uri);
					if (res.code == 200) {
						this.initFiles();
						loading.close();
						this.$message.success("删除成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		file_down() {
			const fileData = this.serverApi + this.selectedFile.uri;
			const url = window.URL.createObjectURL(
				new Blob([fileData], {
					type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8",
				})
			);
			const link = document.createElement("a");
			link.href = url;
			link.setAttribute("download", this.selectedFile.name);
			document.body.appendChild(link);
			link.click();
			document.body.removeChild(link);
			window.URL.revokeObjectURL(url);
		}
	},
};
</script>

<style scoped>
.sc-file-select {
	display: flex;
}

.sc-file-select__files {
	flex: 1;
}

.sc-file-select__item {
	padding: 10px;
}

.sc-file-select__list {
	height: 400px;
}

.sc-file-select__item {
	display: inline-block;
	float: left;
	margin: 0 15px 25px 0;
	width: 130px;
	cursor: pointer;
	background: #f9f9f9;
}

.sc-file-select__item__file {
	width: 110px;
	height: 110px;
	position: relative;
}

.sc-file-select__item__file .el-image {
	width: 110px;
	height: 110px;
}

.sc-file-select__item__box {
	position: absolute;
	top: 0;
	right: 0;
	bottom: 0;
	left: 0;
	border: 2px solid var(--el-color-success);
	z-index: 1;
	display: none;
}

.sc-file-select__item__box::before {
	content: "";
	position: absolute;
	top: 0;
	right: 0;
	bottom: 0;
	left: 0;
	background: var(--el-color-success);
	opacity: 0.2;
	display: none;
}

.sc-file-select__item:hover .sc-file-select__item__box {
	display: block;
}

.sc-file-select__item.active .sc-file-select__item__box {
	display: block;
}

.sc-file-select__item.active .sc-file-select__item__box::before {
	display: block;
}

.sc-file-select__item p {
	margin-top: 10px;
	white-space: nowrap;
	text-overflow: ellipsis;
	overflow: hidden;
	-webkit-text-overflow: ellipsis;
	text-align: center;
}

.sc-file-select__item__checkbox {
	position: absolute;
	width: 20px;
	height: 20px;
	top: 7px;
	right: 7px;
	z-index: 2;
	background: rgba(0, 0, 0, 0.2);
	border: 1px solid #fff;
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
}

.sc-file-select__item__checkbox i {
	font-size: 14px;
	color: #fff;
	font-weight: bold;
	display: none;
}

.sc-file-select__item__select {
	position: absolute;
	width: 20px;
	height: 20px;
	top: 0px;
	right: 0px;
	z-index: 2;
	background: var(--el-color-success);
	display: none;
	flex-direction: column;
	align-items: center;
	justify-content: center;
}

.sc-file-select__item__select i {
	font-size: 14px;
	color: #fff;
	font-weight: bold;
}

.sc-file-select__item.active .sc-file-select__item__checkbox {
	background: var(--el-color-success);
}

.sc-file-select__item.active .sc-file-select__item__checkbox i {
	display: block;
}

.sc-file-select__item.active .sc-file-select__item__select {
	display: flex;
}

.sc-file-select__item__file .item-file {
	width: 110px;
	height: 110px;
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
}

.sc-file-select__item__file .item-file i {
	font-size: 40px;
}

.sc-file-select__item__file .item-file.item-file-doc {
	color: #409eff;
}

.sc-file-select__item__upload {
	position: absolute;
	top: 0;
	right: 0;
	bottom: 0;
	left: 0;
	z-index: 1;
	background: rgba(255, 255, 255, 0.7);
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
}

.sc-file-select__side {
	width: 200px;
	margin-right: 15px;
	border-right: 1px solid rgba(128, 128, 128, 0.2);
	display: flex;
	flex-flow: column;
}

.sc-file-select__side-menu {
	flex: 1;
}

.sc-file-select__side-msg {
	height: 32px;
	line-height: 32px;
}

.sc-file-select__top {
	margin-bottom: 15px;
	display: flex;
	justify-content: space-between;
}

.sc-file-select__upload {
	display: inline-block;
}

.sc-file-select__top .tips {
	font-size: 12px;
	margin-left: 10px;
	color: #999;
}

.sc-file-select__top .tips i {
	font-size: 14px;
	margin-right: 5px;
	position: relative;
	bottom: -0.125em;
}

.sc-file-select__pagination {
	margin: 15px 0;
}

.sc-file-select__do {
	text-align: right;
}

[data-theme="dark"] .sc-file-select__item {
	background: #383838;
}
</style>
