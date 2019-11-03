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

	public static void ChangeHp(int characterId, int change) {
		Character current = ActiveCharacters[characterId];
		current.CurrentHp = Math.Min(current.CurrentHp + change, current.MaxHp);
	}

	public static void ChangeEnergy(int characterId, int change) {
		Character current = ActiveCharacters[characterId];
		current.CurrentEnergy = Math.Min(current.CurrentEnergy + change, current.MaxEnergy);
	}

	public static int AttackDefenseCurve(int level) {
		return (int) (Mathf.Pow(level, 1.5f));
	}

	public static int DamageCurve(int attackLevel, int defenseLevel) {
		print("hi" + attackLevel);
		print(AttackDefenseCurve(attackLevel));
		print(AttackDefenseCurve(defenseLevel));
		print((int)((float)(AttackDefenseCurve(attackLevel) * AttackDefenseCurve(attackLevel)) /
		            (AttackDefenseCurve(attackLevel) + AttackDefenseCurve(defenseLevel))));
		return (int)((float)(AttackDefenseCurve(attackLevel) * AttackDefenseCurve(attackLevel)) /
		       (AttackDefenseCurve(attackLevel) + AttackDefenseCurve(defenseLevel)));
	}

	public static int HpCurve(int level) {
		return (int) (Mathf.Pow(level, Mathf.Log(9980, 100)) + 20);
	}

	public static int EnergyCurve(int level) {
		return (int) (Mathf.Pow(level, Mathf.Log(9, 100) + 1) + 100);
	}
}
