<template>
	<el-card shadow="hover" header="版本信息">
		<div class="main-version" style="text-align: center">
			<img src="img/ver.svg" style="height: 100px" />
			<h2 style="margin-top: 15px">C-SCM {{ $CONFIG.CORE_VER }}</h2>
			<p>{{ remark }}</p>
			<p v-html="details"></p>
		</div>
	</el-card>
</template>

<script>
export default {
	title: "版本信息",
	icon: "el-icon-monitor",
	description: "当前项目版本信息",
	data() {
		return {
			ver: "loading...",
			remark: '',
			details: '',
		};
	},
	mounted() {
		this.getVer();
	},
	methods: {
		async getVer() {
			const res = await this.$API.scmdevversion.ver.get({ 'types': 10, 'sys': 'scm_web' })
			this.ver = res.data;
			this.remark = this.ver.remark;

			var details = this.ver.details;
			if (!details) {
				return;
			}

			details.forEach(element => {
				if (element.types == 1) {
					this.details += '【新增】';
				} else if (element.types == 2) {
					this.details += '【更新】';
				} else if (element.types == 3) {
					this.details += '【删除】';
				} else if (element.types == 4) {
					this.details += '【优化】';
				}
				this.details += element.content + '<br/>';
			});
		},
		/* 更新日志 */
		golog() {
			window.open("");
		},
		/* 版本控制 */
		govcs() {
			window.open("");
		},
	},
};
</script>
<style>
.main-version h2 {
	margin-bottom: 20px;
}

.main-version p {
	text-align: left;
	margin: 5px 0;
	color: #666;
	line-height: 1.8;
}

html.dark .main-version p {
	color: #ffffff;
}
</style>
