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
	List<Vector2> tier1tiles = new List<Vector2> ();
	List<Vector2> tier2tiles = new List<Vector2>();
	List<Vector2> clickedTier2 = new List<Vector2> ();
	List<Vector2> tier3tiles = new List<Vector2>();
	List<Vector2> clickedTier3 = new List<Vector2> ();
	List<Vector2> clickedWater = new List<Vector2> ();

	public void Create() {
		hexGrid = GameObject.Find ("HexGridCreator").GetComponent<HexGrid>();
		Vector2[] tier1Array = hexGrid.GetTierTiles (1);
		Vector2[] tier2Array = hexGrid.GetTierTiles (2);
		Vector2[] tier3Array = hexGrid.GetTierTiles (3);

		foreach (Vector2 hex in tier1Array) {
			tier1tiles.Add (hex);
		}
		foreach (Vector2 hex in tier2Array) {
			tier2tiles.Add (hex);
		}
		foreach (Vector2 hex in tier3Array) {
			tier3tiles.Add (hex);
		}

	}

	public void OnClickHex(Vector2 hex) {
		GameObject clickedHex = hexGrid.getHexes () [(int) hex.y] [(int) hex.x].gameObject;

		if (tier1tiles.Contains (hex)) {
			tier1tiles.Remove (hex);
			RevealLocation (clickedHex, 1);
		} else if (tier2tiles.Contains (hex)) {
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
		int pieceSum;
		int totalLeft;
		int randTile;

		switch (tier) {
		case 1:
			// tier1 stuff
			pieceSum = tier1Monsters + tier1WreckedShips + tier1Storms + tier1Lands;
			totalLeft = tier1tiles.Count;
			if (pieceSum > totalLeft) {
				Debug.Log ("Too many pieces!");
				break;
			}
			randTile = Random.Range (0, totalLeft);
			if (randTile < tier1Monsters) {
				hexObj.GetComponent<Renderer> ().material = monsterMaterial;
				tier1Monsters -= 1;
				Debug.Log ("Monster");
			} else if (randTile < tier1Monsters + tier1WreckedShips) {
				hexObj.GetComponent<Renderer> ().material = wreckedShipMaterial;
				tier1WreckedShips -= 1;
				Debug.Log ("Wrecked Ship");
			} else if (randTile < tier1Monsters + tier1WreckedShips + tier1Storms) {
				hexObj.GetComponent<Renderer> ().material = stormMaterial;
				tier1Storms -= 1;
				Debug.Log ("Storm");
			} else if (randTile < tier1Monsters + tier1WreckedShips + tier1Storms + tier1Lands) {
				hexObj.GetComponent<Renderer> ().material = landMaterial;
				tier1Lands -= 1;
				Debug.Log ("Land");
			} else {
				//Shader beenToShader = new Shader()
				hexObj.GetComponent<Renderer> ().material = waterMaterial;
				Debug.Log ("Water");
			}
			break;
		case 2:
			//tier 2 stuff
			pieceSum = tier2Monsters + tier2WreckedShips + tier2Storms + tier2Lands;
			totalLeft = tier2tiles.Count;
			if (pieceSum > totalLeft) {
				Debug.Log ("Too many pieces!");
				break;
			}
			randTile = Random.Range (0, totalLeft);
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
				//Shader beenToShader = new Shader()
				hexObj.GetComponent<Renderer> ().material = waterMaterial;
				Debug.Log ("Water");
			}
			break;
		case 3:
			// tier 3 stuff
			pieceSum = tier3Monsters + tier3WreckedShips + tier3Storms + tier3Lands;
			totalLeft = tier3tiles.Count;
			if (pieceSum > totalLeft) {
				Debug.Log ("Too many pieces!");
				break;
			}
			randTile = Random.Range (0, totalLeft);
			if (randTile < tier3Monsters) {
				hexObj.GetComponent<Renderer> ().material = monsterMaterial;
				tier3Monsters -= 1;
				Debug.Log ("Monster");
			} else if (randTile < tier3Monsters + tier3WreckedShips) {
				hexObj.GetComponent<Renderer> ().material = wreckedShipMaterial;
				tier3WreckedShips -= 1;
				Debug.Log ("Wrecked Ship");
			} else if (randTile < tier3Monsters + tier3WreckedShips + tier3Storms) {
				hexObj.GetComponent<Renderer> ().material = stormMaterial;
				tier3Storms -= 1;
				Debug.Log ("Storm");
			} else if (randTile < tier3Monsters + tier3WreckedShips + tier3Storms + tier3Lands) {
				hexObj.GetComponent<Renderer> ().material = landMaterial;
				tier3Lands -= 1;
				Debug.Log ("Land");
			} else {
				hexObj.GetComponent<Renderer> ().material = waterMaterial;
				Debug.Log ("Water");
			}
			break;
		default:
			return;
		}
	}
}
