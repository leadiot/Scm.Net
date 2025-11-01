<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="650px" @close="close">
		<el-form ref="formRef" :model="formData" :rules="rules" label-width="100px">
			<el-form-item label="日程内容" prop="title">
				<el-input v-model="formData.title" placeholder="请输入日程内容" :maxlength="100" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="日程类型" prop="types">
				<sc-select v-model="formData.types" :data="typeOptions" placeholder="请选择日程类型" />
			</el-form-item>
			<el-form-item label="日程级别" prop="level">
				<el-select v-model="formData.level" placeholder="请选择日程级别" :style="{ width: '100%' }">
					<el-option v-for="item in levelOptions" :key="item.value" :label="item.label" :value="item.value">
						<span style="float: left">{{ item.label }}</span>
						<span style="float: right"><small class="circle"
								:style="{ 'background-color': item.color }"></small></span></el-option>
				</el-select>
			</el-form-item>
			<el-form-item label="开始时间" prop="start_time_txt">
				<el-date-picker type="datetime" v-model="formData.start_time_txt" :style="{ width: '100%' }"
					placeholder="请选择开始时间" clearable>
				</el-date-picker>
			</el-form-item>
			<el-form-item label="结束时间" prop="end_time_txt">
				<el-date-picker type="datetime" v-model="formData.end_time_txt" :style="{ width: '100%' }"
					placeholder="请选择结束时间" clearable>
				</el-date-picker>
			</el-form-item>
			<el-form-item label="参与人员" prop="users">
				<sc-table-select v-model="formData.users" :model-value="defaultValues" :apiObj="apiObj" :table-width="450"
					:props="props" :style="{ width: '100%' }" multiple clearable collapse-tags collapse-tags-tooltip>
					<el-table-column prop="avatar" label="头像" width="80">
						<template #default="scope">
							<el-avatar :src="$CONFIG.SERVER_URL + scope.row.avatar" size="small"></el-avatar>
						</template>
					</el-table-column>
					<el-table-column prop="namec" label="姓名" width="180"></el-table-column>
					<el-table-column prop="roleGroupName" label="角色"></el-table-column>
				</sc-table-select>
			</el-form-item>
		</el-form>

		<template #footer>
			<el-button @click="close">取 消</el-button>
			<el-button :loading="isSaveing" type="primary" @click="save">
				确 定
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
			titleMap: { add: "新增", edit: "编辑", },
			isSaveing: false,
			visible: false,
			apiObj: this.$API.scmuruser.simple,
			props: {
				label: "namec",
				value: "id",
				keyword: "keyword",
			},
			formData: this.def_data(),
			rules: {
				title: [
					{ required: true, trigger: "blur", message: "请输入日程内容", },
				],
				types: [
					{ required: true, trigger: "change", message: "请选择日程类型", },
				],
				level: [
					{ required: true, trigger: "change", message: "请选择日程级别", },
				],
				start_time_txt: [
					{ required: true, trigger: "change", message: "请选择开始时间", },
				],
				end_time_txt: [
					{ required: true, trigger: "change", message: "请选择结束时间", },
				],
				users: [
					{ required: true, trigger: "change", message: "请选择参与人", },
				],
			},
			typeOptions: [this.$SCM.OPTION_ONE_INT],
			levelOptions: [this.$SCM.OPTION_ONE_INT],
			defaultValues: [],
		};
	},
	mounted() {
		this.init();
	},
	methods: {
		async init() {
			this.$SCM.list_dic(this.typeOptions, 'calendar-type', false);
			this.$SCM.list_dic(this.levelOptions, 'calendar-level', false);
		},
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				title: '',
				types: this.$SCM.ID_ALL_INT,
				level: this.$SCM.ID_ALL_INT,
				start_time: '0',
				start_time_txt: null,
				end_time: '0',
				end_time_txt: null,
				users: [],
			}
		},
		async open(row) {
			if (!row) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmsyscalendar.model.get(row.id);

				// res.data.level = '' + res.data.level;
				// res.data.types = '' + res.data.types;
				this.formData = res.data;
				if (this.formData.start_time) {
					this.formData.start_time_txt = this.$TOOL.dateTimeFormat(this.formData.start_time);
				}
				if (this.formData.end_time) {
					this.formData.end_time_txt = this.$TOOL.dateTimeFormat(this.formData.end_time);
				}
				// this.formData.types = '' + this.formData.types;
				// this.formData.level = '' + this.formData.level;

				this.defaultValues = res.data.users;
			}
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					if (this.formData.start_time_txt) {
						this.formData.start_time = new Date(this.formData.start_time_txt).getTime();
					}
					if (this.formData.end_time_txt) {
						this.formData.end_time = new Date(this.formData.end_time_txt).getTime();
					}
					this.isSaveing = true;
					let res = null;

					if (this.$SCM.is_valid_id(this.formData.id)) {
						res = await this.$API.scmsyscalendar.update.put(this.formData);
					} else {
						res = await this.$API.scmsyscalendar.add.post(this.formData);
					}
					this.isSaveing = false;
					if (res.code == 200) {
						this.$emit("complete");
						this.visible = false;
						this.$message.success("操作成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				}
			});
		},
		close() {
			this.formData = this.def_data();
			this.defaultValues = [];
			this.$refs.formRef.resetFields();
			this.visible = false;
		},
	},
};
</script>
<style scoped>
.circle {
	display: inline-block;
	width: 10px;
	height: 10px;
	border-radius: 5px;
}
</style>
