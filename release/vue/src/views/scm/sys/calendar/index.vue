<template>
	<el-container>
		<el-main class="nopadding">
			<el-header>
				<div class="left-panel">
					<sc-table-select v-model="selectUser" clearable :apiObj="apiObj" :table-width="450" :props="props"
						@change="userChange">
						<el-table-column prop="avatar" label="头像" width="80">
							<template #default="scope">
								<el-avatar :src="$CONFIG.SERVER_URL + scope.row.avatar" size="small"></el-avatar>
							</template>
						</el-table-column>
						<el-table-column prop="namec" label="姓名" width="180"></el-table-column>
						<el-table-column prop="roleGroupName" label="角色"></el-table-column>
					</sc-table-select>
				</div>
				<div class="right-panel">
					<sc-select v-model="param.types" placeholder="日程类型" style="width: 160px" :data="types_list">
					</sc-select>
					<el-select v-model="param.level" placeholder="日程级别" style="width: 160px">
						<el-option v-for="item in level_list" :key="item.value" :label="item.label" :value="item.id">
							<span style="float: left">{{ item.label }}</span>
							<span style="float: right">
								<small class="circle" :style="{ 'background-color': item.color }"></small>
							</span>
						</el-option>
					</el-select>
					<el-button icon="el-icon-search" type="primary" @click="list_data" />
				</div>
			</el-header>
			<el-calendar v-model="toDay" ref="calendar">

				<template #header="{ date }">
					<span>{{ date }}</span>
					<el-button-group>
						<el-button size="small" @click="selectDate('prev-month')">上个月</el-button>
						<el-button size="small" @click="selectDate('today')">今天</el-button>
						<el-button size="small" @click="selectDate('next-month')">下个月</el-button>
					</el-button-group>
				</template>

				<template #date-cell="{ data }">
					<div class="calendar-item">
						<h2>{{ data.day.split("-")[2] }} 日</h2>
						<div v-if="getData(data.day) && data.type == 'current-month'" class="calendar-item-info">
							<template v-for="(item, index) in getData(data.day)" :key="index">
								<div class="calendar-item-text" v-if="index < 2"
									:style="{ 'border-left': '3px solid ' + item.level, }">
									{{ item.title }}
								</div>
							</template>
						</div>
					</div>
				</template>
			</el-calendar>
		</el-main>
		<el-aside style="width: 320px; border-left: 1px solid #e6e6e6">
			<el-container>
				<el-header>
					<h2 class="dayTitle">
						<el-icon><el-icon-calendar /></el-icon>{{ day }}
					</h2>
					<div class="right-panel">
						<el-button icon="el-icon-plus" type="primary" @click="open">新建日程</el-button>
					</div>
				</el-header>
				<el-main>
					<div class="task-list">

						<template v-if="dayItem">
							<el-card shadow="hover" v-for="task in dayItem" :key="task.id" :class="stateMap[task.state]"
								:style="{ 'border-color': task.level, }">
								<h2>{{ task.title }}</h2>
								<div class="task-bottom">
									<el-tag type="success" round size="small">
										{{ getTypesNames(task.types) }}
									</el-tag>
									<div class="level" :style="{ 'background-color': task.level, }">
										{{ getLevelNames(task.level) }}
									</div>
									<el-dropdown size="small">
										<el-button icon="el-icon-more" circle />
										<template #dropdown>
											<el-dropdown-menu>
												<el-dropdown-item @click="delTask(task)"
													icon="el-icon-delete">删除</el-dropdown-item>
												<el-dropdown-item @click="open(task)" divided
													icon="el-icon-edit">编辑</el-dropdown-item>
											</el-dropdown-menu>
										</template>
									</el-dropdown>
								</div>
								<el-divider />
								<el-timeline>
									<el-timeline-item type="primary" :hollow="true">
										{{ task.beginTime }}
									</el-timeline-item>
									<el-timeline-item type="primary" :hollow="true">
										{{ task.endTime }}
									</el-timeline-item>
								</el-timeline>
								<div class="remind">
									<el-popover placement="top-start" v-for="(row, uindex) in task.user" :key="uindex"
										:title="row.fullName" trigger="hover">

										<template #reference>
											<el-avatar size="small" :src="$CONFIG.SERVER_URL + row.avatar" />
										</template>
										<p>{{ row.mobile }}</p>
										<p>{{ row.email }}</p>
									</el-popover>
								</div>
							</el-card>
						</template>

						<template v-else>
							<el-empty description="无工作任务" :image-size="100"></el-empty>
						</template>
					</div>
				</el-main>
			</el-container>
		</el-aside>
		<edit ref="edit" @complete="list_data" />
	</el-container>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
	name: 'scm_sys_calendar',
	components: {
		edit: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		return {
			apiObj: this.$API.scmuruser.simple,
			stateMap: {
				open: "open",
				complete: "complete",
				timeout: "timeout",
			},
			selectUser: {},
			props: {
				label: "namec",
				value: "id",
				keyword: "keyword",
			},
			param: {
				user_id: '0',
				types: this.$SCM.ID_ALL_INT,
				level: this.$SCM.ID_ALL_INT,
				toDay: null,
			},
			types_list: [this.$SCM.OPTION_ALL_INT],
			level_list: [this.$SCM.OPTION_ALL_INT],
			toDay: this.demoDay(),
			data: {},
		};
	},
	computed: {
		day() {
			return this.$TOOL.dateFormat(this.toDay, "yyyy-MM-dd");
		},
		dayItem() {
			return this.getData(this.day);
		},
	},
	mounted() {
		this.init();
		this.list_data();
	},
	methods: {
		async init() {
			this.$SCM.list_dic(this.types_list, 'calendar-type', true);
			this.$SCM.list_dic(this.level_list, 'calendar-level', true);
		},
		async list_data() {
			const res = await this.$API.scmsyscalendar.list.get(this.param);
			if (res == null || res.code != 200) {
				return;
			}

			res.data.forEach((m) => {
				m.beginTime = m.start_time != '0' ? this.$TOOL.dateFormat(eval(m.start_time), "yyyy-MM-dd") : '';
				m.endTime = m.end_time != '0' ? this.$TOOL.dateFormat(eval(m.end_time), "yyyy-MM-dd") : '';
			});
			this.data = this.getGroup(res.data, "beginTime");
		},
		getData(date) {
			return this.data[date];
		},
		demoDay(n = 0) {
			var curDate = new Date();
			var oneDayTime = 24 * 60 * 60 * 1000;
			return new Date(curDate.getTime() + oneDayTime * n);
		},
		userChange(val) {
			///this.selectUser = val;
			this.param.user_id = val.id;
			this.list_data();
		},
		getGroup(data, key) {
			let groups = {};
			data.forEach((c) => {
				let value = c[key];
				groups[value] = groups[value] || [];
				groups[value].push(c);
			});
			return groups;
		},
		selectDate(val) {
			this.$refs.calendar.selectDate(val);
			this.param.toDay = this.$TOOL.dateFormat(this.toDay, "yyyy-MM-dd");
			this.list_data();
		},
		delTask(row) {
			this.$confirm(`确定删除选中的 ${row.title} 吗？`, "提示", {
				type: "warning",
				confirmButtonText: "确定",
				cancelButtonText: "取消",
			}).then(async () => {
				const loading = this.$loading();
				var res = await this.$API.scmsyscalendar.delete.delete([
					row.id,
				]);
				if (res.code == 200) {
					loading.close();
					this.$message.success("删除成功");
					this.list_data();
				} else {
					this.$alert(res.message, "提示", { type: "error" });
				}
			}).catch(() => { });
		},
		open(row) {
			if (row.id) {
				this.$refs.edit.open(row);
			} else {
				this.$refs.edit.open();
			}
		},
		getTypesNames(types) {
			return this.$SCM.get_dic_names(this.types_list, types, '未知');
		},
		getLevelNames(types) {
			return this.$SCM.get_dic_names(this.level_list, types, '未知');
		}
	},
};
</script>

