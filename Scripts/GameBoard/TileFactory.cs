using UnityEngine;

namespace PuzzlePaint
{
    public class TileFactory
    {
        public TileCube[,] Tiles { get; private set; }
        public TileCube StartTile { get; private set; }
        public TileCube FinishTile { get; private set; }
        
        private readonly TileFactoryConfig _factoryConfig;

        public TileFactory(TileFactoryConfig factoryConfig)
        {
            _factoryConfig = factoryConfig;
        }

        public TileCube[,] FillBoard(Texture2D level, Transform container)
        {
            int rows = level.height;
            int columns = level.width;
            float offset = 0.5f;
            
            Tiles = new TileCube[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Color pixel = level.GetPixel(j, i);
                    Vector3 spawnPosition = new Vector3(j - columns / 2f + offset, 0f, i - rows / 2f + offset);

                    foreach (TileConfig config in _factoryConfig.TileConfigs)
                    {
                        if (config.ColorOnTexture.Equals(pixel) == true)
                        {
                            TileCube tile = GameObject.Instantiate(config.Prefab, spawnPosition, Quaternion.identity, container);
                            tile.Init(i, j, config.Type, _factoryConfig.StandartColor, _factoryConfig.FillColor);

                            if (config.Type == TileType.Number)
                                tile.SetNumber(config.CurrentNumber);
                            else if (config.Type == TileType.Start)
                                StartTile = tile;
                            else if (config.Type == TileType.Finish)
                                FinishTile = tile;

                            Tiles[i, j] = tile;
                            break;
                        }
                    }
                }
            }
            
            SetFinishNumber();
            
            return Tiles;
        }

        public void SetFinishNumber()
        {
            int points = Tiles.GetLength(0) * Tiles.GetLength(1);
            
            foreach (TileCube tile in Tiles)
            {
                if (tile.MyType == TileType.Block)
                    points -= 1;
            }
            
            if (points - 1 > 0)
                points -= 1;

            FinishTile.SetNumber(points);
        }
    }
}