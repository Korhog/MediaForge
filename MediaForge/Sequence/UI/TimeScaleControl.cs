using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Sequence.UI
{
    using Helpers;

    public class TimeScaleUnit
    {
        TimeScaleControl m_parent;

        public TimeScaleUnit(TimeScaleControl parent)
        {
            m_parent = parent;
        }
        public TimeSpan Time { get
            {
                var idx = m_parent.Units.IndexOf(this);
                var timeSpan = new TimeSpan(idx * 20000000);
                return timeSpan;
            }
        }

        public string TimeString { get { return string.Format(@"{0:hh\:mm\:ss\:ff}", Time); } }
    }

    public sealed class TimeScaleControl : ItemsControl
    {
        private TimeSpan m_duration;

        public static readonly DependencyProperty UnitsProperty = DependencyProperty.Register(
            "Units",
            typeof(ObservableCollection<TimeScaleUnit>),
            typeof(FrameContainer),
            new PropertyMetadata(new ObservableCollection<TimeScaleUnit>())
        );

        public ObservableCollection<TimeScaleUnit> Units
        {
            get { return (ObservableCollection<TimeScaleUnit>)GetValue(UnitsProperty); }
            set { SetValue(UnitsProperty, value); }
        }

        public TimeScaleControl()
        {
            this.DefaultStyleKey = typeof(TimeScaleControl);

            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));
            Units.Add(new TimeScaleUnit(this));

            ItemsSource = Units;
        }

        public TimeSpan Duration
        {
            get { return m_duration; }
            set { SetDuration(value); }
        }

        private void SetDuration(TimeSpan duration)
        {

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
