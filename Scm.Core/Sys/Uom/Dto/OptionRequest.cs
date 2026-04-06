using Com.Scm.Enums;

namespace Com.Scm.Sys.Uom.Dto
{
    public class OptionRequest
    {
        public ScmUomTypesEnum types { get; set; }
        public ScmUomModesEnum modes { get; set; }
        public ScmUomKindsEnum kinds { get; set; }
        public string lang { get; set; }
    }
}
