namespace Com.Scm.Filters;

/// <summary>
/// 不收集日志
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
public class NoAuditLogAttribute : Attribute
{

}