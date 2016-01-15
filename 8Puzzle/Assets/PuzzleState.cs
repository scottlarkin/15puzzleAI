using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Move{
	public short idx_;
	public short value_;
	
	public Move(short i, short v){
		idx_ = i;
		value_ = v;
	}
}

public class PuzzleState{
	
	public static short puzzleSize_;
	public static short width_;
	public static PuzzleState terminalState_;

	//the configuration of the board
	//short because unity was giving me memory problems.
	public short[] numbers_;
	
	public PuzzleState(int size){

		puzzleSize_ = (short)size;
		width_ = (short)Mathf.Sqrt(puzzleSize_ + 1);
		numbers_ = new short[puzzleSize_ + 1];
		
		//set up the puzzle grid
		for(int i = 0; i < puzzleSize_; i++){
			numbers_[i] = (short)(i + 1);
		}
		
		numbers_[puzzleSize_] = -1;

		terminalState_ = new PuzzleState(this);
	}
	
	public PuzzleState(PuzzleState copy){

		numbers_ = (short[])copy.numbers_.Clone();
	
	}
	
	public int GetNullCoord(){
		
		for(int i = 0; i < puzzleSize_ + 1; i++)
			if(numbers_[i] == -1)
				return i;
		
		return 0;
		
	}
	
	public short[,] NumbersMappedTo2D(){
		
		short[,] nums = new short[width_,width_];
		
		for(int i = 0; i < width_; i++)
			for(int j = 0; j < width_; j++)
				nums[i,j] = numbers_[i*width_+j];
		
		return nums;
		
	}
	
	public List<Move> GetMoveablePieces(){
		
		List<Move> mp = new List<Move>();
		short[,] numbers = NumbersMappedTo2D();
		
		for(int i = 0; i < width_; i++){
			for(int j = 0; j < width_; j++){
				
				short coord = (short)(i * width_ + j); //get the 1D array position of the piece

				//search for a blank piece surrounding the pice at (i,j)

				if(i > 0){
					if(numbers[i - 1, j] == -1){
						mp.Add(new Move(coord,(short)numbers[i, j]));
					}
				}
				if(i < width_ - 1 ){
					if(numbers[i + 1, j] == -1){
						mp.Add(new Move(coord,(short)numbers[i, j]));
					}
				}
				if(j > 0){
					if(numbers[i, j - 1] == -1){
						mp.Add(new Move(coord,(short)numbers[i, j]));
					}
				}
				if(j < width_ - 1){
					if(numbers[i, j + 1] == -1){
						mp.Add(new Move(coord,(short)numbers[i, j]));
					}
				}
				
			}
		}
		
		return mp;
		
	}
	
	public bool CheckValidMove(short n, out Move move){
		
		move = new Move();
		
		foreach(Move m in GetMoveablePieces()){
			if(n == m.value_){
				move = m;
				return true;
			}
		}
		
		Debug.Log ("Invalid Move!");
		return false;
		
	}
		
	public PuzzleState Move(Move m){
		
		//copy the current state of the grid
		PuzzleState ret = new PuzzleState(this);
		
		//update the state
		ret.numbers_[GetNullCoord()] = m.value_;
		ret.numbers_[(int)m.idx_] = -1; 

		return ret;
	}
	
	//for hashset compasrrison
	public override int GetHashCode() { 
		int hc = numbers_.Length;
		foreach(int i in numbers_){
			hc=unchecked(hc*314159 +i);
		}
		return hc;
	}
	
	public override bool Equals(object obj) {
		
		PuzzleState o = (PuzzleState)(obj);
		
		for(int i = 0; i < puzzleSize_ + 1; i++)
		{
			if(numbers_[i] != o.numbers_[i]){
				return false;
			}
		}
		
		return true;
	}
}