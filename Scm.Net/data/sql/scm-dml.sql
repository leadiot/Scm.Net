/* Ver:1 */

/* Ver:2 */

/* Ver:3 */
update scm_sys_menu set layout='console' where uri!='/desktop';

/* Ver:4 */
delete from scm_sys_dic_detail where dic_header_id='1633985708890918912';
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('1633985824104255488','1633985708890918912','0','默认','0','1','0','1',NULL,'1','2','20230310175449','1260380358965334016','20230310081913','1260380358965334016');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('1633986013477081088','1633985708890918912','10','网页端','10','1','0','1',NULL,'1','1','20230310175450','1260380358965334016','20230310081958','1260380358965334016');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('1633986472875003904','1633985708890918912','11','Chrome','11','1','0','11',NULL,'1','2','1776152512306','1000000000000001001','20230310082147','1260380358965334016');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('1633986622896869376','1633985708890918912','12','Firefox','12','1','0','12',NULL,'1','2','1776152527745','1000000000000001001','20230310082223','1260380358965334016');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('1633986692694282240','1633985708890918912','13','Edge','13','1','0','13',NULL,'1','2','1776152540191','1000000000000001001','20230310082240','1260380358965334016');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('1633986761938046976','1633985708890918912','20','Windows桌面','20','1','0','20',NULL,'1','1','1776152569609','1000000000000001001','20230310082256','1260380358965334016');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('2043958205998043136','1633985708890918912','30','Linux桌面','30','1','0','30',NULL,'0','1','1776152585874','1000000000000001001','1776152585874','1000000000000001001');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('2043958263665528832','1633985708890918912','40','MacOS桌面','40','1','0','40',NULL,'0','1','1776152599623','1000000000000001001','1776152599623','1000000000000001001');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('2043958350928023552','1633985708890918912','50','安卓移动','50','1','0','50',NULL,'0','1','1776152620428','1000000000000001001','1776152620428','1000000000000001001');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('2043958432888918016','1633985708890918912','60','苹果移动','60','1','0','60',NULL,'0','1','1776152639969','1000000000000001001','1776152639969','1000000000000001001');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('2043958541739495424','1633985708890918912','70','鸿蒙移动','70','1','0','70',NULL,'0','1','1776152665921','1000000000000001001','1776152665921','1000000000000001001');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('2043958592549294080','1633985708890918912','80','小程序','80','1','0','80',NULL,'0','1','1776152678035','1000000000000001001','1776152678035','1000000000000001001');
insert into scm_sys_dic_detail (id, dic_header_id, codec, namec, value, tag, cat, od, remark, row_delete, row_status, update_time, update_user, create_time, create_user) values('2043958624723800064','1633985708890918912','90','其它','90','1','0','90',NULL,'0','1','1776152685706','1000000000000001001','1776152685706','1000000000000001001');

update scm_sys_menu set view='home/console' where id='1000000000000001101';
update scm_sys_menu set view='home/console/favorites' where id='1000000000000001200';
update scm_sys_menu set view='home/console/profiles' where id='1000000000000001300';
update scm_sys_menu set view='home/console/unitinfo' where id='1000000000000001310';
update scm_sys_menu set view='home/console/userinfo' where id='1000000000000001320';
update scm_sys_menu set view='home/console/oauth' where id='1000000000000001330';
update scm_sys_menu set view='home/console/otp' where id='1000000000000001340';
update scm_sys_menu set od = od + 1 where od > 2;
INSERT INTO [scm_sys_menu] ([id],[client],[types],[lang],[codec],[namec],[icon],[pid],[uri],[layer],[od],[visible],[enabled],[fullpage],[keepAlive],[active],[redirect],[view],[color],[layout],[row_delete],[row_status],[create_time],[create_user],[update_time],[update_user]) VALUES (1000000000000001103,10,1,'zh-cn','monitor','大屏幕','sc-menu-line',1000000000000001000,'/monitor',2,3,1,1,0,1,NULL,NULL,'home/monitor',NULL,'monitor',1,1,1778556492751,1000000000000000001,1778556494591,1000000000000000001);
