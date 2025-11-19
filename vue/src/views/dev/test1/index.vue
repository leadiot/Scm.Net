<template>
	<sc-search ref="search" @search="search">
		<template #search>
			<el-form ref="formRef" label-width="80px" :model="param">
				<el-form-item label="图标格式" prop="type">
					<sc-select v-model="param.type" :data="type_list" @change="changeSet()"
						style="width: 120px;"></sc-select>
				</el-form-item>
				<el-form-item label="图标颜色" prop="color">
					<el-color-picker v-model="color" :predefine="predefineColors" />
				</el-form-item>
				<el-form-item label="数据状态" prop="row_status">
					<sc-select v-model="param.row_status" clearable placeholder="请选择" :data="row_status_list" />
				</el-form-item>
				<el-form-item label="创建时间" prop="create_time">
					<el-date-picker v-model="param.create_time" type="datetimerange" range-separator="至"
						start-placeholder="开始日期" end-placeholder="结束日期">
					</el-date-picker>
				</el-form-item>
				<el-form-item label="搜索内容">
					<el-input v-model="param.key" clearable placeholder="关键字" />
				</el-form-item>
			</el-form>
		</template>
	</sc-search>
	<el-container>
		<el-header>
			<div class="left-panel">
				<el-button type="primary" @click="open_dialog()"><sc-icon name="sc-plus" /></el-button>
				<el-divider direction="vertical"></el-divider>
				<el-button-group>
					<el-tooltip content="启用">
						<el-button type="primary" plain :disabled="selection.length == 0" @click="status_list(1)">
							<sc-icon name="sc-check-circle-line" />
						</el-button>
					</el-tooltip>
					<el-tooltip content="停用">
						<el-button type="primary" plain :disabled="selection.length == 0" @click="status_list(2)">
							<sc-icon name="sc-pause-circle-line" />
						</el-button>
					</el-tooltip>
					<el-tooltip content="删除">
						<el-button type="danger" plain :disabled="selection.length == 0" @click="delete_list">
							<sc-icon name="sc-close-circle-line" />
						</el-button>
					</el-tooltip>
				</el-button-group>
			</div>
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="关键字" @keyup.enter="search()">
					<template #append>
						<el-button type="primary" @click="search()"><sc-icon name="sc-search" /></el-button>
					</template>
				</el-input>
				<el-button @click="show_search">高级</el-button>
			</div>
		</el-header>
		<el-main class="nopadding">
			<el-container>
				<el-aside style="width: 240px;">
					<sc-select v-model="param.set_id" :data="set_list" @change="changeSet()"></sc-select>
					<sc-list v-model="param.cat_id" :data="cat_list" @change="changeCat">
						<template #item="{ item }">
							{{ item.name }}
							<el-tag size="small" type="info">
								{{ item.qty }}
							</el-tag>
						</template>
					</sc-list>
				</el-aside>
				<el-main>
					<el-scrollbar>
						<div class="icon-list">
							<el-empty v-if="!hasIcons()" :image-size="100" description="未查询到相关图标" />
							<el-row v-else>
								<el-col :xs="6" :sm="6" :md="4" :lg="3" :xl="3" v-for="(icon, index) in data"
									:key="index">
									<div class="icon-item" :title="icon.desc" @click="copyCode(icon)">
										<div class="icon-info" :style="{ 'fontSize': size + 'px', 'color': color }">
											<span :class="getIcon(icon)">{{ getText(icon) }}</span>
										</div>
										<div class="icon-desc">
											{{ icon.desc }}
										</div>
									</div>
								</el-col>
							</el-row>
						</div>
					</el-scrollbar>
				</el-main>
			</el-container>
		</el-main>
	</el-container>
	<edit ref="edit" />
</template>

<script>
import { defineAsyncComponent } from "vue";

