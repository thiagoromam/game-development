using System;
using System.Globalization;
using CharacterEditor.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TextLib;

// ReSharper disable ForCanBeConvertedToForeach

namespace CharacterEditor
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _font;
        private Text _text;
        private CharDef _charDef;

        private Texture2D _nullTex;
        private Texture2D _iconsTex;
        private readonly Texture2D[] _legsTex = new Texture2D[2];
        private readonly Texture2D[] _torsoTex = new Texture2D[2];
        private readonly Texture2D[] _headTex = new Texture2D[2];
        private readonly Texture2D[] _weaponTex = new Texture2D[1];

        private const int FaceLeft = 0;
        private const int FaceRight = 1;
        private const int AuxScript = 0;
        private const int AuxTrigs = 1;
        private const int AuxTextures = 2;
        private const int TrigPistolAcross = 0;
        private const int TrigPistolUp = 1;
        private const int TrigPistolDown = 2;
        private const int TrigWrenchUp = 3;
        private const int TrigWrenchDown = 4;
        private const int TrigWrenchDiagUp = 5;
        private const int TrigWrenchDiagDown = 6;
        private const int TrigWrenchUppercut = 7;
        private const int TrigWrenchSmackdown = 8;
        private const int TrigKick = 9;

        private int _selPart;
        private int _selFrame;
        private int _selAnim;
        private int _selKeyFrame;
        private int _selScriptLine;

        private int _curKey;
        private bool _playing;
        private float _curFrame;

        private int _frameScroll;
        private int _animScroll;
        private int _keyFrameScroll;

        private MouseState _mouseState;
        private MouseState _preState;

        private KeyboardState _oldKeyState;

        private bool _mouseClick;

        private int _auxMode = AuxScript;
        private int _trigScroll;

        private EditingMode _editMode = EditingMode.None;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600
            };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            _charDef = new CharDef();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>(@"Fonts/Arial");

            _text = new Text(_spriteBatch, _font);

            _nullTex = Content.Load<Texture2D>(@"gfx/1x1");
            _iconsTex = Content.Load<Texture2D>(@"gfx/icons");

            LoadTextures(_legsTex, @"gfx/legs");
            LoadTextures(_torsoTex, @"gfx/torso");
            LoadTextures(_headTex, @"gfx/head");
            LoadTextures(_weaponTex, @"gfx/weapon");
        }

        private void LoadTextures(Texture2D[] textures, string path)
        {
            for (var i = 0; i < textures.Length; i++)
                textures[i] = Content.Load<Texture2D>(path + (i + 1));
        }

        protected override void UnloadContent()
        {
        }

        private void CopyFrame(int src, int dest)
        {
            var keySrc = _charDef.Frames[src];
            var keyDest = _charDef.Frames[dest];

            keyDest.Name = keySrc.Name;

            for (var i = 0; i < keyDest.Parts.Length; i++)
            {
                var srcPart = keySrc.Parts[i];
                var destPart = keyDest.Parts[i];

                destPart.Index = srcPart.Index;
                destPart.Location = srcPart.Location;
                destPart.Rotation = srcPart.Rotation;
                destPart.Scaling = srcPart.Scaling;
            }
        }

        private void SwapParts(int idx1, int idx2)
        {
            if (idx1 < 0 || idx2 < 0 ||
                idx1 >= _charDef.Frames[_selFrame].Parts.Length ||
                idx2 >= _charDef.Frames[_selFrame].Parts.Length)
                return;

            var i = _charDef.Frames[_selFrame].Parts[idx1];
            var j = _charDef.Frames[_selFrame].Parts[idx2];

            _charDef.Frames[_selFrame].Parts[idx1] = j;
            _charDef.Frames[_selFrame].Parts[idx2] = i;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            _mouseState = Mouse.GetState();

            var xM = _mouseState.X - _preState.X;
            var yM = _mouseState.Y - _preState.Y;

            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_preState.LeftButton == ButtonState.Pressed)
                {
                    _charDef.Frames[_selFrame].Parts[_selPart].Location +=
                        new Vector2(xM / 2.0f, yM / 2.0f);
                }
            }
            else
            {
                if (_preState.LeftButton == ButtonState.Pressed)
                {
                    _mouseClick = true;
                }
            }

            if (_mouseState.RightButton == ButtonState.Pressed)
            {
                if (_preState.RightButton == ButtonState.Pressed)
                {
                    _charDef.Frames[_selFrame].Parts[_selPart].Rotation += yM / 100.0f;
                }
            }

            if (_mouseState.MiddleButton == ButtonState.Pressed)
            {
                if (_preState.MiddleButton == ButtonState.Pressed)
                {
                    _charDef.Frames[_selFrame].Parts[_selPart].Scaling +=
                        new Vector2(xM * 0.01f, yM * 0.01f);
                }
            }

            _preState = _mouseState;

            UpdateKeys();

            var animation = _charDef.Animations[_selAnim];
            var keyframe = animation.KeyFrames[_curKey];

            if (_playing)
            {
                _curFrame += (float)gameTime.ElapsedGameTime.TotalSeconds * 30.0f;

                if (_curFrame > keyframe.Duration)
                {
                    _curFrame -= keyframe.Duration;
                    _curKey++;

                    if (_curKey >= animation.KeyFrames.Length)
                        _curKey = 0;

                    keyframe = animation.KeyFrames[_curKey];
                }
            }
            else
                _curKey = _selKeyFrame;

            if (keyframe.FrameRef < 0)
                _curKey = 0;

            base.Update(gameTime);
        }

        private void UpdateKeys()
        {
            var keyState = Keyboard.GetState();

            var currentKeys = keyState.GetPressedKeys();
            var lastKeys = _oldKeyState.GetPressedKeys();

            for (var i = 0; i < currentKeys.Length; i++)
            {
                var found = false;

                // ReSharper disable once LoopCanBeConvertedToQuery
                for (var y = 0; y < lastKeys.Length; y++)
                {
                    if (currentKeys[i] == lastKeys[y])
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    PressKey(currentKeys[i]);
            }

            _oldKeyState = keyState;
        }

        private void PressKey(Keys key)
        {
            var t = String.Empty;

            switch (_editMode)
            {
                case EditingMode.FrameName:
                    t = _charDef.Frames[_selFrame].Name;
                    break;
                case EditingMode.AnimationName:
                    t = _charDef.Animations[_selAnim].Name;
                    break;
                case EditingMode.PathName:
                    t = _charDef.Path;
                    break;
                case EditingMode.Script:
                    t = _charDef.Animations[_selAnim].KeyFrames[_selKeyFrame].Scripts[_selScriptLine];
                    break;
            }

            if (key == Keys.Back)
            {
                if (t.Length > 0) t = t.Substring(0, t.Length - 1);
            }
            else if (key == Keys.Enter)
            {
                _editMode = EditingMode.None;
            }
            else
            {
                t = (t + (char)key).ToLower();
            }

            switch (_editMode)
            {
                case EditingMode.FrameName:
                    _charDef.Frames[_selFrame].Name = t;
                    break;
                case EditingMode.AnimationName:
                    _charDef.Animations[_selAnim].Name = t;
                    break;
                case EditingMode.PathName:
                    _charDef.Path = t;
                    break;
                case EditingMode.Script:
                    _charDef.Animations[_selAnim].KeyFrames[_selKeyFrame].Scripts[_selScriptLine] = t;
                    break;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_nullTex, new Rectangle(300, 450, 200, 5), new Color(new Vector4(1.0f, 0.0f, 0.0f, 0.5f)));
            _spriteBatch.Draw(_nullTex, new Rectangle(0, 0, 200, 450), new Color(new Vector4(0.0f, 0.0f, 0.0f, 0.5f)));
            _spriteBatch.Draw(_nullTex, new Rectangle(590, 0, 300, 600), new Color(new Vector4(0.0f, 0.0f, 0.0f, 0.5f)));
            _spriteBatch.Draw(_nullTex, new Rectangle(200, 0, 150, 110), new Color(new Vector4(0.0f, 0.0f, 0.0f, 0.5f)));
            _spriteBatch.End();

            if (_selFrame > 0)
                DrawCharacter(new Vector2(400f, 450f), 2f, FaceRight, _selFrame - 1, false, 0.2f);
            if (_selFrame < _charDef.Frames.Length - 1)
                DrawCharacter(new Vector2(400f, 450f), 2f, FaceRight, _selFrame + 1, false, 0.2f);
            DrawCharacter(new Vector2(400f, 450f), 2f, FaceRight, _selFrame, false, 1.0f);

            DrawPalette();
            DrawPartsList();
            DrawFramesList();
            DrawAnimationList();
            DrawKeyFramesList();

            var fref = _charDef.Animations[_selAnim].KeyFrames[_curKey].FrameRef;
            if (fref < 0)
                fref = 0;

            DrawCharacter(new Vector2(500f, 100f), 0.5f, FaceLeft, fref, true, 1.0f);
            if (_playing)
            {
                if (_text.DrawClickText(480, 100, "stop", _mouseState.X, _mouseState.Y, _mouseClick))
                    _playing = false;
            }
            else
            {
                if (_text.DrawClickText(480, 100, "play", _mouseState.X, _mouseState.Y, _mouseClick))
                    _playing = true;
            }

            if (DrawButton(200, 5, 3, _mouseState.X, _mouseState.Y, _mouseClick))
                _charDef.Write();
            if (DrawButton(230, 5, 4, _mouseState.X, _mouseState.Y, _mouseClick))
                _charDef.Read();

            if (_editMode == EditingMode.PathName)
            {
                _text.Color = Color.Lime;

                _text.DrawText(270, 15, _charDef.Path + "*");
            }
            else
            {
                if (_text.DrawClickText(270, 15, _charDef.Path, _mouseState.X, _mouseState.Y, _mouseClick))
                    _editMode = EditingMode.PathName;
            }

            #region Script/Trigs/Textures Selector

            if (_auxMode == AuxScript)
            {
                _text.Color = Color.Lime;
                _text.DrawText(210, 110, "script");
            }
            else
            {
                if (_text.DrawClickText(210, 110, "script", _mouseState.X, _mouseState.Y, _mouseClick))
                    _auxMode = AuxScript;
            }

            if (_auxMode == AuxTrigs)
            {
                _text.Color = Color.Lime;
                _text.DrawText(260, 110, "trigs");
            }
            else
            {
                if (_text.DrawClickText(260, 110, "trigs", _mouseState.X, _mouseState.Y, _mouseClick))
                    _auxMode = AuxTrigs;
            }

            if (_auxMode == AuxTextures)
            {
                _text.Color = Color.Lime;
                _text.DrawText(300, 110, "tex");
            }
            else
            {
                if (_text.DrawClickText(300, 110, "tex", _mouseState.X, _mouseState.Y, _mouseClick))
                    _auxMode = AuxTextures;
            }

            #endregion

            #region Script

            if (_auxMode == AuxScript)
            {
                for (var i = 0; i < 4; i++)
                {
                    var scriptLineDescription = i + ": " +
                                                _charDef.Animations[_selAnim].KeyFrames[_selKeyFrame].Scripts[i];
                    const int scriptLineX = 210;
                    var scriptLineY = 42 + i * 16;

                    if (_editMode == EditingMode.Script && _selScriptLine == i)
                    {
                        _text.Color = Color.Lime;
                        _text.DrawText(scriptLineX, scriptLineY, scriptLineDescription + "*");
                    }
                    else if (_text.DrawClickText(scriptLineX, scriptLineY, scriptLineDescription, _mouseState.X,
                        _mouseState.Y, _mouseClick))
                    {
                        _selScriptLine = i;
                        _editMode = EditingMode.Script;
                    }
                }
            }

            #endregion

            #region Trigs

            if (_auxMode == AuxTrigs)
            {
                if (DrawButton(330, 42, 1, _mouseState.X, _mouseState.Y, _mouseClick))
                    if (_trigScroll > 0) _trigScroll--;
                if (DrawButton(330, 92, 2, _mouseState.X, _mouseState.Y, _mouseClick))
                    if (_trigScroll < 100) _trigScroll++;
                for (var i = 0; i < 4; i++)
                {
                    var t = i + _trigScroll;
                    if (_text.DrawClickText(210, 42 + i * 16, GetTrigName(t), _mouseState.X, _mouseState.Y, _mouseClick))
                    {
                        _charDef.Frames[_selFrame].Parts[_selPart].Index = t + 1000;
                    }
                }
            }

            #endregion

            #region Texture Switching

            if (_auxMode == AuxTextures)
            {
                for (var i = 0; i < 4; i++)
                {
                    if (DrawButton(210 + i * 21, 40, 1, _mouseState.X, _mouseState.Y, _mouseClick, 0.45f))
                    {
                        switch (i)
                        {
                            case 0:
                                if (_charDef.HeadIndex > 0) _charDef.HeadIndex--;
                                break;
                            case 1:
                                if (_charDef.TorsoIndex > 0) _charDef.TorsoIndex--;
                                break;
                            case 2:
                                if (_charDef.LegsIndex > 0) _charDef.LegsIndex--;
                                break;
                            case 3:
                                if (_charDef.WeaponIndex > 0) _charDef.WeaponIndex--;
                                break;
                        }
                    }
                    var t = _charDef.HeadIndex.ToString(CultureInfo.InvariantCulture);
                    switch (i)
                    {
                        case 1:
                            t = _charDef.TorsoIndex.ToString(CultureInfo.InvariantCulture);
                            break;
                        case 2:
                            t = _charDef.LegsIndex.ToString(CultureInfo.InvariantCulture);
                            break;
                        case 3:
                            t = _charDef.WeaponIndex.ToString(CultureInfo.InvariantCulture);
                            break;
                    }
                    _text.Color = Color.White;
                    _text.DrawText(212 + i * 21, 60, t);
                    if (DrawButton(210 + i * 21, 85, 2, _mouseState.X, _mouseState.Y, _mouseClick, 0.45f))
                    {
                        switch (i)
                        {
                            case 0:
                                if (_charDef.HeadIndex < _headTex.Length - 1) _charDef.HeadIndex++;
                                break;
                            case 1:
                                if (_charDef.TorsoIndex < _torsoTex.Length - 1) _charDef.TorsoIndex++;
                                break;
                            case 2:
                                if (_charDef.LegsIndex < _legsTex.Length - 1) _charDef.LegsIndex++;
                                break;
                            case 3:
                                if (_charDef.WeaponIndex < _weaponTex.Length - 1) _charDef.WeaponIndex++;
                                break;
                        }
                    }
                }
            }

            #endregion

            DrawCursor();

            _mouseClick = false;

            base.Draw(gameTime);
        }

        private void DrawCursor()
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(_iconsTex,
                new Vector2(_mouseState.X, _mouseState.Y),
                new Rectangle(0, 0, 32, 32),
                Color.White, 0.0f,
                new Vector2(0, 0),
                1.0f,
                SpriteEffects.None,
                0.0f);

            _spriteBatch.End();
        }

        private void DrawKeyFramesList()
        {
            for (var i = _keyFrameScroll; i < _keyFrameScroll + 13; i++)
            {
                var animation = _charDef.Animations[_selAnim];

                if (i < animation.KeyFrames.Length)
                {
                    var y = (i - _keyFrameScroll) * 15 + 250;
                    var frameRef = animation.KeyFrames[i].FrameRef;

                    var name = "";

                    if (frameRef > -1)
                        name = _charDef.Frames[frameRef].Name;

                    if (i == _selKeyFrame)
                    {
                        _text.Color = Color.Lime;
                        _text.DrawText(5, y, i + ": " + name);
                    }
                    else
                    {
                        if (_text.DrawClickText(5, y, i + ": " + name, _mouseState.X, _mouseState.Y, _mouseClick))
                            _selKeyFrame = i;
                    }

                    if (frameRef > -1)
                    {
                        if (_text.DrawClickText(110, y, "-", _mouseState.X, _mouseState.Y, _mouseClick))
                        {
                            animation.KeyFrames[i].Duration--;
                            if (animation.KeyFrames[i].Duration <= 0)
                            {
                                for (var j = i; j < animation.KeyFrames.Length - 1; j++)
                                {
                                    var keyframe = animation.KeyFrames[j];

                                    keyframe.FrameRef = animation.KeyFrames[j + 1].FrameRef;
                                    keyframe.Duration = animation.KeyFrames[j + 1].Duration;
                                }

                                animation.KeyFrames[animation.KeyFrames.Length - 1].FrameRef = -1;
                            }
                        }

                        _text.DrawText(125, y, animation.KeyFrames[i].Duration.ToString(CultureInfo.InvariantCulture));

                        if (_text.DrawClickText(140, y, "+", _mouseState.X, _mouseState.Y, _mouseClick))
                            animation.KeyFrames[i].Duration++;
                    }
                }
            }

            if (
                DrawButton(170, 250, 1, _mouseState.X, _mouseState.Y, (_mouseState.LeftButton == ButtonState.Pressed)) &&
                _keyFrameScroll > 0)
                _keyFrameScroll--;

            if (
                DrawButton(170, 410, 2, _mouseState.X, _mouseState.Y, (_mouseState.LeftButton == ButtonState.Pressed)) &&
                _keyFrameScroll < _charDef.Animations[_selAnim].KeyFrames.Length - 13)
                _keyFrameScroll++;
        }

        private void DrawAnimationList()
        {
            for (var i = _animScroll; i < _animScroll + 15; i++)
            {
                if (i < _charDef.Animations.Length)
                {
                    var y = (i - _animScroll) * 15 + 5;
                    if (i == _selAnim)
                    {
                        _text.Color = Color.Lime;

                        _text.DrawText(5, y,
                            i + ": " + _charDef.Animations[i].Name +
                            ((_editMode == EditingMode.AnimationName) ? "*" : ""));
                    }
                    else
                    {
                        if (_text.DrawClickText(5, y, i + ": " + _charDef.Animations[i].Name, _mouseState.X,
                            _mouseState.Y, _mouseClick))
                        {
                            _selAnim = i;
                            _editMode = EditingMode.AnimationName;
                        }
                    }
                }
            }

            if (DrawButton(170, 5, 1, _mouseState.X, _mouseState.Y, (_mouseState.LeftButton == ButtonState.Pressed)) &&
                _animScroll > 0)
                _animScroll--;

            if (
                DrawButton(170, 200, 2, _mouseState.X, _mouseState.Y, (_mouseState.LeftButton == ButtonState.Pressed)) &&
                _animScroll < _charDef.Animations.Length - 15)
                _animScroll++;
        }

        private void DrawFramesList()
        {
            for (var i = _frameScroll; i < _frameScroll + 20; i++)
            {
                if (i < _charDef.Frames.Length)
                {
                    var y = (i - _frameScroll) * 15 + 280;

                    if (i == _selFrame)
                    {
                        _text.Color = Color.Lime;

                        _text.DrawText(600, y,
                            i + ": " + _charDef.Frames[i].Name + ((_editMode == EditingMode.FrameName) ? "*" : ""));

                        if (_text.DrawClickText(720, y, "(a)", _mouseState.X, _mouseState.Y, _mouseClick))
                        {
                            var animation = _charDef.Animations[_selAnim];

                            for (var j = 0; j < animation.KeyFrames.Length; j++)
                            {
                                var keyFrame = animation.KeyFrames[j];

                                if (keyFrame.FrameRef == -1)
                                {
                                    keyFrame.FrameRef = i;
                                    keyFrame.Duration = 1;

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_text.DrawClickText(600, y, i + ": " + _charDef.Frames[i].Name, _mouseState.X,
                            _mouseState.Y, _mouseClick))
                        {
                            if (_selFrame != i)
                            {
                                if (String.IsNullOrEmpty(_charDef.Frames[i].Name))
                                    CopyFrame(_selFrame, i);

                                _selFrame = i;
                                _editMode = EditingMode.FrameName;
                            }
                        }
                    }
                }
            }

            if (
                DrawButton(770, 280, 1, _mouseState.X, _mouseState.Y, (_mouseState.LeftButton == ButtonState.Pressed)) &&
                _frameScroll > 0) _frameScroll--;
            if (
                DrawButton(770, 570, 2, _mouseState.X, _mouseState.Y, (_mouseState.LeftButton == ButtonState.Pressed)) &&
                _frameScroll < _charDef.Frames.Length - 20) _frameScroll++;
        }

        private void DrawPartsList()
        {
            for (var i = 0; i < _charDef.Frames[_selFrame].Parts.Length; i++)
            {
                var y = 5 + i * 15;

                _text.Size = 0.75f;

                string line;

                var index = _charDef.Frames[_selFrame].Parts[i].Index;
                if (index < 0)
                    line = "";
                else if (index < 64)
                    line = "head" + index;
                else if (index < 74)
                    line = "torso" + index;
                else if (index < 128)
                    line = "arms" + index;
                else if (index < 192)
                    line = "legs" + index;
                else
                    line = "weapon" + index;

                if (_selPart == i)
                {
                    _text.Color = Color.Lime;
                    _text.DrawText(600, y, i + ": " + line);

                    if (DrawButton(700, y, 1, _mouseState.X, _mouseState.Y, _mouseClick))
                    {
                        SwapParts(_selPart, _selPart - 1);
                        if (_selPart > 0) _selPart--;
                    }

                    if (DrawButton(720, y, 2, _mouseState.X, _mouseState.Y, _mouseClick))
                    {
                        SwapParts(_selPart, _selPart + 1);
                        if (_selPart < _charDef.Frames[_selFrame].Parts.Length - 1)
                            _selPart++;
                    }

                    var part = _charDef.Frames[_selFrame].Parts[_selPart];
                    if (_text.DrawClickText(740, y, (part.Flip == 0 ? "(n)" : "(m)"),
                        _mouseState.X, _mouseState.Y, _mouseClick))
                    {
                        part.Flip = 1 - part.Flip;
                    }

                    if (_text.DrawClickText(762, y, "(r)", _mouseState.X, _mouseState.Y, _mouseClick))
                        part.Scaling = new Vector2(1.0f, 1.0f);

                    if (_text.DrawClickText(780, y, "(x)", _mouseState.X, _mouseState.Y, _mouseClick))
                        part.Index = -1;
                }
                else
                {
                    if (_text.DrawClickText(600, y, i + ": " + line, _mouseState.X, _mouseState.Y, _mouseClick))
                        _selPart = i;
                }
            }
        }

        private bool DrawButton(int x, int y, int index, int mosX, int mosY, bool mouseClick, float scale = 1)
        {
            var r = false;

            var sRect = new Rectangle(32 * (index % 8), 32 * (index / 8), 32, 32);
            var dRect = new Rectangle(x, y, (int)(32 * scale), (int)(32 * scale));

            if (dRect.Contains(mosX, mosY))
            {
                dRect.X -= 1;
                dRect.Y -= 1;
                dRect.Width += 2;
                dRect.Height += 2;

                if (mouseClick)
                    r = true;
            }

            _spriteBatch.Begin();
            _spriteBatch.Draw(_iconsTex, dRect, sRect, Color.White);
            _spriteBatch.End();

            return r;
        }

        private void DrawPalette()
        {
            _spriteBatch.Begin();

            for (var l = 0; l < 4; l++)
            {
                Texture2D texture = null;
                switch (l)
                {
                    case 0:
                        texture = _headTex[_charDef.HeadIndex];
                        break;
                    case 1:
                        texture = _torsoTex[_charDef.TorsoIndex];
                        break;
                    case 2:
                        texture = _legsTex[_charDef.LegsIndex];
                        break;
                    case 3:
                        texture = _weaponTex[_charDef.WeaponIndex];
                        break;
                }

                if (texture != null)
                {
                    for (var i = 0; i < 25; i++)
                    {
                        var sRect = new Rectangle((i % 5) * 64,
                            (i / 5) * 64, 64, 64);

                        var dRect = new Rectangle(i * 23, 467 + l * 32, 23, 32);

                        _spriteBatch.Draw(_nullTex, dRect, new Color(0, 0, 0, 25));

                        if (l == 3)
                        {
                            sRect.X = (i % 4) * 80;
                            sRect.Y = (i / 4) * 64;
                            sRect.Width = 80;

                            if (i < 15)
                            {
                                dRect.X = i * 30;
                                dRect.Width = 30;
                            }
                        }

                        _spriteBatch.Draw(texture, dRect, sRect, Color.White);

                        if (dRect.Contains(_mouseState.X, _mouseState.Y))
                        {
                            if (_mouseClick)
                            {
                                _charDef.Frames[_selFrame].Parts[_selPart].Index = i + 64 * l;
                            }
                        }
                    }
                }
            }

            _spriteBatch.End();
        }

        private void DrawCharacter(Vector2 loc, float scale, int face, int frameIndex, bool preview, float alpha)
        {
            var sRect = new Rectangle();

            var frame = _charDef.Frames[frameIndex];

            _spriteBatch.Begin();

            for (var i = 0; i < frame.Parts.Length; i++)
            {
                var part = frame.Parts[i];
                if (part.Index > -1)
                {
                    sRect.X = ((part.Index % 64) % 5) * 64;
                    sRect.Y = ((part.Index % 64) / 5) * 64;
                    sRect.Width = 64;
                    sRect.Height = 64;

                    if (part.Index >= 192)
                    {
                        sRect.X = ((part.Index % 64) % 3) * 80;
                        sRect.Width = 80;
                    }

                    var rotation = part.Rotation;

                    var location = part.Location * scale + loc;
                    var scaling = part.Scaling * scale;
                    if (part.Index >= 128) scaling *= 1.35f;

                    if (face == FaceLeft)
                    {
                        rotation = -rotation;
                        location.X -= part.Location.X * scale * 2.0f;
                    }

                    if (part.Index >= 1000 && alpha >= 1f)
                    {
                        _spriteBatch.End();
                        _text.Color = Color.Lime;
                        if (preview)
                        {
                            _text.Size = 0.45f;
                            _text.DrawText((int)location.X, (int)location.Y, "*");
                            _text.Size = 0.75f;
                        }
                        else
                        {
                            _text.Size = 1f;
                            _text.DrawText((int)location.X, (int)location.Y, "*" + GetTrigName(part.Index - 1000));
                            _text.Size = 0.75f;
                        }
                        _spriteBatch.Begin();
                    }
                    else
                    {
                        Texture2D texture = null;

                        var t = part.Index / 64;
                        switch (t)
                        {
                            case 0:
                                texture = _headTex[_charDef.HeadIndex];
                                break;
                            case 1:
                                texture = _torsoTex[_charDef.TorsoIndex];
                                break;
                            case 2:
                                texture = _legsTex[_charDef.LegsIndex];
                                break;
                            case 3:
                                texture = _weaponTex[_charDef.WeaponIndex];
                                break;
                        }

                        var color = new Color(255, 255, 255, (byte)(alpha * 255));

                        if (!preview && _selPart == i)
                            color = new Color(255, 0, 0, (byte)(alpha * 255));

                        var flip = (face == FaceRight && part.Flip == 0) || (face == FaceLeft && part.Flip == 1);

                        if (texture != null)
                        {
                            _spriteBatch.Draw(texture, location, sRect, color, rotation,
                                new Vector2(sRect.Width / 2f, 32f), scaling,
                                (flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 1.0f);
                        }
                    }
                }
            }

            _spriteBatch.End();
        }

        private static string GetTrigName(int idx)
        {
            switch (idx)
            {
                case TrigPistolAcross:
                    return "pistol across";
                case TrigPistolDown:
                    return "pistol down";
                case TrigPistolUp:
                    return "pistol up";
                case TrigWrenchDown:
                    return "wrench down";
                case TrigWrenchSmackdown:
                    return "wrench smackdown";
                case TrigWrenchDiagUp:
                    return "wrench diag up";
                case TrigWrenchDiagDown:
                    return "wrench diag down";
                case TrigWrenchUp:
                    return "wrench up";
                case TrigWrenchUppercut:
                    return "wrench uppercut";
                case TrigKick:
                    return "kick";
            }
            return "";
        }
    }
}