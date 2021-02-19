using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class TMPCoords : MonoBehaviour
{
    public GameObject parentObject;
    public TextMeshPro locationText;


    private void updateText(){

        Vector3 cameraPosition = Camera.main.transform.position;
        locationText.text = "Coords: " + parentObject.transform.position.ToString("f3") +
                            "\nDistance: " + PlaneAreaBehavior.findDistance(parentObject.transform.position, cameraPosition);
    }

    private void emptyText(){
        locationText.text = "";
    }


    void Awake(){
        emptyText();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // updateText();

        // rotates text to face camera
        locationText.transform.rotation = Quaternion.LookRotation(locationText.transform.position - Camera.main.transform.position);
        
    }
}
