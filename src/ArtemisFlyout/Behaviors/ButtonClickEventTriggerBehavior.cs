using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace ArtemisFlyout.Behaviors
{
    public class PointerReleasedTriggerBehavior : Trigger<Button>
    {
        public static readonly StyledProperty<KeyModifiers> KeyModifiersProperty =
            AvaloniaProperty.Register<PointerReleasedTriggerBehavior, KeyModifiers>(nameof(KeyModifiers));


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
                AssociatedObject.AddHandler(InputElement.PointerPressedEvent, Button_PointerReleased, RoutingStrategies.Tunnel);
            }
        }


        private void Button_PointerReleased(object sender, PointerPressedEventArgs e)
        {
            if (AssociatedObject is { } && KeyModifiers == e.KeyModifiers)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, e);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject is { })
            {
                AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, Button_PointerReleased);
            }
        }
    }
}