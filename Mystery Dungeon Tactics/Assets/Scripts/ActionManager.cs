using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private void Start() {
        
        
    }

    public static void MoveCharacter(int characterId, MapPosition destination) {
        Character current = CharacterManager.ActiveCharacters[characterId];
        MapPosition previousPosition = current.Position;
        
        RawMapManager.Map[current.Position.Z][current.Position.X].CharacterId = -1;
        RawMapManager.Map[destination.Z][destination.X].CharacterId = characterId;
        CharacterManager.ActiveCharacters[characterId].Position = destination;
        CharacterManager.ActiveCharacters[characterId].Parent.transform.position = new Vector3(destination.X, 0, destination.Z);

        CharacterManager.ActiveCharacters[characterId].CurrentEnergy -=
            MapAlgorithms.ManhattanDistance(previousPosition, destination);
    }
}
