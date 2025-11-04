<template>
		<el-container>
			<el-aside width="240px" style="padding: 15px;">
				<el-form ref="form" :model="formData" label-width="auto" label-position="top">
					<el-form-item label="编码方案">
						<sc-select v-model="formData.format" :data="format_list"></sc-select>
					</el-form-item>
					<el-form-item label="条码位置">
						<sc-select v-model="formData.position" :data="position_list"></sc-select>
					</el-form-item>
					<el-form-item label="字体名称">
						<sc-select v-model="formData.fontName" :data="font_list"></sc-select>
					</el-form-item>
					<el-form-item label="字体大小">
						<el-input-number v-model="formData.fontSize"></el-input-number>
					</el-form-item>
					<el-form-item label="条码">
						<el-input v-model="formData.meta" placeholder="请输入条码，多个请以空格或换行分隔，扫描文字请以/分隔。如：12345678/这是测试文字"
							type="textarea" rows="10"></el-input>
					</el-form-item>
					<el-form-item>
						<el-button @click="genBarcode()">生成</el-button>
					</el-form-item>
				</el-form>
			</el-aside>
			<el-main>
				<el-card class="barcode-list">
					<div v-if="barcodes && barcodes.length > 0">
						<div v-for="barcode in barcodes" :key="barcode" class="barcode-item">
							<el-image :src="genImageUrl(barcode)" />
							<div class="barcode-text">{{ barcode }}</div>
						</div>
					</div>
					<div v-else>
						<el-empty>
							请在左侧输入条码内容。
						</el-empty>
					</div>
				</el-card>
			</el-main>
		</el-container>
</template>
<script>
export default {
	name: 'tools_barcode',
	data() {
		return {
			baseUrl: this.$API.toolsbarcode.barcode.url,
			formData: {
				meta: '',
				format: '0',
				showText: '0',
				position: '10',
				fontName: '3',
				fontSize: 32,
				width: 800,
				height: 120,
			},
			format_list: [this.$SCM.OPTION_ONE],
			position_list: [this.$SCM.OPTION_ONE_INT],
			option_list: [],
			font_list: [this.$SCM.OPTION_ONE],
			barcodes: [],
			option: {},
			timestamp: 0,
		};
	},
	mounted() {
		this.getOption();
		this.getFonts();
		this.$SCM.list_dic(this.position_list, 'barcode_position', false);
	},
	methods: {
		async getOption() {
			this.format_list.length = 1;
			var res = await this.$API.toolsbarcode.option.get();
			this.option_list = res.data;
			res.data.forEach(element => {
				element.label = element.codec + '' + element.namec;
				element.value = element.id;
				this.format_list.push(element);
			});
		},
		async getFonts() {
			var res = await this.$API.toolsbarcode.fonts.get();
			this.$SCM.prepare(this.font_list, res, false);
		},
		genBarcode() {
			this.barcodes.length = 0;
			if (!this.formData.format || this.formData.format === '0') {
				this.$message.warning('请选择条码方案！');
				return;
			}
			if (!this.formData.meta) {
				this.$message.warning('请输入条码内容！');
				return;
			}

			// this.option = {};
			// for (var i = 0; i < this.option_list.length; i++) {
			// 	if (this.option_list[i].id == this.format) {
			// 		this.option = this.option_list[i];
			// 		break;
			// 	}
			// }

			this.timestamp = new Date().getTime();
			var reg = /\s+/;
			var arr = this.formData.meta.split(reg);
			this.barcodes = [].concat(arr);
		},
		genImageUrl(barcode) {
			return `${this.baseUrl}?text=${barcode}&format=${this.formData.format}&position=${this.formData.position}&fontName=${this.formData.fontName}&fontSize=${this.formData.fontSize}&width=${this.formData.width}&height=${this.formData.height}&timestamp=${this.timestamp}`;
		},
	},
};
</script>
<style scoped>
.barcode-list {
	width: 100%;
	max-width: 842px;
	margin: 0 auto;
}

.barcode-list .barcode-item {
	text-align: center;
	margin-bottom: 20px;
	border-bottom: 1px dashed #eeeeee;
}

.barcode-list .barcode-text {
	font-size: 20px;
	margin-bottom: 20px;
}

.option_left {
	float: left;
}

.option_right {
	float: right;
	color: var(--el-text-color-secondary);
	font-size: 13px;
}
</style>