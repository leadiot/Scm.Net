import config from "./config";
import api from "./api";
import crypto from "./utils/crypto";
import tool from "./utils/tool";
import http from "./utils/request";
import scm from "./utils/scm";
import { permission, rolePermission } from "./utils/permission";

import scCat from "./components/scCat";
import scComment from "./components/scComment";
import scDialog from "./components/scDialog";
import scFileImport from "./components/scFileImport";
import scFilterBar from "./components/scFilterBar";
import scDynamicForm from "./components/scDynamicForm";
import scDynamicTable from "./components/scDynamicTable";
import scIcon from "./components/scIcon";
import scList from "./components/scList";
import scPageHeader from "./components/scPageHeader";
import scPanel from "./components/scPanel";
import scQrCode from "./components/scQrCode";
import scSearch from "./components/scSearch";
import scSelect from "./components/scSelect";
import scSelectFilter from "./components/scSelectFilter";
import scSummary from "./components/scSummary";
import scTable from "./components/scTable";
import scTableSelect from "./components/scTableSelect";
import scTitle from "./components/scTitle";
import scUpload from "./components/scUpload";
import scUploadFile from "./components/scUpload/file";
import scUploadMultiple from "./components/scUpload/multiple";
import scWaterfall from "./components/scWaterfall";
import scWaterMark from "./components/scWaterMark";

import scStatusIndicator from "./components/scMini/scStatusIndicator";
import scTrend from "./components/scTrend";

import auth from "./directives/auth";
import role from "./directives/role";
import time from "./directives/time";
import copy from "./directives/copy";
import errorHandler from "./utils/errorHandler";

import * as elIcons from "@element-plus/icons-vue";
// import * as scIcons from "./assets/icons";

export default {
	install(app) {
		//挂载全局对象
		app.config.globalProperties.$CONFIG = config;
		app.config.globalProperties.$CRYPTO = crypto;
		app.config.globalProperties.$TOOL = tool;
		app.config.globalProperties.$HTTP = http;
		app.config.globalProperties.$API = api;
		app.config.globalProperties.$AUTH = permission;
		app.config.globalProperties.$ROLE = rolePermission;
		app.config.globalProperties.$SCM = scm;

		//注册全局组件
		app.component("scComment", scComment);
		app.component("scCat", scCat);
		app.component("scIcon", scIcon);
		app.component("scTable", scTable);
		app.component("scSearch", scSearch);
		app.component("scFilterBar", scFilterBar);
		app.component("scUpload", scUpload);
		app.component("scUploadFile", scUploadFile);
		app.component("scUploadMultiple", scUploadMultiple);
		app.component("scDynamicForm", scDynamicForm);
		app.component("scDynamicTable", scDynamicTable);
		app.component("scTableSelect", scTableSelect);
		app.component("scPageHeader", scPageHeader);
		app.component("scSelect", scSelect);
		app.component("scDialog", scDialog);
		app.component("scTitle", scTitle);
		app.component("scWaterfall", scWaterfall);
		app.component("scWaterMark", scWaterMark);
		app.component("scQrCode", scQrCode);
		app.component("scStatusIndicator", scStatusIndicator);
		app.component("scTrend", scTrend);
		app.component("scSelectFilter", scSelectFilter);
		app.component("scFileImport", scFileImport);
		app.component("scList", scList);
		app.component("scPanel", scPanel);
		app.component("scSummary", scSummary);

		//注册全局指令
		app.directive("auth", auth);
		app.directive("role", role);
		app.directive("time", time);
		app.directive("copy", copy);

		//统一注册el-icon图标
		for (let icon in elIcons) {
			app.component(`ElIcon${icon}`, elIcons[icon]);
		}
		//统一注册sc-icon图标
		// for (let icon in scIcons) {
		// 	app.component(`ScIcon${icon}`, scIcons[icon]);
		// }

		//关闭async-validator全局控制台警告
		window.ASYNC_VALIDATOR_NO_WARNING = 1;

		//全局代码错误捕捉
		app.config.errorHandler = errorHandler;
	},
};
