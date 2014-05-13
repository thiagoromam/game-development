using System.Collections.Generic;
using GraphicalUserInterfaceLib.Api;
using GraphicalUserInterfaceLib.Api.Controls;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable ForCanBeConvertedToForeach

namespace GraphicalUserInterfaceLib
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
        }

        public void AddComponent(IControlComponent component)
        {
            _components.Add(component);
        }
        public void AddControl(IControl control)
        {
            _controls.Add(control);
        }
        public void AddTextControl(ITextControl textControl)
        {
            _textControls.Add(textControl);
        }
        public void AddButton(IButton button)
        {
            AddComponent(button);
            AddControl(button);
        }
        public void AddFlipButton(IFlipTextButton flipButton)
        {
            AddComponent(flipButton);
            AddTextControl(flipButton);
        }
        public void AddTextEditor(ITextEditor textEditor)
        {
            AddComponent(textEditor);
            AddTextControl(textEditor);
        }
        public void AddRadioButtonList(ITextButtonList textButtonList)
        {
            AddComponent(textButtonList);
            AddTextControl(textButtonList);
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