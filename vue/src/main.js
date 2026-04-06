import ElementPlus from "element-plus";
import "element-plus/dist/index.css";
import "element-plus/theme-chalk/display.css";
import scui from "./scui";
import i18n from "./locales";
import router from "./router";
import pinia from "./stores";
import { createApp } from "vue";
import App from "./App.vue";

if (document.getElementById("versionCheck").style.display != "block") {
    const app = createApp(App);
    
    app.use(pinia);
    app.use(router);
    app.use(ElementPlus, { size: "default" });
    app.use(i18n);
    app.use(scui);
    
    //挂载app
    app.mount("#app");
}
