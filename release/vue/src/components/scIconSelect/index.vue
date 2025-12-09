<template>
	<div class="sc-icon-select">
		<div class="sc-icon-select__wrapper" :class="{ hasValue: value }" @click="open">
			<el-button style="padding: 7px;">
				<sc-icon :name="value || 'sc-reduce-btn'" style="margin: 0px;" :size="16"></sc-icon>
			</el-button>
		</div>
		<el-dialog title="图标选择器" v-model="dialogVisible" :width="760" destroy-on-close append-to-body>
			<div class="sc-icon-select__dialog">
				<el-form>
					<el-form-item prop="key">
						<el-input class="sc-icon-select__search-input" v-model="param.key" placeholder="搜索" clearable>
							<template #prepend>
								<sc-select v-model="param.set_id" :data="set_list" @change="changeSet"
									style="width: 115px"></sc-select>
							</template>
							<template #append>
								<el-button @click="search()">
									<sc-icon name="sc-search" />
								</el-button>
							</template>
						</el-input>
					</el-form-item>
				</el-form>
				<el-row>
					<el-col :span="6">
						<sc-list v-model="param.cat_id" :data="cat_list" @change="changeCat" style="height: 400px;">
							<template #item="{ item }">
								{{ item.name }}
								<el-tag size="small" type="info">
									{{ item.qty }}
								</el-tag>
							</template>
						</sc-list>
					</el-col>
					<el-col :span="18">
						<div class="sc-icon-select__list" style="height: 400px;">
							<el-scrollbar>
								<ul>
									<el-empty v-if="!hasIcons()" :image-size="100" description="未查询到相关图标" />
									<li v-for="icon in data" :key="icon.id" @click="selectIcon(icon)"
										:title="icon.desc">
										<div class="icon-item">
											<div class="icon-info">
												<span :class="getIcon(icon)">{{ getText(icon) }}</span>
											</div>
											<div class="icon-desc">
												{{ icon.desc }}
											</div>
										</div>
									</li>
								</ul>
							</el-scrollbar>
						</div>
					</el-col>
				</el-row>
			</div>
			<template #footer>
				<el-button @click="clear" text>清除</el-button>
				<el-button @click="dialogVisible = false">取消</el-button>
			</template>
		</el-dialog>
	</div>
</template>

<script>
export default {
	props: {
		modelValue: { type: String, default: "" },
		disabled: { type: Boolean, default: false },
	},
	data() {
		return {
			value: "",
			param: {
				set_id: '0',
				cat_id: '0',
				key: "",
			},
			dialogVisible: false,
			mode: 0,
			set_list: [],
			cat_list: [],
			data: [],
		};
	},
	watch: {
		modelValue(val) {
			this.value = val;
		},
		value(val) {
			this.$emit("update:modelValue", val);
		},
		searchText(val) {
			this.search(val);
		},
	},
	mounted() {
		this.value = this.modelValue;
	},
	methods: {
		open() {
			if (this.disabled) {
				return false;
			}

			this.listSet();
			this.dialogVisible = true;
		},
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
			await this.search();
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
		selectIcon(icon) {
			if (!icon) {
				return false;
			}

			this.value = icon.name;
			this.dialogVisible = false;
		},
		clear() {
			this.value = "";
			this.dialogVisible = false;
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
		}
	},
};
</script>

<style scoped>
.sc-icon-select {
	display: inline-flex;
}

.sc-icon-select__wrapper {
	cursor: pointer;
	display: inline-flex;
}

.sc-icon-select__wrapper:deep(.el-input__wrapper).is-focus {
	box-shadow: 0 0 0 1px var(--el-input-hover-border-color) inset;
}

.sc-icon-select__wrapper:deep(.el-input__inner) {
	flex-grow: 0;
	width: 0;
}

.sc-icon-select__wrapper:deep(.el-input__icon) {
	margin: 0;
	font-size: 16px;
}

.sc-icon-select__wrapper.hasValue:deep(.el-input__icon) {
	color: var(--el-text-color-regular);
}

.sc-icon-select__list {
	height: 270px;
	overflow: auto;
}

.sc-icon-select__list ul {}

.sc-icon-select__list li {
	display: inline-block;
	width: 80px;
	height: 80px;
	margin: 5px;
	vertical-align: top;
	transition: all 0.1s;
	border-radius: 4px;
	position: relative;
}

.sc-icon-select__list li i {
	display: inline-block;
	width: 100%;
	height: 100%;
	font-size: 26px;
	color: #6d7882;
	display: flex;
	justify-content: center;
	align-items: center;
	border-radius: 4px;
}

.sc-icon-select__list li:hover {
	box-shadow: 0 0 1px 4px var(--el-color-primary);
	background: var(--el-color-primary-light-9);
}

.sc-icon-select__list li:hover i {
	color: var(--el-color-primary);
}

.icon-item {
	display: flex;
	flex-direction: column;
	justify-content: space-between;
	align-items: center;
	cursor: pointer;
	width: 100%;
	height: 100%;
}

.icon-item .icon-info {
	text-align: center;
	flex-basis: 100%;
	display: flex;
	align-items: center;
	-webkit-transition: font-size 0.25s linear, width 0.25s linear;
	-moz-transition: font-size 0.25s linear, width 0.25s linear;
	transition: font-size 0.25s linear, width 0.25s linear;
	font-size: 32px;
}

.icon-item .icon-desc {
	color: #666;
	text-align: center;
	width: 100%;
	padding: 2px 8px;
	display: block;
	word-break: keep-all;
	white-space: nowrap;
	overflow: hidden;
}
</style>
