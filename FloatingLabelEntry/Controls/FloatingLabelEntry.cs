using Microsoft.Maui.Controls.Shapes;

namespace FloatingLabelEntry.Controls
{
    public class FloatingLabelEntry : ContentView
    {
        private Label _label;
        private Entry _entry;
        private Border _border;

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), typeof(string), typeof(FloatingLabelEntry), defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnEntryTextChanged);

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            nameof(Placeholder), typeof(string), typeof(FloatingLabelEntry));

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
            nameof(LabelText), typeof(string), typeof(FloatingLabelEntry));

        public static readonly BindableProperty LabelFlowDirectionProperty = BindableProperty.Create(
            nameof(LabelFlowDirection), typeof(FlowDirection), typeof(FloatingLabelEntry), propertyChanged: OnLabelFlowDirectionChanged);

        public static readonly BindableProperty EntryFlowDirectionProperty = BindableProperty.Create(
            nameof(EntryFlowDirection), typeof(FlowDirection), typeof(FloatingLabelEntry), propertyChanged: OnEntryFlowDirectionChanged);

        public static readonly BindableProperty EntryKeyboardProperty = BindableProperty.Create(
            nameof(EntryKeyboard), typeof(Keyboard), typeof(FloatingLabelEntry), Keyboard.Default, propertyChanged: OnEntryKeyboardChanged);

        public static readonly BindableProperty EntryIsEnabledProperty = BindableProperty.Create(
            nameof(EntryIsEnabled), typeof(bool), typeof(FloatingLabelEntry), defaultValue: true, propertyChanged: OnEntryIsEnabledChanged);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                if (Text != value)
                    SetValue(TextProperty, value);
            }
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public FlowDirection LabelFlowDirection
        {
            get => (FlowDirection)GetValue(EntryFlowDirectionProperty);
            set => SetValue(EntryFlowDirectionProperty, value);
        }

        public FlowDirection EntryFlowDirection
        {
            get => (FlowDirection)GetValue(LabelFlowDirectionProperty);
            set => SetValue(LabelFlowDirectionProperty, value);
        }

        public Keyboard EntryKeyboard
        {
            get => (Keyboard)GetValue(EntryKeyboardProperty);
            set => SetValue(EntryKeyboardProperty, value);
        }

        public bool EntryIsEnabled
        {
            get => (bool)GetValue(EntryIsEnabledProperty);
            set => SetValue(EntryIsEnabledProperty, value);
        }

        public FloatingLabelEntry()
        {
            _label = new Label
            {
                Padding = new Thickness(15, 0),
                FontSize = 14,
                TextColor = (Color)Application.Current.Resources["Gray300"],
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = (double)Application.Current.Resources["EntryHeight"],
                BackgroundColor = Colors.Transparent,
                Scale = 1.0,
                AnchorX = 0 // for RightToLeft Languages, set this to 1, according to the parent
            };
            _label.SetBinding(Label.TextProperty, new Binding(nameof(LabelText), source: this));
            _label.SetBinding(Label.FlowDirectionProperty, new Binding(nameof(LabelFlowDirection), source: this));

            _border = new Border
            {
                Padding = 10,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = (Color)Application.Current.Resources["Gray80"],
                HeightRequest = (double)Application.Current.Resources["EntryHeight"],
                Stroke = (Color)Application.Current.Resources["Gray150"],
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(10)
                },
                StrokeThickness = 1.5
            };

            _entry = new Entry
            {
                Placeholder = "",
                HeightRequest = (double)Application.Current.Resources["EntryHeight"]
            };

            _entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this));
            _entry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
            _entry.SetBinding(Entry.KeyboardProperty, new Binding(nameof(EntryKeyboard), source: this));
            _entry.SetBinding(Entry.FlowDirectionProperty, new Binding(nameof(EntryFlowDirection), source: this));

            _entry.Focused += (s, e) => OnFocusChanged(true);
            _entry.Unfocused += (s, e) => OnFocusChanged(false);

            _border.Content = _entry;

            var grid = new Grid
            {
                RowSpacing = 0,
                HeightRequest = 60,
                Children = { _border, _label }
            };

            Content = grid;
        }

        private async void OnFocusChanged(bool isFocused)
        {
            if (isFocused || !string.IsNullOrEmpty(_entry.Text))
            {
                await Task.WhenAll(
                    _label.TranslateTo(0, -30, 200, Easing.CubicInOut),
                    _label.ScaleTo(0.80, 200, Easing.CubicInOut)
                );
                _label.TextColor = (Color)Application.Current.Resources["PrimaryDarkText"];
                //_label.FontSize = 10;
            }
            else
            {
                await Task.WhenAll(
                    _label.TranslateTo(0, 0, 200, Easing.CubicInOut),
                    _label.ScaleTo(1.0, 200, Easing.CubicInOut)
                );
                _label.TextColor = (Color)Application.Current.Resources["Gray300"];
                //_label.FontSize = 14;
            }
        }

        private static void OnEntryTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            string value = (string)newValue;

            var control = (FloatingLabelEntry)bindable;
            var focused = control._entry.IsFocused || !string.IsNullOrEmpty(value);

            control.OnFocusChanged(focused);
        }

        private static void OnLabelFlowDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((FloatingLabelEntry)bindable)._label.FlowDirection = (FlowDirection)newValue;
        }

        private static void OnEntryFlowDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((FloatingLabelEntry)bindable)._entry.FlowDirection = (FlowDirection)newValue;
        }

        private static void OnEntryKeyboardChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (FloatingLabelEntry)bindable;
            control._entry.Keyboard = (Keyboard)newValue;
        }

        private static void OnEntryIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((FloatingLabelEntry)bindable)._entry.IsEnabled = (bool)newValue;
        }
    }
}
