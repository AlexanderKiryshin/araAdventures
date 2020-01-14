using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets._Scripts;
using UnityEngine;

namespace Assets
{

    [CreateAssetMenu(fileName = "GameObjectData", menuName = "GameObjectData", order = 51)]
    public class GameObjectData:ScriptableObject
    {
        public GameObject fruit;
        public GameObject hex3x;
        public GameObject dirt;
        public GameObject exploder;
        public GameObject grass;
        public GameObject ice;
        public GameObject stone;
        public GameObject turnerLeft;
        public GameObject turnerRight;
        public GameObject doubleStone;
    }
}
