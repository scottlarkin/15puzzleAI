using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	int gSize = 8;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {

		if (GUI.Button(new Rect(250, 10, 200, 50),"15 grid")){
			gameObject.GetComponent<Script>().Reset(15);
			gSize = 15;
		}

		if (GUI.Button(new Rect(250, 70, 200, 50),"8 grid")){
			gameObject.GetComponent<Script>().Reset(8);
			gSize = 8;
		}
		
		if (GUI.Button(new Rect(10, 10, 200, 50),"Shuffle grid")){
			gameObject.GetComponent<Script>().Shuffle();
		}

		if(gSize == 8){
			if (GUI.Button(new Rect(10, 70, 200, 50), "Breadth First Solve")){
				StartCoroutine(gameObject.GetComponent<Script>().Solve());
			}
		}

		if (GUI.Button(new Rect(10, 130, 200, 50), "A* Heuristic Solve")){
			StartCoroutine(gameObject.GetComponent<Script>().AStarSolve());
		}
		
	}
}
