
//手机号码
export function verifyPhone(rule, value, callback) {
	let reg = /^[1][3, 4, 5, 6, 7, 8, 9][0-9]{9}$/
	if(!reg.test(value)){
		return callback(new Error('请输入正确的手机号码'))
	}
	callback()
}

//车牌号码
export function verifyCars(rule, value, callback) {
	let reg = /^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-HJ-NP-Z][A-HJ-NP-Z0-9]{4,5}[A-HJ-NP-Z0-9挂学警港澳]$/
	if(!reg.test(value)){
		return callback(new Error('请输入正确的车牌号码'))
	}
	callback()
}

//电子邮件
export function verifyEmail(rule, value, callback) {
	let reg = /^([0-9a-zA-Z_.-])+@([0-9a-zA-Z_.-])+\.([a-zA-Z]+)$/
	if(!reg.test(value)){
		return callback(new Error('请输入正确的电子邮件'))
	}
	callback()
}
