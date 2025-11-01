<template>
	<sc-dialog v-model="visible" show-fullscreen :title="title" width="600px" @close="close">
		<sc-form-table v-model="formData.details" :addTemplate="addTemplate" placeholder="暂无投票项">
			<el-table-column prop="title" label="标题">
				<template #default="scope">
					<el-input v-model="scope.row.title" placeholder="请输入标题"></el-input>
				</template>
			</el-table-column>
			<el-table-column prop="remark" label="说明">
				<template #default="scope">
					<el-input v-model="scope.row.remark" placeholder="请输入说明"></el-input>
				</template>
			</el-table-column>
		</sc-form-table>

		<template #footer>
			<el-button @click="close">取 消</el-button>
			<el-button :loading="isSaveing" type="primary" @click="save">
				提 交
			</el-button>
		</template>
	</sc-dialog>
</template>
<script>
export default {
	emits: ["complete"],
	data() {
		return {
			mode: "add",
			title: '投票明细',
			addTemplate: {
				id: this.$SCM.DEF_ID,
				header_id: '0',
				title: '',
				remark: ''
			},
			isSaveing: false,
			visible: false,
			formData: this.def_data()
		};
	},
	mounted() {
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				details: [],
			}
		},
		async open(row) {
			this.formData.id = row.id;
			var res = await this.$API.scmsysvotedetail.list.get({ 'id': row.id });
			this.formData.details = res.data;
			this.visible = true;
		},
		async save() {
			if (this.formData.details.length == 0) {
				this.$message.warning('需要添加投票项~');
				return
			}
			this.isSaveing = true;
			let res = await this.$API.scmsysvotedetail.batch.post(this.formData);
			this.isSaveing = false;
			if (res.code == 200) {
				this.$emit("complete");
				this.visible = false;
				this.$message.success("操作成功");
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
		close() {
			this.formData = this.def_data();
			this.visible = false;
		},
	},
};
</script>
