using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Service
{
    [ApiExplorerSettings(GroupName = "scm")]
    public class ScmSecService : ISecService
    {
        public SecConfig Get()
        {
            return new SecConfig();
        }
    }
}
