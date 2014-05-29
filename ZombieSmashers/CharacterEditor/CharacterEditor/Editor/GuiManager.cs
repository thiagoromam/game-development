using CharacterEditor.Editor.Controls.Animations;
using CharacterEditor.Editor.Controls.File;
using CharacterEditor.Editor.Controls.Frames;
using CharacterEditor.Editor.Controls.Icons;
using CharacterEditor.Editor.Controls.KeyFrames;
using CharacterEditor.Editor.Controls.Parts;

namespace CharacterEditor.Editor
{
    public class GuiManager : GraphicalUserInterfaceLib.GuiManager
    {
        public GuiManager()
        {
            AddIconsPalette();
            AddPartsPalette();
            AddFramesPalette();
            AddAnimationsPalette();
            AddKeyFramesPalette();
            AddFileControls();
        }

        private void AddIconsPalette()
        {
            var iconsPalette = new IconsPalette();
            AddComponent(iconsPalette);
            AddControl(iconsPalette);
        }

        private void AddPartsPalette()
        {
            var partsPalette = new PartsPalette();
            AddComponent(partsPalette);
            AddControl(partsPalette);
        }

        private void AddFramesPalette()
        {
            var framesPalette = new FramesPalette();
            AddComponent(framesPalette);
            AddControl(framesPalette);
        }

        private void AddAnimationsPalette()
        {
            var animationsPalette = new AnimationsPalette();
            AddComponent(animationsPalette);
            AddControl(animationsPalette);
        }

        private void AddKeyFramesPalette()
        {
            var keyFramesPalette = new KeyFramePalette();
            AddComponent(keyFramesPalette);
            AddControl(keyFramesPalette);
        }

        private void AddFileControls()
        {
            var loadButton = new LoadButton();
            AddComponent(loadButton);
            AddControl(loadButton);

            var saveButton = new SaveButton();
            AddComponent(saveButton);
            AddControl(saveButton);

            var fileNameEditor = new FileNameEditor();
            AddComponent(fileNameEditor);
            AddTextControl(fileNameEditor);
        }
    }
}