using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellPlacer : MonoBehaviour {
	public static CellPlacer instance;
	public MazeEffect c_MazeEffect = MazeEffect.Normal;
	public bool generateOnUpdate = true;
	GameObject mazeHolder;

	public GameObject cellPrefab, floorTilePrefab;
	public Vector3 gridSize = new Vector3(25,1,25);
	public Vector2 startPos = new Vector2(0,0);
	[HideInInspector] public Transform[,] cellGrid;

	public List<Cell> sortingStack = new List<Cell>();			//used for generation
	public List<Cell> outputStack = new List<Cell>();			//all cells selected
	public List<Cell> reverseOutputStack = new List<Cell>();	//all cells not selected by the algorithmn
	public List<Cell> wallStack = new List<Cell> ();			//all wall cells

	float cellWidth = 0f;
	bool mazeComplete = false;
	int maxStackAmount = 0;
	bool popUpCheck;
	int adjacentCheckAmount;
	Cell exitCell, entranceCell;

	public enum MazeEffect{
		Normal,
		Reverse,
		WeightDent
	};
	//------------------------------------ Starters

	void Awake(){
		if (!instance)
			instance = this;
	}

	void Start(){
		GenerateMaze ();
	}


	public void GenerateMaze(){
		if (!mazeHolder) {
			mazeHolder = new GameObject ();
			mazeHolder.name = "mazeHolder";
		}
		mazeComplete = false;
		maxStackAmount = 0;
		cellWidth = cellPrefab.transform.lossyScale.x;
		outputStack.Clear ();
		sortingStack.Clear ();
		cellGrid = new Transform[(int)gridSize.x, (int)gridSize.z];

		SetupCells ();

		SetAdjacents ();

		SetDiagonals ();

		ApplyMazeEffect ();	//put c_MazeEffect on normal for default generation

		SetStart ();	//starts the generation process
	}

	//------------------------------------ Generation Functions
	//spawns cells
	//weights cells
	//adds cells to the transform[,]
	public void SetupCells(){
		for (int x = 0; x < gridSize.x; x++) {
			for (int z = 0; z < gridSize.z; z++) {
				GameObject newCell;
				newCell = Instantiate (cellPrefab, new Vector3 (x * cellWidth, cellPrefab.transform.localScale.z, z * cellWidth), Quaternion.identity) as GameObject;
				cellGrid [x, z] = newCell.transform;								//adding to transform[,]
				newCell.name = string.Format("({0},{1})",x,z);						//names cells
				newCell.GetComponent<Cell> ().cellWeight = Random.Range (0, 100);	//weight allocation
				newCell.transform.SetParent(mazeHolder.transform);					//setting transform
			}
		}

	}

	//sets up walls
	public void PlaceWalls(){
		
		for(int i = 0; i < gridSize.x; i++){
			//bottom hori wall
			GameObject newCell;
			newCell = Instantiate (cellPrefab, new Vector3 (i * cellWidth, cellPrefab.transform.localScale.z, -1 * cellWidth), Quaternion.identity) as GameObject;
			newCell.GetComponent<Cell> ().isWall = true;
			wallStack.Add (newCell.GetComponent<Cell> ());
			//top hori wall
			GameObject newCell2;
			newCell2 = Instantiate (cellPrefab, new Vector3 (i * cellWidth, cellPrefab.transform.localScale.z, gridSize.z * cellWidth), Quaternion.identity) as GameObject;
			newCell2.GetComponent<Cell> ().isWall = true;
			wallStack.Add (newCell2.GetComponent<Cell> ());
		}

		//left wall
		for(int i = 0; i < gridSize.z; i++){
			GameObject newCell;
			newCell = Instantiate (cellPrefab, new Vector3 (-1 *cellWidth, cellPrefab.transform.localScale.z, i * cellWidth), Quaternion.identity) as GameObject;
			newCell.GetComponent<Cell> ().isWall = true;
			wallStack.Add (newCell.GetComponent<Cell> ());
		}

		//right wall
		for(int i = 0; i < gridSize.z; i++){
			GameObject newCell;
			newCell = Instantiate (cellPrefab, new Vector3 (gridSize.x *cellWidth, cellPrefab.transform.localScale.z, i * cellWidth), Quaternion.identity) as GameObject;
			newCell.GetComponent<Cell> ().isWall = true;
			wallStack.Add (newCell.GetComponent<Cell> ());
		}
	}

	//Sets the adjacents of each cell
	void SetAdjacents(){
		for(int x = 0; x < gridSize.x; x++){
			for(int z = 0; z < gridSize.z; z++){
				Transform cell;
				cell = cellGrid[x,z];
				Cell cScript = cell.GetComponent<Cell>();
				if(x - 1 >= 0){
					cScript.Adjacents.Add(cellGrid[x-1,z]);
				}
				if(x + 1 < gridSize.x){
					cScript.Adjacents.Add(cellGrid[x+1,z]);
				}
				if(z - 1 >= 0){
					cScript.Adjacents.Add(cellGrid[x,z-1]);
				}
				if(z + 1 < gridSize.z){
					cScript.Adjacents.Add(cellGrid[x,z+1]);
				}
				cScript.Adjacents.Sort(SortByLowestWeight);
			}
		}
	}

	//Sets the diagonals of each cell
	void SetDiagonals(){
		for(int x = 0; x < gridSize.x; x++){
			for(int z = 0; z < gridSize.z; z++){
				Transform cell;
				cell = cellGrid[x,z];
				Cell cScript = cell.GetComponent<Cell>();
				if(x - 1 >= 0 && z - 1 >= 0){	
					cScript.Diagonals.Add(cellGrid[x-1, z-1]);	
				}
				if(x + 1 < gridSize.x && z + 1 < gridSize.z){ 
					cScript.Diagonals.Add(cellGrid[x+1,z+1]);	
				}
				if(z - 1 >= 0 && x + 1 < gridSize.x){
					cScript.Diagonals.Add(cellGrid[x+1,z-1]);	
				}
				if(z + 1 < gridSize.z && x - 1 >= 0){
					cScript.Diagonals.Add(cellGrid[x-1,z+1]);	
				}
				cScript.Diagonals.Sort(SortByLowestWeight);
			}
		}
	}



	//------------------------------------ Algo
	//sets up starting point
	void SetStart(){
		sortingStack.Add (cellGrid[(int)startPos.x, (int)startPos.y].GetComponent<Cell>());
		outputStack.Add (cellGrid [(int)startPos.x, (int)startPos.y].GetComponent<Cell>());
		cellGrid [(int)startPos.x, (int)startPos.y].GetComponent<Cell> ().visited = true;
		entranceCell = outputStack [0];
		cellGrid [(int)startPos.x, (int)startPos.y].GetComponent<Cell> ().visited = true;
		List<Transform> pAdj = PossibleAdjacents (cellGrid [(int)startPos.x, (int)startPos.y].GetComponent<Cell>());
		pAdj.Sort (SortByLowestWeight);
		StartCoroutine (DepthFirstGen ());	

	}

	//runs through and generates the maze path
	IEnumerator DepthFirstGen(){
		while (!mazeComplete) {
			MoveToNext ();
			yield return null;
		}
		//on ending will go through all cells and change height depending on visted bool val
		for(int x = 0; x < gridSize.x; x++){
			for(int z = 0; z < gridSize.z; z++){
				if (cellGrid [x, z].GetComponent<Cell> ().visited == popUpCheck) {
					//these are tiles you can walk on
					GameObject floorTile = Instantiate (floorTilePrefab, cellGrid[x,z].transform.position, Quaternion.Euler(new Vector3(90,0,0))) as GameObject;
					floorTile.transform.localScale = new Vector3 (cellPrefab.transform.localScale.x, cellPrefab.transform.localScale.z, 1); 
					Destroy (cellGrid [x, z].gameObject);
					cellGrid [x, z] = null;
				} else {
					cellGrid [x, z].GetComponent<Renderer> ().material.color = Color.white;
					if (c_MazeEffect == MazeEffect.Reverse) {
						reverseOutputStack.Add (cellGrid [x, z].GetComponent<Cell>());	//adds walls to reverse output stack if correct gen effect
					}
				}
				exitCell.GetComponent<Renderer> ().material.color = Color.red;
				entranceCell.GetComponent<Renderer> ().material.color = Color.green;
			}
		}
		PlaceWalls (); //Puts the walls around the map
		CellChanger.instance.BeginCellChanging ();	//CELL CHANGER CALL
		//Debug.Log ("MAZE DONE!");
	}

	//picks and moves to next cell
	void MoveToNext(){
		//gets the cell currently ontop of the sorting stack list
		Cell c_Cell = sortingStack[sortingStack.Count - 1];
		//gets all possible routes from the current cell
		List<Transform> posAdj = PossibleAdjacents (c_Cell);
		if (posAdj != null) {
			outputStack.Add (posAdj [0].GetComponent<Cell> ());	//adds to stacks
			sortingStack.Add (posAdj [0].GetComponent<Cell> ());
			posAdj [0].GetComponent<Cell> ().visited = true;
			posAdj [0].GetComponent<Renderer> ().material.color = Color.blue;
			for (int i = 0; i < posAdj [0].GetComponent<Cell> ().Adjacents.Count; i++) {
				posAdj [0].GetComponent<Cell> ().Adjacents [i].GetComponent<Cell> ().adjacentsOpened++;
				for (int j = 0; j < posAdj [0].GetComponent<Cell> ().Adjacents.Count; j++) {
					posAdj [0].GetComponent<Cell> ().Adjacents [j].GetComponent<Cell> ().Adjacents.Remove (posAdj [0]);
				}
				for (int j = 0; j < posAdj [0].GetComponent<Cell> ().Diagonals.Count; j++) {
					posAdj [0].GetComponent<Cell> ().Diagonals [j].GetComponent<Cell> ().Diagonals.Remove (posAdj [0]);
				}
			}
//			posAdj [0].position += new Vector3 (0, .5f, 0);
			if (sortingStack.Count > maxStackAmount) {
				maxStackAmount = sortingStack.Count;
				exitCell = sortingStack [maxStackAmount - 1];
			}
			if (!generateOnUpdate) {
				MoveToNext ();
			}
		} else {
			//retruns to last cell if there are no possible routes to take
			ReturnToLast ();
		}

	}
	//returns to last cell than attempts to pick another
	void ReturnToLast(){
		//removes last cell from sorting stack
		if (sortingStack.Count == 1) {
			mazeComplete = true;
			sortingStack.Clear ();
		} else {
			sortingStack.Remove (sortingStack [sortingStack.Count - 1]);
			if (!generateOnUpdate) {
				MoveToNext ();
			}
		}
	}


	//VARS
	//gets a cell and gives all possible adjacents that can be moved to
	//a cell can be moved to if the opened adjacents are greater than 2
	List<Transform> PossibleAdjacents(Cell sampleCell){
		List<Transform> returnList = new List<Transform> ();
		for (int i = 0; i < sampleCell.Adjacents.Count; i++) {
			if (sampleCell.Adjacents [i].GetComponent<Cell> ().adjacentsOpened < adjacentCheckAmount && !sampleCell.Adjacents [i].GetComponent<Cell> ().visited) {
				returnList.Add(sampleCell.Adjacents[i].transform);
			}
		}
		if (returnList.Count >= 2) {
			returnList.Sort (SortByLowestWeight);	//sorts list if there are enough options
		}
		//returns appropriate object
		if (returnList.Count == 0) {
			return null;
		} else {
			return returnList;
		}
	}

	//sorting method used in SetAdjacents()
	int SortByLowestWeight(Transform inputA, Transform inputB){
		int a = inputA.GetComponent<Cell>().cellWeight; //a's weight
		int b = inputB.GetComponent<Cell>().cellWeight;
		return a.CompareTo(b);
	}


	//------------------------------------ Effects
	void ApplyMazeEffect(){
		popUpCheck = false;
		adjacentCheckAmount = 2;

		if (c_MazeEffect == MazeEffect.WeightDent) {
			//changes height based on weight
			for(int x = 0; x < gridSize.x; x++){
				for(int z = 0; z < gridSize.z; z++){
					Transform selectedCell = cellGrid [x, z];
					selectedCell.position += new Vector3 (0, selectedCell.GetComponent<Cell>().cellWeight / 80f, 0);
				}
			}

		}
		else if (c_MazeEffect == MazeEffect.Reverse) {
			popUpCheck = true;
			adjacentCheckAmount = 3;
		}
	}




}
