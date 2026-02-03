<template>
	<el-container>
		<el-aside width="260px" v-loading="showGrouploading">
			<el-container>
				<el-header>
					<sc-select v-model="param.kind" @change="kindChange" :data="kind_list">
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
							<el-image v-if="item.kind == 3" :src="serverApi + item.uri"
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
			kind_list: [this.$SCM.OPTION_ALL_INT],//文件类型列表
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
		this.serverApi = this.$API.scmadmfile.view.url + '?file=';
		this.init();
		this.initFiles();
		this.$SCM.list_dic(this.kind_list, 'file_kind', true);
	},
	methods: {
		async init() {
			const res = await this.$API.scmadmfile.list.get({ path: '/' });
			if (!res || res.code != 200) {
				return;
			}

			this.group = res.data;
		},
		async initFiles() {
			this.previewList = [];
			this.value = [];
			const res = await this.$API.scmadmfile.files.get(this.param);
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
			if (this.selectedFile.kind == 2) {
				this.viewText();
				return;
			}
			if (this.selectedFile.kind == 3) {
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
		kindChange() {
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
					var res = await this.$API.scmadmfile.delFolder.delete(data.uri);
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
					var res = await this.$API.scmadmfile.delFile.delete(this.selectedFile.uri);
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

<style scoped></style>