export default {
	name: 'scui_scicon',
	components: {
		copy: defineAsyncComponent(() => import("./copy")),
		edit: defineAsyncComponent(() => import("./edit")),
	},
	data() {
		return {
			param: {
				set_id: this.$SCM.ID_ONE,
				cat_id: this.$SCM.ID_ONE,
				type: '',
				row_status: this.$SCM.DEF_STATUS,
				create_time: '',
				key: ''
			},
			selection: [],
			size: 32,
			color: '#1a2947',
			predefineColors: this.$CONFIG.PREDEFINE_COLORS,
			set_list: [this.$SCM.OPTION_ONE],
			cat_list: [this.$SCM.OPTION_ONE],
			type_list: [{ 'id': 1, 'label': '线型', 'value': 1 }, { 'id': 2, 'label': '填充', 'value': '2' }, { 'id': 3, 'label': '圆角', 'value': '3' }, { 'id': 4, 'label': '方形', 'value': '4' }],
			data: [],
			row_status_list: [this.$SCM.OPTION_ALL_INT],
		};
	},
	watch: {
		searchText(val) {
			this.search(val);
		},
	},
	mounted() {
		this.$SCM.list_status(this.row_status_list, true);
		this.listSet();
	},
	methods: {
		async status_item(e, row) {
			this.$SCM.status_item(this, this.$API.scmdevicon.status, row, row.row_status);
		},
		status_list(status) {
			this.$SCM.status_list(this, this.$API.scmdevicon.status, this.selection, status);
		},
		async delete_item(row) {
			this.$SCM.delete_item(this, this.$API.scmdevicon.delete, row);
		},
		delete_list() {
			this.$SCM.delete_list(this, this.$API.scmdevicon.delete, this.selection);
		},
		show_search() {
			this.$refs.search.open(this.param.key);
		},
		open_dialog(row) {
			if (!row) {
				row = {};
				row.set_id = this.param.set_id;
				row.cat_id = this.param.cat_id;
				row.type = this.param.type;
			}
			this.$refs.edit.open(row);
		},
		listSet() {
			this.$SCM.list_option(this.set_list, this.$API.scmdeviconcat.option, {}, false);
		},
		async changeSet() {
			this.cat = '';
			var catRes = await this.$API.scmdeviconcat.list.get({ 'pid': this.param.set_id });
			if (!catRes || catRes.code != 200) {
				return;
			}
			this.cat_list = catRes.data;
		},
		async changeCat(row) {
			if (!row) {
				return;
			}

			this.param.cat_id = row.id;
			this.search();
		},
		async search() {
			var iconRes = await this.$API.scmdevicon.list.get(this.param);
			if (!iconRes || iconRes.code != 200) {
				return;
			}

			this.data = iconRes.data;
		},
		hasIcons() {
			return this.data && this.data.length > 0;
		},
		getName(icon) {
			var name = icon.name;
			if (name.startsWith('ms-')) {
				return icon.name;
			}

			// if (icon.type == 'both') {
			// 	name += (this.param.type ? '-fill' : '-line');
			// } else if (icon.type) {
			// 	name += '-' + icon.type;
			// }
			return name;
		},
		getText(icon) {
			var name = icon.name;
			if (name.startsWith('sc-')) {
				return '';
			}

			return name.substring(3);
		},
		getIcon(icon) {
			var name = icon.name;
			if (name.startsWith('ms-')) {
				return 'material-symbols-' + (this.param.type ? 'rounded' : 'outlined');
			}

			return 'scfont ' + this.getName(icon);
		},
		copyCode(icon) {
			this.$refs.edit.open(icon);
		}
	},
};
</script>

<style scoped>
.icon-list {}

.icon-list .icon-item {
	display: inline-block;
	display: flex;
	flex-direction: column;
	justify-content: space-between;
	align-items: center;
	border: 1px solid #fff;
	border-radius: 5px;
	width: 100%;
	height: 100%;
	min-height: 80px;
	transition: all 0.1s;
	border-radius: 4px;
	cursor: pointer;
	padding: 5px;
}

.icon-list .icon-item:hover {
	/* box-shadow: 0 0 1px 4px var(--el-color-primary); */
	background: var(--el-color-primary-light-9);
	border: 1px solid var(--el-color-primary);
}

.icon-list .icon-item .icon-info {
	text-align: center;
	flex-basis: 100%;
	display: flex;
	align-items: center;
	-webkit-transition: font-size 0.25s linear, width 0.25s linear;
	-moz-transition: font-size 0.25s linear, width 0.25s linear;
	transition: font-size 0.25s linear, width 0.25s linear;
}

.icon-list .icon-item .icon-info:hover {
	font-size: 100px;
}

.icon-list .icon-item .icon-desc {
	color: #afb7c7;
	text-align: center;
	width: 100%;
	padding: 2px 8px;
	display: block;
	word-break: keep-all;
	white-space: nowrap;
	overflow: hidden;
}

.icon-list .active {
	background: var(--el-color-primary-light-9);
	border: 1px solid var(--el-color-primary);
}
</style>