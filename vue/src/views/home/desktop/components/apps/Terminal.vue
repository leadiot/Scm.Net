<template>
	<div class="terminal-app">
		<div class="terminal-output" ref="output">
			<div v-for="(line, index) in output" :key="index" class="output-line">
				{{ line }}
			</div>
		</div>
		<div class="terminal-input">
			<span class="prompt">C:\Users\User&gt;</span>
			<input
				v-model="command"
				@keyup.enter="executeCommand"
				placeholder="输入命令..."
				ref="input"
			/>
		</div>
	</div>
</template>

<script>
import { ref, nextTick } from 'vue';

export default {
	name: 'Terminal',
	setup() {
		const command = ref('');
		const output = ref([
			'Microsoft Windows [版本 10.0.19045]',
			'(c) Microsoft Corporation。保留所有权利。',
			'',
		]);
		const input = ref(null);
		const outputEl = ref(null);

		const executeCommand = () => {
			if (command.value.trim()) {
				output.value.push(`C:\\Users\\User> ${command.value}`);

				const cmd = command.value.toLowerCase().trim();
				if (cmd === 'cls' || cmd === 'clear') {
					output.value = [];
				} else if (cmd === 'help') {
					output.value.push('可用命令:');
					output.value.push('  help - 显示帮助信息');
					output.value.push('  cls  - 清空屏幕');
					output.value.push('  date - 显示当前日期');
					output.value.push('  time - 显示当前时间');
					output.value.push('  echo - 显示消息');
				} else if (cmd === 'date') {
					output.value.push(new Date().toLocaleDateString('zh-CN'));
				} else if (cmd === 'time') {
					output.value.push(new Date().toLocaleTimeString('zh-CN'));
				} else if (cmd.startsWith('echo ')) {
					output.value.push(command.value.substring(5));
				} else {
					output.value.push(`'${command.value}' 不是内部或外部命令，也不是可运行的程序或批处理文件。`);
				}

				output.value.push('');
				command.value = '';

				nextTick(() => {
					if (outputEl.value) {
						outputEl.value.scrollTop = outputEl.value.scrollHeight;
					}
				});
			}
		};

		return { command, output, input, outputEl, executeCommand };
	},
};
</script>

<style scoped>
.terminal-app {
	height: 100%;
	display: flex;
	flex-direction: column;
	background-color: #0c0c0c;
	color: #cccccc;
	font-family: 'Consolas', 'Courier New', monospace;
	font-size: 14px;
}

.terminal-output {
	flex: 1;
	padding: 10px;
	overflow-y: auto;
}

.output-line {
	white-space: pre-wrap;
	word-break: break-all;
	line-height: 1.4;
}

.terminal-input {
	display: flex;
	align-items: center;
	padding: 10px;
	border-top: 1px solid #333;
}

.prompt {
	color: #00ff00;
	margin-right: 8px;
}

.terminal-input input {
	flex: 1;
	background: transparent;
	border: none;
	color: #cccccc;
	font-family: inherit;
	font-size: inherit;
	outline: none;
}

.terminal-input input::placeholder {
	color: #666;
}
</style>
