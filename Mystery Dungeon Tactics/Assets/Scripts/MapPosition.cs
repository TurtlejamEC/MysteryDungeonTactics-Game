using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MapPosition {
	public int X { get; set; }
	public int Z { get; set; }

	public MapPosition(int x, int z) {
		X = x;
		Z = z;
	}

	public static MapPosition operator +(MapPosition a, MapPosition b) {
		return new MapPosition(a.X + b.X, a.Z + b.Z);
	}

	public override string ToString() {
		return $"({X}, {Z})";
	}
}