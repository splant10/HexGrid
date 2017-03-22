using UnityEngine;
using System.Collections.Generic;
using System;

public class HexGrid : MonoBehaviour 
{
	public Transform hexPrefab;
	public Material landMaterial;
	public Material tier3Material;
	public Material tier2Material;
	public int islandDepth = 1;
	public int tier3Depth = 2;
	public int tier2Depth = 3;

	public int gridWidth = 15;
	public int gridHeight = 15;

	float hexWidth = 1.732f;
	float hexHeight = 2.0f;
	public float gap = 0.0f;

	Vector3 startPos;

	Transform[][] hexes;
	GameObject centerHex;

	void Start()
	{
		hexes = new Transform[gridHeight][];
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
		populateIsland ();
		populateTier3 ();
		populateTier2 ();
	}

	GameObject getCenterHex() {
		int centerX = (int) Mathf.Floor (gridWidth / 2);
		int centerY = (int) Mathf.Floor (gridHeight / 2);
		return hexes [centerX] [centerY].gameObject;
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
			Debug.Log (n);
			populateRing (tier3Material, n);
			n += 1;
		}

	}

	void populateTier2() {
		int n = islandDepth + tier3Depth + 1;
		int top = n + tier2Depth;
		while (n < top) {
			populateRing (tier2Material, n);
			n += 1;
		}
	}

	void populateRing(Material mat, int ringNum) {

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
			Debug.Log (coord);
		}

	}

}