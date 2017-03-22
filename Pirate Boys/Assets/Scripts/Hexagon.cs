using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour {

	public int x;
	public int y;

	public Hexagon(Vector2 coordinate) {
		this.x = (int) coordinate.x;
		this.y = (int) coordinate.y;
	}

	public void setHexagon(Vector2 vec) {
		this.x = (int) vec.x;
		this.y = (int) vec.y;
	}

	public Vector2 getNext(int direction) {
		
		int offsetX;
		offsetX = ((this.y % 2) == 0) ? this.x : this.x+1;

		// starting from W going NE
		switch (direction) {
		case 1: // NE
			return new Vector2(offsetX-1, this.y+1);
		case 2: // E
			return new Vector2(this.x-1, this.y);
		case 3: // SE
			return new Vector2(offsetX-1, this.y-1);
		case 4: // SW
			return new Vector2(offsetX, this.y-1);
		case 5: // W
			return new Vector2(this.x+1, this.y);
		case 6: // NW
			return new Vector2(offsetX, this.y+1);
		default:
			return new Vector2 (0, 0);
		}
	}

	public Vector2 getCoords() {
		return new Vector2 (x, y);
	}
}
