using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
	public static Dictionary<int, Character> ActiveCharacters = new Dictionary<int, Character>();

	public static void AddCharacter(Character newCharacter) {
		ActiveCharacters.Add(newCharacter.Id, newCharacter);
	}

	public static void ChangeEnergy(int characterId, int change) {
		Character current = ActiveCharacters[characterId];
		current.CurrentEnergy = Math.Min(current.CurrentEnergy + change, current.MaxEnergy);
	}

	public void Start() {
	}
}
