using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.Additional
{
    public class EmptyAdditional : IAdditional
    {
        public IAdditional ShallowCopy()
        {
            throw new NotImplementedException();
        }
        public TileBase GetTile()
        {
            return null;
        }
    }
}
