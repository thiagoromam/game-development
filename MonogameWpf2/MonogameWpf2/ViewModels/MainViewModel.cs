using MonogameWpf2.GameModules;
using MonogameWpf2.Models;
using MonogameWpf2.Module;
using MonogameWpf2.Util;

namespace MonogameWpf2.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        private bool _isFlayoutOpen;
        private bool _isHelpFlyoutOpen;

        public MainViewModel()
        {
            Effects = new EffectsCollection();
            Injection.Container.Map<EffectsCollection>().To(Effects);

            GameModule = new MainGameModule();
        }

        public EffectsCollection Effects { get; private set; }
        public bool IsEffectFlyoutOpen
        {
            get { return _isFlayoutOpen; }
            set
            {
                if (value.Equals(_isFlayoutOpen)) return;
                _isFlayoutOpen = value;
                OnPropertyChanged();
            }
        }
        public bool IsHelpFlyoutOpen
        {
            get { return _isHelpFlyoutOpen; }
            set
            {
                if (value == _isHelpFlyoutOpen) return;
                _isHelpFlyoutOpen = value;
                OnPropertyChanged();
            }
        }
        public IGameModule GameModule { get; private set; }
    }
}