import config from "@/config";
import tool from "@/utils/tool";
const signalR = require("@microsoft/signalr");

const socket = {};

/**
 * SingalR Hub
 */
socket.open_wss = function (key, fun) {
	var token = tool.data.get("TOKEN");
	var connection = window.scm_connection;
	if (!connection) {
		connection = new signalR.HubConnectionBuilder()
			.withUrl(config.SignalR_URL, {
				accessTokenFactory: () => token,
			})
			.configureLogging(signalR.LogLevel.Error)
			.withAutomaticReconnect()
			.build();
		connection.start().catch((err) => console.error(err));
		window.scm_connection = connection;
	}

	connection.on(key, fun);
};

/**
 * 发送消息
 * @param {*} method
 * @param {*} obj
 * @returns
 */
socket.send_msg = async function (method, obj) {
	var connection = window.scm_connection;
	if (connection) {
		await connection.invoke(method, obj);
		return true;
	}
	return false;
};

export default socket;
