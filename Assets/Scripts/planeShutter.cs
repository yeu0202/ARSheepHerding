using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class planeShutter : MonoBehaviour
{
    
    private static ARPlaneManager arPlaneManager;


    void Awake(){
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void togglePlanes(){
        // doesn't work and I don't know why
        // foreach (var plane in arPlaneManager.trackables){
        //     plane.transform.localScale = new Vector3 (0, 0, 0);
        // }
        arPlaneManager.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
