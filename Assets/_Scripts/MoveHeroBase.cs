﻿using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets._Scripts
{
    public class MoveHeroBase:Singleton<MoveHeroBase>
    {
        public Action<Position> positionChanged;
        protected int layer = 0;
        private Position heroPosition;
        public Position HeroPosition
        {
            get => heroPosition;
            set { heroPosition = value; }
        }
    }
}
