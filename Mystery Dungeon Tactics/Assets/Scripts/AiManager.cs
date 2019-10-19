using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiManager : MonoBehaviour {
    public static void CompleteTurn(int characterId) {
	    if (CharacterManager.ActiveCharacters[characterId].CurrentEnergy == 0) {
		    CharacterManager.ChangeEnergy(characterId, (int) (CharacterManager.ActiveCharacters[characterId].MaxEnergy * 0.5f));
	    } else {
		    Move(characterId);
	    }
    }

    private static void Move(int characterId) {
	    // Find spot to move to that is open and doesn't have anybody else.
	    Character current = CharacterManager.ActiveCharacters[characterId];

	    List<AlgorithmTile> possiblePositions = MapAlgorithms.ResultMapToAlgorithmTileList(
		    MapAlgorithms.BfsReturnRange(RawMapManager.Map, current.Position, Math.Min(current.Movement, current.CurrentEnergy), true, true)
		    );

	    // Randomize list
	    for (int i = possiblePositions.Count - 1; i >= 1; i--) {
		    int swap = Random.Range(0, i);
		    AlgorithmTile tmp = possiblePositions[i];
		    possiblePositions[i] = possiblePositions[swap];
		    possiblePositions[swap] = tmp;
	    }

	    MapPosition newPosition = current.Position;
	    for (int i = 0; i < possiblePositions.Count; i++) {
		    if (RawMapManager.Map[possiblePositions[i].Position.Z][possiblePositions[i].Position.X].TileId == 0 &&
		        RawMapManager.Map[possiblePositions[i].Position.Z][possiblePositions[i].Position.X].CharacterId == -1) {
			    newPosition = possiblePositions[i].Position;
			    break;
		    }
	    }

/*	    string message = current.Position.ToString();
	    message += Environment.NewLine;
	    foreach (var i in possiblePositions) {
		    message += $", {i}";
	    }
	    print(message);*/

	    ActionManager.MoveCharacter(characterId, newPosition);
    }
}
