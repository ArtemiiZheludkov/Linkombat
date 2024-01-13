using UnityEngine;

namespace PuzzlePaint
{
    [System.Serializable]
    public class TileConfig
    {
        public TileType Type;
        public int CurrentNumber;
        public TileCube Prefab;
        public Color ColorOnTexture;
    }
}