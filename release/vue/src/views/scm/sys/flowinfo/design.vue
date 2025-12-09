<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-page-header content="流程设计" @back="goBack" />
			</div>
			<div class="right-panel">
				<el-button :loading="isSaveing" type="primary" @click="save">发布</el-button>
			</div>
		</el-header>
		<el-container class="flow-container">
			<sc-workflow v-model="data.nodeConfig"></sc-workflow>
		</el-container>
	</el-container>
</template>
<script>
import { useRoute } from "vue-router";
import scWorkflow from "@/components/scWorkflow";

export default {
	name: "design",
	components: {
		scWorkflow,
	},
	data() {
		return {
			isSaveing: false,
			data: {},
		};
	},
	mounted() {
		this.init();
	},
	methods: {
		async init() {
			var route = useRoute();
			var id = route.query.id;
			if (!id) {
				this.data = this.def_flow(0);
				return;
			}

			var res = await this.$API.scmsysflowinfo.readflow.get({ 'id': id });
			if (!res.data || res.data.length < 1) {
				this.data = this.def_flow(id);
				return;
			}

			this.data = JSON.parse(res.data);
		},
		gen_flow(list, pid, root) {
			if (!list) {
				return;
			}

			for (var i = 0; i < list.length; i += 1) {
				var item = list[i];
				if (item.pid != pid) {
					continue;
				}

				var node = this.gen_node(item);
				root.childNode = node;
				this.gen_flow(list, item.id, node);
			}
		},
		gen_node(item) {
			var node = {};
			node.id = item.id;
			node.type = item.types;
			node.nodeName = item.names;
			node.nodeRoleList = item.role_list;
			node.nodeUserList = item.user_list;
			return node;
		},
		def_flow(id) {
			return {
				id: id,
				name: "请假审批",
				nodeConfig: {
					nodeName: "发起人",
					type: 1,
					nodeRoleList: [],
					childNode: {
						nodeName: "条件路由",
						type: 4,
						conditionNodes: [
							{
								nodeName: "长期",
								type: 5,
								priorityLevel: 1,
								conditionMode: 1,
								conditionList: [
									{
										label: "请假天数",
										field: "day",
										operator: ">",
										value: "7",
									},
								],
								childNode: {
									nodeName: "领导审批",
									type: 2,
									setType: 1,
									nodeUserList: [
										{
											id: "360000197302144442",
											name: "何敏",
										},
									],
									nodeRoleList: [],
									examineLevel: 1,
									directorLevel: 1,
									selectMode: 1,
									termAuto: false,
									term: 0,
									termMode: 1,
									examineMode: 1,
									directorMode: 0,
								},
							},
							{
								nodeName: "短期",
								type: 5,
								priorityLevel: 2,
								conditionMode: 1,
								conditionList: [],
								childNode: {
									nodeName: "直接主管审批",
									type: 2,
									setType: 2,
									nodeUserList: [],
									nodeRoleList: [],
									examineLevel: 1,
									directorLevel: 1,
									selectMode: 1,
									termAuto: false,
									term: 0,
									termMode: 1,
									examineMode: 1,
									directorMode: 0,
								},
							},
						],
						childNode: {
							nodeName: "抄送人",
							type: 3,
							userSelectFlag: true,
							nodeUserList: [
								{
									id: "220000200908305857",
									name: "何秀英",
								},
							],
						},
					},
				},
			}
		},
		goBack() {
			this.$router.go(-1);
		},
		async save() {
			this.isSaveing = true;
			var res = await this.$API.scmsysflowinfo.saveflow.post(this.data);
			this.isSaveing = false;
			if (res.code == 200) {
				this.$message.success("发布成功");
			} else {
				this.$alert(res.message, "提示", { type: "error" });
			}
		},
	},
};
</script>
<style scoped>
.flow-container {
	height: calc(100% - 60px);
}

.flow-main {
	width: 100%;
	height: calc(100% - 0px);
	overflow: auto;
}

.basic-card {
	width: 900px;
	height: calc(100% - 40px);
	margin: 20px auto 0;
}

.flow-setting-title {
	margin-bottom: 15px;
}

:deep(.basic-main) .el-switch__core {
	width: 60px;
}
</style>
