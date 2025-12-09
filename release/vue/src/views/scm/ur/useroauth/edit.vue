<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="用户" prop="user_id"> 
	<el-input 
		v-model="formData.user_id" 
		placeholder="请输入用户" 
		:maxlength="20" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="登录网站" prop="src"> 
	<el-input 
		v-model="formData.src" 
		placeholder="请输入登录网站" 
		:maxlength="11" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="UnionID" prop="union_id"> 
	<el-input 
		v-model="formData.union_id" 
		placeholder="请输入UnionID" 
		:maxlength="32" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="用户" prop="user"> 
	<el-input 
		v-model="formData.user" 
		placeholder="请输入用户" 
		:maxlength="32" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="昵称" prop="name"> 
	<el-input 
		v-model="formData.name" 
		placeholder="请输入昵称" 
		:maxlength="32" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="性别" prop="sex"> 
	<el-input 
		v-model="formData.sex" 
		placeholder="请输入性别" 
		:maxlength="4" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="头像" prop="avatar"> 
	<el-input 
		v-model="formData.avatar" 
		placeholder="请输入头像" 
		:maxlength="256" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="" prop="access_token"> 
	<el-input 
		v-model="formData.access_token" 
		placeholder="请输入" 
		:maxlength="256" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="" prop="refresh_token"> 
	<el-input 
		v-model="formData.refresh_token" 
		placeholder="请输入" 
		:maxlength="256" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="过期时间" prop="expires_in"> 
	<el-input 
		v-model="formData.expires_in" 
		placeholder="请输入过期时间" 
		:maxlength="20" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="错误代码" prop="err_code"> 
	<el-input 
		v-model="formData.err_code" 
		placeholder="请输入错误代码" 
		:maxlength="8" 
		show-word-limit 
		clearable 
	></el-input> 
</el-form-item> 
<el-form-item label="错误描述" prop="err_msg"> 
	<el-input 
		v-model="formData.err_msg" 
		placeholder="请输入错误描述" 
		:maxlength="128" 
		show-word-limit 
		clearable 
	></el-input> 
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
	data() {
		return {
			mode: "add",
			titleMap: {add: "新增",edit: "编辑"},
			visible: false,
			isSaveing: false,
			formData: this.def_data(),
			rules: {},
		};
	},
	mounted() {
	},
	methods: {
	    def_data(){
		    return {
				id: this.$SCM.DEF_ID,
				user_id:'', 
src:'', 
union_id:'', 
user:'', 
name:'', 
sex:'', 
avatar:'', 
access_token:'', 
refresh_token:'', 
expires_in:'', 
err_code:'', 
err_msg:'', 

			}
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				var res = await this.$API.scmuruseroidc.edit.get(row.id);
				this.formData = res.data;
			}
			this.visible = true;
		},
		save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.isSaveing = true;
					let res = null;
					if (this.$SCM.is_valid_id(this.formData.id)) {
						res = await this.$API.scmuruseroidc.update.put(this.formData);
					} else {
						res = await this.$API.scmuruseroidc.add.post(this.formData);
					}
					this.isSaveing = false;
					if (res.code == 200) {
						this.$emit("complete");
						this.visible = false;
						this.$message.success("保存成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				}
			});
		},
		close() {
			this.formData = this.def_data();
			this.$refs.formRef.resetFields();
			this.visible = false;
		},
	},
};
</script>

<style scoped>
.el-select {
	width: 100%;
}
</style>