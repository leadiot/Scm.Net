<template>
	<el-select v-bind="$attrs" :loading="loading">
		<el-option v-for="item in options" :key="item[props.value]" :label="item[props.label]"
			:value="item[props.value]" :disabled="item.disabled">
			<slot name="option" :data="item"></slot>
		</el-option>
	</el-select>
</template>

<script>
import config from "@/config/select";

export default {
	props: {
		apiObj: { type: Object, default: () => { } },
		params: { type: Object, default: () => ({}) },
		data: { type: Object, default: () => { } },
	},
	data() {
		return {
			loading: false,
			options: [],
			props: config.props
		}
	},
	created() {
		//如果有默认值就去请求接口获取options
		this.options = this.data;
		if (this.apiObj && this.apiObj.url) {
			this.getRemoteData()
		}
	},
	methods: {
		//获取数据
		async getRemoteData() {
			this.loading = true;
			let res = await this.apiObj.get(this.params);
			this.options = res.data;
			this.loading = false;
		}
	}
}
</script>
