【打开方式】
1、安装Node.js；
2、安装VScode，或Trae；
3、打开文件夹至Scm.Net/Vue目录
4、运行npm update，更新项目依赖环境；
5、运行npm run serve，启动服务；
6、打开浏览器，访问地址：https://localhost:2800；
7、登录用户及口令：admin/123456。

【环境配置】
如果您没有本地服务，请修改.env.development文件，将VUE_APP_API_BASEURL修改为https://api.c-scm.net，可直接使用现有云服务器；
如下为生产演示环境：
VUE_APP_API_BASE = https://api.c-scm.net

【应用配置】
以下配置文件的相关配置均可以修改：
1、.env相关的文件；
2、public/config.js；
3、src/config/index.js；

【待办事项】
1、富文本编辑功能；--已完成 2025-11-17
2、大屏展示功能；
3、短信配置功能；
4、邮件配置功能；