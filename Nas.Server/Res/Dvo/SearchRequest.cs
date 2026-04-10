using Com.Scm.Enums;

namespace Com.Scm.Nas.Res.Dvo
{
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 0：根据目录查询，
        /// 1：根据分类查询
        /// </summary>
        public SearchOption opt { get; set; }

        public long dir_id { get; set; }

        public ScmFileKindEnum kind { get; set; }
    }

    public enum SearchOption
    {
        None,
        ByDir,
        ByKind
    }
}
