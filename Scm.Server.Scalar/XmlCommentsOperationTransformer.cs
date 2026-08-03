using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text;
using System.Xml.XPath;

namespace Com.Scm.Scalar
{
    /// <summary>
    /// 读取程序集生成的 XML 文档注释文件，填充接口的摘要（summary）、
    /// 说明（remarks）与参数描述（param）。
    /// .NET 10 原生 OpenAPI 的 XML 注释源生成器只对字面量文档名的 AddOpenApi 调用生效，
    /// 动态注册的多文档场景需要在运行时自行实现。
    /// </summary>
    internal sealed class XmlCommentsOperationTransformer : IOpenApiOperationTransformer
    {
        /// <summary>
        /// XML 注释成员名 → 节点导航器
        /// </summary>
        private readonly Dictionary<string, XPathNavigator> _members;

        public XmlCommentsOperationTransformer(IEnumerable<string> xmlFiles)
        {
            _members = new Dictionary<string, XPathNavigator>(StringComparer.Ordinal);

            foreach (var file in xmlFiles)
            {
                try
                {
                    var doc = new XPathDocument(file);
                    var navigator = doc.CreateNavigator();
                    var nodes = navigator.Select("/doc/members/member");
                    foreach (XPathNavigator node in nodes)
                    {
                        var name = node.GetAttribute("name", string.Empty);
                        if (!string.IsNullOrEmpty(name) && !_members.ContainsKey(name))
                        {
                            _members[name] = node.Clone();
                        }
                    }
                }
                catch
                {
                    // XML 文件损坏时跳过，不影响文档生成
                }
            }
        }

        public Task TransformAsync(
            OpenApiOperation operation,
            OpenApiOperationTransformerContext context,
            CancellationToken cancellationToken)
        {
            var methodInfo = GetMethodInfo(context);
            if (methodInfo == null || methodInfo.DeclaringType == null)
            {
                return Task.CompletedTask;
            }

            if (!_members.TryGetValue(GetMemberName(methodInfo), out var member))
            {
                return Task.CompletedTask;
            }

            var summary = SelectText(member, "summary");
            if (!string.IsNullOrEmpty(summary))
            {
                operation.Summary = summary;
            }

            var remarks = SelectText(member, "remarks");
            if (!string.IsNullOrEmpty(remarks))
            {
                operation.Description = remarks;
            }

            if (operation.Parameters != null)
            {
                foreach (var parameter in operation.Parameters)
                {
                    var description = SelectText(member, $"param[@name='{parameter.Name}']");
                    if (!string.IsNullOrEmpty(description))
                    {
                        parameter.Description = description;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static MethodInfo GetMethodInfo(OpenApiOperationTransformerContext context)
        {
            return (context.Description?.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
        }

        private static string SelectText(XPathNavigator member, string expression)
        {
            var node = member.SelectSingleNode(expression);
            return node?.Value?.Trim();
        }

        /// <summary>
        /// 生成 XML 注释中的成员名（M: 前缀格式）
        /// </summary>
        private static string GetMemberName(MethodInfo method)
        {
            var builder = new StringBuilder("M:");
            builder.Append(method.DeclaringType.FullName);
            builder.Append('.').Append(method.Name);

            var parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                builder.Append('(');
                builder.Append(string.Join(",", parameters.Select(p => GetTypeName(p.ParameterType))));
                builder.Append(')');
            }

            return builder.ToString();
        }

        private static string GetTypeName(Type type)
        {
            if (type.IsGenericParameter)
            {
                return $"`{type.GenericParameterPosition}";
            }

            if (type.IsGenericType)
            {
                var fullName = type.GetGenericTypeDefinition().FullName ?? type.Name;
                var index = fullName.IndexOf('`');
                if (index > 0)
                {
                    fullName = fullName.Substring(0, index);
                }
                return fullName + "{" + string.Join(",", type.GetGenericArguments().Select(GetTypeName)) + "}";
            }

            if (type.IsArray)
            {
                return GetTypeName(type.GetElementType()) + "[" + new string(',', type.GetArrayRank() - 1) + "]";
            }

            if (type.IsByRef)
            {
                return GetTypeName(type.GetElementType()) + "@";
            }

            return type.FullName ?? type.Name;
        }
    }
}
