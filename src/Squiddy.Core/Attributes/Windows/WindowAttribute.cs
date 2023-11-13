namespace Squiddy.Core.Attributes.Windows;

[AttributeUsage(AttributeTargets.Class)]
public class WindowAttribute : Attribute
{
    public string Title { get; set; }

    public WindowAttribute(string title)
    {
        Title = title;
    }

}
