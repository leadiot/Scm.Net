using Com.Scm.Response;
using System;
using System.Collections.Generic;

namespace Com.Scm
{
    [Obsolete]
    public class ScmSearchResponse<T> : ScmApiResponse
    {
        /// <summary>
        /// 数据集
        /// </summary>
        public List<T> Items { get; set; }

        public void SetSuccess(List<T> items)
        {
            _rr = true;
            _rc = 0;
            Items = items;
        }
    }
}
