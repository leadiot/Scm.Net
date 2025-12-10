<template>
	<div v-if="pageLoading">
		<el-main>
			<el-card shadow="never">
				<el-skeleton :rows="1"></el-skeleton>
			</el-card>
			<el-card shadow="never" style="margin-top: 15px;">
				<el-skeleton></el-skeleton>
			</el-card>
		</el-main>
	</div>
	<ucWork v-if="dashboard == '1'" @on-mounted="onMounted"></ucWork>
	<ucData v-if="dashboard == '2'" @on-mounted="onMounted"></ucData>
	<widgets v-else @on-mounted="onMounted"></widgets>
</template>

<script>
import { defineAsyncComponent } from 'vue';
const ucWork = defineAsyncComponent(() => import('./work'));
const ucData = defineAsyncComponent(() => import('./data'));
const widgets = defineAsyncComponent(() => import('./widgets'));

export default {
	name: "dashboard",
	components: {
		ucWork,
		ucData,
		widgets
	},
	data() {
		return {
			pageLoading: true,
			dashboard: '0'
		}
	},
	created() {
		var theme = this.$TOOL.data.get("USER_THEME") || {};
		this.dashboard = theme.home || '2';
	},
	mounted() {

	},
	methods: {
		onMounted() {
			this.pageLoading = false;
		}
	}
}
</script>