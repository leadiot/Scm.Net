<template>
	<div class="calculator-app">
		<div class="display">
			<div class="expression">{{ expression || '0' }}</div>
			<div class="result">{{ result }}</div>
		</div>
		<div class="buttons">
			<button @click="clear" class="btn function">C</button>
			<button @click="backspace" class="btn function">⌫</button>
			<button @click="inputOperator('%')" class="btn function">%</button>
			<button @click="inputOperator('/')" class="btn operator">÷</button>

			<button @click="inputNumber('7')" class="btn">7</button>
			<button @click="inputNumber('8')" class="btn">8</button>
			<button @click="inputNumber('9')" class="btn">9</button>
			<button @click="inputOperator('*')" class="btn operator">×</button>

			<button @click="inputNumber('4')" class="btn">4</button>
			<button @click="inputNumber('5')" class="btn">5</button>
			<button @click="inputNumber('6')" class="btn">6</button>
			<button @click="inputOperator('-')" class="btn operator">−</button>

			<button @click="inputNumber('1')" class="btn">1</button>
			<button @click="inputNumber('2')" class="btn">2</button>
			<button @click="inputNumber('3')" class="btn">3</button>
			<button @click="inputOperator('+')" class="btn operator">+</button>

			<button @click="toggleSign" class="btn">±</button>
			<button @click="inputNumber('0')" class="btn">0</button>
			<button @click="inputDecimal" class="btn">.</button>
			<button @click="calculate" class="btn equals">=</button>
		</div>
	</div>
</template>

<script>
import { ref } from 'vue';

export default {
	name: 'Calculator',
	setup() {
		const expression = ref('');
		const result = ref('0');

		const inputNumber = (num) => {
			expression.value += num;
		};

		const inputOperator = (op) => {
			const lastChar = expression.value.slice(-1);
			if (['+', '-', '*', '/', '%'].includes(lastChar)) {
				expression.value = expression.value.slice(0, -1) + op;
			} else {
				expression.value += op;
			}
		};

		const inputDecimal = () => {
			const parts = expression.value.split(/[\+\-\*\/\%]/);
			const lastPart = parts[parts.length - 1];
			if (!lastPart.includes('.')) {
				expression.value += '.';
			}
		};

		const clear = () => {
			expression.value = '';
			result.value = '0';
		};

		const backspace = () => {
			expression.value = expression.value.slice(0, -1);
		};

		const toggleSign = () => {
			if (expression.value) {
				if (expression.value.startsWith('-')) {
					expression.value = expression.value.slice(1);
				} else {
					expression.value = '-' + expression.value;
				}
			}
		};

		const calculate = () => {
			try {
				if (expression.value) {
					const calculateExpr = new Function('return ' + expression.value);
					const evalResult = calculateExpr();
					result.value = Number.isInteger(evalResult)
						? evalResult.toString()
						: evalResult.toFixed(8).replace(/\.?0+$/, '');
				}
			} catch (error) {
				result.value = '错误';
			}
		};

		return {
			expression,
			result,
			inputNumber,
			inputOperator,
			inputDecimal,
			clear,
			backspace,
			toggleSign,
			calculate,
		};
	},
};
</script>

<style scoped>
.calculator-app {
	height: 100%;
	display: flex;
	flex-direction: column;
	background-color: #f5f5f5;
}

.display {
	padding: 20px;
	background-color: #fff;
	border-bottom: 1px solid #e0e0e0;
}

.expression {
	font-size: 18px;
	color: #666;
	min-height: 24px;
	text-align: right;
	word-break: break-all;
}

.result {
	font-size: 36px;
	font-weight: bold;
	color: #333;
	text-align: right;
	margin-top: 10px;
	word-break: break-all;
}

.buttons {
	flex: 1;
	display: grid;
	grid-template-columns: repeat(4, 1fr);
	gap: 1px;
	background-color: #e0e0e0;
	padding: 1px;
}

.btn {
	border: none;
	background-color: #fff;
	font-size: 24px;
	cursor: pointer;
	transition: background-color 0.2s;
	padding: 20px;
}

.btn:hover {
	background-color: #f0f0f0;
}

.btn:active {
	background-color: #e0e0e0;
}

.btn.function {
	background-color: #f5f5f5;
	color: #ff9500;
}

.btn.operator {
	background-color: #ff9500;
	color: #fff;
}

.btn.operator:hover {
	background-color: #ff8000;
}

.btn.equals {
	background-color: #4cd964;
	color: #fff;
}

.btn.equals:hover {
	background-color: #3cb054;
}
</style>
