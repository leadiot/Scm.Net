<template>
	<div class="calendar-app">
		<div class="calendar-header">
			<button @click="prevMonth" class="nav-btn">‹</button>
			<h3>{{ currentYear }}年 {{ currentMonth + 1 }}月</h3>
			<button @click="nextMonth" class="nav-btn">›</button>
		</div>
		<div class="calendar-grid">
			<div class="weekday" v-for="day in weekdays" :key="day">{{ day }}</div>
			<div
				v-for="date in calendarDays"
				:key="date.key"
				class="day"
				:class="{ today: date.isToday, otherMonth: date.otherMonth }"
			>
				{{ date.day }}
			</div>
		</div>
	</div>
</template>

<script>
import { ref, computed } from 'vue';

export default {
	name: 'Calendar',
	setup() {
		const today = new Date();
		const currentYear = ref(today.getFullYear());
		const currentMonth = ref(today.getMonth());
		const weekdays = ['日', '一', '二', '三', '四', '五', '六'];

		const calendarDays = computed(() => {
			const firstDay = new Date(currentYear.value, currentMonth.value, 1);
			const lastDay = new Date(currentYear.value, currentMonth.value + 1, 0);
			const days = [];

			// 上个月的日期
			const firstDayOfWeek = firstDay.getDay();
			for (let i = firstDayOfWeek - 1; i >= 0; i--) {
				const date = new Date(currentYear.value, currentMonth.value, -i);
				days.push({
					key: `prev-${i}`,
					day: date.getDate(),
					otherMonth: true,
					isToday: false,
				});
			}

			// 当前月的日期
			for (let i = 1; i <= lastDay.getDate(); i++) {
				const isToday =
					i === today.getDate() &&
					currentMonth.value === today.getMonth() &&
					currentYear.value === today.getFullYear();
				days.push({
					key: `current-${i}`,
					day: i,
					otherMonth: false,
					isToday,
				});
			}

			// 下个月的日期
			const remainingDays = 42 - days.length;
			for (let i = 1; i <= remainingDays; i++) {
				days.push({
					key: `next-${i}`,
					day: i,
					otherMonth: true,
					isToday: false,
				});
			}

			return days;
		});

		const prevMonth = () => {
			if (currentMonth.value === 0) {
				currentMonth.value = 11;
				currentYear.value--;
			} else {
				currentMonth.value--;
			}
		};

		const nextMonth = () => {
			if (currentMonth.value === 11) {
				currentMonth.value = 0;
				currentYear.value++;
			} else {
				currentMonth.value++;
			}
		};

		return {
			currentYear,
			currentMonth,
			weekdays,
			calendarDays,
			prevMonth,
			nextMonth,
		};
	},
};
</script>

<style scoped>
.calendar-app {
	height: 100%;
	display: flex;
	flex-direction: column;
	padding: 20px;
}

.calendar-header {
	display: flex;
	justify-content: space-between;
	align-items: center;
	margin-bottom: 20px;
}

.calendar-header h3 {
	font-size: 18px;
	color: #333;
}

.nav-btn {
	width: 32px;
	height: 32px;
	border: none;
	border-radius: 4px;
	background-color: #f5f5f5;
	cursor: pointer;
	font-size: 20px;
	transition: background-color 0.3s;
}

.nav-btn:hover {
	background-color: #e0e0e0;
}

.calendar-grid {
	display: grid;
	grid-template-columns: repeat(7, 1fr);
	gap: 1px;
	background-color: #e0e0e0;
	border: 1px solid #e0e0e0;
	border-radius: 4px;
	overflow: hidden;
}

.weekday {
	padding: 10px;
	text-align: center;
	background-color: #f5f5f5;
	font-weight: bold;
	color: #666;
}

.day {
	padding: 15px;
	text-align: center;
	background-color: #fff;
	cursor: pointer;
	transition: background-color 0.3s;
}

.day:hover {
	background-color: #f5f5f5;
}

.day.today {
	background-color: #409eff;
	color: #fff;
	font-weight: bold;
}

.day.otherMonth {
	color: #ccc;
}
</style>
