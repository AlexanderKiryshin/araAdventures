using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    public class PathFindAssistant:Singleton<PathFindAssistant>
    {
        [SerializeField]
        private List<SortedDictionary<int, Position>> pathVariants;
        private List<Position> currentPath;

        public void Awake()
        {
            currentPath=new List<Position>();
            MoveHero.instance.positionChanged += AddNextPointPath;
        }

        public bool TryGetNextPointPath(out Position position)
        {
            if (pathVariants != null)
            {
                foreach (var path in pathVariants)
                {
                    if(path.TryGetValue(currentPath.Count+1,out var newPos))
                    {
                        position = newPos;
                        return true;
                    }

                }
            }

            position = new Position(0,0);
            return false;
        }

        public void AddNextPointPath(Position position)
        {
            currentPath.Add(position);
            if (pathVariants != null)
            {
                for (int i=0;i < pathVariants.Count;i++)
                {
                    if (pathVariants[i].TryGetValue(currentPath.Count, out var nextPoint))
                    {
                        if (position.x==nextPoint.x&&position.y==nextPoint.y)
                        { }
                        else
                        {
                            pathVariants.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
    }
}
