namespace Squiddy.Core.Attributes.Services;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceOrderAttribute : Attribute
{
    public int Order { get; }

    public ServiceOrderAttribute(int order)
    {
        Order = order;
    }
}
