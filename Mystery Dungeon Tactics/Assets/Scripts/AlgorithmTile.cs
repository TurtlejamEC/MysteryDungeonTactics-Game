using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class AlgorithmTile {
    public MapPosition Position { get; set; }
    public bool Visited { get; set; }
    public int Distance { get; set; }

    public AlgorithmTile(MapPosition position, bool visited, int distance) {
        Position = position;
        Visited = visited;
        Distance = distance;
    }

    public override string ToString() {
        return Position.ToString();
    }
}