using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Service
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class ScmSecService : ISecService
    {
        public SecConfig Get()
        {
            return new SecConfig();
        }
    }
}
