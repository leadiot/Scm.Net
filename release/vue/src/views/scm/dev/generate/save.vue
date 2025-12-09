<template>
	<sc-dialog v-model="visible" show-fullscreen :title="titleMap[mode]" width="1100px" destroy-on-close @close="close">
		<el-form ref="formRef" :model="formData" :rules="rules" size="default" label-width="100px">
			<el-row>
				<el-col :span="8">
					<el-form-item label="命名空间" prop="namespace">
						<el-input v-model="formData.namespace" placeholder="例如：Sys" :maxlength="32" show-word-limit
							clearable prefix-icon="el-icon-collection-tag" :style="{ width: '100%' }"></el-input>
					</el-form-item>
				</el-col>
				<el-col :span="8">
					<el-form-item label="API分组" prop="group">
						<el-input v-model="formData.group" placeholder="例如：Sys" :maxlength="32" show-word-limit
							clearable prefix-icon="el-icon-discount" :style="{ width: '100%' }"></el-input>
					</el-form-item>
				</el-col>
				<el-col :span="8">
					<el-form-item label="栅格" prop="isGrid">
						<el-switch v-model="formData.isGrid" />
					</el-form-item>
				</el-col>
			</el-row>
		</el-form>
		<el-table ref="table" :data="tableData" height="450" row-key="dbColumnName">
			<el-table-column label="#" type="index" width="50"></el-table-column>
			<el-table-column prop="dbColumnName" label="列名" />
			<el-table-column prop="dataType" label="数据类型" />
			<el-table-column prop="isPrimarykey" label="是否主键" width="100">
				<template #default="scope">
					<el-tag disable-transitions :type="scope.row.isPrimarykey ? 'success' : 'danger'">
						{{ scope.row.isPrimarykey ? "是" : "否" }}
					</el-tag>
				</template>
			</el-table-column>
			<el-table-column prop="isNullable" label="为空" width="120">
				<template #default="scope">
					<el-tag disable-transitions :type="scope.row.isNullable ? 'success' : 'danger'">
						{{ scope.row.isNullable ? "是" : "否" }}
					</el-tag>
				</template>
			</el-table-column>
			<el-table-column prop="isSearch" label="搜索" width="100">
				<template #default="scope">
					<el-switch v-model="scope.row.isSearch" />
				</template>
			</el-table-column>
			<el-table-column prop="isResult" label="列表展示" width="100">
				<template #default="scope">
					<el-switch v-model="scope.row.isResult" />
				</template>
			</el-table-column>
			<el-table-column prop="isUpdate" label="添加/编辑" width="100">
				<template #default="scope">
					<el-switch v-model="scope.row.isUpdate" />
				</template>
			</el-table-column>
			<el-table-column prop="columnDescription" label="描述" />
		</el-table>

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
			titleMap: { add: "生成代码", },
			tableData: [],
			apiObj: this.$API.scmdevgen.column,
			isSaveing: false,
			visible: false,
			formData: {
				tableName: '',
				namespace: '',
				types: 2,
				isGrid: false,
				group: '',
				tableColumnInfo: []
			},
			rules: {
				namespace: [
					{ required: true, trigger: "blur", message: "例如：Sys", },
				],
				isGrid: [
					{ required: true, trigger: "change", message: "单选框组不能为空", },
				],
				group: [
					{ required: true, trigger: "blur", message: "例如：Sys", },
				],
			},
			typesOptions: [
				{
					label: "全部表",
					value: 1,
				},
				{
					label: "部分表",
					value: 2,
				},
			],
			SearchEnabledColumns: [],
			ResultIgnoredColumns: ["id", "unit_id", "row_delete"],
			UpdateIgnoredColumns: ["id", "unit_id", "row_delete", "row_status", "create_time", "create_user", "update_time", "update_user"],
		};
	},
	mounted() {
		this.init();
	},
	methods: {
		async save() {
			this.$refs.formRef.validate(async (valid) => {
				if (valid) {
					this.formData.tableColumnInfo = this.tableData;
					//this.isSaveing = true;
					const res = await this.$API.scmdevgen.code.post(
						this.formData
					);

					this.download(res, 'application/octet-stream', 'code.zip');

					//this.isSaveing = false;
					// if (res.code == 200) {
					// 	this.$message.success(
					// 		"生成成功，到根目录【Generator】文件夹中查看"
					// 	);
					// } else {
					// 	this.$alert(res.message, "提示", { type: "error" });
					// }
				}
			});

		},
		download(res, fileType, fileName) {
			var blob = new Blob([res], { 'type': fileType });
			var element = document.createElement('a');
			var href = window.URL.createObjectURL(blob);
			element.href = href;
			element.download = fileName;
			// document.body.appendChild(element);
			element.click();
			// document.body.removeChild(element);
			window.URL.revokeObjectURL(href);
		},
		async init() {
			var res = await this.$API.scmdevgen.option.get();
			if (!res || res.code != 200) {
				return;
			}

			var data = res.data;
			if (!data) {
				return;
			}

			this.SearchEnabledColumns = data.SearchEnabledColumns || [];
			this.ResultIgnoredColumns = data.ResultIgnoredColumns || [];
			this.UpdateIgnoredColumns = data.UpdateIgnoredColumns || [];
		},
		async open(row) {
			this.formData.tableName = row.name;
			const res = await this.$API.scmdevgen.column.get({ table: row.name });
			this.tableData = res.data.items;
			this.tableData.forEach(item => {
				item.isSearch = this.match(this.SearchEnabledColumns, item.dbColumnName);
				item.isResult = !this.match(this.ResultIgnoredColumns, item.dbColumnName);
				item.isUpdate = !this.match(this.UpdateIgnoredColumns, item.dbColumnName);
			})

			this.visible = true;
		},
		match(list, key) {
			for (var i = 0; i < list.length; i += 1) {
				if (list[i] == key) {
					return true;
				}
			}
			return false;
		},
		close() {
			this.visible = false;
		},
	},
};
</script>
