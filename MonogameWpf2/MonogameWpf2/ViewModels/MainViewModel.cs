using MonogameWpf2.Models;
using MonogameWpf2.Util;

namespace MonogameWpf2.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        private bool _isFlayoutOpen;

        public MainViewModel()
        {
            Effects = new EffectsCollection();

            Injection.Container.Map<EffectsCollection>().To(Effects);
        }

        public EffectsCollection Effects { get; private set; }
        public bool IsEffectFlayoutOpen
        {
            get { return _isFlayoutOpen; }
            set
            {
                if (value.Equals(_isFlayoutOpen)) return;
                _isFlayoutOpen = value;
                OnPropertyChanged();
            }
        }
    }
}