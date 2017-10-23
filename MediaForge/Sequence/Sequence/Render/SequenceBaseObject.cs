using System.Linq;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace Sequence
{
    using System;
    using UI;

    /// <summary>
    /// 
    /// </summary>  
    public partial class SequenceBaseObject
    {
        public enum SequenceUpdateResult
        {
            Active,
            Unactive
        }

        public virtual SequenceUpdateResult Update(TimeSpan time)
        {
            if (time < StartTime || time > StartTime + Duration)
                return SequenceUpdateResult.Unactive;
            return SequenceUpdateResult.Active;
        }              
    }
}
