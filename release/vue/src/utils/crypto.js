
/* 常用加解密 */
import CryptoJS from "crypto-js";

export default {
	//MD5加密
	MD5(data) {
		return CryptoJS.MD5(data).toString();
	},
	SHA(data) {
		return CryptoJS.SHA256(data).toString();
	},
	//BASE64加解密
	BASE64: {
		encrypt(data) {
			return CryptoJS.enc.Base64.stringify(CryptoJS.enc.Utf8.parse(data));
		},
		decrypt(cipher) {
			return CryptoJS.enc.Base64.parse(cipher).toString(
				CryptoJS.enc.Utf8
			);
		},
	},
	AES_SECRETKEY: "dd0308a654ea42eab695bf060241b5aa",
	//AES加解密
	AES: {
		encrypt(data, secretKey) {
			const result = CryptoJS.AES.encrypt(
				data,
				CryptoJS.enc.Utf8.parse(secretKey),
				{
					mode: CryptoJS.mode.ECB,
					padding: CryptoJS.pad.Pkcs7,
				}
			);
			return result.toString();
		},
		decrypt(cipher, secretKey) {
			const result = CryptoJS.AES.decrypt(
				cipher,
				CryptoJS.enc.Utf8.parse(secretKey),
				{
					mode: CryptoJS.mode.ECB,
					padding: CryptoJS.pad.Pkcs7,
				}
			);
			return CryptoJS.enc.Utf8.stringify(result);
		},
	},
};
