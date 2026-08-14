namespace Com.Scm
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ScmTableAttribute : Attribute
    {
        public bool IsIgnore { get; set; } = true;

        public string Description { get; set; }

        public ScmTableAttribute() { }

        public ScmTableAttribute(bool isIgnore)
        {
            IsIgnore = isIgnore;
        }

        public ScmTableAttribute(string description)
        {
            Description = description;
        }
    }
}
