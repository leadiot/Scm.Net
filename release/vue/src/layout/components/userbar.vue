<template>
	<div class="user-bar">
		<div title="机构信息">
			<label>{{ unitName }}</label>
		</div>
		<el-divider direction="vertical"></el-divider>
		<div class="panel-item hidden-sm-and-down" @click="search" title="搜索">
			<sc-icon name="sc-search" />
		</div>
		<div class="screen panel-item hidden-sm-and-down" @click="screen" title="全屏显示">
			<sc-icon name="sc-fullscreen" />
		</div>
		<div class="tasks panel-item" @click="tasks" title="任务中心">
			<sc-icon name="sc-sorting-vertical" />
		</div>
		<div class="msg panel-item" @click="showMsg" title="新消息">
			<el-badge :hidden="msgList.length == 0" :value="msgList.length" class="badge" type="danger">
				<sc-icon name="sc-message-2-line" />
			</el-badge>
			<el-drawer title="新消息" v-model="msgVisible" :size="400" append-to-body destroy-on-close>
				<el-container>
					<el-main class="nopadding">
						<el-scrollbar>
							<ul class="msg-list">
								<li v-for="item in msgList" v-bind:key="item.id">
									<a :href="item.link" target="_blank">
										<div class="msg-list__icon">
											<el-badge is-dot type="danger">
												<el-avatar :size="40" :src="item.avatar"></el-avatar>
											</el-badge>
										</div>
										<div class="msg-list__main">
											<h2>{{ item.title }}</h2>
											<p>{{ item.remark }}</p>
										</div>
										<div class="msg-list__time">
											<p>{{ getTime(item.create_time) }}</p>
										</div>
									</a>
								</li>
								<el-empty v-if="msgList.length == 0" description="暂无新消息" :image-size="100"></el-empty>
							</ul>
						</el-scrollbar>
					</el-main>
					<el-footer>
						<el-button type="primary" @click="goMessage">消息中心</el-button>
						<el-button @click="markRead">全部设为已读</el-button>
					</el-footer>
				</el-container>
			</el-drawer>
		</div>
		<el-divider direction="vertical"></el-divider>
		<el-dropdown class="user panel-item" trigger="click" @command="handleUser">
			<div class="user-avatar">
				<el-avatar :size="30" :src="avatar" :fit="fit" v-if="show_avatar"></el-avatar>
				<el-avatar :size="30" v-else>{{ userNameF }}</el-avatar>
				<label>{{ userName }}</label>
				<el-icon class="el-icon--right"><el-icon-arrow-down /></el-icon>
			</div>
			<template #dropdown>
				<el-dropdown-menu>
					<el-dropdown-item command="uc">帐号信息</el-dropdown-item>
					<el-dropdown-item command="clearCache">清除缓存</el-dropdown-item>
					<el-dropdown-item divided command="outLogin">退出登录</el-dropdown-item>
				</el-dropdown-menu>
			</template>
		</el-dropdown>
	</div>

	<el-dialog v-model="searchVisible" :width="700" title="搜索" class="drawerBG" center destroy-on-close>
		<search @success="searchVisible = false"></search>
	</el-dialog>

	<el-drawer v-model="tasksVisible" :size="450" title="任务中心" class="drawerBG" destroy-on-close>
		<tasks></tasks>
	</el-drawer>
</template>

<script>
import socket from "@/utils/socket"
import search from "./search.vue";
import tasks from "./tasks.vue";

