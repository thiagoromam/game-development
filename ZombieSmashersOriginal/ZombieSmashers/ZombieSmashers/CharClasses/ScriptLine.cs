using System;

namespace ZombieSmashers.CharClasses
{
    public class ScriptLine
    {
        private readonly Commands _command;
        private readonly string _sParam;
        private readonly int _iParam;

        public ScriptLine(string line)
        {
            var split = line.Split(' ');

            try
            {
                switch (split[0].Trim().ToLower())
                {
                    case "setanim":
                        _command = Commands.SetAnim;
                        _sParam = split[1];
                        break;
                    case "goto":
                        _command = Commands.Goto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "ifupgoto":
                        _command = Commands.IfUpGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "ifdowngoto":
                        _command = Commands.IfDownGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "float":
                        _command = Commands.Float;
                        break;
                    case "unfloat":
                        _command = Commands.UnFloat;
                        break;
                    case "slide":
                        _command = Commands.Slide;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "backup":
                        _command = Commands.Backup;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "setjump":
                        _command = Commands.SetJump;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "joymove":
                        _command = Commands.JoyMove;
                        break;
                    case "clearkeys":
                        _command = Commands.ClearKeys;
                        break;
                    case "setuppergoto":
                        _command = Commands.SetUpperGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "setlowergoto":
                        _command = Commands.SetLowerGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "setatkgoto":
                        _command = Commands.SetAtKGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "setanygoto":
                        _command = Commands.SetAnyGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "setsecgoto":
                        _command = Commands.SetSecondaryGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "setsecupgoto":
                        _command = Commands.SetSecUpGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "setsecdowngoto":
                        _command = Commands.SetSecDownGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "play":
                        _command = Commands.PlaySound;
                        _sParam = split[1];
                        break;
                    case "ethereal":
                        _command = Commands.Ethereal;
                        break;
                    case "solid":
                        _command = Commands.Solid;
                        break;
                    case "speed":
                        _command = Commands.Speed;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "hp":
                        _command = Commands.Hp;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "deathcheck":
                        _command = Commands.DeathCheck;
                        break;
                    case "ifdyinggoto":
                        _command = Commands.IfDyingGoto;
                        _iParam = Convert.ToInt32(split[1]);
                        break;
                    case "killme":
                        _command = Commands.KillMe;
                        break;
                    case "ai":
                        _command = Commands.Ai;
                        _sParam = split[1];
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public Commands Command
        {
            get { return _command; }
        }

        // ReSharper disable once InconsistentNaming
        public int IParam
        {
            get { return _iParam; }
        }

        public String SParam
        {
            get { return _sParam; }
        }
    }
}