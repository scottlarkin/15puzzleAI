using UnityEngine;
using System.Collections;

public class CubeClick : MonoBehaviour {

	private Script s;

	// Use this for initialization
	void Start () {

		s = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Script>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){

		Move move;

		if(s.state_.CheckValidMove(short.Parse(renderer.material.name.Split(' ')[0]),out move)){
			s.SwapPiece(move);
		}

	}
}
