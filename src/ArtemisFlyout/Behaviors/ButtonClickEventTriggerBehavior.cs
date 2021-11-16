using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Behaviors
{
    public class ButtonClickEventTriggerBehavior : Trigger<Button>
    {
        private KeyModifiers _savedKeyModifiers = KeyModifiers.None;
        public static readonly StyledProperty<KeyModifiers> KeyModifiersProperty =
            AvaloniaProperty.Register<ButtonClickEventTriggerBehavior, KeyModifiers>(nameof(KeyModifiers));


        public KeyModifiers KeyModifiers
        {
            get => GetValue(KeyModifiersProperty);
            set => SetValue(KeyModifiersProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is { })
            {
                AssociatedObject.Click += AssociatedObject_OnClick;
                AssociatedObject.AddHandler(InputElement.KeyDownEvent, Button_OnKeyDown, RoutingStrategies.Tunnel);
                AssociatedObject.AddHandler(InputElement.KeyUpEvent, Button_OnKeyUp, RoutingStrategies.Tunnel);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject is { })
            {
                AssociatedObject.Click -= AssociatedObject_OnClick;
                AssociatedObject.RemoveHandler(InputElement.KeyDownEvent, Button_OnKeyDown);
                AssociatedObject.RemoveHandler(InputElement.KeyUpEvent, Button_OnKeyUp);
            }
        }

        private void AssociatedObject_OnClick(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject is { } && KeyModifiers == _savedKeyModifiers)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, e);
            }
        }

        private void Button_OnKeyDown(object sender, KeyEventArgs e)
        {
            _savedKeyModifiers = e.KeyModifiers;
        }

        private void Button_OnKeyUp(object sender, KeyEventArgs e)
        {
            _savedKeyModifiers = KeyModifiers.None;
        }
    }
}