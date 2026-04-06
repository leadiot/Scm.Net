<!--
 * @Descripttion: 处理iframe持久化，涉及store(VUEX)
 * @version: 1.0
 * @Author: sakuya
 * @Date: 2021年6月30日13:20:41
 * @LastEditors:
 * @LastEditTime:
-->

<template>
	<div v-show="$route.meta.type=='iframe'" class="iframe-pages">
		<iframe v-for="item in iframeList" :key="item.meta.url" v-show="$route.meta.url==item.meta.url" :src="item.meta.url" frameborder='0'></iframe>
	</div>
</template>

<script>
	import { useIframeStore } from "@/stores/iframe";
	import { useGlobalStore } from "@/stores/global";

	export default {
		setup() {
			const iframeStore = useIframeStore()
			const globalStore = useGlobalStore()
			return { iframeStore, globalStore }
		},
		data() {
			return {

			}
		},
		watch: {
			$route(e) {
				this.push(e)
			},
		},
		created() {
			this.push(this.$route);
		},
		computed:{
			iframeList(){
				return this.iframeStore.iframeList
			},
			ismobile(){
				return this.globalStore.ismobile
			},
			layoutTags(){
				return this.globalStore.layoutTags
			}
		},
		mounted() {

		},
		methods: {
			push(route){
				if(route.meta.type == 'iframe'){
					if(this.ismobile || !this.layoutTags){
						this.iframeStore.setIframeList(route)
					}else{
						this.iframeStore.pushIframeList(route)
					}
				}else{
					if(this.ismobile || !this.layoutTags){
						this.iframeStore.clearIframeList()
					}
				}
			}
		}
	}
</script>

<style scoped>
	.iframe-pages {width:100%;height:100%;background: #fff;}
	iframe {border:0;width:100%;height:100%;display: block;}
</style>
