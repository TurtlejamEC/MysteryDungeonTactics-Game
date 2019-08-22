using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MapTile {
	// Contains tile ID to describe what kind of tile it is
	public int TileId { get; set; }
	
	// -1 means no character. Else, it is a character ID at tile
	public int CharacterId { get; set; }

	public MapTile(int tileId, int characterId) {
		TileId = tileId;
		CharacterId = characterId;
	}
}