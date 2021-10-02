using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VisualEffectsTileMap : MonoBehaviour {
    [SerializeField] private Tilemap _tilemap;

    public void SetTilemapColor(Color color) {
        _tilemap.color = color;
    }

    public void RemoveHighlightFromNodes(Vector3[] nodePositions) {
        HighlightNodes(nodePositions, null);
    }

    public void ClearAllTiles() {
        _tilemap.ClearAllTiles();
    }

    public void HighlightNodes(Vector3[] nodePositions, TileBase tileToUse) {
        var tileCellPositions = new List<Vector3Int>();
        foreach (var nodePosition in nodePositions) {
            var cellPosition = _tilemap.WorldToCell(nodePosition);
            tileCellPositions.Add(cellPosition);
        }

        var tiles = new TileBase[tileCellPositions.Count];
        for (var i = 0; i < tiles.Length; i++) {
            tiles[i] = tileToUse;
        }
        _tilemap.SetTiles(tileCellPositions.ToArray(), tiles);
    }
}