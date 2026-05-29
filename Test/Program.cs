using Com.Scm.Token;
using Com.Scm.Utils;
using SqlSugar;

public class Program
{
    public static void Main(string[] args)
    {
        var sql = "insert into scm_sys_menu(id,types,client,lang,codec,namec,pid,od,layer,uri,redirect,[view],icon,active,color,visible,enabled,fullpage,keepAlive,row_delete,row_status,create_time,create_user,update_time,update_user,layout,width,height,resizable,center,showInDesktop,showInTaskbar) values (1505751488931172353,1,10,'zh-cn','scm_msg_message','消息管理',1721430726089510912,4,1,'/scm/msg/message',NULL,'scm/msg/message','sc-bell-line',NULL,NULL,1,1,0,1,1,1,1690267745593,1000000000000001001,1715521564733,1000000000000001001,1,0,0,0,0,0,0);";
        var sql2 = SqlSugarUtils.EscapeSql(DbType.MySql, sql);
        Console.WriteLine(sql2);
    }
}
