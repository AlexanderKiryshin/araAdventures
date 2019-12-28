using Assets.Scripts;

namespace Assets._Scripts
{
    public class FakeMoveHero
    {
        protected int layer = 0;
        private Position heroPosition;
        public Position HeroPosition
        {
            get => heroPosition;
            set { heroPosition = value; }
        }
        public FakeMoveHero ShallowCopy()
        {
            return (FakeMoveHero)this.MemberwiseClone();
        }
    }
}
