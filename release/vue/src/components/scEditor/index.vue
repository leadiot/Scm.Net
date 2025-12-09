/**
* 纯文本编辑器
*/
<template>
	<el-container class="notepad">
		<el-header class="notepad-toolbar">
			<el-input />
		</el-header>
		<el-main class="nopadding notepad-container">
			<div class="notepad-content">
				<div class="notepad-title" v-if="titleVisible">
					<input v-model="titleRef" :placeholder="titlePlaceholder" @keydown.enter="handleEnter" />
				</div>
				<el-input style="height: 900px;overflow-y: hidden;" v-model="contentRef" type="textarea" :mode="mode"
					:placeholder="contentPlaceholder" />
			</div>
		</el-main>
	</el-container>
</template>

<script>
export default {
	props: {
		mode: { type: String, default: 'simple' },
		titlePlaceholder: { type: String, default: '请输入标题...' },
		contentPlaceholder: { type: String, default: '请输入内容...' },
		title: { type: String, default: '' },
		titleVisible: { type: Boolean, default: true },
		content: { type: String, default: '' },
	},
	setup() {
		return {
			titleRef: '',
			contentRef: ''
		};
	}
}
</script>