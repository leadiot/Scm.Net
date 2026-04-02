const tool = {};

tool.trim = function (x) {
	if (!x) {
		return x;
	}
	return x.toString().replace(/^\s+|\s+$/gm, "");
};

tool.setCache = function (key, obj) {
	tool.data.set(key, obj);
	tool.cookie.set(key, obj);
	tool.session.set(key, obj);
	tool.local.set(key, obj);
};

tool.getCache = function (key, obj) {
	var tmp = tool.data.get(key);
	if (tmp) {
		return tmp;
	}
	tmp = tool.cookie.get(key);
	if (tmp) {
		return tmp;
	}
	tmp = tool.session.get(key);
	if (tmp) {
		return tmp;
	}
	tmp = tool.local.get(key);
	if (tmp) {
		return tmp;
	}

	return obj;
};

tool.removeCache = function (key) {
	tool.data.remove(key);
	tool.cookie.remove(key);
	tool.session.remove(key);
	tool.local.remove(key);
};

tool._cache = [];

/** 临时缓存 */
tool.data = {
	set(key, obj) {
		var _set = JSON.stringify(obj);
		tool._cache[key] = _set;
	},
	get(key, def) {
		var data = tool._cache[key];
		if (!data) {
			return def;
		}

		try {
			data = JSON.parse(data);
		} catch (err) {
			return def;
		}
		return data;
	},
	remove(key) {
		tool._cache = tool._cache.filter((tmp) => {
			return tmp != key;
		});
	},
	clear() {
		tool._cache = [];
	},
};

/* localStorage */
tool.local = {
	set(key, obj) {
		var _set = JSON.stringify(obj);
		return localStorage.setItem(key, _set);
	},
	get(key, def) {
		var data = localStorage.getItem(key);
		if (!data) {
			return def;
		}

		try {
			data = JSON.parse(data);
		} catch (err) {
			return def;
		}
		return data;
	},
	remove(key) {
		return localStorage.removeItem(key);
	},
	clear() {
		return localStorage.clear();
	},
};

/*sessionStorage*/
tool.session = {
	set(key, obj) {
		var _set = JSON.stringify(obj);
		return sessionStorage.setItem(key, _set);
	},
	get(key, def) {
		var data = sessionStorage.getItem(key);
		if (!data) {
			return def;
		}

		try {
			data = JSON.parse(data);
		} catch (err) {
			return def;
		}
		return data;
	},
	remove(key) {
		return sessionStorage.removeItem(key);
	},
	clear() {
		return sessionStorage.clear();
	},
};

/*cookie*/
tool.cookie = {
	set(key, value, config = {}) {
		var cfg = {
			expires: null,
			path: null,
			domain: null,
			secure: false,
			httpOnly: false,
			...config,
		};
		if (typeof value != "string") {
			value = JSON.stringify(value);
		}
		var cookieStr = `${key}=${encodeURIComponent(value)}`;
		if (cfg.expires) {
			var exp = new Date();
			exp.setTime(exp.getTime() + parseInt(cfg.expires || 1) * 1000);
			cookieStr += `;expires=${exp.toGMTString()}`;
		}
		if (cfg.path) {
			cookieStr += `;path=${cfg.path}`;
		}
		if (cfg.domain) {
			cookieStr += `;domain=${cfg.domain}`;
		}
		cookieStr += ";samesite=Lax";
		document.cookie = cookieStr;
	},
	get(key, def) {
		var arr = document.cookie.match(
			new RegExp("(^| )" + key + "=([^;]*)(;|$)")
		);
		if (arr != null) {
			return decodeURIComponent(arr[2]);
		} else {
			return def;
		}
	},
	remove(key) {
		var exp = new Date();
		exp.setTime(exp.getTime() - 1);
		document.cookie = `${key}=;expires=${exp.toGMTString()};SameSite=Lax;`;
	},
};

/* Fullscreen */
tool.screen = function (element) {
	var isFull = !!(
		document.webkitIsFullScreen ||
		document.mozFullScreen ||
		document.msFullscreenElement ||
		document.fullscreenElement
	);
	if (isFull) {
		if (document.exitFullscreen) {
			document.exitFullscreen();
		} else if (document.msExitFullscreen) {
			document.msExitFullscreen();
		} else if (document.mozCancelFullScreen) {
			document.mozCancelFullScreen();
		} else if (document.webkitExitFullscreen) {
			document.webkitExitFullscreen();
		}
	} else {
		if (element.requestFullscreen) {
			element.requestFullscreen();
		} else if (element.msRequestFullscreen) {
			element.msRequestFullscreen();
		} else if (element.mozRequestFullScreen) {
			element.mozRequestFullScreen();
		} else if (element.webkitRequestFullscreen) {
			element.webkitRequestFullscreen();
		}
	}
};

