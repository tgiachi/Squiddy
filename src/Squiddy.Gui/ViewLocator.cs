using System;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Squiddy.Ui.Core.Attributes;

namespace Squiddy.Gui;

public class ViewLocator : IDataTemplate
{
    public Control Build(object data)
    {
        var app = (Squiddy.Gui.App)App.Current;

        if (data.GetType().GetCustomAttribute<ViewAttribute>() != null)
        {
            var attribute = data.GetType().GetCustomAttribute<ViewAttribute>();
            var viewModel = app.Container.GetRequiredService(data.GetType());
            var view = app.Container.GetRequiredService(attribute.ViewType) as UserControl;


            view.DataContext = viewModel;
            viewModel.GetType().GetMethod("Initialize")?.Invoke(viewModel, null);
            return view;
        }


        return new TextBlock { Text = "Not Found: " + data.GetType().Name };
    }

    public bool Match(object data) => data is ReactiveObject;
}
