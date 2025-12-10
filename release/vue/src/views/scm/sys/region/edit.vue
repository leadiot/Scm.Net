<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="650px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="上级城市" prop="parentIdList">
				<el-input v-model="formData.parentName" :maxlength="30" disabled :style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="区域名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入城市名称" :maxlength="50" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-form-item label="区域编码" prop="codec">
				<el-input v-model="formData.codec" placeholder="请输入城市编码" :maxlength="30" show-word-limit clearable
					:style="{ width: '100%' }"></el-input>
			</el-form-item>
			<el-row>
				<el-col :span="12">
					<el-form-item label="经度" prop="lng">
						<el-input v-model="formData.lng" placeholder="请输入经度" :maxlength="30" show-word-limit clearable
							:style="{ width: '100%' }"></el-input>
					</el-form-item>
				</el-col>
				<el-col :span="12">
					<el-form-item label="维度" prop="lat">
						<el-input v-model="formData.lat" placeholder="请输入维度" :maxlength="30" show-word-limit clearable
							:style="{ width: '100%' }"></el-input>
					</el-form-item>
				</el-col>
			</el-row>
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
	emits: ['complete'],
	data() {
		return {
			mode: "add",
			titleMap: { add: "新增", edit: "编辑" },
			isSaveing: false,
			visible: false,
			formData: {
				id: 0,
				parentId: 0,
				parentName: '',
				namec: undefined,
				codec: undefined,
				lng: undefined,
				lat: undefined,
			},
			rules: {
				namec: [
					{ required: true, trigger: "blur", message: "请输入城市名称" },
				],
				codec: [
					{ required: true, trigger: "blur", message: "请输入城市编码" },
				],
				lng: [],
				lat: [],
			}
		};
	},
	mounted() {
	},
	methods: {
		async open(parent, row) {
			if (!row) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.sysregion.model.get(row.id);
				this.formData = res.data;
			}
			
			this.formData.parentId = parent.content.id;
			this.formData.parentName = parent.title;

			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = null;
					if (this.formData.id === 0) {
						res = await this.$API.sysregion.add.post(this.formData);
					} else {
						res = await this.$API.sysregion.update.put(this.formData);
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
			this.formData = {
				id: 0,
				parentId: 0,
				parentName: '',
				namec: undefined,
				codec: undefined,
				lng: undefined,
				lat: undefined,
			};
			this.$refs.formRef.resetFields();
			this.visible = false;
		},
	},
};
</script>
