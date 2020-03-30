using Assets._Scripts.FakeHexes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
   public class DoubleHex: BaseHexType
    {
        public DoubleHex(Position position, int layer):base(position,layer)
        {
        }

        public override void OnLeaveHex(Position nextHex)
        {
          //  StartCoroutine(HideGrass());
            OnChangeHex(new NormalHex(Position,Layer),HideGrass);
            base.OnLeaveHex(nextHex);
        }

        public IEnumerator HideGrass()
        {
            Debug.LogError("XXXX");
            var renderer = Instance.transform.GetChild(0).GetComponent<MeshRenderer>();
            var material = renderer.materials[1];
            for (int i = 0; i < 100; i++)
            {
               // renderer.materials[0].color = new Color(renderer.materials[0].color.r, renderer.materials[0].color.g, renderer.materials[0].color.b, renderer.materials[0].color.a - 0.01f);
                renderer.materials[1].color = new Color(renderer.materials[1].color.r, renderer.materials[1].color.g, renderer.materials[1].color.b, renderer.materials[1].color.a+0.01f);
                yield return new WaitForSeconds(0.01f);
            }
        }

        public override TileBase GetTile()
        {
            return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.DOUBLE_HEX) ;
        }

        public override bool isDestoyeble()
        {
            return true;
        }

        public override BaseFakeHexType GetFakeHex()
        {
            return new FakeDoubleHex(Position, Layer);
        }
       /* public override void OnEnterHex(Position previousCoordinate)
        {
        }*/
    }
}
