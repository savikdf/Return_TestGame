using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell : MonoBehaviour {
	public int cellWeight = 0;

	public List<Transform> Adjacents;
	public List<Transform> Diagonals;

	public int adjacentsOpened = 0;
//	public int diagonalsOpened = 0;
	public bool visited = false;
	public bool isWall = false;
}
