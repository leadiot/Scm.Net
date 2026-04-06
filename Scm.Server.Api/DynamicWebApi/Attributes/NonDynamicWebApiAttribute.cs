namespace Com.Scm.DynamicWebApi.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class NonDynamicWebApiAttribute:Attribute
    {
        
    }
}