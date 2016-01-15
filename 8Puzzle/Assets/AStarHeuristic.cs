using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AStarHeuristic {

	public class DuplicateKeyComparer<TKey>: IComparer<TKey> where TKey : IComparable
	{
		
		public int Compare(TKey x, TKey y)
		{
			int result = x.CompareTo(y);
			
			if (result == 0)
				return 1; 
			else
				return result;
		}
		
	}

	public class Node{

		public Node(){

		}

		public Node parent_;
		public PuzzleState state_;
		public Move move_;
		public int gScore_;

		public int FScore(){
			return gScore_ + AStarHeuristic.GetManhattanHeuristic(state_);
		}

		public override bool Equals (object obj)
		{
			Node n = (Node)obj;
			return state_.Equals(n.state_);
		}
	}

	HashSet<PuzzleState> closedSet_;
	SortedList<int, Node> openSet_;
	
	public AStarHeuristic(PuzzleState baseState){

		openSet_ = new SortedList<int, Node>(new DuplicateKeyComparer<int>());
		closedSet_ = new HashSet<PuzzleState>();

		Node baseNode = new Node();

		baseNode.parent_ = null;
		baseNode.state_ = baseState;
		baseNode.gScore_ = 0;

		if(baseState.Equals(PuzzleState.terminalState_)){
			Debug.Log ("found goal");
			return;
		}

		openSet_.Add(baseNode.FScore(), baseNode);


	}

	public Node Solve(){

		Node baseN = new Node();
		if(openSet_.Count > 0){
			baseN = openSet_.Values[0];
		}

		while(openSet_.Count > 0){
			
			Node current = openSet_.Values[0];
			
			if(current.state_.Equals(PuzzleState.terminalState_)){
				Debug.Log ("found the goal!");
				return current;
			}
			
			openSet_.RemoveAt(0);
			closedSet_.Add(current.state_);
			
			foreach(Move move in current.state_.GetMoveablePieces()){
				
				PuzzleState state = current.state_.Move(move);
				
				if(closedSet_.Contains(state))
					continue;
				
				int gScore = current.gScore_ + 1; //1 is always the move cost for the puzzle game
				
				Node newNode = new Node();
				newNode.gScore_ = gScore;
				newNode.state_ = state;
				newNode.move_ = move;
				newNode.parent_ = current;
								
				if(!openSet_.ContainsValue(newNode) || !closedSet_.Contains(newNode.state_)){
					openSet_.Add(newNode.FScore(), newNode);
				}
				
			}

		}

		return baseN;
		Debug.Log ("no solution found");

	}

	public List<Move> GetMoveList(Node terminalState){
		
		if(!terminalState.state_.Equals(PuzzleState.terminalState_)){
			Debug.Log("cannot get move list from non terminal state");
			return new List<Move>();
		}
		
		openSet_.Clear();
		closedSet_.Clear();
		
		List<Move> ret = new List<Move>();
		
		Node n = terminalState;
		ret.Add(terminalState.move_);
		
		while(n.parent_ != null){
			n = n.parent_;
			if(n.parent_!=null)
				ret.Add(n.move_);
		}	
		
		ret.Reverse();
		
		return ret;
		
	}
	
	public static int GetManhattanHeuristic(PuzzleState state){

		short[,] nums = state.NumbersMappedTo2D();
		short[,] terminalNums = PuzzleState.terminalState_.NumbersMappedTo2D();
		int h = 0;

		for(int i = 0; i < PuzzleState.width_; i++){
			for(int j = 0; j < PuzzleState.width_; j++){

				int tn = terminalNums[i,j];

				if(tn != -1 && nums[i,j] != tn){

					bool found = false;

					for(int x = 0; (x < PuzzleState.width_) && !found; x++){
						for(int y = 0; (y < PuzzleState.width_) && !found; y++){

							if(nums[x,y] == tn){
				
								h += (int)(Mathf.Abs(x - i) + Mathf.Abs(y - j));
								found = true;
							}
						}
					}
				}
			}
		}

		return h;

	}
}



