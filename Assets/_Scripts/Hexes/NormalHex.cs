using Assets._Scripts.FakeHexes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
    public class NormalHex : BaseHexType
    {
        public NormalHex(Position position, int layer) : base(position, layer)
        {
        }
        public override void OnLeaveHex(Position nextHex)
        {
          
           //animator.Play("Falling");
          // GameObject.Destroy(Instance);          
            base.OnLeaveHex(nextHex);
            OnDestroyHex(Position, Layer, DestroyHex);
        }
        public override void OnLeaveHexEvent()
        {
            base.OnLeaveHexEvent();
        }
        public IEnumerator DestroyHex()
        {
            yield return new WaitForSeconds(MoveHero.TIME_WALK*0.7f);
            var firstObject = Instance.transform.GetChild(0);
            firstObject.gameObject.SetActive(false);
            var secondObject = Instance.transform.GetChild(1);
            secondObject.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            GameObject.Destroy(Instance);
        }
        public override TileBase GetTile()
        {
            return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.NORMAL_HEX);
        }

        public override bool isDestoyeble()
        {
            return true;
        }
        public override BaseFakeHexType GetFakeHex()
        {
            return new FakeNormalHex(Position, Layer);
        }
        /* public override void OnEnterHex(Position previousCoordinate)
         {
         }*/
    }
}
