using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;



public class PlaneAreaManager : MonoBehaviour
{

    private ARPlaneManager arPlaneManager;

    // static List<ARRaycastHit> hits = new List<ARRaycastHit>();



    void Awake(){
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // void togglePlanes(){
    //     // doesn't work and I don't know why
    //     foreach (var plane in arPlaneManager.trackables){
    //         plane.gameObject.SetActive(false);
    //     }
    //     arPlaneManager.enabled = false;
    // }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0){
            var touch = Input.GetTouch(0); 
            if(touch.phase == TouchPhase.Ended){
                if(Input.touchCount == 1){
                    Ray raycast = Camera.main.ScreenPointToRay(touch.position);
                    if(Physics.Raycast(raycast, out RaycastHit raycastHit)){
                        var planeAreaBehaviour = raycastHit.collider.gameObject.GetComponent<PlaneAreaBehavior>(); 
                        if (planeAreaBehaviour != null){
                            planeAreaBehaviour.addSheep(); 
                            planeAreaBehaviour.ToggleAreaView(); 
                            planeShutter.togglePlanes();
                            // togglePlanes();
                        }
                    }
                } 
            } 
        }
    }
}
