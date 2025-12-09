<template>
	<el-container>
		<el-main class="nopadding">
			<el-empty description="No Audit!!!" image="/img/noaudit.svg">
				<h4>您无权访问此页面!</h4>
				<p>请与您的管理员确认，您是有访问此页面的权限。</p>
			</el-empty>
		</el-main>
	</el-container>
</template>

<script>
export default {
	name: 'scm_noaudit',
	data() {
		return {
			app_code: '',
		};
	},
	mounted() { },
	methods: {
	}
};
</script>

<style scoped>
h4 {
	font-size: 20px;
	color: #333;
}

p {
	font-size: 12px;
	color: #999;
	margin: 20px 0;
}
</style>