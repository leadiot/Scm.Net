<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" :model="formData" :rules="rules" label-width="100px">
			<el-row>
				<el-col :span="12">
					<el-form-item label="任务分组" prop="group">
						<el-input v-model="formData.group" placeholder="请输入任务分组任务分组" :maxlength="30" show-word-limit
							clearable :style="{ width: '100%' }"></el-input>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item label="任务名称" prop="names">
						<el-input v-model="formData.names" placeholder="请输入任务名称" :maxlength="50" show-word-limit clearable
							:style="{ width: '100%' }"></el-input>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item label="任务类型" prop="types">
						<sc-select v-model="formData.types" placeholder="请选择任务类型" clearable :style="{ width: '100%' }"
							:data="triggerTypeOptions">
						</sc-select>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item label="间隔时间" prop="cron">
						<sc-cron v-model="formData.cron" placeholder="请输入Cron定时规则" clearable></sc-cron>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item v-if="formData.types == 2" label="请求类型" prop="api_method">
						<el-radio-group v-model="formData.api_method">
							<el-radio v-for="(item, index) in requestTypeOptions" :key="index" :value="item.value"
								:disabled="item.disabled">
								{{ item.label }}
							</el-radio>
						</el-radio-group>
					</el-form-item>
				</el-col>
			</el-row>
			<el-form-item v-if="formData.types == 1" label="类名" prop="dll_uri">
				<el-input v-model="formData.dll_uri" placeholder="请输入任务执行类名" :maxlength="200" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item v-if="formData.types == 2" label="请求地址" prop="api_uri">
				<el-input v-model="formData.api_uri" placeholder="请输入请求地址" :maxlength="200" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-row>
				<el-col :span="12">
					<el-form-item v-if="formData.types == 2" label="授权名">
						<el-input v-model="formData.api_auth_key" placeholder="请输入授权名" :maxlength="200" show-word-limit
							clearable :style="{ width: '100%' }"></el-input>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item v-if="formData.types == 2" label="授权值">
						<el-input v-model="formData.api_auth_value" placeholder="请输入授权值" :maxlength="500" show-word-limit
							clearable :style="{ width: '100%' }"></el-input>
					</el-form-item>
				</el-col>
			</el-row>
			<el-form-item v-if="formData.types == 2" label="请求参数">
				<el-input v-model="formData.api_parameter" type="textarea" placeholder="请输入请求参数" :maxlength="800"
					show-word-limit :autosize="{ minRows: 3, maxRows: 3 }" :style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="任务描述" prop="description">
				<el-input v-model="formData.description" type="textarea" placeholder="请输入任务描述" :maxlength="200"
					show-word-limit :autosize="{ minRows: 3, maxRows: 3 }" :style="{ width: '100%' }"></el-input>
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
import scCron from "@/components/scCron";
export default {
	components: {
		scCron,
	},
	emits: ["complete", "success", "closed"],
	data() {
		return {
			mode: "add",
			titleMap: { add: "新增", edit: "编辑" },
			isSaveing: false,
			visible: false,
			formData: this.initTask(),
			rules: {
				names: [
					{ required: true, trigger: "blur", message: "请输入任务名称" },
				],
				group: [
					{ required: true, trigger: "blur", message: "请输入分组名称" },
				],
				types: [
					{ required: true, trigger: "change", message: "请选择任务类型" },
				],
				cron: [
					{ required: true, trigger: "blur", message: "请输入任务时间间隔" },
				],
				// remark: [
				// 	{ required: true, trigger: "blur", message: "请输入任务描述" },
				// ],
				api_uri: [
					{ required: true, trigger: "blur", message: "请输入调用的API地址" },
				],
				api_method: [
					{ required: true, trigger: "change", message: "请选择API访问类型" },
				],
				//api_auth_key: [
				//    { required: true, message: '请输入Api授权名', trigger: 'blur' }
				//],
				//api_auth_value: [
				//    { required: true, message: '请输入Api授权值', trigger: 'blur' }
				//],
				//api_parameter: [
				//    { required: true, message: '请输入API参数', trigger: 'blur' }
				//],
				dll_uri: [
					{ required: true, message: "请输入类名", trigger: "blur" },
				],
				dll_method: [
					{ required: true, message: '请输入方法名', trigger: 'blur' }
				]
			},
			triggerTypeOptions: [
				{
					label: "Class",
					value: 1,
				},
				{
					label: "Api",
					value: 2,
				},
			],
			requestTypeOptions: [
				{
					label: "GET",
					value: "GET",
				},
				{
					label: "POST",
					value: "POST",
				},
			],
		};
	},
	methods: {
		async open(row) {
			if (!row) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				Object.assign(this.formData, row);
			}
			this.visible = true;
		},
		initTask() {
			//const time = this.$TOOL.dateFormat(new Date());
			return {
				names: "",
				group: "",
				cron: "",
				api_uri: "",
				remark: "",
				handle: 1,
				types: 0,
				api_method: "GET",
				api_auth_key: "",
				api_auth_value: "",
				api_parameter: "",
				dll_uri: "",
				dll_method: "",
				id: this.$SCM.DEF_ID,
			};
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = null;
					if (this.$SCM.is_valid_id(this.formData.id)) {
						res = await this.$API.scmsysquartz.update.put(this.formData);
					} else {
						res = await this.$API.scmsysquartz.add.post(this.formData);
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
			this.$refs.formRef.resetFields();
			this.formData = this.initTask();
			this.visible = false;
		},
	},
};
</script>
