using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[Serializable]
public class Ability {

    public enum ElementId {
        Air,
        Darkness,
        Electricity,
        Fire,
        Ground,
        Ice,
        Neutral,
        Plant,
        Water,
    };

    public enum ModifierModeId {
        Add,
        Multiply
    }

    public enum ShapeId {
        Circle,
        Line
    }

    public enum EffectId {
        Absorption,
        Burn,
        Confidence,
        Fatigue,
        Immobilized,
        Poison,
        Rapidity,
        Regeneration,
        Resilience,
        Slow,
        Vulnerability,
        Weakness,
    }

    public int id;
    public string name;
    public int element;
    public int modifierMode;
    public int modifierValue;
    public int range;
    public int shapeType;
    public int shapeSize;
    public int[] effect;

    public Ability(int id, string name, int element, int modifierMode, int modifierValue, int range, int shapeType, int shapeSize, int[] effect) {
        this.id = id;
        this.name = name;
        this.element = element;
        this.modifierMode = modifierMode;
        this.modifierValue = modifierValue;
        this.range = range;
        this.shapeType = shapeType;
        this.shapeSize = shapeSize;
        this.effect = effect;
    }

//
//    public override String ToString() {
//        return $"Name: {Name}";
//    }
}