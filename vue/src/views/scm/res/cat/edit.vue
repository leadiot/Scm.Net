<template>
	<sc-dialog v-model="visible" show-fullscreen destroy-on-close :title="titleMap[mode]" width="750px" @close="close">
		<el-form ref="formRef" label-width="100px" :model="formData" :rules="rules">
			<el-form-item label="应用" prop="app">
				<sc-select v-model="formData.app" placeholder="请选择应用" :data="app_list" @change="change_app()"></sc-select>
			</el-form-item>
			<el-form-item label="上级" prop="pid">
				<el-tree-select v-model="formData.pid" :data="parentIdOptions" :default-expand-all="true"
					:check-strictly="true" :style="{ width: '100%' }" />
			</el-form-item>
			<el-form-item label="名称" prop="namec">
				<el-input v-model="formData.namec" placeholder="请输入名称" :maxlength="32" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="图标" prop="image">
				<el-input v-model="formData.image" placeholder="请输入图标" :maxlength="256" show-word-limit
					clearable></el-input>
			</el-form-item>
			<el-form-item label="显示排序" prop="od">
				<el-input v-model="formData.od" placeholder="请输入显示排序" :maxlength="11" show-word-limit clearable></el-input>
			</el-form-item>
			<el-form-item label="引用数量" prop="qty">
				<el-input v-model="formData.qty" placeholder="请输入引用数量" :maxlength="11" show-word-limit clearable></el-input>
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
			titleMap: { add: "新增", edit: "编辑" },
			visible: false,
			isSaveing: false,
			formData: this.def_data(),
			rules: {
				app: [
					{ required: true, trigger: "change", pattern: this.$SCM.REGEX_ID, message: "请选择应用", },
				],
				pid: [
					{ required: true, trigger: "change", message: "请选择上级", },
				],
				namec: [
					{ required: true, trigger: "blur", message: "请输入名称", },
				],
			},
			app_list: [],
			parentIdOptions: [],
		};
	},
	mounted() {
		this.$SCM.list_app(this.app_list, 0, false);
	},
	methods: {
		def_data() {
			return {
				id: this.$SCM.DEF_ID,
				app: '0',
				od: 0,
				namec: '',
				image: '',
				qty: 0,
			}
		},
		change_app() {
			this.initTree(this.formData.app);
		},
		async initTree(app, id) {
			const t = await this.$API.scmrescat.list.get({ 'app': app });
			let _tree = [{ id: "1", value: "0", label: "默认", parentId: "0" }];
			t.data.some((m) => {
				if (id && m.id == id) {
					return;
				}
				_tree.push({
					id: m.id,
					value: m.id,
					label: m.namec,
					parentId: m.pid,
				});
			});
			this.parentIdOptions = this.$TOOL.changeTree(_tree);
		},
		async open(row) {
			if (!row || !row.id) {
				this.mode = "add";
			} else {
				this.mode = "edit";
				this.initTree(row.app, row.id);

				var res = await this.$API.scmrescat.edit.get(row.id);
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
						res = await this.$API.scmrescat.update.put(this.formData);
					} else {
						res = await this.$API.scmrescat.add.post(this.formData);
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