<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button icon="el-icon-edit" type="primary" @click="addNew">新建通知</el-button>
				<el-button icon="el-icon-files" type="primary" :disabled="list.length == 0 || !isReader"
					@click="setRead">全部标记已读</el-button>
				<el-button plain type="danger" :disabled="selection.length == 0" @click="batchDel">
					<sc-icon name="sc-delete-line" />删除
				</el-button>
				<el-button type="info" :disabled="selection.length == 0" @click="archives">
					<sc-icon name="sc-archive-line" />存档
				</el-button>
			</div>
			<div class="right-panel">
				<div class="right-panel-search">
					<el-input v-model="param.key" clearable placeholder="关键字" />
					<el-button type="primary" @click="search"><sc-icon name="sc-search" /></el-button>
				</div>
			</div>
		</el-header>
		<el-container class="notice-container">
			<el-aside width="240px">
				<el-menu default-active="1">
					<el-menu-item index="1" @click="searchFolder(1)">
						<el-icon><el-icon-message /></el-icon>
						<span>收件箱</span>
						<el-tag type="plain" round v-if="summary.unread > 0">{{ summary.unread }}</el-tag>
					</el-menu-item>
					<el-menu-item index="2" @click="searchFolder(2)">
						<el-icon><el-icon-position /></el-icon>
						<span>发件箱</span>
					</el-menu-item>
					<el-menu-item index="3" @click="searchStatus(1)">
						<el-icon><el-icon-finished /></el-icon>
						<span>草稿箱</span>
						<el-tag type="plain" round v-if="summary.draft > 0">{{ summary.draft }}</el-tag>
					</el-menu-item>
					<el-menu-item index="4" @click="searchStatus(2)">
						<el-icon><el-icon-delete /></el-icon>
						<span>已删除</span>
						<el-tag type="plain" round v-if="summary.delete > 0">{{ summary.delete }}</el-tag>
					</el-menu-item>
					<el-menu-item index="5" @click="searchStatus(3)">
						<el-icon><el-icon-takeaway-box /></el-icon>
						<span>已存档</span>
						<el-tag type="plain" round v-if="summary.archive > 0">{{ summary.archive }}</el-tag>
					</el-menu-item>
					<el-divider>
						<el-icon><el-icon-star-filled /></el-icon>
					</el-divider>
					<el-menu-item index="6" @click="searchReaded(0)">
						<el-icon><el-icon-files /></el-icon>
						<span>全部</span>
					</el-menu-item>
					<el-menu-item index="7" @click="searchReaded(1)">
						<el-icon><el-icon-message /></el-icon>
						<span>未读</span>
						<el-tag type="plain" round v-if="summary.unread > 0">{{ summary.unread }}</el-tag>
					</el-menu-item>
					<el-menu-item index="8" @click="searchReaded(2)">
						<el-icon><el-icon-message-box /></el-icon>
						<span>已读</span>
					</el-menu-item>
				</el-menu>
			</el-aside>
			<el-aside width="320px">
				<div class="notice-list-title">
					<el-checkbox v-model="selectRadio" size="large" @change="checkChange" />
					<el-dropdown @command="handleDropdown" :disabled="!isReader">
						<span class="el-dropdown-link">
							<el-icon><el-icon-filter /></el-icon> 筛选器
						</span>
						<template #dropdown>
							<el-dropdown-menu>
								<el-dropdown-item command="0"><el-icon><el-icon-files /></el-icon>全部</el-dropdown-item>
								<el-dropdown-item
									command="2"><el-icon><el-icon-message-box /></el-icon>已读</el-dropdown-item>
								<el-dropdown-item
									command="1"><el-icon><el-icon-message /></el-icon>未读</el-dropdown-item>
								<el-dropdown-item command="3"><el-icon><el-icon-link /></el-icon>带文件</el-dropdown-item>
							</el-dropdown-menu>
						</template>
					</el-dropdown>
				</div>
				<div class="notice-list-wall">
					<div class="notice-item" v-for="(it, index) in list" :key="index"
						:class="it.checked ? 'active' : ''" @click="goInfo(it)">
						<div class="item-avatar">
							<el-avatar :src="resHead(it.sender.avatar)" size="small"></el-avatar>
							<el-checkbox size="large" v-model="it.checked" @change="itemChange(it)" />
						</div>
						<div class="item-info">
							<div class="item-uname" :class="it.unread ? 'unread' : ''">
								<h3 class="uname">{{ it.sender.namec }}-({{ it.sender.names }})</h3>
								<div class="item-tool">
									<el-tooltip :content="it.unread ? '标记为已读' : '标记为未读'" placement="top">
										<el-icon
											@click.stop="setSignRead(it)"><el-icon-message /></el-icon></el-tooltip>
									<el-tooltip :content="it.is_arc ? '标记为存档' : '取消存档'" placement="top">
										<el-icon @click.stop="setArchive(it)"><el-icon-takeaway-box /></el-icon>
									</el-tooltip>
								</div>
							</div>
							<div class="title-time bold">
								<h3>{{ it.title }}</h3>
								<div class="time">{{ resTime(it.create_time) }}</div>
							</div>
							<p>{{ subStr(it.content) }}</p>
						</div>
						<div class="item-del" @click="tableDel(it)">
							<el-icon><el-icon-delete /></el-icon>
						</div>
					</div>
					<el-result icon="info" title="" v-if="list.length == 0">

						<template #sub-title>
							<p>空空如也~</p>
						</template>
					</el-result>
				</div>
			</el-aside>
			<el-main class="nopadding notice-main">
				<div v-show="lookType == 0" class="default-main">
					<el-icon><el-icon-message /></el-icon>
					<p>请选择左侧通知内容查看~</p>
				</div>
				<info ref="info" v-show="lookType == 1" :model="infoModel" @reply="goReply" />
				<edit ref="edit" v-show="lookType == 2" :model="infoModel" @addComplete="sendComplete" />
			</el-main>
		</el-container>
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_msg_notice',
	components: {
		info: defineAsyncComponent(() => import("./components/info")),
		edit: defineAsyncComponent(() => import("./components/edit")),
	},
	data() {
		return {
			apiObj: this.$API.scmmsgnotice.page,
			summary: {},//汇总信息
			list: [],//消息列表
			param: {
				key: "",
				type: 1,
				folder: 0,// 0默认，1收件箱，2发件箱
				status: 0,// 0默认，1草稿，2删除，3归档
				readed: 0,
				page: 1,
				limit: 20,
			},
			infoModel: { id: '0' },//当前对象
			lookType: 0, // 0=默认 1=详情 2=编辑
			isReader: true,//是否收件人
			isSender: false,//是否发件人
			selectRadio: false,
			selection: [],
			isIndeterminate: false,
		};
	},
	mounted() {
		this.init();
		this.initTotal();
	},
	methods: {
		async init() {
			var res = await this.$API.scmmsgnotice.page.get(this.param);
			if (!res || res.code != 200) {
				return;
			}

			this.list = res.data.items;
		},
		async initTotal() {
			var res = await this.$API.scmmsgnotice.summary.get();
			this.summary = res.data || {};
		},
		//查询详情
		async goInfo(m) {
			let http = this.isSender ? this.$API.scmmsgnotice.edit : this.$API.scmmsgnotice.view;
			var res = await http.get(m.id);
			if (!res || res.code != 200) {
				return;
			}

			this.infoModel = res.data;
			this.lookType = this.isSender && this.infoModel.handle == 1 ? 2 : 1;
			this.initTotal();
		},
		//新建通知
		addNew() {
			this.lookType = 2;
			this.infoModel = { id: this.$SCM.DEF_ID, user: null };
		},
		//回复通知
		goReply(user) {
			this.lookType = 2;
			this.infoModel = { id: this.$SCM.DEF_ID, user: user };
		},
		//添加完成通知
		sendComplete() {
			this.initTotal();
		},
		search() {

		},
		searchFolder(folder) {
			this.param.folder = folder;
			this.param.status = 0;
			this.param.readed = 0;
			this.isReader = folder == 1;
			this.isSender = folder == 2;
			this.lookType = 0;
			this.init();
		},
		searchStatus(status) {
			this.param.status = status;
			this.param.folder = 0;
			this.param.readed = 0;
			this.isReader = false;
			this.isSender = status == 1;//草稿
			this.lookType = 0;
			this.init();
		},
		searchReaded(readed) {
			this.param.status = 0;
			this.param.folder = 0;
			this.param.readed = readed;
			this.isReader = true;
			this.isSender = false;
			this.lookType = 0;
			this.init();
		},
		handleDropdown(number) {
			this.param.status = 0;
			if (number == 3) {
				this.param.type = number;
			} else {
				this.param.folder = number;
				this.param.type = 2;
			}
			this.lookType = 0;
			this.init();
		},
		delHtmlTag(str) {
			return str.replace(/<[^>]+>/g, "");
		},
		subStr(str, length) {
			if (!length) {
				length = 12;
			}
			let s = this.delHtmlTag(str);
			return s.substr(0, length);
		},
		resHead(img) {
			if (img && img.indexOf("http") == -1) {
				img = this.$CONFIG.SERVER_URL + img;
			}
			return img;
		},
		resTime(t) {
			const date = new Date(Number(t));
			const month = date.getMonth() + 1;
			const strDate = date.getDate();
			return month + "月" + strDate + "日";
		},
		//设置/取消存档
		async setArchive(it) {
			let http = it.status == 0 ? this.$API.scmmsgnotice.arced : this.$API.scmmsgnotice.unarc;
			let res = await http.put([it.id]);
			if (res.code == 200) {
				this.init();
				this.initTotal();
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
		//设置已读/取消
		async setSignRead(it) {
			let http = it.unread ? this.$API.scmmsgnotice.readed : this.$API.scmmsgnotice.unread;

			let res = await http.put([it.id]);
			if (res.code == 200) {
				this.init();
				this.initTotal();
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
		//删除
		async tableDel(row) {
			var res = await this.$API.scmmsgnotice.status.put({
				ids: [row.id],
				status: 3,
			});
			if (res.code == 200) {
				this.init();
				this.initTotal();
				this.lookType = 0;
				this.$message.success("删除成功");
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
		//批量删除
		batchDel() {
			this.optionFun("删除", 3);
		},
		//存档
		archives() {
			this.optionFun("存档", 2);
		},
		//设置已读
		setRead() {
			let checkedArr = this.list
				.filter((m) => m.checked)
				.map((item) => {
					return item.id;
				});

			this.$confirm(`确定要执行已读操作吗？`, "提示", {
				type: "warning",
				confirmButtonText: "确定",
				cancelButtonText: "取消",
			})
				.then(async () => {
					const loading = this.$loading();
					var res = await this.$API.scmmsgnotice.read.put(checkedArr);
					if (res.code == 200) {
						loading.close();
						this.init();
						this.initTotal();
						this.$message.success("操作成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		selectionChange(selection) {
			this.selection = selection;
		},
		optionFun(tip, status) {
			let checkedArr = this.list
				.filter((m) => m.checked)
				.map((item) => {
					return item.id;
				});
			if (checkedArr.length == 0) {
				this.$alert("请选择要" + tip + "的项", "提示", {
					type: "error",
				});
				return;
			}
			this.$confirm(
				`确定${tip}选中的 ${checkedArr.length} 项吗？`,
				"提示",
				{
					type: "warning",
					confirmButtonText: "确定",
					cancelButtonText: "取消",
				}
			)
				.then(async () => {
					const loading = this.$loading();
					var res = await this.$API.scmmsgnotice.status.put({
						ids: checkedArr,
						status: status,
					});
					if (res.code == 200) {
						loading.close();
						this.init();
						this.initTotal();
						this.lookType = 0;
						this.$message.success("操作成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		checkChange(e) {
			let checked = this.list.map((item) => {
				return item.id;
			});
			this.list.forEach((item) => {
				item.checked = e;
			});
			this.selection = e ? checked : [];
			this.isIndeterminate = e;
		},
		itemChange(e) {
			console.log("e", e);
		},
	},
};
</script>

<style scoped>
.notice-ft {
	margin-right: 20px;
}

.notice-container .el-aside {
	/* border-right: none; */
	background-color: #ffffff;
}

.notice-container .el-aside .el-menu-item {
	position: relative;
	padding: 13px 20px;
	height: auto;
	line-height: inherit;
}

.notice-container .el-aside .el-menu-item .el-tag {
	position: absolute;
	right: 10px;
}

.notice-list-title {
	display: flex;
	padding: 20px;
	border-bottom: 1px solid #e6e7e8;
}

.notice-list-title .el-checkbox.el-checkbox--large,
.notice-list-title .el-checkbox {
	height: auto;
	flex: 1;
}

.notice-list-wall {}

.notice-item {
	display: flex;
	align-items: center;
	border-left: 3px solid #ffffff;
	border-bottom: 1px solid #e6e7e8;
}

.notice-item:hover,
.notice-item.active {
	border-left: 3px solid #409eff;
	background-color: #f5f7fa;
}

.notice-item:hover .item-del .el-icon {
	display: inline-block;
}

.notice-item:hover .item-avatar .el-checkbox,
.notice-item.active .item-avatar .el-checkbox {
	display: inline-block;
}

.notice-item:hover .item-avatar .el-avatar,
.notice-item.active .item-avatar .el-avatar {
	display: none;
}

.notice-item:hover .item-tool,
.notice-item.active .item-tool {
	display: block;
}

.item-info {
	padding: 10px;
	width: 80%;
	line-height: 24px;
}

.item-info p,
.item-info h3 {
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
	font-weight: 400;
}

.item-info h3.uname {
	flex: 1;
	font-weight: 500;
}

.item-avatar {
	width: 42px;
	text-align: center;
	padding-left: 10px;
}

.item-avatar .el-checkbox {
	display: none;
}

.item-del {
	width: 30px;
	text-align: center;
	font-size: 15px;
	cursor: pointer;
	height: 92px;
	padding-top: 40px;
}

.item-del .el-icon {
	display: none;
}

.item-del:hover {
	background-color: #fef2f3;
	color: #e6182c;
}

.item-uname {
	display: flex;
}

.item-uname .unread {
	font-weight: bold;
}

.item-tool {
	font-size: 16px;
	display: none;
}

.title-time {
	display: flex;
	align-items: center;
}

.title-time h3 {
	width: 70%;
}

.item-tool .el-icon {
	margin-right: 10px;
	cursor: pointer;
}

.default-main {
	text-align: center;
	padding-top: 10%;
	font-size: var(--el-font-size-base);
	color: var(--el-text-color-regular);
}

.default-main .el-icon {
	font-size: 60px;
	color: #999999;
}
</style>
