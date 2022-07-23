using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile {
    private TileType _tileType;

    public TileType Type {
        get => _tileType;
        set {
            _tileType = value;
            
            _skipTurn = true;
            IsDirty = true;
        }
    }

    public Vector2Int Position { get; private set; }
    protected readonly GameField gameField;

    public int Life { get; set; }
    private const int MaxLife = 75;

    private bool _skipTurn;

    private List<Tile> _neighbours;
    
    public bool IsDirty;

    public Tile(Vector2Int pos, GameField gameField, TileType type) {
        Position = pos;
        this.gameField = gameField;

        Type = type;

        if (Type == TileType.Predator) Life = MaxLife;
    }

    public void SetNeighbours() {
        _neighbours = gameField.GetNeighbours(Position);
    }

    public void Update() {
        if (Type == TileType.Empty || _skipTurn) {
            _skipTurn = false;
            return;
        }

        switch (Type) {
            case TileType.Prey:
                UpdatePrey();
                break;
            case TileType.Predator:
                UpdatePredator();
                break;
        }
    }

    private void UpdatePrey() {
        Life++;
        if (Life > MaxLife) {
            Tile t = GetRandomNeighbourOfType(TileType.Empty);

            if (t != null) {
                Life = 0;

                t.Type = TileType.Prey;
                t.Life = 0;

                gameField.PreyNum++;
            }
        }

        Tile newTile = GetRandomNeighbourOfType(TileType.Empty);

        if (newTile != null) {
            Type = TileType.Empty;

            newTile.Type = TileType.Prey;
            newTile.Life = Life;
        }
    }

    private void UpdatePredator() {
        Life--;
        if (Life < 0) {
            Type = TileType.Empty;
            gameField.PredatorNum--;
        } else {
            Tile prey = GetRandomNeighbourOfType(TileType.Prey);

            if (prey != null) {
                Life = MaxLife;

                prey.Type = TileType.Predator;
                prey.Life = MaxLife;

                gameField.PreyNum--;
                gameField.PredatorNum++;
            }


            Tile empty = GetRandomNeighbourOfType(TileType.Empty);

            if (empty != null) {
                empty.Type = TileType.Predator;
                empty.Life = Life;

                Type = TileType.Empty;
            }
        }
    }
    
    public Tile GetRandomNeighbourOfType(TileType type) {
        int i = 0;
        while (i++ < 8) {
            int n = Random.Range(0, _neighbours.Count);

            if (_neighbours[n].Type == type) return _neighbours[n];
        }

        return null;
    }

    public void SetToNewPrey() {
        Life = 0;
        Type = TileType.Prey;

        gameField.PreyNum++;
    }
    
    public void SetToNewPredator() {
        Life = MaxLife;
        Type = TileType.Predator;
        
        gameField.PredatorNum++;
    }
}

public enum TileType {
    Empty,
    Prey,
    Predator
}