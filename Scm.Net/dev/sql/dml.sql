/* Ver:1 */
ALTER TABLE scm_ur_terminal ADD icon varchar(32) NULL;
ALTER TABLE scm_ur_terminal ADD online TINYINT NULL;
ALTER TABLE scm_ur_terminal ADD remark varchar(256) NULL;

update scm_ur_terminal set icon='ms-laptop' where types=10;
update scm_ur_terminal set icon='ms-computer' where types=20;
update scm_ur_terminal set icon='ms-smartphone' where types=30;
update scm_ur_terminal set icon='ms-smartphone' where types=40;
update scm_ur_terminal set icon='ms-tablet' where types=50;

CREATE TABLE [scm_gtd_header] (
	[id] bigint NOT NULL PRIMARY KEY, 
	[user_id] bigint, 
	[cat_id] bigint, 
	[title] varchar(256), 
	[remark] varchar(1024), 
	[urgent] tinyint, 
	[important] tinyint, 
	[priority] tinyint NOT NULL DEFAULT 0, 
	[remind] tinyint NOT NULL DEFAULT 0, 
	[cron] varchar(128), 
	[notice] tinyint NOT NULL DEFAULT 0, 
	[last_time] bigint, 
	[next_time] bigint, 
	[handle] tinyint NOT NULL DEFAULT 0, 
	[row_status] tinyint, 
	[update_time] bigint, 
	[update_user] bigint, 
	[create_time] bigint, 
	[create_user] bigint
);

CREATE INDEX [IDX_scm_gtd_header_user_id]
	ON [scm_gtd_header] ([user_id]);

ALTER TABLE [scm_gtd_header] ADD row_delete TINYINT NOT NULL DEFAULT 0;

update scm_sys_menu set codec='console', uri='/console' where id='1000000000000001100';