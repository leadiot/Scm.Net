import config from "@/config";
import http from "@/utils/request";
import tool from "@/utils/tool";

const scm = {};

/**默认头像 */
scm.DEF_AVATAR = "0.png";

scm.REGEX_ID = /^[1-9]\d{15,18}$/;
scm.REGEX_INT = /^[1-9]\d*$/;
scm.REGEX_CODEC = /^[-_0-9a-zA-Z]{4,32}$/;
scm.REGEX_NAMEC = /^\S{4,64}$/;
scm.REGEX_NAMEF = /^\S{4,128}$/;
scm.REGEX_NUMBER = /^\d+$/;

scm.REGEX_NAMEC_NAS = /^\S{1,250}$/;

/**系统ID */
scm.SYS_ID = "1000000000000000001";
/**默认ID */
scm.DEF_ID = "0";
/**默认状态 */
scm.DEF_STATUS = 1;

/**单选ID */
scm.ID_ONE = "0";
/**单选ID整数 */
scm.ID_ONE_INT = 0;
/**所有ID */
scm.ID_ALL = "1000000000000000001";
/**所有ID整数 */
scm.ID_ALL_INT = 0;

/**所有选项 */
scm.OPTION_ALL = { label: "所有", id: scm.ID_ALL, value: scm.ID_ALL };
/**所有选项（整数） */
scm.OPTION_ALL_INT = { label: "所有", id: scm.ID_ALL, value: scm.ID_ALL_INT };
/**请选择选项 */
scm.OPTION_ONE = { label: "请选择", id: scm.ID_ONE, value: scm.ID_ONE };
/**请选择选项（整数） */
scm.OPTION_ONE_INT = { label: "请选择", id: scm.ID_ONE, value: scm.ID_ONE_INT };

scm.cache = [];

scm.is_valid_id = function (text) {
	return scm.REGEX_ID.test(text);
};

scm.is_valid_int = function (text) {
	return scm.REGEX_INT.test(text);
};

scm.is_valid_codec = function (text) {
	return scm.REGEX_CODEC.test(text);
};

scm.is_valid_namec = function (text) {
	return scm.REGEX_NAMEC.test(text);
};

scm.getUrl = function (url) {
	if (!url) {
		return "";
	}
	return config.SERVER_URL + url;
};

scm.getApiUrl = function (url) {
	if (!url) {
		return "";
	}
	return config.API_URL + url;
};

scm.encode_pass = function (pass) {
	var len1 = 4;
	var len2 = 4;
	var len3 = 4;
	var len = pass.length >> 1;

	var idx1 = Math.floor(Math.random() * len);
	var idx2 = Math.floor(Math.random() * len) + idx1;

	var tmp1 = tool.randomString(len1, false);
	var tmp2 = tool.randomString(len2, false);
	var tmp3 = tool.randomString(len3, false);

	var buf = "";
	var num1 = idx1.toString(16);
	var num2 = (len1 + idx2).toString(16);
	buf += (num1.length < 2 ? "0" : "") + num1;
	buf += (num2.length < 2 ? "0" : "") + num2;
	buf += pass.slice(0, idx1);
	buf += tmp1;
	buf += pass.slice(idx1, idx2);
	buf += tmp2;
	buf += pass.slice(idx2);
	buf += tmp3;
	return buf;
};

scm.status_item = async function (dom, http, data, status) {
	if (!data.id) {
		return;
	}

	let param = { ids: [data.id], status: status };
	var res = await http.post(param);
	if (res.code == 200) {
		//dom.$refs.table.refresh();
		//dom.$message.success("操作完成。");
	} else {
		dom.$alert(res.message, "提示", { type: "error" });
	}
};

scm.status_list = function (dom, http, list, status) {
	let text = status == 1 ? "启用" : "禁用";
	dom.$confirm(`确定${text}选中的 ${list.length} 项吗？`, "提示", {
		type: "warning",
		confirmButtonText: "确定",
		cancelButtonText: "取消",
	})
		.then(async () => {
			const loading = dom.$loading();
			let ids = [];
			list.forEach((element) => {
				ids.push(element.id);
			});
			let param = { ids: ids, status: status };
			var res = await http.post(param);
			if (res.code == 200) {
				dom.$refs.table.refresh();
				loading.close();
				dom.$message.success("操作完成。");
			} else {
				dom.$alert(res.message, "提示", { type: "error" });
			}
		})
		.catch(() => { });
};

scm.delete_item = async function (dom, http, data) {
	if (!data.id) {
		return;
	}

	// var res = await http.delete({ 'ids': data.id });
	var res = await http.delete(data.id);
	if (res.code == 200) {
		dom.$refs.table.refresh();
		dom.$message.success("操作完成。");
	} else {
		dom.$alert(res.message, "提示", { type: "error" });
	}
};

