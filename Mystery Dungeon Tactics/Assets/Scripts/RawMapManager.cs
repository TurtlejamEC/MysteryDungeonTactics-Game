using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawMapManager : MonoBehaviour {
	public enum TileTypes {
		Floor,
		Wall
	};

	public static MapTile[][] Map { get; set; }
	public TextAsset mapLayout;
	private string[] _mapRows;
	public List<GameObject> mapTiles = new List<GameObject>();

	public void GenerateMap() {
		_mapRows = mapLayout.text.Split('\n');
		Map = new MapTile[_mapRows.Length][];
		for (int i = 0; i < Map.Length; i++) {
			Map[i] = new MapTile[_mapRows[i].Length];

			for (int j = 0; j < Map[i].Length; j++) {
				Map[i][j] = new MapTile(_mapRows[i][j] - '0', -1);
			}
		}

		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				Instantiate(mapTiles[Map[i][j].TileId], new Vector3(j, 0, i), Quaternion.identity);
			}
		}
	}

	public void PrintMap() {
		string arrayString = "";
		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				arrayString += string.Format("{0} ", Map[i][j].TileId);
			}
			arrayString += System.Environment.NewLine + System.Environment.NewLine;
		}
		Debug.Log(arrayString);
	}

	public static MapPosition FindEmptyFloorPosition() {
		int x = Random.Range(0, Map[0].Length - 1);
		int z = Random.Range(0, Map.Length - 1);
		while (Map[z][x].TileId == 1) {
			x = Random.Range(0, Map[0].Length - 1);
			z = Random.Range(0, Map.Length - 1);
		}
		return new MapPosition(x, z);
	}

}
