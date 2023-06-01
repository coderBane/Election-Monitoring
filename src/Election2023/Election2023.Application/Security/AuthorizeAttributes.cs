namespace Election2023.Application.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute(){ }

    public string Roles { get; set; } = string.Empty;

    public string Policy { get; set; } = string.Empty;
}