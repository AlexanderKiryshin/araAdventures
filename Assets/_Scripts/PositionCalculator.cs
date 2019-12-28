using UnityEngine;

namespace Assets.Scripts
{
    public static class PositionCalculator
    {
        public static Position GetOppositeSidePosition(Position previousCoordinate, Position position)
        {
            if (position.y % 2 == 0)
            {
                if (previousCoordinate.x == position.x && previousCoordinate.y + 1 == position.y)
                {
                    return new Position(position.x - 1, position.y + 1);
                }

                if (previousCoordinate.x - 1 == position.x && previousCoordinate.y == position.y)
                {
                    return new Position(position.x - 1, position.y);
                }

                if (previousCoordinate.x == position.x && previousCoordinate.y - 1 == position.y)
                {
                    return new Position(position.x - 1, position.y - 1);
                }

                if (previousCoordinate.x + 1 == position.x && previousCoordinate.y - 1 == position.y)
                {
                    return new Position(position.x, position.y - 1);
                }

                if (previousCoordinate.x + 1 == position.x && previousCoordinate.y == position.y)
                {
                    return new Position(position.x + 1, position.y);
                }

                if (previousCoordinate.x + 1 == position.x && previousCoordinate.y + 1 == position.y)
                {
                    return new Position(position.x, position.y + 1);
                }
            }
            else
            {
                if (previousCoordinate.x-1 == position.x && previousCoordinate.y + 1 == position.y)
                {
                    return new Position(position.x, position.y + 1);
                }

                if (previousCoordinate.x - 1 == position.x && previousCoordinate.y == position.y)
                {
                    return new Position(position.x - 1, position.y);
                }

                if (previousCoordinate.x-1 == position.x && previousCoordinate.y - 1 == position.y)
                {
                    return new Position(position.x , position.y - 1);
                }

                if (previousCoordinate.x  == position.x && previousCoordinate.y - 1 == position.y)
                {
                    return new Position(position.x+1, position.y - 1);
                }

                if (previousCoordinate.x + 1 == position.x && previousCoordinate.y == position.y)
                {
                    return new Position(position.x + 1, position.y);
                }

                if (previousCoordinate.x  == position.x && previousCoordinate.y + 1 == position.y)
                {
                    return new Position(position.x+1, position.y + 1);
                }
            }

            return new Position(0,0);
        }

        public static Position[] GetAroundSidePositions(Position position)
        {
            var result=new Position[6];
            if (position.y % 2 == 0)
            {
                result[0]=new Position(position.x, position.y - 1);
                result[1] = new Position(position.x + 1, position.y);
                result[2] = new Position(position.x, position.y + 1);
                result[3] = new Position(position.x - 1, position.y + 1);
                result[4] = new Position(position.x - 1, position.y);
                result[5] = new Position(position.x - 1, position.y - 1);
            }
            else
            {
                result[0] = new Position(position.x+1, position.y - 1);
                result[1] = new Position(position.x + 1, position.y);
                result[2] = new Position(position.x+1, position.y + 1);
                result[3] = new Position(position.x, position.y + 1);
                result[4] = new Position(position.x - 1, position.y);
                result[5] = new Position(position.x , position.y - 1);
            }

            return result;
        }

        public static Position[] GetLaserPositions(Position previousCoordinate, Position position)
        {
            if (position.y % 2 == 0)
            {
                if (previousCoordinate.x == position.x && previousCoordinate.y + 1 == position.y)
                {                    
                    return  new Position[]{ new Position(position.x , position.y + 1), new Position(position.x-1, position.y) };
                }

                if (previousCoordinate.x - 1 == position.x && previousCoordinate.y == position.y)
                {
                    return new Position[] { new Position(position.x-1, position.y -1), new Position(position.x - 1, position.y+1) };
                }

                if (previousCoordinate.x == position.x && previousCoordinate.y - 1 == position.y)
                {
                    return new Position[] { new Position(position.x - 1, position.y), new Position(position.x , position.y-1) };
                }

                if (previousCoordinate.x + 1 == position.x && previousCoordinate.y - 1 == position.y)
                {
                    return new Position[] { new Position(position.x - 1, position.y - 1), new Position(position.x+1, position.y) };
                }

                if (previousCoordinate.x + 1 == position.x && previousCoordinate.y == position.y)
                {
                    return new Position[] { new Position(position.x, position.y - 1), new Position(position.x - 1, position.y - 1) };
                }

                if (previousCoordinate.x + 1 == position.x && previousCoordinate.y + 1 == position.y)
                {
                    return new Position[] { new Position(position.x + 1, position.y), new Position(position.x - 1, position.y + 1) };
                }
            }
            else
            {
                if (previousCoordinate.x - 1 == position.x && previousCoordinate.y + 1 == position.y)
                {
                    return new Position[] { new Position(position.x - 1, position.y), new Position(position.x + 1, position.y+1) };
                   // return new Vector2Int(position.x, position.y + 1);
                }

                if (previousCoordinate.x - 1 == position.x && previousCoordinate.y == position.y)
                {
                    return new Position[] { new Position(position.x , position.y + 1), new Position(position.x, position.y - 1) };
                  //  return new Vector2Int(position.x - 1, position.y);
                }

                if (previousCoordinate.x - 1 == position.x && previousCoordinate.y - 1 == position.y)
                {
                    return new Position[] { new Position(position.x - 1, position.y), new Position(position.x + 1, position.y - 1) };
                    //  return new Vector2Int(position.x, position.y - 1);
                }

                if (previousCoordinate.x == position.x && previousCoordinate.y - 1 == position.y)
                {
                    return new Position[] { new Position(position.x, position.y-1), new Position(position.x + 1, position.y) };
                    // return new Vector2Int(position.x + 1, position.y - 1);
                }

                if (previousCoordinate.x + 1 == position.x && previousCoordinate.y == position.y)
                {
                    return new Position[] { new Position(position.x+1, position.y - 1), new Position(position.x+1, position.y + 1) };
                   // return new Vector2Int(position.x + 1, position.y);
                }

                if (previousCoordinate.x == position.x && previousCoordinate.y + 1 == position.y)
                {
                    return new Position[] { new Position(position.x+1 , position.y),new Position(position.x , position.y + 1) };
                    // return new Vector2Int(position.x + 1, position.y + 1);
                }
            }
            return null;
        }

