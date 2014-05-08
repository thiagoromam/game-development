using System.Collections.Generic;
using MapEditor.Editor.Buttons.File;
using MapEditor.Editor.Buttons.Map;
using MapEditor.Gui;
using MapEditor.Gui.Controls;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable ForCanBeConvertedToForeach

namespace MapEditor.Editor
{
    public class GuiManager
    {
        private readonly List<IControlComponent> _components;
        private readonly List<IControl> _controls;
        private readonly List<ITextControl> _textControls; 

        public GuiManager()
        {
            _components = new List<IControlComponent>();
            _controls = new List<IControl>();
            _textControls = new List<ITextControl>();

            AddButtons();
            AddFlipButtons();
            AddTextEditors();
        }

        private void AddButtons()
        {
            AddButton(new SaveButton(5, 65));
            AddButton(new LoadButton(40, 65));
        }
        private void AddButton(Button button)
        {
            _components.Add(button);
            _controls.Add(button);
        }

        private void AddFlipButtons()
        {
            AddFlipButton(new MapLayerButton(5, 5));
            AddFlipButton(new DrawingModeButton(5, 25));
        }
        private void AddFlipButton(IFlipTextButton flipButton)
        {
            _components.Add(flipButton);
            _textControls.Add(flipButton);
        }

        private void AddTextEditors()
        {
            AddTextEditor(new MapPathEditor(5, 45));
        }
        private void AddTextEditor(TextEditor textEditor)
        {
            _components.Add(textEditor);
            _textControls.Add(textEditor);
        }

        public void Update()
        {
            for (var i = 0; i < _components.Count; i++)
                _components[i].Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _controls.Count; i++)
                _controls[i].Draw(spriteBatch);

            for (var i = 0; i < _textControls.Count; i++)
                _textControls[i].Draw();
        }
    }
}