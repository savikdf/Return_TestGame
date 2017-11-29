using UnityEngine;
using System.Collections;

public class CellChanger : MonoBehaviour {
	public static CellChanger instance;
	public GameObject portalTilePrefab;

	//------------------------------------

	void Awake(){
		if (!instance)
			instance = this;
	}

	//Called from CELLPLACER
	public void BeginCellChanging(){
		for (int x = 0; x < CellPlacer.instance.gridSize.x; x++) {
			for (int z = 0; z < CellPlacer.instance.gridSize.z; z++) {
				if (CellPlacer.instance.cellGrid [x, z] != null) {
					if (CellPlacer.instance.cellGrid [x, z].GetComponent<Cell> ().Adjacents.Count == 0 && CellPlacer.instance.cellGrid [x, z].GetComponent<Cell> ().Diagonals.Count == 0) {
						GameObject floorTile = Instantiate (portalTilePrefab, CellPlacer.instance.cellGrid[x,z].transform.position, Quaternion.Euler(new Vector3(90,0,0))) as GameObject;
						floorTile.transform.localScale = new Vector3 (CellPlacer.instance.cellPrefab.transform.localScale.x, CellPlacer.instance.cellPrefab.transform.localScale.z, 1);
						Destroy(CellPlacer.instance.cellGrid [x, z].gameObject);
					}
				}
			}			
		}


	
	}



}
