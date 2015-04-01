using System.Collections.ObjectModel;
using MonogameWpf2.Util;

namespace MonogameWpf2.Models
{
    public class EffectsCollection : NotificationObject
    {
        private StampEffect _selected;

        public EffectsCollection()
        {
            Collection = new ObservableCollection<StampEffect>();
        }

        public ObservableCollection<StampEffect> Collection { get; private set; }
        public StampEffect Selected
        {
            get { return _selected; }
            set
            {
                if (Equals(value, _selected)) return;
                _selected = value;
                OnPropertyChanged();
            }
        }
    }
}