using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets._Scripts.Additional;
using Sirenix.OdinInspector;

namespace Assets._Scripts.Model
{
    [Serializable]
    public class FruitCount
   {
       public int minCount;
       public int maxCount;

       public BaseFruit fruit;
   }
}