/**
 * 进入全页面显示
 */
tool.showFullPage = function (element) {
	var style = element.style;
	if (!style) {
		return;
	}

	tool.saveOld(style, "position", "fixed");
	tool.saveOld(style, "top", "0px");
	tool.saveOld(style, "left", "0px");
	tool.saveOld(style, "width", null);
	tool.saveOld(style, "height", null);
	tool.saveOld(style, "right", "0px");
	tool.saveOld(style, "bottom", "0px");
};
/**
 * 退出全页面显示
 */
tool.exitFullPage = function (element) {
	var style = element.style;
	if (!style) {
		return;
	}
	tool.readOld(style, "position");
	tool.readOld(style, "top");
	tool.readOld(style, "left");
	tool.readOld(style, "width");
	tool.readOld(style, "height");
	tool.readOld(style, "right");
	tool.readOld(style, "bottom");
};

tool.saveOld = function (style, attr, val) {
	style["old" + attr] = style[attr];
	style[attr] = val;
};
tool.readOld = function (style, attr) {
	style[attr] = style["old" + attr];
};

/* 复制对象 */
tool.objCopy = function (obj) {
	return JSON.parse(JSON.stringify(obj));
};

tool.getDate = function (date) {
	if (!date) {
		date = new Date();
	}
	var y = date.getFullYear();
	var m = date.getMonth() + 1;
	var d = date.getDate();
	return y + "-" + tool.lpad(m, 2, "0") + "-" + tool.lpad(d, 2, "0");
};

tool.getTime = function (date) {
	if (!date) {
		date = new Date();
	}
	var h = date.getHours();
	var m = date.getMinutes();
	var s = date.getSeconds();
	return (
		tool.lpad(h, 2, "0") +
		":" +
		tool.lpad(m, 2, "0") +
		":" +
		tool.lpad(s, 2, "0")
	);
};

tool.format = function (str, len, pad) {
	pad = pad || "*";
	if (!str) {
		return pad.repeat(len);
	}

	const t = str.length;
	if (t > len) {
		return str.substring(0, len);
	}
	if (t < len) {
		return str.padEnd(len, pad);
	}

	return str;
};

tool.dateTimeFormat = function (time) {
	if (!time) {
		return "";
	}
	time = Number(time);
	if (time < 1) {
		return "";
	}

	var date = new Date(time);
	var year = date.getFullYear();
	var month = date.getMonth() + 1;
	var day = date.getDate();
	var hour = date.getHours();
	var minute = date.getMinutes();
	var second = date.getSeconds();
	return `${tool.lpad(year, 4)}-${tool.lpad(month)}-${tool.lpad(
		day
	)} ${tool.lpad(hour)}:${tool.lpad(minute)}:${tool.lpad(second)}`;
};

tool.lpad = function (val, total = 2, str = "0") {
	return val.toString().padStart(total, str);
};

tool.rpad = function (val, total = 2, str = "0") {
	return val.toString().padEnd(total, str);
};

/* 日期格式化
 * yyyy:年
 * MM:月
 * dd:日
 * hh:12制小时
 * HH:24制小时
 * mm:分
 * ss:秒
 * q:季度
 * S:毫秒
 * E:星期
 */
