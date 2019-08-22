using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public static class MapAlgorithms {
    public static AlgorithmTile[][] BfsReturnRange (MapTile[][] map, MapPosition start, int range) {
        // Set up resultMap board
        AlgorithmTile[][] resultMap = new AlgorithmTile[map.Length][];
        for (int i = 0; i < map.Length; i++) {
            resultMap[i] = new AlgorithmTile[map[i].Length];
        }

        for (int z = 0; z < resultMap.Length; z++) {
            for (int x = 0; x < resultMap[z].Length; x++) {
                resultMap[z][x] = new AlgorithmTile(new MapPosition(x, z), false, -1);
            }
        }
        
        // BFS
        MapPosition[] cardinalDirections = {
            new MapPosition(0, 1),
            new MapPosition(1, 0),
            new MapPosition(0, -1),
            new MapPosition(-1, 0)
        };
        Queue<MapPosition> nextPositions = new Queue<MapPosition>();
        resultMap[start.Z][start.X].Distance = 0;
        resultMap[start.Z][start.X].Visited = true;
        nextPositions.Enqueue(start);
        while (nextPositions.Any()) {
            MapPosition current = nextPositions.Dequeue();

            // Check to see if the current position is the furthest possible distance from start
            if (resultMap[current.Z][current.X].Distance >= range) {
                int dist = resultMap[current.Z][current.X].Distance;
                continue;
            }

            for (int i = 0; i < 4; i++) {
                MapPosition considering = current + cardinalDirections[i];
                
                // Outside map?
                if (considering.X < 0 || considering.X >= resultMap[0].Length ||
                    considering.Z < 0 || considering.Z >= resultMap.Length) {
                    continue;
                }

                // Already visited?
                if (resultMap[considering.Z][considering.X].Visited) {
                    continue;
                }

                resultMap[considering.Z][considering.X].Visited = true;
                resultMap[considering.Z][considering.X].Distance = resultMap[current.Z][current.X].Distance + 1;
                nextPositions.Enqueue(considering);
            }
        }

        return resultMap;
    }

    public static List<AlgorithmTile> ResultMapToAlgorithmTileList(AlgorithmTile[][] map) {
        List<AlgorithmTile> result = new List<AlgorithmTile>();
        for (int z = 0; z < map.Length; z++) {
            for (int x = 0; x < map[z].Length; x++) {
                if (map[z][x].Visited) {
                    result.Add(map[z][x]);
                }
            }
        }

        return result;
    }
}