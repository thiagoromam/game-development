using MapEditor.Gui;
using MapEditor.Gui.Controls;
using MapEditor.Ioc;
using MapEditor.Ioc.Api.Gui;
using MapEditor.Ioc.Api.Map;
using MapEditor.Ioc.Api.Settings;
using Microsoft.Xna.Framework;

// ReSharper disable ForCanBeConvertedToForeach
namespace MapEditor.Editor.Controls.Map.Ledge
{
    public class LedgePallete : IControlComponent, ITextControl
    {
        private const int YIncrement = 20;
        private readonly int _x;
        private readonly int _y;
        private readonly IReadonlyMapData _mapData;
        private readonly IGuiText _text;
        private readonly LedgeSelector _ledgeSelector;
        private readonly FlipTextButton<int>[] _ledgeFlagsButtons;
        private readonly IReadOnlySettings _settings;

        public LedgePallete(int x, int y)
        {
            _x = x;
            _y = y;
            _mapData = App.Container.Resolve<IReadonlyMapData>();
            _text = App.Container.Resolve<IGuiText>();
            _settings = App.Container.Resolve<IReadOnlySettings>();
            _ledgeSelector = new LedgeSelector(x, y, YIncrement, _mapData);
            _ledgeFlagsButtons = new FlipTextButton<int>[_mapData.Ledges.Length];
            CreateLedgeFlagsButtons();
            
            var ledgesLoader = App.Container.Resolve<ILedgesLoader>();
            ledgesLoader.LedgesLoaded += UpdateLedgeFlagsButtonValues;
        }

        private void CreateLedgeFlagsButtons()
        {
            for (var i = 0; i < _mapData.Ledges.Length; i++)
            {
                var button = new FlipTextButton<int>(_x + 160, _y + i * YIncrement);
                button.AddOption(0, "soft");
                button.AddOption(1, "hard");

                var ledge = _mapData.Ledges[i];
                button.Value = ledge.Flags;
                button.Change = v => ledge.Flags = v;

                _ledgeFlagsButtons[i] = button;
            }
        }

        private void UpdateLedgeFlagsButtonValues()
        {
            for (var i = 0; i < _ledgeFlagsButtons.Length; i++)
                _ledgeFlagsButtons[i].Value = _mapData.Ledges[i].Flags;
        }

        public void Update()
        {
            if (_settings.CurrentDrawingMode != DrawingMode.Ledge)
                return;

            _ledgeSelector.Update();

            for (var i = 0; i < _ledgeFlagsButtons.Length; i++)
                _ledgeFlagsButtons[i].Update();
        }

        public void Draw()
        {
            if (_settings.CurrentDrawingMode != DrawingMode.Ledge)
                return;

            _ledgeSelector.Draw();
            DrawNodes();
            DrawLedgeFlagsButtons();
        }

        private void DrawNodes()
        {
            var position = new Vector2(_x + 100, _y);
            for (var i = 0; i < _mapData.Ledges.Length; i++)
            {
                _text.Draw("n" + _mapData.Ledges[i].TotalNodes, position);
                position.Y += YIncrement;
            }
        }

        private void DrawLedgeFlagsButtons()
        {
            for (var i = 0; i < _ledgeFlagsButtons.Length; i++)
                _ledgeFlagsButtons[i].Draw();
        }
    }
}