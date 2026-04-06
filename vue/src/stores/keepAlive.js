import { defineStore } from 'pinia'

export const useKeepAliveStore = defineStore('keepAlive', {
	state: () => ({
		keepLiveRoute: [],
		routeKey: null,
		routeShow: true,
	}),
	actions: {
		pushKeepLive(component) {
			if (!this.keepLiveRoute.includes(component)) {
				this.keepLiveRoute.push(component)
			}
		},
		removeKeepLive(component) {
			const index = this.keepLiveRoute.indexOf(component)
			if (index !== -1) {
				this.keepLiveRoute.splice(index, 1)
			}
		},
		clearKeepLive() {
			this.keepLiveRoute = []
		},
		setRouteKey(key) {
			this.routeKey = key
		},
		setRouteShow(key) {
			this.routeShow = key
		}
	}
})