        public static float GetAngleAroundNearHexes(Position heroPosition, Position hex)
        {
            if (heroPosition.y % 2 == 0)
            {
                if (hex.x == heroPosition.x && hex.y + 1 == heroPosition.y)
                {
                    return 60;
                }

                if (hex.x - 1 == heroPosition.x && hex.y == heroPosition.y)
                {
                    return 0;
                }

                if (hex.x == heroPosition.x && hex.y - 1 == heroPosition.y)
                {
                    return -60;
                }

                if (hex.x + 1 == heroPosition.x && hex.y - 1 == heroPosition.y)
                {
                    return -120;
                }

                if (hex.x + 1 == heroPosition.x && hex.y == heroPosition.y)
                {
                    return -180;
                }

                if (hex.x + 1 == heroPosition.x && hex.y + 1 == heroPosition.y)
                {
                    return -240;
                }
            }
            else
            {
                if (hex.x - 1 == heroPosition.x && hex.y + 1 == heroPosition.y)
                {
                    return 60;
                }

                if (hex.x - 1 == heroPosition.x && hex.y == heroPosition.y)
                {
                    return 0;
                }

                if (hex.x - 1 == heroPosition.x && hex.y - 1 == heroPosition.y)
                {
                    return -60;
                }

                if (hex.x == heroPosition.x && hex.y - 1 == heroPosition.y)
                {
                    return -120;
                }

                if (hex.x + 1 == heroPosition.x && hex.y == heroPosition.y)
                {
                    return -180;
                }

                if (hex.x == heroPosition.x && hex.y + 1 == heroPosition.y)
                {
                    return -240;
                }
            }

            return 0;
        }
        public static Position GetAdjustmentPosition(Position position, bool isLeft, Position center)
        {
            if (center.y % 2 == 0)
            {
                if (position.x == center.x && position.y + 1 == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x-1, position.y);
                    }
                    else
                    {
                        return new Position(position.x +1, position.y+1);
                    }
                }

                if (position.x - 1 == center.x && position.y == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x - 1, position.y-1);
                    }
                    else
                    {
                        return new Position(position.x -1, position.y + 1);
                    }
                }

                if (position.x == center.x && position.y - 1 == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x +1, position.y-1);
                    }
                    else
                    {
                        return new Position(position.x -1, position.y );
                    }
                }

                if (position.x + 1 == center.x && position.y - 1 == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x +1, position.y);
                    }
                    else
                    {
                        return new Position(position.x, position.y-1);
                    }
                }

                if (position.x + 1 == center.x && position.y == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x , position.y+1);
                    }
                    else
                    {
                        return new Position(position.x , position.y-1);
                    }
                }

                if (position.x + 1 == center.x && position.y + 1 == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x , position.y+1);
                    }
                    else
                    {
                        return new Position(position.x + 1, position.y);
                    }
                }
            }
            else
            {
                if (position.x - 1 == center.x && position.y + 1 == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x - 1, position.y);
                    }
                    else
                    {
                        return new Position(position.x , position.y + 1);
                    }
                }

                if (position.x - 1 == center.x && position.y == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x , position.y-1);
                    }
                    else
                    {
                        return new Position(position.x, position.y + 1);
                    }
                }

                if (position.x - 1 == center.x && position.y - 1 == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x , position.y-1);
                    }
                    else
                    {
                        return new Position(position.x - 1, position.y);
                    }
                }

                if (position.x == center.x && position.y - 1 == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x + 1, position.y);
                    }
                    else
                    {
                        return new Position(position.x - 1, position.y - 1);
                    }
                }

                if (position.x + 1 == center.x && position.y == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x +1, position.y+1);
                    }
                    else
                    {
                        return new Position(position.x + 1, position.y - 1);
                    }
                }

                if (position.x == center.x && position.y + 1 == center.y)
                {
                    if (isLeft)
                    {
                        return new Position(position.x - 1, position.y+1);
                    }
                    else
                    {
                        return new Position(position.x + 1, position.y);
                    }
                }
            }
            return new Position(0, 0);
        }
    }
}
