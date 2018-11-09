using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour {

/* keeps track of if user has placed Object or not */ 

	static private bool _placeOpen;
	static public bool PlaceOpen {
		get { return _placeOpen; }
		set { _placeOpen = value; }
	}


}
