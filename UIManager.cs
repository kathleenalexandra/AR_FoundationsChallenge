using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField]
    public Button SetPlacement;

    [SerializeField]
    public Button ReOrigin;

	// Use this for initialization
	void Start () {
		SetPlacement.onClick.AddListener(PlacementEvent);
        ReOrigin.onClick.AddListener(PlacementEvent);
        ReOrigin.gameObject.SetActive(false); 
	}


	void PlacementEvent() {
         if (SetPlacement.gameObject.activeSelf != true ) {
          SetPlacement.gameObject.SetActive(true); 
          ReOrigin.gameObject.SetActive(false); 
          TaskManager.PlaceOpen = true; 
    } else {
            SetPlacement.gameObject.SetActive(false); 
            ReOrigin.gameObject.SetActive(true);
            TaskManager.PlaceOpen = false; 
        }   
    }
	
	// Update is called once per frame
	void Update () {


		
	}
}
