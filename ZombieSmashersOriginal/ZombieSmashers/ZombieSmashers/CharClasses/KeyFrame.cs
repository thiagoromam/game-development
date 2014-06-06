namespace ZombieSmashers.CharClasses
{
    public class KeyFrame
    {
        public int FrameRef;
        public int Duration;

        private readonly ScriptLine[] _scripts;

        public KeyFrame()
        {
            FrameRef = -1;
            Duration = 0;

            _scripts = new ScriptLine[4];
        }

        public ScriptLine[] Scripts
        {
            get { return _scripts; }
        }
    }
}