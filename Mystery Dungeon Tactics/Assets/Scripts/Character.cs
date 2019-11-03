using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Character {
	public string Name { get; set; }
	public int Speed { get; set; }
	public int Id { get; set; }
	public int Movement { get; set; }
	public int Attack { get; set; }
	public int Defense { get; set; }
	public int CurrentHp { get; set; }
	public int MaxHp { get; set; }
	public int CurrentEnergy { get; set; }
	public int MaxEnergy { get; set; }

	public MapPosition Position { get; set; }

	public GameObject Parent { get; set; }

    public bool IsControllable { get; set; }

    public Character(string name, int speed, int id, int movement, int attack, int defense, int hp, int energy, GameObject parent, bool isControllable) {
		Name = name;
		Speed = speed;
		Id = id;
		Movement = movement;
		Attack = attack;
		Defense = defense;
		MaxHp = CurrentHp = CharacterManager.HpCurve(hp);
		MaxEnergy = CurrentEnergy = CharacterManager.EnergyCurve(energy);
		Parent = parent;
        IsControllable = isControllable;
	}
    
    public override String ToString() {
	    return $"Name: {Name}, Attack: {Attack}, Defense: {Defense}, HP: {CurrentHp}/{MaxHp}, Energy: {CurrentEnergy}/{MaxEnergy}";
    }
}