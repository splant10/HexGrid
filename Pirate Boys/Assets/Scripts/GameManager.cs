using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int tier1Monsters;
	public int tier1WreckedShips;
	public int tier1Storms;
	public int tier1Lands;

	public int tier2Monsters;
	public int tier2WreckedShips;
	public int tier2Storms;
	public int tier2Lands;

	public int tier3Monsters;
	public int tier3WreckedShips;
	public int tier3Storms;
	public int tier3Lands;

	public Material monsterMaterial;
	public Material wreckedShipMaterial;
	public Material stormMaterial;
	public Material waterMaterial;
	public Material landMaterial;

	HexGrid hexGrid;
	List<Vector2> tier2tiles = new List<Vector2>();
	List<Vector2> clickedTier2 = new List<Vector2> ();
	List<Vector2> tier3tiles = new List<Vector2>();
	List<Vector2> clickedTier3 = new List<Vector2> ();
	List<Vector2> clickedWater = new List<Vector2> ();

	public void Create() {
		hexGrid = GameObject.Find ("HexGridCreator").GetComponent<HexGrid>();
		Vector2[] tier2Array = hexGrid.GetTierTiles (2);
		Vector2[] tier3Array = hexGrid.GetTierTiles (3);

		foreach (Vector2 hex in tier2Array) {
			tier2tiles.Add (hex);
		}
		foreach (Vector2 hex in tier3Array) {
			tier3tiles.Add (hex);
		}

	}

	public void OnClickHex(Vector2 hex) {
		GameObject clickedHex = hexGrid.getHexes () [(int) hex.y] [(int) hex.x].gameObject;

		if (tier2tiles.Contains (hex)) {
			clickedTier2.Add (hex);
			tier2tiles.Remove (hex);
			RevealLocation (clickedHex, 2);

		} else if (tier3tiles.Contains (hex)) {
			clickedTier3.Add (hex);
			tier3tiles.Remove (hex);
			RevealLocation (clickedHex, 3);

		} else {
			if (!clickedWater.Contains (hex)) {
				clickedWater.Add (hex);
				RevealLocation (clickedHex, 1);
			}
		}
	}

	public void RevealLocation(GameObject hexObj, int tier) {
		switch (tier) {
		case 1:
				//water stuff
			break;
		case 2:
			//tier 2 stuff
			int pieceSum = tier2Monsters + tier2WreckedShips + tier2Storms;
			int tier2Left = tier2tiles.Count;
			if (pieceSum > tier2Left) {
				Debug.Log ("Too many pieces!");
				break;
			}

			int randTile = Random.Range (0, tier2Left);
			if (randTile < tier2Monsters) {
				hexObj.GetComponent<Renderer> ().material = monsterMaterial;
				tier2Monsters -= 1;
				Debug.Log ("Monster");
			} else if (randTile < tier2Monsters + tier2WreckedShips) {
				hexObj.GetComponent<Renderer> ().material = wreckedShipMaterial;
				tier2WreckedShips -= 1;
				Debug.Log ("Wrecked Ship");
			} else if (randTile < tier2Monsters + tier2WreckedShips + tier2Storms) {
				hexObj.GetComponent<Renderer> ().material = stormMaterial;
				tier2Storms -= 1;
				Debug.Log ("Storm");
			} else if (randTile < tier2Monsters + tier2WreckedShips + tier2Storms + tier2Lands) {
				hexObj.GetComponent<Renderer> ().material = landMaterial;
				tier2Lands -= 1;
				Debug.Log ("Land");
			} else {
				hexObj.GetComponent<Renderer> ().material = waterMaterial;
				Debug.Log ("Water");
			}
			break;
		case 3:
				// tier 3 stuff
			break;
		default:
			break;
		}
	}
}
