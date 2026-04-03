<template>
	<el-container>
		<el-aside width="220px">
			<el-tree ref="category" class="menu" node-key="label" default-expand-all :data="category"
				current-node-key="info" :highlight-current="true" :expand-on-click-node="false" @node-click="typeClick">
			</el-tree>
		</el-aside>
		<el-container>
			<el-main class="nopadding">
				<el-container>
					<el-header>
						<div class="left-panel">
							<el-button icon="el-icon-delete" plain type="danger" :disabled="selection.length == 0"
								@click="batch_del" />
							<el-button type="danger" plain @click="clearlog">清空日志</el-button>
						</div>
						<div class="right-panel">
							<el-date-picker v-model="search.time" type="datetimerange" range-separator="至"
								value-format="YYYY-MM-DD hh:mm:ss" start-placeholder="开始日期"
								end-placeholder="结束日期"></el-date-picker>
							<el-button icon="el-icon-search" type="primary" @click="upsearch" />
						</div>
					</el-header>
					<el-header style="height: 200px">
						<scCharts height="100%" :option="logsChartOption"></scCharts>
					</el-header>
					<el-main class="nopadding">
						<sc-table ref="table" :table-name="tableName" :apiObj="apiObj" stripe :column="column"
							highlightCurrentRow :hide-context-menu="false" @selection-change="selectionChange"
							@row-click="rowClick">
							<el-table-column fixed type="selection" width="60" />
							<template #levelName="{ data }">
								<el-icon v-if="data.level == '1'" style="color: #409eff">
									<el-icon-info-filled />
								</el-icon>
								<el-icon v-if="data.level == '2'" style="color: #e6a23c">
									<el-icon-bell />
								</el-icon>
								<el-icon v-if="data.level == '3'" style="color: #78e853">
									<el-icon-warning-filled />
								</el-icon>
								<el-icon v-if="data.level == '4'" style="color: #f56c6c">
									<el-icon-circle-close-filled />
								</el-icon>
								<el-icon v-if="data.level == '5'" style="color: #9e87ff">
									<el-icon-info-filled />
								</el-icon>
							</template>
						</sc-table>
					</el-main>
				</el-container>
			</el-main>
		</el-container>
	</el-container>

	<el-drawer v-model="infoDrawer" title="日志详情" :size="600" destroy-on-close>
		<info ref="info"></info>
	</el-drawer>
</template>

<script>
import info from "./info";
import scCharts from "@/components/scCharts";

export default {
	name: 'log_api',
	components: {
		info,
		scCharts,
	},
	data() {
		return {
			tableName: 'log_api',
			infoDrawer: false,
			logsChartOption: {
				color: ["#409eff", "#e6a23c", "#78e853", "#f56c6c", "#9e87ff"],
				grid: {
					top: "0px",
					left: "10px",
					right: "10px",
					bottom: "0px",
				},
				tooltip: {
					trigger: "axis",
				},
				xAxis: {
					type: "category",
					boundaryGap: false,
					data: [],
				},
				yAxis: {
					show: false,
					type: "value",
				},
				series: [],
			},
			category: [
				{
					label: "日志级别",
					children: [
						{ label: "debug", type: "level", value: "1" },
						{ label: "info", type: "level", value: "2" },
						{ label: "warn", type: "level", value: "3" },
						{ label: "error", type: "level", value: "4" },
						{ label: "fatal", type: "level", value: "5" },
					],
				},
				{
					label: "日志类型",
					children: [
						{ label: "登录日志", type: "type", value: 1 },
						{ label: "操作日志", type: "type", value: 2 },
					],
				},
			],
			column: [
				{ label: "id", prop: "id", hide: true },
				{ prop: "levelName", label: "级别", width: 60 },
				{ prop: "ip", label: "IP", width: 120 },
				{ prop: "operate_time", label: "操作时间", width: 160, align: 'left' },
				{ prop: "operate_user", label: "操作用户", width: 120 },
				{ prop: "url", label: "操作地址", minWidth: 200, align: 'left', showOverflowTooltip: true },
				{ prop: "method", label: "提交类型", width: 80 },
				{ prop: "browser", label: "浏览器", width: 100 },
				{ prop: "agent", label: "用户代理", width: 100, showOverflowTooltip: true },
				{ prop: "duration", label: "执行时长(毫秒)", width: 120 },
			],
			apiObj: this.$API.scmlogapi.page,
			search: {
				time: undefined,
			},
			selection: [],
		};
	},
	mounted() {
		this.initChart();
	},
	methods: {
		async initChart() {
			var res = await this.$API.scmlogapi.chart.get();
			if (res.code != 200) {
				this.$alert(res.message, "提示", { type: "error" });
				return;
			}
			this.logsChartOption.xAxis.data = res.data.time;
			let seriesRes = [];
			res.data.count.forEach((m) => {
				seriesRes.push({
					data: m,
					type: "bar",
					stack: "log",
					barWidth: "15px",
				});
			});
			this.logsChartOption.series = seriesRes;
		},
		upsearch() {
			if (!this.search.time) {
				this.$message.error("请选择日期");
				return;
			}
			this.$refs.table.reload({ time: this.search.time.join("/") });
		},
		batch_del() {
			this.$confirm(
				`确定删除选中的 ${this.selection.length} 项吗？`,
				"提示",
				{
					type: "warning",
					confirmButtonText: '确定',
					cancelButtonText: '取消',
				}
			)
				.then(async () => {
					const loading = this.$loading();
					let ids = [];
					this.selection.forEach((element) => {
						ids.push(element.id);
					});
					var res = await this.$API.scmlogapi.delete.delete(
						ids.join(",")
					);
					if (res.code == 200) {
						this.$refs.table.refresh();
						loading.close();
						this.$message.success("删除成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		clearlog() {
			this.$confirm(
				`确定要清空日志所有记录吗？`,
				"提示",
				{
					type: "warning",
					confirmButtonText: '确定',
					cancelButtonText: '取消',
				}
			)
				.then(async () => {
					const loading = this.$loading();
					var res = await this.$API.scmlogapi.clear.delete();
					if (res.code == 200) {
						this.$refs.table.refresh();
						loading.close();
						this.$message.success("删除成功");
					} else {
						this.$alert(res.message, "提示", { type: "error" });
					}
				})
				.catch(() => { });
		},
		selectionChange(selection) {
			this.selection = selection;
		},
		typeClick(data) {
			if (data.type == "level") {
				this.$refs.table.reload({ level: data.value });
			}
			if (data.type == "type") {
				this.$refs.table.reload({ type: data.value });
			}
		},
		rowClick(row) {
			this.infoDrawer = true;
			this.$nextTick(() => {
				this.$refs.info.setData(row);
			});
		},
	},
};
</script>

<style></style>