export default {
	components: {
		search,
		tasks,
	},
	data() {
		return {
			userName: '',
			userNameF: '',
			unitName: '',
			avatar: '',
			fit: 'cover',
			show_avatar: true,
			searchVisible: false,
			tasksVisible: false,
			msgQty: 0,
			msgVisible: false,
			msgList: [],
		};
	},
	created() {
		var userInfo = this.$TOOL.data.get("USER_INFO");
		this.userName = userInfo.userName;
		this.userNameF = this.userName.substring(0, 1);
		this.unitName = userInfo.unitName;
		this.avatar = this.$SCM.get_avatar(userInfo.avatar);
		this.show_avatar = this.avatar != null && this.avatar != '';
		this.init();
		this.listMsg();
	},
	methods: {
		async init() {
			socket.open_wss("ReceiveMessage", (res) => {
				if (!res || !res.success) {
					return;
				}

				this.listMsg();
			});
		},
		async listMsg() {
			const res = await this.$API.scmmsgmessage.page.get({
				page: 1,
				limit: 10,
				cat: 'msg12'
			});
			if (!res || res.code != 200) {
				return;
			}

			this.msgList = res.data.items;
			this.msgQty = this.msgList.length;
		},
		//个人信息
		handleUser(command) {
			if (command == "uc") {
				this.$router.push({ path: "/home/usercenter" });
			}
			if (command == "cmd") {
				this.$router.push({ path: "/cmd" });
			}
			if (command == "clearCache") {
				this.$confirm(
					"清除缓存会将系统初始化，包括登录状态、主题、语言设置等，是否继续？",
					"提示",
					{ type: "info", })
					.then(() => {
						const loading = this.$loading();
						this.$TOOL.data.clear();
						this.$router.replace({ path: "/login" });
						setTimeout(() => {
							loading.close();
							location.reload();
						}, 1000);
					})
					.catch(() => {
						//取消
					});
			}
			if (command == "outLogin") {
				this.$confirm("确认是否退出当前用户？", "提示", {
					type: "warning",
					confirmButtonText: "退出",
					confirmButtonClass: "el-button--danger",
				})
					.then(() => {
						this.$router.replace({ path: "/login" });
					})
					.catch(() => {
						//取消退出
					});
			}
		},
		//全屏
		screen() {
			var element = document.documentElement;
			this.$TOOL.screen(element);
		},
		//显示短消息
		showMsg() {
			this.listMsg();
			this.msgVisible = true;
		},
		getTime(time) {
			return this.$TOOL.dateTimeFormat(time);
		},
		//标记已读
		async markRead() {
			let ids = [];
			this.msgList.forEach((item) => {
				ids.push(item.id);
			});
			const res = await this.$API.scmmsgmessage.read.put(ids);
			if (res.code == 200) {
				this.$message.success("操作成功~");
				this.msgList = [];
			} else {
				this.$message.warning(res.message);
			}
		},
		goMessage() {
			this.$router.push({ path: "/msg/message" });
		},
		//搜索
		search() {
			this.searchVisible = true;
		},
		//任务
		tasks() {
			this.tasksVisible = true;
		},
	},
};
</script>

<style scoped>
.user-bar {
	display: flex;
	align-items: center;
	height: 100%;
}

.user-bar .panel-item {
	padding: 0 10px;
	cursor: pointer;
	height: 100%;
	display: flex;
	align-items: center;
}

.user-bar .panel-item i {
	font-size: 16px;
}

.user-bar .panel-item:hover {
	background: rgba(0, 0, 0, 0.1);
}

.user-bar .user-avatar {
	height: 49px;
	display: flex;
	align-items: center;
}

.user-bar .user-avatar label {
	display: inline-block;
	margin-left: 5px;
	font-size: 12px;
	cursor: pointer;
}

.msg-list li {
	border-top: 1px solid #eee;
}

.msg-list li a {
	display: flex;
	padding: 20px;
}

.msg-list li a:hover {
	background: #ecf5ff;
}

.msg-list__icon {
	width: 40px;
	margin-right: 15px;
}

.msg-list__main {
	flex: 1;
}

.msg-list__main h2 {
	font-size: 15px;
	font-weight: normal;
	color: #333;
}

.msg-list__main p {
	font-size: 12px;
	color: #999;
	line-height: 1.8;
	margin-top: 5px;
}

.msg-list__time {
	width: 100px;
	text-align: right;
	color: #999;
}

.dark .msg-list__main h2 {
	color: #d0d0d0;
}

.dark .msg-list li {
	border-top: 1px solid #363636;
}

.dark .msg-list li a:hover {
	background: #383838;
}

.ver-alert {
	color: red;
}
</style>
