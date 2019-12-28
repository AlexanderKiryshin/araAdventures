using Assets._Scripts.FakeHexes;
using Assets.Scripts;
using Assets.Scripts.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts
{
#if (UNITY_EDITOR)
    public class Utility:Singleton<Utility>
    {
        [SerializeField]
        GameObjectData gameObjectData;
        public List<BaseFakeHexType> LoadLevelTiles(Tilemap tilemap)
        {
            var hexes = new List<BaseFakeHexType>();
            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {

                    TileBase tileBase =
                        tilemap.GetTile(new Vector3Int(x, y, 0));
                    if (tileBase == null)
                    {
                        continue;
                    }

                    BaseFakeHexType hextype = null;
                    switch (tileBase.name)
                    {
                        case "DoubleHex":
                            hextype = new FakeDoubleHex(new Position(x, y), 0);
                            hextype.Model = gameObjectData.grass;
                            break;
                        case "NormalHex":
                            hextype = new FakeNormalHex(new Position(x, y), 0);
                            hextype.Model = gameObjectData.dirt;
                            break;
                        case "UndestractableHex":
                            hextype = new FakeUndestractableHex(new Position(x, y), 0);
                            hextype.Model = gameObjectData.stone;
                            break;
                        case "CreatingAroundHex":
                            hextype = new FakeCreatingAroundHex(new Position(x, y), 0);
                            hextype.Model = gameObjectData.exploder;
                            break;
                        case "CreatingAwayHex":
                            hextype = new FakeCreatingAwayHex(new Position(x, y), 0);
                            hextype.Model = gameObjectData.hex3x;
                            break;
                        case "ClockwiseRotatingHex":
                            hextype = new FakeRotatingHex(new Position(x, y), 0, true);
                            hextype.Model = gameObjectData.turnerRight;
                            break;
                        case "CounterClockwiseRotateHex":
                            hextype = new FakeRotatingHex(new Position(x, y), 0, false);
                            hextype.Model = gameObjectData.turnerLeft;
                            break;
                        case "IceHex":
                            hextype = new FakeIceHex(new Position(x, y), 0);
                            hextype.Model = gameObjectData.ice;
                            break;
                        case "LaserHex":
                            hextype = new FakeLaserHex(new Position(x, y), 0);
                            break;
                    }

                    if (hextype != null)
                    {
                        hexes.Add(hextype);
                    }
                }
            }
            return hexes;
        }
    }
#endif
}
