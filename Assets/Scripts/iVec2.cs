using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iVec2 {
	public int x = 0;
	public int y = 0;

	public iVec2( int _x = 0, int _y = 0 ){
		x = _x;
		y = _y;
	}

	public override bool Equals (object obj)
	{
		if (obj.GetType () == typeof(iVec2)) {
			return (x == ((iVec2)obj).x) && (y == ((iVec2)obj).y);
		} else {
			return false;
		}
	}

	static public iVec2 operator + ( iVec2 a, iVec2 b ){
		return new iVec2( a.x+b.x, a.y + b.y );
	}

	static public bool operator == (iVec2 a, iVec2 b){
		return (a.x == b.x) && (a.y == b.y);
	}

	static public bool operator != (iVec2 a, iVec2 b){
		return (a.x != b.x) || (a.y != b.y);
	}
}