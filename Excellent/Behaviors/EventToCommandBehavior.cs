using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Excellent.Behaviors;

public class EventToCommandBehavior : Behavior<FrameworkElement>
{
    private EventInfo? eventInfo;
    private Delegate? eventHandler;

    public static readonly DependencyProperty EventProperty =
        DependencyProperty.Register(nameof(Event), typeof(string), typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior), new PropertyMetadata(null));

    public string Event
    {
        get => (string)GetValue(EventProperty);
        set => SetValue(EventProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (EventToCommandBehavior)d;
        if (behavior.AssociatedObject != null)
        {
            behavior.AttachHandler((string)e.NewValue);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AttachHandler(Event);
    }

    protected override void OnDetaching()
    {
        DetachHandler();
        base.OnDetaching();
    }

    private void AttachHandler(string eventName)
    {
        if (string.IsNullOrEmpty(eventName)) return;

        DetachHandler();

        eventInfo = AssociatedObject.GetType().GetEvent(eventName);
        if (eventInfo == null)
            throw new ArgumentException($"Event '{eventName}' not found on type {AssociatedObject.GetType().Name}");

        var methodInfo = typeof(EventToCommandBehavior).GetMethod(nameof(ExecuteCommand), 
            BindingFlags.Instance | BindingFlags.NonPublic);
        eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType!, this, methodInfo!);
        eventInfo.AddEventHandler(AssociatedObject, eventHandler);
    }

    private void DetachHandler()
    {
        if (eventInfo != null && eventHandler != null)
        {
            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }
    }

    private void ExecuteCommand(object sender, EventArgs e)
    {
        var parameter = CommandParameter;
        if (parameter == null && e is RoutedPropertyChangedEventArgs<double> args)
        {
            parameter = args.NewValue;
        }

        if (Command?.CanExecute(parameter) == true)
        {
            Command.Execute(parameter);
        }
    }
}