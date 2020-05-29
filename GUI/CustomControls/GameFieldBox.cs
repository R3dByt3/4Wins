using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.CustomControls
{
    public class GameFieldBox : Control
    {
        public Brush BoxColor
        {
            get { return (Brush)GetValue(BoxColorProperty); }
            set { SetValue(BoxColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxColorProperty =
            DependencyProperty.Register("BoxColor", typeof(Brush), typeof(GameFieldBox), new PropertyMetadata(Brushes.Blue));

        public Brush StoneColor
        {
            get { return (Brush)GetValue(StoneColorProperty); }
            set { SetValue(StoneColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoneColorProperty =
            DependencyProperty.Register("StoneColor", typeof(Brush), typeof(GameFieldBox), new PropertyMetadata(Brushes.Black));

        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(GameFieldBox), new PropertyMetadata(0));

        public int Row
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Row.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowProperty =
            DependencyProperty.Register("Row", typeof(int), typeof(GameFieldBox), new PropertyMetadata(0));

        public bool HasStone
        {
            get { return (bool)GetValue(HasStoneProperty); }
            set { SetValue(HasStoneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasStoneProperty =
            DependencyProperty.Register("HasStone", typeof(bool), typeof(GameFieldBox), new PropertyMetadata(false));

        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent EntersEvent = EventManager.RegisterRoutedEvent(
            "Enters", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GameFieldBox));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Enters
        {
            add { AddHandler(EntersEvent, value); }
            remove { RemoveHandler(EntersEvent, value); }
        }

        private void RaiseEntersEvent()
        {
            RoutedEventArgs eventArgs = new RoutedEventArgs(EntersEvent);
            RaiseEvent(eventArgs);
        }

        public override void OnApplyTemplate()
        {
            //var rectangle = Template.FindName("Rectangle", this) as RectangleGeometry;

            base.OnApplyTemplate();
        }

        static GameFieldBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GameFieldBox), new FrameworkPropertyMetadata(typeof(GameFieldBox)));
        }

        public GameFieldBox()
        {
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            RaiseEntersEvent();
            base.OnMouseLeftButtonUp(e);
        }
    }
}
