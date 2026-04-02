<template>
	<img ref="img" />
</template>

<script>
import QRCode from "qrcode"

export default {
	props: {
		text: { type: String, required: true, default: "" },
		size: { type: Number, default: 100 },
		logo: { type: String, default: "" },
		logoSize: { type: Number, default: 30 },
		logoPadding: { type: Number, default: 5 },
		colorDark: { type: String, default: "#000000" },
		colorLight: { type: String, default: "#ffffff" },
		correctionLevel: { type: String, default: "M" },
	},
	data() {
		return {
			qrcode: null
		}
	},
	watch: {
		text() {
			this.draw()
		}
	},
	mounted() {
		this.draw()
	},
	methods: {
		async create() {
			const canvas = document.createElement("canvas")
			await QRCode.toCanvas(canvas, this.text, {
				width: this.size,
				margin: 0,
				color: {
					dark: this.colorDark,
					light: this.colorLight
				},
				errorCorrectionLevel: this.correctionLevel.toUpperCase()
			})
			this.qrcode = canvas
		},
		async drawLogo() {
			return new Promise((resolve) => {
				const logo = new Image()
				logo.crossOrigin = "anonymous"
				logo.src = this.logo
				const logoPos = (this.size - this.logoSize) / 2
				const rectSize = this.logoSize + this.logoPadding
				const rectPos = (this.size - rectSize) / 2
				const ctx = this.qrcode.getContext("2d")
				
				logo.onload = () => {
					ctx.fillStyle = this.colorLight
					ctx.fillRect(rectPos, rectPos, rectSize, rectSize)
					ctx.drawImage(logo, logoPos, logoPos, this.logoSize, this.logoSize)
					resolve()
				}
				logo.onerror = () => {
					console.warn("Failed to load logo image")
					resolve()
				}
			})
		},
		async draw() {
			await this.create()
			if (this.logo) {
				await this.drawLogo()
			}
			this.$refs.img.src = this.qrcode.toDataURL("image/png")
		},
	}
}
</script>
