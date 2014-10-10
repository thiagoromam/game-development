using System.Threading;

namespace MonogameWpf
{
    public partial class MainWindow
    {
        private EditorGame _game;

        public MainWindow()
        {
            InitializeComponent();

            var handler = RenderPanel.Handle;
            new Thread(() =>
            {
                _game = new EditorGame(handler);
                _game.Run();
            }).Start();

            Closed += (s, e) => _game.Exit();
        }
    }
}
