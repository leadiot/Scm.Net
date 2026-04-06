using Com.Scm.Dao;
using Com.Scm.Workflow;
using SqlSugar;

namespace Com.Scm.Sys.Workflow
{
    [SugarTable("scm_flow_auth")]
    public class FlowAuthDao : ScmDataDao
    {
        public long flow_id { get; set; }

        public FlowAuthTypesEnum types { get; set; }

        public long data_id { get; set; }
    }
}
