<template>
	<div class="sc-water-mark" ref="scWaterMark">
		<slot></slot>
	</div>
</template>

<script>
export default {
	name: 'scWatermark',
	props: {
		text: { type: String, required: true, default: "" },
		subtext: { type: String, default: "" },
		color: { type: String, default: "rgba(128,128,128,0.2)" },
		angle: { type: Number, default: -30 },
		fontSize: { type: Number, default: 20 },
		fontFamily: { type: String, default: "SimHei" },
		subtextFontSize: { type: Number, default: 14 },
		subtextFontFamily: { type: String, default: "Microsoft YaHei" },
	},
	data() {
		return {
		}
	},
	mounted() {
		this.create()
	},
	methods: {
		create() {
			this.clear()
			//创建画板
			var canvas = document.createElement('canvas')
			canvas.width = 150
			canvas.height = 150
			canvas.style.display = 'none'
			//绘制文字
			var text = canvas.getContext('2d')
			text.rotate(this.angle * Math.PI / 180)
			text.translate(-75, 25)
			text.fillStyle = this.color
			text.font = `bold ${this.fontSize}px ${this.fontFamily}`
			text.textAlign = "center"
			text.fillText(this.text, canvas.width / 2, canvas.height / 2)
			text.font = `${this.subtextFontSize}px ${this.subtextFontFamily}`
			text.fillText(this.subtext, canvas.width / 2, canvas.height / 2 + 20)
			//创建水印容器
			var watermark = document.createElement('div')
			watermark.setAttribute('class', 'watermark')
			const styleStr = `position:absolute;top:0;left:0;right:0;bottom:0;z-index:99;pointer-events:none;background-repeat:repeat;background-image:url('${canvas.toDataURL("image/png")}');`
			watermark.setAttribute('style', styleStr);
			this.$refs.scWaterMark.appendChild(watermark)
		},
		clear() {
			var wmDom = this.$refs.scWaterMark.querySelector('.watermark')
			wmDom && wmDom.remove()
		}
	}
}
</script>

<style scoped>
.sc-water-mark {
	position: relative;
	display: inherit;
	width: 100%;
	height: 100%;
}
</style>
