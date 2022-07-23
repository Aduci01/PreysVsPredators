using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

public class GameField : MonoBehaviour {
    private Tile[,] _tiles;

    [SerializeField] private StatUi ui;
    
    public int PredatorNum { get; set; }
    public int PreyNum { get; set; }

    public void Init(int width, int height) {
        _tiles = new Tile[width, height];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                int r = Random.Range(0, 1000);

                if (r < 950) {
                    _tiles[i, j] = new Tile(new Vector2Int(i, j), this, TileType.Empty);
                }
                else if (r < 999) {
                    _tiles[i, j] = new Tile(new Vector2Int(i, j), this, TileType.Prey);
                    PreyNum++;
                }
                else {
                    _tiles[i, j] = new Tile(new Vector2Int(i, j), this, TileType.Predator);
                    PredatorNum++;
                }
            }
        }

        foreach (var t in _tiles) {
            t.SetNeighbours();
        }
    }

    public void NextTurn() {
        int xLength = _tiles.GetLength(0);
        int yLength = _tiles.GetLength(1);
        
        for (int i = 0; i < xLength; i++) {
            for (int j = 0; j < yLength; j++) {
                _tiles[i, j].IsDirty = false;
            }
        }
        
        for (int i = 0; i < xLength; i++) {
            for (int j = 0; j < yLength; j++) {
                _tiles[i, j].Update();
            }
        }
    }

    public void ChangeSprite(SpriteRenderer spriteRenderer) {
        /*foreach (var tile in _dirtyTiles) {
            spriteRenderer.sprite.texture.SetPixel(Random.Range(0, 200), Random.Range(0, 200), Random.ColorHSV());
        }*/

        int xLength = _tiles.GetLength(0);
        int yLength = _tiles.GetLength(1);

        for (int i = 0; i < xLength; i++) {
            for (int j = 0; j < yLength; j++) {
                if (!_tiles[i, j].IsDirty) {
                    continue;
                }
                
                Color color = GetTileColor(_tiles[i, j]);
                spriteRenderer.sprite.texture.SetPixel(i, j, color);
            }
        }

        spriteRenderer.sprite.texture.Apply();
        ui.SetStats(PredatorNum, PreyNum);
    }

    private Color GetTileColor(Tile t) {
        TileType type = t.Type;
        if (type == TileType.Predator) {
            return Color.red;
        }

        if (type == TileType.Prey) {
            return Color.green;
        }
        
        return Color.black;
    }

    public List<Tile> GetNeighbours(Vector2Int pos) {
        List<Tile> tiles = new List<Tile>();
        
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) continue;

                int x = pos.x + i;
                int y = pos.y + j;
                
                if (x < 0 || y < 0 || x >= _tiles.GetLength(0) || y >= _tiles.GetLength(1)) continue;
                
                tiles.Add(_tiles[x, y]);
            }
        }

        return tiles;
    }

    public void SetTileTo(int x, int y, TileType type) {
        if (type == TileType.Predator) {
            _tiles[x, y].SetToNewPredator();
        } else if (type == TileType.Prey) {
            _tiles[x, y].SetToNewPrey();
        }
    }
}
