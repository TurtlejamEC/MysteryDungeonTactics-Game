using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private void Start() {
        
        
    }
    
    public static void DealDamage(int sourceCharacterId, int targetCharacterId) {
        int damage = CharacterManager.DamageCurve(CharacterManager.ActiveCharacters[sourceCharacterId].Attack,
            CharacterManager.ActiveCharacters[targetCharacterId].Defense);
        CharacterManager.ChangeHp(targetCharacterId, -damage);
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

    public static List<AlgorithmTile> GetPossiblePositions(int characterId) {
        Character current = CharacterManager.ActiveCharacters[characterId];

        if (UiManager.selectedAbilityId == -1) { // Move
            return MapAlgorithms.ResultMapToAlgorithmTileList(
                MapAlgorithms.BfsReturnRange(RawMapManager.Map, current.Position, Math.Min(current.Movement, current.CurrentEnergy), true, true)
            );
        } else if (UiManager.selectedAbilityId == 0) {
            return MapAlgorithms.ResultMapToAlgorithmTileList(
                MapAlgorithms.BfsReturnRange(RawMapManager.Map, current.Position, 1, false, true)
            );
        } else {
            return MapAlgorithms.ResultMapToAlgorithmTileList(
                MapAlgorithms.BfsReturnRange(RawMapManager.Map, current.Position, 2, false, true)
            );
        }
    }
}
