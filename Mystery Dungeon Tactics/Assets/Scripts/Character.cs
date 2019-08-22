using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Character {
	public string Name { get; set; }
	public int Speed { get; set; }
	public int Id { get; set; }
	public int Movement { get; set; }

	public MapPosition Position { get; set; }

	public GameObject Parent { get; set; }

    public bool IsControllable { get; set; }

    public Character(string name, int speed, int id, int movement, GameObject parent, bool isControllable) {
		Name = name;
		Speed = speed;
		Id = id;
		Movement = movement;
		Parent = parent;
        IsControllable = isControllable;
	}
}