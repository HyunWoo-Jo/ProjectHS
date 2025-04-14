using UnityEngine;

namespace Data
{
    public enum TileType {
        Road_Horizontal,
        Road_Vertical,
        Road_Cross,
        Road_VerticalRight,
        Road_HorizontalUp,
        Road_HorizontalDown,
        Road_VerticalLeft,
        Road_LeftDown,
        Road_RightUp,
        Road_RightDown,
        Road_LeftUp,
        Ground,
    }

    public struct MapData
    {
        public Vector3 position;
        public TileType type;
    }
}
