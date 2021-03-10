using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

namespace Assets._Scripts.Model
{  
    [Serializable]
    public class PacksData :SerializedBehaviour
    {
        [SerializeField]
        public GameObject[] levels; 
    }
}