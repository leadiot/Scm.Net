<template>
	<el-card class="page">
		<el-row>
			<el-col>
				<div class="feed">
					<div class="item">
						<span>反馈网址：</span>
						{{ header.url }}
					</div>
					<div class="item">
						<span>反馈事项：</span>
						{{ header.title }}
					</div>
					<div class="item">
						<span>内容说明：</span>
						{{ header.remark }}
					</div>
				</div>
			</el-col>
		</el-row>
		<el-row>
			<el-col>
				<sc-list :data="list">
					<template #item="{ item }">
						<sc-summary :title="this.$TOOL.dateTimeFormat(item.create_time)"
							:summary="toHtml(item.content)"></sc-summary>
					</template>
				</sc-list>
			</el-col>
		</el-row>
		<el-row>
			<el-col class="reply">
				<el-input v-model="formData.content" placeholder="请输入回复内容" type="textarea" rows="5" :maxlength="1024"
					show-word-limit clearable></el-input>
			</el-col>
		</el-row>
		<el-row>
			<el-col style="text-align: right;">
				<el-button type="primary" @click="save">回复</el-button>
			</el-col>
		</el-row>
	</el-card>
</template>
<script>
export default {
	data() {
		return {
			mode: "add",
			titleMap: { add: "新增", edit: "编辑" },
			isSaveing: false,
			list: [],
			header: {},
			formData: this.def_data()
		};
	},
	mounted() {
		var hid = this.$route.query.id;
		this.formData.header_id = hid;
		this.init(hid);
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				header_id: '0',
				content: '',
			}
		},
		async init(hid) {
			var res = await this.$API.scmsysfeedback.view.get(hid);
			if (!res || res.code != 200) {
				return;
			}

			this.header = res.data;
			this.list = res.data.details;
		},
		async save() {
			if (!this.formData.content) {
				this.$message.warning('请输入回复内容！');
				return;
			}

			this.isSaveing = true;
			var res = await this.$API.scmsysfeedback.save.post(this.formData);
			this.isSaveing = false;
			if (res.code == 200) {
				this.$emit("complete");
				this.formData.content = '';
				this.init(this.formData.header_id);
				this.$message.success("保存成功");
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
		toHtml(content) {
			if (!content) {
				return '';
			}
			return content.replace(/\n/g, '<br/>');
		}
	},
};
</script>
<style scoped>
.page {
	max-width: 960px;
	background-color: white;
	margin: 0 auto;
}

.feed {
	display: flex;
	flex-direction: column;
	border: 1px solid #ccc;
	margin-bottom: 10px;
}

.feed .item {
	display: flex;
	flex-direction: row;
	justify-content: flex-start;
	padding: 5px;
}

.reply {
	margin-top: 10px;
	margin-bottom: 10px;
}
</style>