【项目说明】
模块核心服务逻辑。

【注意事项】
1、所有继承自ApiService或AppService的类，系统默认为采用依赖注入模式；

【项目依赖】
1、Scm.Common：框架提供的一些公共基础方法；
2、Scm.Common.Dto：Dto基类相关；
3、Scm.Common.Text：字符相关工具；
4、Scm.Dao：Dao基类相关；
5、Scm.Dsa.Dba.Sugar：数据库切面服务；
6、Scm.Server：框架提供的服务相关方法；
7、Scm.Server.Api：框架提供的动态接口类库，用于提供Service的接口转换；
8、Scm.Server.Dao：服务端Dao基类相关；
9、SqlSuagerCore：数据库ORM框架；
10、MiniExcel：Excel文件快速导入导出辅助类。