scm.delete_list = function (dom, http, list) {
	dom.$confirm(`确定删除选中的 ${list.length} 项吗？`, "提示", {
		type: "warning",
		confirmButtonText: "确定",
		cancelButtonText: "取消",
	})
		.then(async () => {
			var loading = dom.$loading();
			let ids = [];
			list.forEach((element) => {
				ids.push(element.id);
			});
			var res = await http.delete(ids.join(","));
			if (res.code == 200) {
				dom.$refs.table.refresh();
				loading.close();
				dom.$message.success("操作成功。");
			} else {
				dom.$alert(res.message, "提示", { type: "error" });
			}
		})
		.catch(() => { });
};

/**
 * 追加【所有】选项
 * @param {Array} arr
 * @returns
 */
scm.option_all = function (arr) {
	if (arr != null) {
		arr.unshift(scm.OPTION_ALL);
	}
	return arr;
};

/**
 * 追加【请选择】选项
 * @param {Array} arr
 * @returns
 */
scm.option_one = function (arr) {
	if (arr != null) {
		arr.unshift(scm.OPTION_ONE);
	}
	return arr;
};

/**
 * 下拉列表准备
 * @param {*} list
 * @param {*} res
 * @param {*} all
 * @returns
 */
scm.prepare = function (list, res, all) {
	if (list == null) {
		return;
	}

	list.length = 0;
	if (all) {
		scm.option_all(list);
	} else if (all == false) {
		scm.option_one(list);
	}
	if (res.code != 200) {
		return;
	}
	res.data.some((m) => {
		list.push(m);
	});
};

/**
 * 获取字典列表
 * @param {Array} list 输入列表
 * @param {String} key 字典值
 * @param {Boolean} all 默认值类型：true:所有，false:请选择
 * @param {Boolean} useCatch 是否启用缓冲
 * @returns
 */
scm.list_dic = async function (list, key, all, useCatch) {
	if (!list) {
		return;
	}

	list.length = 0;
	if (all) {
		list.push(scm.OPTION_ALL_INT);
	} else if (all == false) {
		list.push(scm.OPTION_ONE_INT);
	}

	var data = useCatch ? scm.cache[key] : null;
	if (data == null) {
		var res = await http.get(`${config.API_URL}/scmdic/option/` + key);
		if (!res || res.code != 200) {
			return;
		}

		data = res.data;
		if (useCatch) {
			scm.cache[key] = data;
		}
	}

	data.some((m) => {
		list.push(m);
	});
};

/**
 * 获取配置
 * @param {String} key 字典值
 * @param {String} def 默认值
 * @param {Boolean} useCatch 是否启用缓冲
 * @returns
 */
scm.list_cfg = async function (key, def, useCatch) {
	if (!key) {
		return def;
	}

	var data = useCatch ? scm.cache[key] : null;
	if (data == null) {
		var res = await http.get(
			`${config.API_URL}/scmsysconfig/config/` + key
		);
		if (!res || res.code != 200) {
			return;
		}

		data = res.data;
		if (!data) {
			return def;
		}
		data = data.value;
		if (useCatch) {
			scm.cache[key] = data;
		}
	}

	return data ?? def;
};

/**
 * 获取数据状态列表
 * @param {*} list
 * @param {*} all
 */
scm.list_status = async function (list, all) {
	scm.list_dic(list, "status", all, true);
};

/**
 * 获取性别列表
 * @param {*} list
 * @param {*} all
 */
scm.list_sex = async function (list, all) {
	scm.list_dic(list, "sex", all, true);
};

/**
 * 获取字典名称
 * @param {Array} list 字典列表
 * @param {String} key 字典键
 * @param {String} def 默认值
 * @returns
 */
scm.get_dic_names = function (list, key, def) {
	if (!list) {
		return def;
	}
	var obj = list.find((item) => {
		return item.value == key;
	});
	return obj ? obj.label : "";
};

/**
 * 获取分类列表
 * @param {*} list
 * @param {*} param
 * @param {*} all
 */
scm.list_cat = async function (list, param, all) {
	var res = await http.get(`${config.API_URL}/scmrescat/option`, param);
	scm.prepare(list, res, all);
};

/**
 * 获取标签列表
 * @param {*} list
 * @param {*} param
 * @param {*} all
 */
scm.list_tag = async function (list, app, all) {
	var res = await http.get(`${config.API_URL}/scmrestag/option/` + app);
	scm.prepare(list, res, all);
};

/**
 * 获取下拉选项
 * @param {*} list
 * @param {*} api
 * @param {*} param
 * @param {*} all
 */
scm.list_option = async function (list, api, param, all) {
	if (!api) {
		return;
	}

	var res = await api.get(param);
	scm.prepare(list, res, all);
};

/**
 * 获取列表名称
 * @param {Array} options 字典列表
 * @param {String} key 字典键
 * @param {String} def 默认值
 * @returns
 */
scm.get_option_names = function (options, key, def) {
	if (!options) {
		return def;
	}
	key = "" + key;
	var obj = options.find((item) => {
		return item.value == key;
	});
	return obj ? obj.label : "";
};

