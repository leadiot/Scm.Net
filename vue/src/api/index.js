/**
 * @description 自动import导入所有 api 模块
 */

const modules = {};

const files = import.meta.glob("./model/**/*.js", { eager: true });

Object.keys(files).forEach((key) => {
	let k = key.replace(/^\.\/model\//, "").replace(/\.js$/, "");
	if (k.indexOf("/") > -1) {
		k = k.split("/").pop();
	}
	modules[k] = files[key].default;
});

export default modules;
