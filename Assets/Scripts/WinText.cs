using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class WinText : MonoBehaviour
{
    public TextMeshPro winText;


    private void updateText(){
        winText.text = "You won!";
    }
    private void updateSheepNum(){
        // winText.text = "NumOfSheep = " + PlaneAreaBehavior.sheepInBarn.ToString();
    }

    private void emptyText(){
        winText.text = "";
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
        if(PlaneAreaBehavior.wonGame == true)
            updateText();
        else
            updateSheepNum();

        // rotates text to face camera
        winText.transform.rotation = Quaternion.LookRotation(winText.transform.position - Camera.main.transform.position);
        
    }
}
