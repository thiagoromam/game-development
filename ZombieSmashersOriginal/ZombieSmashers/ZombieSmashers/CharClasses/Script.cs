namespace ZombieSmashers.CharClasses
{
    public class Script
    {
        private readonly Character _character;

        public Script(Character character)
        {
            _character = character;
        }

        public void DoScript(int animIdx, int keyFrameIdx)
        {
            var charDef = _character.Definition;
            var animation = charDef.Animations[animIdx];
            var keyFrame = animation.KeyFrames[keyFrameIdx];

            var done = false;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < keyFrame.Scripts.Length; i++)
            {
                if (done)
                    break;

                var line = keyFrame.Scripts[i];
                if (line == null)
                    continue;

                switch (line.Command)
                {
                    case Commands.SetAnim:
                        _character.SetAnim(line.SParam);
                        break;
                    case Commands.Goto:
                        _character.AnimFrame = line.IParam;
                        done = true;
                        break;
                    case Commands.IfUpGoto:
                        if (_character.KeyUp)
                        {
                            _character.AnimFrame = line.IParam;
                            done = true;
                        }
                        break;
                    case Commands.IfDownGoto:
                        if (_character.KeyDown)
                        {
                            _character.AnimFrame = line.IParam;
                            done = true;
                        }
                        break;
                    case Commands.Float:
                        _character.Floating = true;
                        break;
                    case Commands.UnFloat:
                        _character.Floating = false;
                        break;
                    case Commands.Slide:
                        _character.Slide(line.IParam);
                        break;
                    case Commands.Backup:
                        _character.Slide(-line.IParam);
                        break;
                    case Commands.SetJump:
                        _character.SetJump(line.IParam);
                        break;
                    case Commands.JoyMove:
                        if (_character.KeyLeft)
                            _character.Trajectory.X = -_character.Speed;
                        else if (_character.KeyRight)
                            _character.Trajectory.X = _character.Speed;
                        break;
                    case Commands.ClearKeys:
                        _character.PressedKey = PressedKeys.None;
                        break;
                    case Commands.SetUpperGoto:
                        _character.GotoGoal[(int)PressedKeys.Upper] = line.IParam;
                        break;
                    case Commands.SetLowerGoto:
                        _character.GotoGoal[(int)PressedKeys.Lower] = line.IParam;
                        break;
                    case Commands.SetAtKGoto:
                        _character.GotoGoal[(int)PressedKeys.Attack] = line.IParam;
                        break;
                    case Commands.SetAnyGoto:
                        _character.GotoGoal[(int)PressedKeys.Upper] = line.IParam;
                        _character.GotoGoal[(int)PressedKeys.Lower] = line.IParam;
                        _character.GotoGoal[(int)PressedKeys.Attack] = line.IParam;
                        break;
                    case Commands.SetSecondaryGoto:
                        _character.GotoGoal[(int)PressedKeys.Secondary] = line.IParam;
                        _character.GotoGoal[(int)PressedKeys.SecUp] = line.IParam;
                        _character.GotoGoal[(int)PressedKeys.SecDown] = line.IParam;
                        break;
                    case Commands.SetSecUpGoto:
                        _character.GotoGoal[(int)PressedKeys.SecUp] = line.IParam;
                        break;
                    case Commands.SetSecDownGoto:
                        _character.GotoGoal[(int)PressedKeys.SecDown] = line.IParam;
                        break;
                }
            }
        }
    }
}