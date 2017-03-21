using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon {

	private int x;
	private int y;

	public Hexagon(Vector2 coordinate) {
		this.x = coordinate.x;
		this.y = coordinate.y;
	}

	public Hexagon getNext(int direction) {
		
		int offsetX;
		offsetX = ((this.y % 2) == 0) ? this.x : this.x+1;

		switch (direction) {
		case 1: // NW
			return new Hexagon(new Vector2(offsetX, this.y+1));
			break;
		case 2: // NE
			return new Hexagon(new Vector2(offsetX-1, this.y+1));
			break;
		case 3: // E
			return new Hexagon(new Vector2(this.x-1, this.y));
			break;
		case 4: // SE
			return new Hexagon(new Vector2(offsetX-1, this.y-1));
			break;
		case 5: // SW
			return new Hexagon(new Vector2(offsetX, this.y-1));
			break;
		case 6: // W
			return new Hexagon(new Vector2(this.x+1, this.y));
			break;
		}
	}
}
