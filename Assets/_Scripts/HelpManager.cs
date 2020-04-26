using System;
using System.Collections.Generic;
using System.Xml.XPath;
using Assets._Scripts.Model;
using Assets.Scripts;
using Assets._Scripts.FakeHexes;
using UnityEngine;

namespace Assets._Scripts
{
    public class HelpManager:Singleton<HelpManager>
    {
        public Dictionary<HexEnum, int> limitPasses;
        public const int NUMBER_TIPS = 5;
        private int numberAvailibleTips;
        private List<Position> path;
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

            if(PathFindAssistant.instance.TryGetNextPointPath(out var newPointPath))
            {
                path=new List<Position>();
                path.Add(MoveHero.instance.HeroPosition);
                path.Add(newPointPath);
            }
            else
            {
                path = PathFinder.GetPath(LevelManager.instance.GetFruitList(), LevelManager.instance.MakeListFakeHexes(),
                    MoveHero.instance.HeroPosition,limitPasses);
            }
          
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
                    if (PathFindAssistant.instance.TryGetNextPointPath(out var newPointPath))
                    {
                        path = new List<Position>();
                        path.Add(MoveHero.instance.HeroPosition);
                        path.Add(newPointPath);
                    }
                    else
                    {
                        path = PathFinder.GetPath(LevelManager.instance.GetFruitList(),
                            LevelManager.instance.MakeListFakeHexes(),
                            MoveHero.instance.HeroPosition,limitPasses);
                    }
                }
                else
                {
                    firstStepHelp = false;
                }

                if (path != null)
                {

                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        if (path[i].x == heroPosition.x && path[i].y == heroPosition.y)
                        {
                            numberAvailibleTips--;
                            nextPosition = path[i + 1];
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
