import { defineStore } from 'pinia'

export const useIframeStore = defineStore('iframe', {
	state: () => ({
		iframeList: []
	}),
	actions: {
		setIframeList(route) {
			this.iframeList = []
			this.iframeList.push(route)
		},
		pushIframeList(route) {
			let target = this.iframeList.find((item) => item.path === route.path)
			if (!target) {
				this.iframeList.push(route)
			}
		},
		removeIframeList(route) {
			const index = this.iframeList.findIndex((item) => item.path === route.path)
			if (index !== -1) {
				this.iframeList.splice(index, 1)
			}
		},
		refreshIframe(route) {
			const item = this.iframeList.find((item) => item.path === route.path)
			if (item) {
				const url = route.meta.url
				item.meta.url = ''
				setTimeout(() => {
					item.meta.url = url
				}, 200)
			}
		},
		clearIframeList() {
			this.iframeList = []
		}
	}
})
