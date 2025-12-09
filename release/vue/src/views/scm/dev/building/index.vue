<template>
	<el-container>
		<el-main class="nopadding">
			<el-empty description="Building..." image="/img/building.svg">
				<h4>页面建设中……</h4>
				<p>您访问的页面还在建设中，请稍后尝试。</p>
			</el-empty>
		</el-main>
	</el-container>
</template>

<script>
export default {
	name: 'dev_building',
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
