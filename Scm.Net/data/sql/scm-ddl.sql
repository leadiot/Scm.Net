/** Ver:5 */
 ALTER TABLE [scm_sys_menu] ADD COLUMN [layout1] INT;
 UPDATE [scm_sys_menu] SET [layout1]=1 WHERE [layout]='console';
 UPDATE [scm_sys_menu] SET [layout1]=2 WHERE [layout]='desktop';
 UPDATE [scm_sys_menu] SET [layout1]=3 WHERE [layout]='monitor';
 ALTER TABLE [scm_sys_menu] DROP COLUMN [layout];
 ALTER TABLE [scm_sys_menu] RENAME COLUMN [layout1] TO [layout];