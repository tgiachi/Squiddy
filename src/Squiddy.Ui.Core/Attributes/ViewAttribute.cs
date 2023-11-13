namespace Squiddy.Ui.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ViewAttribute : Attribute
{
    public Type ViewType { get; }

    public ViewAttribute(Type viewType)
    {
        ViewType = viewType;
    }

}
