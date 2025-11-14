<template>
	<div class="sc-dynamic-table">
		<el-table :data="data" ref="table" :key="toggleIndex" border stripe>
			<el-table-column type="index" width="50" fixed="left">
				<template #header>
					<el-button type="primary" v-show="isAdd" icon="el-icon-plus" size="small" circle
						@click="rowAdd"></el-button>
				</template>
				<template #default="scope">
					<div class="sc-dynamic-table-handle">
						<span>{{ scope.$index + 1 }}</span>
						<el-button type="danger" icon="el-icon-delete" size="small" plain circle
							@click="rowDel(scope.row, scope.$index)" />
					</div>
				</template>
			</el-table-column>
			<el-table-column label="" width="58" v-if="dragSort">
				<template #default>
					<el-tag class="move" style="cursor: move"><el-icon-d-caret style="width: 1em; height: 1em" /></el-tag>
				</template>
			</el-table-column>
			<slot></slot>
			<!-- <el-table-column min-width="1"></el-table-column> -->
			<template #empty>
				{{ placeholder }}
			</template>
		</el-table>
	</div>
</template>

<script>
import Sortable from "sortablejs";

export default {
	props: {
		modelValue: { type: Array, default: () => [] },
		isAdd: { type: Boolean, default: true },
		addTemplate: { type: Object, default: () => { } },
		placeholder: { type: String, default: "暂无数据" },
		dragSort: { type: Boolean, default: false },
		tableHeight: { type: Object, default: null }
	},
	data() {
		return {
			data: [],
			toggleIndex: 0,
		};
	},
	mounted() {
		this.data = this.modelValue;
		if (this.dragSort) {
			this.rowDrop();
		}
	},
	watch: {
		modelValue() {
			this.data = this.modelValue;
		},
		data: {
			handler() {
				this.$emit("update:modelValue", this.data);
			},
			deep: true,
		},
	},
	methods: {
		rowDrop() {
			const _this = this;
			const tbody = this.$refs.table.$el.querySelector(
				".el-table__body-wrapper tbody"
			);
			Sortable.create(tbody, {
				handle: ".move",
				animation: 300,
				ghostClass: "ghost",
				onEnd({ newIndex, oldIndex }) {
					const tableData = _this.data;
					const currRow = tableData.splice(oldIndex, 1)[0];
					tableData.splice(newIndex, 0, currRow);
					_this.toggleIndex += 1;
					_this.$nextTick(() => {
						_this.rowDrop();
					});
				},
			});
		},
		rowAdd() {
			const temp = JSON.parse(JSON.stringify(this.addTemplate));
			this.data.push(temp);
		},
		rowDel(row, index) {
			this.data.splice(index, 1);
		},
	},
};
</script>

<style scoped>
.sc-dynamic-table .sc-dynamic-table-handle {
	text-align: center;
}

.sc-dynamic-table .sc-dynamic-table-handle span {
	display: inline-block;
}

.sc-dynamic-table .sc-dynamic-table-handle button {
	display: none;
}

.sc-dynamic-table .hover-row .sc-dynamic-table-handle span {
	display: none;
}

.sc-dynamic-table .hover-row .sc-dynamic-table-handle button {
	display: inline-block;
}
</style>
