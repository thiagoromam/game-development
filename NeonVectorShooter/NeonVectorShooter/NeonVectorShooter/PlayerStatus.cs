using Microsoft.Xna.Framework;

namespace NeonVectorShooter
{
    public static class PlayerStatus
    {
        private const float MultiplierExpireTime = 0.8f;
        private const int MaxMultiplier = 20;

        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int Multiplier { get; private set; }

        private static float _multiplierTimeLeft;
        private static int _scoreForExtraLife;

        static PlayerStatus()
        {
            Reset();
        }

        public static void Reset()
        {
            Score = 0;
            Multiplier = 1;
            Lives = 4;
            _scoreForExtraLife = 2000;
            _multiplierTimeLeft = 0;
        }

        public static void Update(GameTime gameTime)
        {
            if (Multiplier == 1 || (_multiplierTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds) > 0)
                return;
                
            _multiplierTimeLeft = MultiplierExpireTime;
            ResetMultiplier();
        }

        public static void AddPoints(int basePoints)
        {
            if (PlayerShip.Instance.IsDead)
                return;

            Score += basePoints*Multiplier;
            while (Score >= _scoreForExtraLife)
            {
                _scoreForExtraLife += 2000;
                Lives++;
            }
        }

        public static void IncreaseMultiplier()
        {
            if (PlayerShip.Instance.IsDead)
                return;

            _multiplierTimeLeft = MultiplierExpireTime;
            if (Multiplier < MaxMultiplier)
                Multiplier++;
        }

        private static void ResetMultiplier()
        {
            Multiplier = 1;
        }

        public static void RemoveLife()
        {
            Lives--;
        }
    }
}