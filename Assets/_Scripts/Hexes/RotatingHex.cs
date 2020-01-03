using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
    public class RotatingHex : BaseHexType
    {
        public bool isClockwiseRotating;
        public RotatingHex(Position position, int layer, bool isClockwiseRotating) : base(position, layer)
        {
            this.isClockwiseRotating = isClockwiseRotating;
        }
       /* public override void OnLeaveHex()
        {
            // OnDestroyHex(Position, Layer);
        }*/

        public override TileBase GetTile()
        {
            if (isClockwiseRotating)
            {
                return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.CLOCKWISE_ROTATING_HEX);
            }
            else
            {
                return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.COUNTER_CLOCKWISE_ROTATING_HEX);
            }

        }

        public override bool isDestoyeble()
        {
            return false;
        }

        public override void OnEnterHex(Position previousCoordinate)
        {
            base.OnEnterHex(previousCoordinate);
            Position[] positions = PositionCalculator.GetAroundSidePositions(Position);
            var levelManager = GameObject.FindObjectOfType<LevelManager>();
            var hexes = new List<BaseHexType>();
            var newPositions = new List<Position>();
            var hexesForRotate=new List<IHexType>();
            foreach (var position in positions)
            {
                levelManager.TryGetHex(position, Layer, out var hex);
                if (hex != null)
                {
                    hexesForRotate.Add(hex);
                    hexes.Add(hex);
                    newPositions.Add(PositionCalculator.GetAdjustmentPosition(position, !isClockwiseRotating, Position));
                    levelManager.DestroyHex(position, Layer,Destroy);
                }
            }
            for (int i = 0; i < hexes.Count; i++)
            {
                hexes[i].Position = newPositions[i];
                levelManager.CreateHex(hexes[i]);
            }

        }

        public IEnumerator RotateHexes()
        {
            Position[] positions = PositionCalculator.GetAroundSidePositions(Position);
            var levelManager = GameObject.FindObjectOfType<LevelManager>();
            var promhexes = new List<BaseHexType>();
            var newPositions = new List<Position>();
            var hexesForRotate = new List<IHexType>();
            foreach (var position in positions)
            {
                levelManager.TryGetHex(position, Layer, out var hex);
                if (hex != null)
                {
                    hexesForRotate.Add(hex);
                    promhexes.Add(hex);
                    newPositions.Add(PositionCalculator.GetAdjustmentPosition(position, !isClockwiseRotating, Position));
                    levelManager.DestroyHex(position, Layer, Destroy);
                }
            }
            for (int i = 0; i < promhexes.Count; i++)
            {
                promhexes[i].Instance.transform.DOMoveY(Instance.transform.position.y + 1, 1f);
                promhexes[i].Position = newPositions[i];
                levelManager.CreateHex(promhexes[i]);
            }
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < promhexes.Count; i++)
            {
                promhexes[i].Instance.transform.DORotate(levelManager.levelTilemap.GetCellCenterWorld(new Vector3Int(promhexes[i].Position.x, promhexes[i].Position.y, 0)), 1f);
            }



            /*   foreach (var hex in hexes)
               {
                   Vector2Int[] positions = PositionCalculator.GetAroundSidePositions(Position);
                   Vector2Int newPosition= PositionCalculator.GetAdjustmentPosition(position, !isClockwiseRotating, Position)
               }*/
            yield return new WaitForSeconds(0);
        }
    }
}
