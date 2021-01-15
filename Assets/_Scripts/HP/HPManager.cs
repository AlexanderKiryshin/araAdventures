using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;

namespace Assets._Scripts.HP
{
    public class HPManager:Singleton<HPManager>
    {
        public HPController hpBarOnLevelSelect;
    }
}
