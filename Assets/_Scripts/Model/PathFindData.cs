using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Model
{
    public class PathFindData:ScriptableObject
    {

        public List<Position> path;
        public int lengthRightPath;
        public bool limitExceeded;
        public float difficulty;

        public PathFindData(List<Position> path, int lengthRightPath, bool limitExceeded)
        {
            this.path = path;
            this.lengthRightPath = lengthRightPath;
            this.limitExceeded = limitExceeded;            
        }
    }
}
