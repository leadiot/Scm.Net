<template>
	<el-container>
		<el-header>
			<div class="left-panel">
				<label>图集：</label>
				<sc-select v-model="param.set_id" :data="set_list" @change="changeSet()"
					style="width: 120px;"></sc-select>
				<label>图标格式：</label>
				<sc-select v-model="param.type" :data="type_list" @change="changeSet()"
					style="width: 120px;"></sc-select>
				<label>图标颜色：</label>
				<el-color-picker v-model="color" :predefine="predefineColors" />
			</div>
			<div class="right-panel">
				<el-input v-model="param.key" clearable placeholder="关键字" @keyup.enter="search()">
					<template #append>
						<el-button type="primary" @click="search"><sc-icon name="sc-search" /></el-button>
					</template>
				</el-input>
			</div>
		</el-header>
		<el-main class="nopadding">
			<el-container>
				<el-aside style="width: 240px;">
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
	<copy ref="copy" />
</template>

<script>
import { defineAsyncComponent } from "vue";

export default {
	name: 'scui_scicon',
	components: {
		copy: defineAsyncComponent(() => import("./copy")),
	},
	data() {
		return {
			param: {
				set_id: this.$API.ID_ONE_INT,
				cat_id: '',
				type: '',
				key: ''
			},
			size: 32,
			color: '#1a2947',
			predefineColors: this.$CONFIG.PREDEFINE_COLORS,
			set_list: [],
			cat_list: [],
			type_list: [{ 'id': 1, 'label': '线型', 'value': 1 }, { 'id': 2, 'label': '填充', 'value': '2' }, { 'id': 3, 'label': '圆角', 'value': '3' }, { 'id': 4, 'label': '方形', 'value': '4' }],
			data: []
		};
	},
	watch: {
		searchText(val) {
			this.search(val);
		},
	},
	mounted() {
		this.listSet();
	},
	methods: {
		listSet() {
			this.$SCM.list_option(this.set_list, this.$API.scmresiconcat.option, {}, false);
		},
		async changeSet() {
			this.cat = '';
			var catRes = await this.$API.scmresiconcat.list.get({ 'pid': this.param.set_id });
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
			var iconRes = await this.$API.scmresicon.list.get(this.param);
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
			this.$refs.copy.open(this.getName(icon), this.color, this.size);
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