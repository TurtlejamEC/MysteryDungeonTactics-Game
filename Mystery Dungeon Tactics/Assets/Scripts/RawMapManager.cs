using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

	// Mode: 0 = TileId, 1 = CharacterId
	public void PrintMap(int mode) {
		string arrayString = "";
		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				arrayString += string.Format("{0} ", mode == 0 ? Map[i][j].TileId : Map[i][j].CharacterId);
			}
			arrayString += System.Environment.NewLine + System.Environment.NewLine;
		}
		Debug.Log(arrayString);
	}

	public void showCharacterIds() {
		for (int i = 0; i < Map.Length; i++) {
			for (int j = 0; j < Map[i].Length; j++) {
				if (Map[i][j].CharacterId == -1) {
					Gizmos.color = Color.yellow;
				} else {
					Gizmos.color = Color.blue;
				}
				
				Gizmos.DrawWireCube(new Vector3(j, 0, i), Vector3.one);
			}
		}
	}

	private void OnDrawGizmos() {
		showCharacterIds();
	}

	public static MapPosition FindEmptyFloorPosition() {
		int x = Random.Range(0, Map[0].Length - 1);
		int z = Random.Range(0, Map.Length - 1);
		while (Map[z][x].TileId == 1 || Map[z][x].CharacterId != -1) {
			x = Random.Range(0, Map[0].Length - 1);
			z = Random.Range(0, Map.Length - 1);
		}
		return new MapPosition(x, z);
	}

}
