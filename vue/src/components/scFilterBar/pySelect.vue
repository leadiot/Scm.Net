<template>
	<el-select v-bind="$attrs" :filter-method="filterMethod" @visible-change="visibleChange">
		<el-option v-for="field in optionsList" :key="field.value" :label="field.label" :value="field"></el-option>
	</el-select>
</template>

<script>
	import { match } from 'pinyin-pro'

	export default {
		props: {
			options: { type: Array, default: () => [] }
		},
		data() {
			return {
				optionsList: [],
				optionsList_: []
			}
		},
		mounted() {
			this.optionsList = this.options
			this.optionsList_ = [...this.options]
		},
		methods: {
			filterMethod(keyword){
				if(keyword){
					this.optionsList = this.optionsList_.filter((item) => 
						match(item.label, keyword) !== false
					)
				}else{
					this.optionsList = this.optionsList_
				}
			},
			visibleChange(isopen){
				if(isopen){
					this.optionsList = this.optionsList_
				}
			}
		}
	}
</script>
