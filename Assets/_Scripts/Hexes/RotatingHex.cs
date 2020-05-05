using System;
using Assets._Scripts.FakeHexes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
    public class RotatingHex : BaseHexType
    {
        public Action EndRotateAction;
        public bool isClockwiseRotating;
        public RotatingHex(Position position, int layer, bool isClockwiseRotating) : base(position, layer)
        {
            this.isClockwiseRotating = isClockwiseRotating;
            EndRotateAction += EndRotate;

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

      /*  public override void OnEnterHex(Position previousCoordinate)
        {
            LevelManager.instance.RotateHexes(this);
           // base.OnEnterHex(previousCoordinate);
            Position[] positions = PositionCalculator.GetAroundSidePositions(Position);
            var hexes = new List<BaseHexType>();
            var newPositions = new List<Position>();
            var hexesForRotate=new List<BaseHexType>();
            foreach (var position in positions)
            {
                LevelManager.instance.TryGetHex(position, Layer, out var hex);
                if (hex != null)
                {
                    hexesForRotate.Add(hex);
                    newPositions.Add(PositionCalculator.GetAdjustmentPosition(position, !isClockwiseRotating, Position));
					//LevelManager.instance.DestroyHex(position, Layer,Destroy);
                }
            }
            for (int i = 0; i < hexes.Count; i++)
            {
                hexes[i].Position = newPositions[i];
                LevelManager.instance.CreateHex(hexes[i]);
            }

        }*/
        public override void OnEnterHexEvent()
        {
            ((MoveHero)MoveHero.instance).SetIdleAnimation();
            ((MoveHero)MoveHero.instance).SetNextPosition();
            LevelManager.instance.RotateHexes(this);            
        }

        public void EndRotate()
        {          
            ((MoveHero)MoveHero.instance).EndMove -= OnEnterHexEvent;
            ((MoveHero)MoveHero.instance).UnlockInput();
        }
        public override BaseFakeHexType GetFakeHex()
        {
            return new FakeRotatingHex(Position, Layer, isClockwiseRotating);
        }
    }
}
