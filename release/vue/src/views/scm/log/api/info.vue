<template>
	<el-main style="padding:0 20px;">
		<el-descriptions :column="1" border size="small" class="log-desc">
			<el-descriptions-item label="请求接口">{{ data.url }}</el-descriptions-item>
			<el-descriptions-item label="请求方法">{{ data.method }}</el-descriptions-item>
			<el-descriptions-item label="日志级别">{{ data.levelName }}</el-descriptions-item>
			<el-descriptions-item label="日志时间">{{ data.operate_time }}</el-descriptions-item>
		</el-descriptions>
		<el-collapse v-model="activeNames" style="margin-top: 20px;">
			<el-collapse-item title="参数" name="1">
				<el-alert :title="data.parameters" :type="typeMap[data.level]" :closable="false"></el-alert>
			</el-collapse-item>
			<el-collapse-item title="详细" name="2">
				<div class="code">
					{{ data.content }}
				</div>
			</el-collapse-item>
		</el-collapse>
	</el-main>
</template>

<script>
export default {
	data() {
		return {
			data: {},
			activeNames: ['1'],
			typeMap: {
				'info': "info",
				'warn': "warning",
				'error': "error"
			}
		}
	},
	methods: {
		setData(data) {
			this.data = data
		}
	}
}
</script>

<style scoped>
.code {
	background: #848484;
	padding: 15px;
	color: #fff;
	font-size: 12px;
	border-radius: 4px;
}
</style>
<style>
.log-desc {
	word-wrap: break-word;
	word-break: break-all;
}

.log-desc .el-descriptions__label {
	width: 90px;
}
</style>