<style scoped>
.calendar-item h2 {
	font-size: 14px;
	padding-left: 8px;
}

.calendar-item-info {
	margin-top: 5px;
}

.calendar-item-info p {
	margin-top: 5px;
}

.task-list .el-card {
	margin-bottom: 15px;
	border-left: 5px solid #ddd;
	cursor: pointer;
}

.task-list h2 {
	font-size: 14px;
	font-weight: normal;
}

.task-list .remind {
	display: flex;
}

.task-list .remind .el-avatar {
	margin-right: 10px;
}

.task-list .el-divider--horizontal {
	margin: 12px 0;
}

.task-list .el-timeline-item {
	padding-bottom: 10px;
}

.task-list .el-timeline-item:last-child {
	padding-bottom: 0px;
}

.task-bottom {
	display: flex;
	justify-content: space-between;
	align-items: center;
	margin-top: 10px;
}

.task-bottom .level {
	border-radius: 10px;
	background-color: #67c23a;
	padding: 2px 10px;
	color: #ffffff;
}

.dayTitle {
	font-size: 14px;
	display: flex;
	align-items: center;
}

.dayTitle i {
	color: #999;
	margin-right: 10px;
}

.circle {
	display: inline-block;
	width: 10px;
	height: 10px;
	border-radius: 5px;
}

.calendar-item-text {
	white-space: nowrap;
	overflow: hidden;
	text-overflow: ellipsis;
	background-color: #fdf6ed;
	padding: 2px 5px;
	border-left: 3px solid #f56c6c;
	margin: 2px 0;
}

.el-calendar-table .el-calendar-day {
	padding: 8px 0px !important;
}
</style>
