using Assets.Scripts;
using UnityEngine;

namespace Assets._Scripts
{
    public class MoveHeroBase:MonoBehaviour
    {
        protected int layer = 0;
        private Position heroPosition;
        public Position HeroPosition
        {
            get => heroPosition;
            set { heroPosition = value; }
        }
    }
}
