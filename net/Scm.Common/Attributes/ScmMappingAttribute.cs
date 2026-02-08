using System;

namespace Com.Scm.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScmMappingAttribute : Attribute
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public ScmMappingAttribute(string name, object value = null)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