tool.dateFormat = function (time, fmt = "yyyy-MM-dd hh:mm:ss") {
	var date = new Date(time);
	var y = date.getFullYear();
	var M = date.getMonth() + 1;
	var d = date.getDate();
	var h24 = date.getHours();
	var h12 = h24 % 12;
	var m = date.getMinutes();
	var s = date.getSeconds();
	var q = Math.floor((date.getMonth() + 3) / 3);
	var S = date.getMilliseconds();
	var E = date.getDay();
	var o = {
		"M+": M, //月份
		"d+": d, //日
		"h+": h12,
		"H+": h24, //小时
		"m+": m, //分
		"s+": s, //秒
		"q+": q, //季度
		S: S, //毫秒
	};
	const week = [
		"\u65e5",
		"\u4e00",
		"\u4e8c",
		"\u4e09",
		"\u56db",
		"\u4e94",
		"\u516d",
	];
	if (/(y+)/.test(fmt)) {
		fmt = fmt.replace(RegExp.$1, (y + "").substr(4 - RegExp.$1.length));
	}
	if (/(E+)/.test(fmt)) {
		fmt = fmt.replace(
			RegExp.$1,
			(RegExp.$1.length > 1
				? RegExp.$1.length > 2
					? "\u661f\u671f"
					: "\u5468"
				: "") + week[E]
		);
	}
	for (var k in o) {
		if (new RegExp("(" + k + ")").test(fmt)) {
			fmt = fmt.replace(
				RegExp.$1,
				RegExp.$1.length == 1
					? o[k]
					: ("00" + o[k]).substr(("" + o[k]).length)
			);
		}
	}
	return fmt;
};

/* 千分符 */
tool.groupSeparator = function (num) {
	num = num + "";
	if (!num.includes(".")) {
		num += ".";
	}
	return num
		.replace(/(\d)(?=(\d{3})+\.)/g, function ($0, $1) {
			return $1 + ",";
		})
		.replace(/\.$/, "");
};

tool.changeTree = function (data, pid) {
	if (!pid) {
		pid = '0';
	}
	if (data.length > 0) {
		data.forEach((item) => {
			const parentId = item.parentId;
			if (parentId) {
				data.forEach((ele) => {
					if (ele.id === parentId) {
						let childArray = ele.children;
						if (!childArray) {
							childArray = [];
						}

						childArray.push(item);
						ele.children = childArray;
					}
				});
			}
		});
	}
	return data.filter((item) => item.parentId == pid);
};

tool.uuid = function (length = 32) {
	const num = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	let str = "";
	for (let i = 0; i < length; i++) {
		str += num.charAt(Math.floor(Math.random() * num.length));
	}
	return str;
};

tool.objKeySort = function (arys) {
	var newkey = Object.keys(arys).sort();
	var newObj = {}; //创建一个新的对象，用于存放排好序的键值对
	for (var i = 0; i < newkey.length; i++) {
		//遍历newkey数组
		newObj[newkey[i]] = arys[newkey[i]];
		//向新创建的对象中按照排好的顺序依次增加键值对
	}
	let resStr = "";
	for (const key in newObj) {
		if (newObj[key]) {
			resStr += key + newObj[key];
		}
	}

	return resStr;
};
tool.stringToByte = function (str) {
	var len, c;
	len = str.length;
	var bytes = [];
	for (var i = 0; i < len; i++) {
		c = str.charCodeAt(i);
		if (c >= 0x010000 && c <= 0x10ffff) {
			bytes.push(((c >> 18) & 0x07) | 0xf0);
			bytes.push(((c >> 12) & 0x3f) | 0x80);
			bytes.push(((c >> 6) & 0x3f) | 0x80);
			bytes.push((c & 0x3f) | 0x80);
		} else if (c >= 0x000800 && c <= 0x00ffff) {
			bytes.push(((c >> 12) & 0x0f) | 0xe0);
			bytes.push(((c >> 6) & 0x3f) | 0x80);
			bytes.push((c & 0x3f) | 0x80);
		} else if (c >= 0x000080 && c <= 0x0007ff) {
			bytes.push(((c >> 6) & 0x1f) | 0xc0);
			bytes.push((c & 0x3f) | 0x80);
		} else {
			bytes.push(c & 0xff);
		}
	}
	return new Int8Array(bytes);
};

/* 计算文件大小 */
tool.fileSizeFormat = function (limit) {
	if (!limit) {
		return "";
	}

	var num = Number(limit);
	if (!isNaN(num)) {
		return "";
	}

	var size = "";
	if (num < 0.1 * 1024) {
		//小于0.1KB，则转化成B
		size = num.toFixed(2) + "B";
	} else if (num < 0.1 * 1024 * 1024) {
		//小于0.1MB，则转化成KB
		size = (num / 1024).toFixed(2) + "KB";
	} else if (num < 0.1 * 1024 * 1024 * 1024) {
		//小于0.1GB，则转化成MB
		size = (num / (1024 * 1024)).toFixed(2) + "MB";
	} else {
		//其他转化成GB
		size = (num / (1024 * 1024 * 1024)).toFixed(2) + "GB";
	}

	var index = size.indexOf("."); //获取小数点处的索引
	var dou = size.substring(index + 1, index + 3); //获取小数点后两位的值
	if (dou == "00") {
		//判断后两位是否为00，如果是则删除00
		return size.substring(0, index) + size.substring(index + 3, index + 5);
	}
	return size;
};

