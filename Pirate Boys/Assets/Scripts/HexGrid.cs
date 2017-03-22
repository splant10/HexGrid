using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

//[ExecuteInEditMode]
public class HexGrid : MonoBehaviour 
{
	public int gridWidth = 15;
	public int gridHeight = 15;

	public Transform hexPrefab;
	public Material landMaterial;
	public Material tier3Material;
	public Material tier2Material;
	public int islandDepth = 1;
	public int tier3Depth = 2;
	public int tier2Depth = 3;

	public bool trim = true;

	float hexWidth = 1.732f;
	float hexHeight = 2.0f;
	public float gap = 0.0f;

	Vector3 startPos;

	Transform[][] hexes;
	GameObject centerHex;

	Vector2[] tier2tiles;
	Vector2[] tier3tiles;

	void Start()
	{
		hexes = new Transform[gridHeight][];
		tier2tiles = new Vector2[0];
		tier3tiles = new Vector2[0];
		AddGap();
		CalcStartPos();
		CreateGrid();
	}

	void AddGap()
	{
		hexWidth += hexWidth * gap;
		hexHeight += hexHeight * gap;
	}

	void CalcStartPos()
	{
		float offset = 0;
		if (gridHeight / 2 % 2 != 0)
			offset = hexWidth / 2;

		float x = -hexWidth * (gridWidth / 2) - offset;
		float z = hexHeight * 0.75f * (gridHeight / 2);

		startPos = new Vector3(x, 0, z);
	}

	Vector3 CalcWorldPos(Vector2 gridPos)
	{
		float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;

		float x = startPos.x + gridPos.x * hexWidth + offset;
		float z = startPos.z - gridPos.y * hexHeight * 0.75f;

		return new Vector3(x, 0, z);
	}

	void CreateGrid()
	{
		Transform hex;
		for (int y = 0; y < gridHeight; y++)
		{
			Transform[] row = new Transform[gridWidth];
			for (int x = 0; x < gridWidth; x++)
			{
				Vector2 h = new Vector2 (x, y);
				hex = Instantiate (hexPrefab) as Transform;
				hex.GetComponent<Hexagon> ().setHexagon (h);

				Vector2 gridPos = new Vector2(x, y);
				hex.position = CalcWorldPos(gridPos);
				hex.parent = this.transform;
				hex.name = "Hexagon" + x + "|" + y;

				row [x] = hex;
			}
			hexes [y] = row;
		}
		if (trim) {
			if (gridWidth >= gridHeight) {
				for (int i = 0; i < gridHeight; i += 2) {
					Destroy (hexes [i] [0].gameObject);
				}
			} else {
				for (int i = 0; i < gridWidth; i += 2) {
					Destroy (hexes [i] [0].gameObject);
				}
			}
		}
		populateIsland ();
		populateTier3 ();
		populateTier2 ();
		populateTier1 ();

		GameObject.Find ("GameManager").GetComponent<GameManager> ().Create ();
	}

	GameObject getCenterHex() {
		int centerX = (int) Mathf.Floor (gridWidth / 2);
		int centerY = (int) Mathf.Floor (gridHeight / 2);
		return hexes [centerY] [centerX].gameObject;
	}

	void populateIsland() {
		centerHex = getCenterHex();
		Debug.Log ("Center: " + centerHex);
		centerHex.GetComponent<Renderer> ().material = landMaterial;

		int n = 1;
		while (n < islandDepth + 1) {
			populateRing (landMaterial, n);
			n += 1;
		}
	}

	void populateTier3() {
		int n = islandDepth + 1;
		int top = n + tier3Depth;
		while (n < top) {
			Vector2[] newRing = populateRing (tier3Material, n);
			Vector2[] newTier3 = new Vector2[tier3tiles.Length + newRing.Length];

			tier3tiles.CopyTo(newTier3, 0);
			newRing.CopyTo(newTier3, tier3tiles.Length);
			tier3tiles = newTier3;
			n += 1;
		}

	}

	void populateTier2() {
		int n = islandDepth + tier3Depth + 1;
		int top = n + tier2Depth;
		while (n < top) {
			Vector2[] newRing = populateRing (tier2Material, n);
			Vector2[] newTier2 = new Vector2[tier2tiles.Length + newRing.Length];

			tier2tiles.CopyTo(newTier2, 0);
			newRing.CopyTo(newTier2, tier2tiles.Length);
			tier2tiles = newTier2;
			n += 1;
		}
	}

	void populateTier1 () {

	}

	Vector2[] populateRing(Material mat, int ringNum) {

		Vector2[] hexagonCoords = new Vector2 [ringNum * 6];

		Hexagon currHexagon = getCenterHex ().GetComponent<Hexagon> ();
		int n = 0;
		while (n < ringNum) { // move to start hexagon
			Vector2 next = currHexagon.getNext (5); // move west until we get to our ring start
			currHexagon.setHexagon(next);
			n += 1;
		}

		int index = 0;
		for (int dir = 1; dir <= 6; ++dir) { // turn six times
			for (int count = 0; count < ringNum; ++count) {
				hexagonCoords [index] = currHexagon.getCoords();
				Vector2 next = currHexagon.getNext (dir);
				currHexagon.setHexagon (next);
				index += 1;
			}
		}

		// Reset hex coords to center
		int centerX = (int) Mathf.Floor (gridWidth / 2);
		int centerY = (int) Mathf.Floor (gridHeight / 2);
		currHexagon.setHexagon(new Vector2(centerX, centerY));


		foreach (var coord in hexagonCoords) {
			int x = (int) coord.x;
			int y = (int) coord.y;
			try {
				hexes [y] [x].gameObject.GetComponent<Renderer>().material = mat;
			} catch (Exception e) {
				// can't access that hex.
			}
		}
		return hexagonCoords;

	}

	public Vector2[] GetTierTiles(int tier) {
		switch (tier) {
		case 1:
			List<Vector2> allTiles = new List<Vector2> ();
			for (int x = 0; x < gridWidth; ++x) {
				for (int y = 0; y < gridHeight; ++y) {
					allTiles.Add (new Vector2 (x, y));
				}
			}
			foreach (Vector2 tile in allTiles) {
				if (tier2tiles.Contains (tile) || tier3tiles.Contains(tile)) {
					allTiles.Remove (tile);
				}
			}
			return allTiles.ToArray();
		case 2:
			return this.tier2tiles;
		case 3:
			return this.tier3tiles;
		default:
			return new Vector2[0];
		}
	}

	public Transform[][] getHexes() {
		return hexes;
	}

}