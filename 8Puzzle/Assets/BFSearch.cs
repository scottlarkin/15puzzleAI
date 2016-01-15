using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BFSearch {

	public class Node{

		public static BFSearch searcher_;
		public static int expansions_;
		public PuzzleState state_;
		public Move move_;
		public Node parent_;

		public Node(PuzzleState s, Node p){
			parent_ = p;
			state_ = s;
		}

		public Node(PuzzleState s, BFSearch bfs){
			parent_ = null;
			searcher_ = bfs;
			state_ = s;
			expansions_ = 1;
		}

		public bool Next(out Node s){

			s = this;
			List<Move> moves = state_.GetMoveablePieces();

			foreach(Move m in moves){

				//get the game state that results from making a move
				PuzzleState newState = state_.Move(m);

				//check if the resulting state is terminal
				if(newState.Equals(PuzzleState.terminalState_)){
					s = new Node(newState, this);
					s.move_ = m;
					return true;
				}

				//if we havent already seen this game state, add a new node to the tree
				if(!searcher_.foundStates_.Contains(newState)){
					Node newNode = new Node(newState, this);
					newNode.move_ = m;
					searcher_.foundStates_.Add(newState);
					searcher_.searchQueue_.Enqueue(newNode);

				}
			}

			return false;
		}
	}

	bool done_ = false;

	public Queue<Node> searchQueue_;
	public HashSet<PuzzleState> foundStates_;

	Node baseNode;

	public BFSearch(PuzzleState baseState){

		searchQueue_ = new Queue<Node>();
		foundStates_ = new HashSet<PuzzleState>();
		baseNode = new Node(baseState, this);

	}

	public Node GetTerminalNode(){

		Node ret;

		done_ = baseNode.Next(out ret);
		
		while(!done_){
			//take node off front of queue and get its children
			done_ = searchQueue_.Dequeue().Next(out ret);
		}
	
		return ret;
	}

	public List<Move> GetMoveList(Node terminalState){

		if(!terminalState.state_.Equals(PuzzleState.terminalState_)){
			Debug.Log("cannot get move list from non terminal state");
			return new List<Move>();
		}

		searchQueue_.Clear();
		foundStates_.Clear();

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

}