tool.randomNumber = function (length = 6) {
	const characters = "0123456789";
	const charactersLength = characters.length;

	let result = "";
	for (let i = 0; i < length; i++) {
		result += characters.charAt(Math.floor(Math.random() * charactersLength));
	}
	return result;
};

tool.randomString = function (length, upper = false) {
	let result = "";
	const characters = upper
		? "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
		: "0123456789abcdefghijklmnopqrstuvwxyz";
	const charactersLength = characters.length;

	for (let i = 0; i < length; i++) {
		result += characters.charAt(Math.floor(Math.random() * charactersLength));
	}

	return result;
};

tool.getBrowerInfo = function () {
	var userAgent = window.navigator.userAgent;
	var browerInfo = {
		type: "unknown",
		version: "unknown",
		userAgent: userAgent,
	};
	if (document.documentMode) {
		browerInfo.type = "IE";
		browerInfo.version = document.documentMode + "";
	} else if (userAgent.indexOf("Firefox") > -1) {
		browerInfo.type = "Firefox";
		browerInfo.version = userAgent.match(/Firefox\/([\d.]+)/)[1];
	} else if (userAgent.indexOf("Opera") > -1) {
		browerInfo.type = "Opera";
		browerInfo.version = userAgent.match(/Opera\/([\d.]+)/)[1];
	} else if (userAgent.indexOf("Edg") > -1) {
		browerInfo.type = "Edg";
		browerInfo.version = userAgent.match(/Edg\/([\d.]+)/)[1];
	} else if (userAgent.indexOf("Chrome") > -1) {
		browerInfo.type = "Chrome";
		browerInfo.version = userAgent.match(/Chrome\/([\d.]+)/)[1];
	} else if (userAgent.indexOf("Safari") > -1) {
		browerInfo.type = "Safari";
		browerInfo.version = userAgent.match(/Safari\/([\d.]+)/)[1];
	}
	return browerInfo;
};

tool.isSatisfyBrower = function () {
	var minVer = {
		Chrome: 71,
		Firefox: 65,
		Safari: 12,
		Edg: 97,
		IE: 999,
	};
	var browerInfo = tool.getBrowerInfo();
	var materVer = browerInfo.version.split(".")[0];
	return materVer >= minVer[browerInfo.type];
};

tool.isMobile = function () {
	//var userAgent = window.navigator.userAgent;
	return !!navigator.userAgent.match(/(iPhone|iPod|Android|ios)/i);
};

/*
 * 显示桌面通知
 * @param {string} title - 消息标题
 * @param {object} options - 消息配置
 * @param {number} duration - 消息时长
 * @param {function} onclick
 * @param {function} onshow
 * @param {function} onclose
 * @param {function} onerror
 * @return {object}
 */
tool.showNotify = async function (data) {
	if (!data) {
		return;
	}

	// 构造实例
	function notify(data) {
		var title = data.title || "";
		var duration = data.duration || 5000;
		var options = data.options || {};
		options.requireInteraction = true;
		let notification = new Notification(title, options);
		setTimeout(() => notification.close(), duration);
		// 绑定事件
		let methods = ["onclick", "onshow", "onclose", "onerror"];
		for (let i = 0; i < methods.length; i++) {
			let method = methods[i];
			if (typeof options[method] === "function") {
				notification[method] = options[method];
			}
		}
		return notification;
	}

	// 一些判断
	if (window.Notification) {
		if (Notification.permission === "granted") {
			return notify(data);
		} else if (Notification.permission === "default") {
			let [err, status] = await Notification.requestPermission()
				.then((status) => [null, status])
				.catch((err) => [err, null]);
			return err || status === "denied" ? Promise.reject() : notify(data);
		}
	}
	return Promise.reject();
};

/**
 * 显示滚动标题
 * @param {object} data 消息内容
 * @returns void
 */
tool.showTitle = function (data) {
	if (!data) {
		return;
	}

	var title = data.title || "";
	function notify() {
		var arr = title.split("");
		arr.push(arr.shift());
		title = arr.join("");
		document.title = title;
	}

	// 滚动标题
	setInterval(notify, 300);
};

export default tool;
