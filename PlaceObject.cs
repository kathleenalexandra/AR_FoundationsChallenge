
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using System;

/* I have taken the base touch and plane generation functionality for utalizing AR foundation taken and from https://github.com/Unity-Technologies/arfoundation-samples,
particularly, I utalized the update function to determine when there was user touch on the generated planes, and getter and setter for the prefab. In this I mainly looked to the PlaneonPlane example found here: https://github.com/Unity-Technologies/arfoundation-samples/blob/master/Assets/Scripts/PlaceOnPlane.cs*/  

[RequireComponent(typeof(ARSessionOrigin))]
public class PlaceObject : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    public Camera viewCamera;
    public GameObject cursorPrefab;

    private GameObject cursorInstance;
    private float maxCursorDistance = 30;
    private bool isCursor = false;
    private Vector3 cursorPosition; 
    

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }
    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>


    public GameObject spawnedObject { get; private set; }

    ARSessionOrigin m_SessionOrigin;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    void Awake() {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        TaskManager.PlaceOpen = true; 
    }

    private void UpdateCursor() {
    /* the video appears to demonstrate a cursor that follows the user's gaze, here we cast a ray that will
    hit what is present in the environment, in this case the generated planes*/ 
        Ray ray = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.collider.tag.Equals("plane")) {  /* only show cursor, when on a plane */   
            cursorInstance.transform.position = hit.point; 
    /* this keeps track of where the user cursor is, so we can spawn the object where the user's cursor currently is ontop of the generated plane */ 
            cursorPosition = hit.point;     
        } else {
            cursorInstance.transform.position = ray.origin + ray.direction.normalized * maxCursorDistance;
            }
        }
    }

    void Update(){ 
    if (Input.touchCount > 0 && TaskManager.PlaceOpen == true){
            Touch touch = Input.GetTouch(0);
            if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon)){
                Pose hitPose = s_Hits[0].pose;
                if (spawnedObject == null ) { 
                    spawnedObject = Instantiate(m_PlacedPrefab, cursorPosition, hitPose.rotation ); 
                } else { 
                    spawnedObject.transform.position = cursorPosition; 
                }
            }
        } 

    else if (TaskManager.PlaceOpen == true) {
    /* if there is not a cursor in the scene and the user has not placed an object, create a cursor */  
            if (isCursor == false) {
                cursorInstance = Instantiate(cursorPrefab);
                isCursor = true;
    /* if there is a cursor, call the function that will update it to reflect where the user is looking */
            } else {
                UpdateCursor();
            }
        }
    /* if the user has already placed the object, remove the cursor */ 
    else if (TaskManager.PlaceOpen == false) {
            Destroy(cursorInstance); 
            isCursor = false;
        } else {
            return; 
        }
    }
}