/**
 * 获取应用列表
 * @param {*} list
 * @param {*} types
 * @param {*} all
 */
scm.list_app = async function (list, types, all) {
	var res = await http.get(`${config.API_URL}/scmdevapp/option/` + types);
	scm.prepare(list, res, all);
};

/**
 * 获取区域列表
 * @param {输出目标列表} list
 * @param {上级ID} pid
 * @param {是否添加默认选项} all
 * @returns
 */
scm.list_region = async function (list, pid, all) {
	if (!pid) {
		return;
	}

	var res = await http.get(`${config.API_URL}/scmsysregion/option/` + pid);
	scm.prepare(list, res, all);
};

scm.get_table = async function (key, def) {
	if (!key) {
		return def;
	}
	var res = await http.get(`${config.API_URL}/scmsystable/` + key);
	if (!res || res.code != 200) {
		return def;
	}

	var data = res.data;
	if (!data) {
		return def;
	}
	return scm.make_table(data.details, def);
};
scm.make_table = function (dst, src) {
	if (!dst || dst.length < 1) {
		return src;
	}
	var tmp = [];
	dst.forEach((dstObj) => {
		for (var i = 0; i < src.length; i += 1) {
			var srcObj = src[i];
			if (dstObj.prop == srcObj.prop) {
				tmp.push(Object.assign(srcObj, dstObj));
				break;
			}
		}
	});
	return tmp;
};
scm.save_table = async function (key, cfg) {
	if (!key) {
		return false;
	}
	var res = await http.post(`${config.API_URL}/scmsystable/save`, {
		codec: key,
		details: cfg,
	});
	if (!res || res.code != 200) {
		return false;
	}
	return true;
};

/**
 * 本地临时存储数据
 * @param {*} key
 * @param {*} val
 * @returns
 */
scm.save_cache = function (key, val) {
	var type = typeof val || "string";

	var tmp = val;
	if (type != "number" && type != "string" && type != "boolean") {
		tmp = JSON.stringify(val);
	}

	if (localStorage) {
		localStorage.setItem(key, tmp);
	}

	scm.cache[key] = tmp;
};

/**
 * 读取本地临时数据
 * @param {*} key
 * @param {*} val
 * @returns
 */
scm.read_cache = function (key, val) {
	var tmp = null;
	if (localStorage) {
		tmp = localStorage.getItem(key);
	} else {
		tmp = scm.cache[key];
	}

	return tmp || val;
};

/**
 * 移除指定键值
 * @param {*} key
 * @returns
 */
scm.drop_cache = function (key) {
	if (localStorage) {
		localStorage.removeItem(key);
		return;
	}

	scm.cache = scm.cache.filter((tmp) => {
		return tmp != key;
	});
};

/**
 *
 * @param {*} key
 * @param {*} val
 * @returns
 */
scm.read_json = function (key, val) {
	var tmp = null;
	if (localStorage) {
		tmp = localStorage.getItem(key);
	} else {
		tmp = scm.cache[key];
	}

	return tmp ? JSON.parse(tmp) : val;
};

scm.get_avatar = function (avatar) {
	if (!avatar) {
		avatar = scm.DEF_AVATAR;
	}
	return config.SERVER_URL + "/data/avatar/" + avatar;
};

scm.get_user_avatar = function (user) {
	if (!user) {
		return scm.get_avatar();
	}
	return scm.get_avatar(user.avatar);
};

scm.recursive_menu = function (menuList, pid) {
	if (!pid) {
		pid = "0";
	}

	return scm.recursive_menu2(menuList, pid);
};

scm.get_sub_menu = function (menuList, pid) {
	var list = [];

	for (var i = 0; i < menuList.length; i += 1) {
		var menu = menuList[i];
		if (menu.pid != pid) {
			continue;
		}

		var idx = -1;
		for (var j = 0; j < list.length; j += 1) {
			if (list[j].od > menu.od) {
				idx = j;
				break;
			}
		}
		if (idx > -1) {
			list.splice(idx, 0, menu);
		} else {
			list.push(menu);
		}
	}
	return list;
};

scm.recursive_menu2 = function (menuList, pid) {
	if (!pid) {
		return null;
	}

	var subList = scm.get_sub_menu(menuList, pid);
	if (subList.length < 1) {
		return null;
	}

	var list = [];
	var types = ["", "menu", "iframe", "link", "button", "input"];
	for (var i = 0; i < subList.length; i += 1) {
		var menu = subList[i];

		var children = scm.recursive_menu2(menuList, menu.id);

		var item = {
			id: menu.id,
			path: menu.uri,
			name: menu.codec,
			component: menu.view,
			meta: {
				id: menu.id,
				title: menu.namec,
				icon: menu.icon,
				type: types[menu.types],
				hidden: !menu.visible,
				fullpage: !!menu.fullpage,
				keepAlive: menu.keepAlive,
				affix: menu.codec == "dashboard",
			},
			children: children,
		};
		list.push(item);
	}

	return list;
};

export default scm;
