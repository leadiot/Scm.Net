/** Ver:5 */
 alter table scm_sys_menu add column layout1 intger;
 update scm_sys_menu set layout1 = 1 where layout='console';
 update scm_sys_menu set layout1 = 2 where layout='desktop';
 update scm_sys_menu set layout1 = 3 where layout='monitor';
 alter table scm_sys_menu drop column layout;
 ALTER TABLE scm_sys_menu rename COLUMN layout1 to layout;