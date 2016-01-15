using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


public class Script : MonoBehaviour {

	public PuzzleState state_;

	Vector3 nullPosition;
	List<GameObject> cubes_;

	int size = 8;


	public void Reset(int size){
		foreach(GameObject c in cubes_)
			Destroy(c);

		state_ = new PuzzleState(size);
		cubes_ = new List<GameObject>();
		
		short[,] nums2D = state_.NumbersMappedTo2D();
		
		int idx = 1;
		for(int i = 0; i < PuzzleState.width_; i++){
			for(int j = 0; j < PuzzleState.width_; j++){
				
				if(idx++ == PuzzleState.puzzleSize_ + 1) break;
				
				Material m = (Material)Resources.Load(nums2D[i,j].ToString(), typeof(Material));
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				
				cube.renderer.material = m;
				cube.transform.position = new Vector3(j,-i,-1.38f);
				cube.transform.Rotate(new Vector3(0,0,180));
				cube.AddComponent("CubeClick");
				
				cubes_.Add(cube);
				
			}
		}
		
		nullPosition = new Vector3(PuzzleState.width_ - 1 , -(PuzzleState.width_ - 1), -1.38f);
		

	}

	void Start () {
		cubes_ = new List<GameObject>();
		Reset(8);
	}
 
	public IEnumerator Solve(){

		BFSearch searcher = new BFSearch(state_);
		
		List<Move> moveList = searcher.GetMoveList(searcher.GetTerminalNode());

		foreach(Move move in moveList){

			SwapPiece(move);

			//delay the moves to visualise the solving
			yield return new WaitForSeconds(0.2f);
			//yield return null;
		}

	}

	public IEnumerator AStarSolve(){
		AStarHeuristic aStar = new AStarHeuristic(state_);

		List<Move> moveList = aStar.GetMoveList(aStar.Solve());

		foreach(Move move in moveList){
			
			SwapPiece(move);
			
			//delay the moves to visualise the solving
			yield return new WaitForSeconds(0.2f);
			//yield return null;
		}
	}

	public IEnumerator SlowShuffle(){

		for(int i = 0; i < 100; i++){
			
			Move move = state_.GetMoveablePieces().OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();

			SwapPiece(move);

			yield return new WaitForSeconds(0.02f);

		}

	}

	public void Shuffle(){
		//shuffle by making random valid moves to ensure the puzzle is solveable
		for(int i = 0; i < 50; i++){

			Move move = state_.GetMoveablePieces().OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
			SwapPiece(move);
		}
	
	}
	
	public void SwapPiece(Move piece){

		if(!state_.GetMoveablePieces().Contains(piece)){
			Debug.Log ("this shouldnt happen...!");
			return;
		}

		Vector3 p = cubes_[(int)piece.value_ - 1].transform.position;
		cubes_[(int)piece.value_ - 1].transform.position = nullPosition;
		nullPosition = p;

		state_ = state_.Move(piece);

	}

	// Update is called once per frame
	void Update () {
	
	}
}
