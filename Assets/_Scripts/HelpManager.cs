using System;
using Assets._Scripts.Model;
using Assets.Scripts;
using UnityEngine;

namespace Assets._Scripts
{
    public class HelpManager:Singleton<HelpManager>
    {
        public const int NUMBER_TIPS = 5;
        private int numberAvailibleTips;
        private PathFindData path;
        public static Action helpUse;
        public static Action helpAlreadyActivated;
        public Vector2Int Offset { set; get; }
        public void Awake()
        {
            helpUse = HelpActivate;
        }

        public  int GetNumberAvailibleTips
        {
            get { return numberAvailibleTips; }
        }

        private bool firstStepHelp = false;
        void HelpActivate()
        {
            if (numberAvailibleTips > 0)
            {
                helpAlreadyActivated?.Invoke();
                return;
            }
            path = PathFinder.GetPath(LevelManager.instance.GetFruitList(), LevelManager.instance.MakeListFakeHexes(),
                MoveHero.instance.HeroPosition);
            if (path != null)
            {
                numberAvailibleTips = NUMBER_TIPS;
                firstStepHelp = true;
            }
            else
            {
                LevelManager.instance.pathNotFoundAction?.Invoke();
            }

            LevelManager.instance.DeselectCells();
            LevelManager.instance.SelectCells(MoveHero.instance.HeroPosition);
        }

        public bool TryGetHelpStep(Position heroPosition,out Position nextPosition)
        {
            if (numberAvailibleTips > 0)
            {
                if (!firstStepHelp)
                {
                    path = PathFinder.GetPath(LevelManager.instance.GetFruitList(),
                        LevelManager.instance.MakeListFakeHexes(),
                        MoveHero.instance.HeroPosition);
                }
                else
                {
                    firstStepHelp = false;
                }

                if (path != null)
                {
                    for (int i = 0; i < path.path.Count - 1; i++)
                    {
                        if (path.path[i].x == heroPosition.x && path.path[i].y == heroPosition.y)
                        {
                            numberAvailibleTips--;
                            nextPosition = path.path[i + 1];
                            return true;
                        }
                    }
                }
            }
            nextPosition=new Position(0,0);
            return false;
        }
    }
}
