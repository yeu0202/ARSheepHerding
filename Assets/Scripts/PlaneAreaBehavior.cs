using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class PlaneAreaBehavior : MonoBehaviour
{
    public TextMeshPro areaText;
    public ARPlane arPlane;
    public GameObject sheepPrefab;
    public GameObject postPrefab;
    public GameObject hutPrefab;
    public GameObject wallPrefab;
    private static int num_sheep = 5;
    GameObject[] sheepArray = new GameObject[num_sheep];
    Material[] sheepMaterials = new Material[num_sheep];
    bool[] sheepRotated = new bool[num_sheep];
    private GameObject sheepHut;
    int sheepindex = 0;
    bool gameStarted = false;
    public static int sheepInBarn = 0;

    public static bool wonGame = false;

    float minX;
    float maxX;
    float minY;
    float maxY;

    // sheep rotation stuff
    // private float rotateSpeed = 9999999.99f;



    // this doesn't seem to work
    private void ArPlane_BoundaryChanged(ARPlaneBoundaryChangedEventArgs obj){
        areaText.text = "Length: " + arPlane.size.x.ToString() + "\nWidth: " + 
                        arPlane.size.y.ToString() + 
                        "Area: " + CalculatePlaneArea(arPlane).ToString();
    }

    private void updateText(){
        areaText.text = "Length: " + arPlane.size.x.ToString() + "\nWidth: " + 
                        arPlane.size.y.ToString() + 
                        "\nArea: " + CalculatePlaneArea(arPlane).ToString();
        areaText.text = ""; // comment or uncomment to hide the text
    }

    private float CalculatePlaneArea(ARPlane plane){
        return plane.size.x * plane.size.y;
    }

    public void ToggleAreaView(){
        if(areaText.enabled) areaText.enabled = false;
        else areaText.enabled = true;
    }

    public void addSheep(){
        if(gameStarted){
            return;
        }
        gameStarted = true;

        /*
            Welcome to spaghetti land
        */

        Vector3 planeNormal = arPlane.normal;
        //rotation variable for looking at plane normal, upright
        Quaternion orientToPlane = Quaternion.LookRotation(planeNormal, Vector3.up);

        // scale calculation
        float scaleSize = 0;
        if(arPlane.size.x > arPlane.size.y){
            scaleSize = 2*arPlane.size.x;
        }
        else{
            scaleSize = 2*arPlane.size.y;
        }

        for(int i=0; i<4; i++){
            GameObject tempPost = (GameObject)Instantiate(postPrefab);

            //assign rotation to painting.
            tempPost.transform.rotation = orientToPlane;
            //flip the rotation or else it's backwards
            tempPost.transform.rotation *= Quaternion.Euler(0,180f,0);

            
            // move sheep to plane position?
            tempPost.transform.position = arPlane.transform.position;
            if(i == 0){
                Vector3 temp = new Vector3((float)(-arPlane.size.x/2.0), 0.01f, (float)(-arPlane.size.y/2.0));
                tempPost.transform.position += temp;
            }
            if(i == 1){
                Vector3 temp = new Vector3((float)(arPlane.size.x/2.0), 0.01f, (float)(-arPlane.size.y/2.0));
                tempPost.transform.position += temp;
            }
            if(i == 2){
                Vector3 temp = new Vector3((float)(arPlane.size.x/2.0), 0.01f, (float)(arPlane.size.y/2.0));
                tempPost.transform.position += temp;
            }
            if(i == 3){
                Vector3 temp = new Vector3((float)(-arPlane.size.x/2.0), 0.01f, (float)(arPlane.size.y/2.0));
                tempPost.transform.position += temp;
            }

            // scale object
            tempPost.transform.localScale = new Vector3(scaleSize*0.5f, scaleSize*0.5f, scaleSize*0.5f);
        }

        for(int i=0; i<4; i++){
            // make walls
            GameObject wallGameObject = (GameObject)Instantiate(wallPrefab);

            //assign rotation to painting.
            wallGameObject.transform.rotation = orientToPlane;
            //flip the rotation or else it's backwards
            wallGameObject.transform.rotation *= Quaternion.Euler(0,180f,0);

            float wallScaleBase = 50.0f;
            float wallScaleX = arPlane.size.y * wallScaleBase;
            float wallScaleY = arPlane.size.x * wallScaleBase;

            // float vertShift = 0.101f;
            // move sheep to plane position?
            wallGameObject.transform.position = arPlane.transform.position;
            if(i == 0){
                Vector3 temp = new Vector3((float)(-arPlane.size.x/2.0), 0.01f, 0);
                wallGameObject.transform.position += temp;
                wallGameObject.transform.localScale = new Vector3(scaleSize * 0.5f, wallScaleX, scaleSize * 0.5f);
            }
            if(i == 1){
                Vector3 temp = new Vector3((float)(arPlane.size.x/2.0), 0.01f, 0);
                wallGameObject.transform.position += temp;
                wallGameObject.transform.localScale = new Vector3(scaleSize * 0.5f, wallScaleX, scaleSize * 0.5f);
            }
            if(i == 2){
                Vector3 temp = new Vector3(0, 0.01f, (float)(arPlane.size.y/2.0));
                wallGameObject.transform.position += temp;
                wallGameObject.transform.localScale = new Vector3(wallScaleY, scaleSize * 0.5f, scaleSize * 0.5f);
            }
            if(i == 3){
                Vector3 temp = new Vector3(0, 0.01f, (float)(-arPlane.size.y/2.0));
                wallGameObject.transform.position += temp;
                wallGameObject.transform.localScale = new Vector3(wallScaleY, scaleSize * 0.5f, scaleSize * 0.5f);
            }

        }



        // make hut
        GameObject tempHutObject = (GameObject)Instantiate(hutPrefab);

        //assign rotation to painting.
        tempHutObject.transform.rotation = orientToPlane;
        //flip the rotation or else it's backwards
        tempHutObject.transform.rotation *= Quaternion.Euler(0,180f,0);

        // move sheep to plane position?
        tempHutObject.transform.position = arPlane.transform.position;
        Vector3 tempHutVector = new Vector3(0, 0.01f, 0);
        tempHutObject.transform.position += tempHutVector;

        tempHutObject.transform.localScale = new Vector3(scaleSize * 0.5f, scaleSize * 0.5f, scaleSize * 0.5f);

        sheepHut = tempHutObject;


        // calculate coordinates to find range for randomizing location of sheep
        Vector3 centerPosition = arPlane.transform.position;
        minX = centerPosition.x - (arPlane.size.x/2.0f)*0.8f;
        maxX = centerPosition.x + (arPlane.size.x/2.0f)*0.8f;
        minY = centerPosition.z - (arPlane.size.y/2.0f)*0.8f;
        maxY = centerPosition.z + (arPlane.size.y/2.0f)*0.8f;

        for(int i=0; i<5; i++){
            Vector3 sheepPosition = new Vector3(Random.Range(minX, maxX), centerPosition.y, Random.Range(minY, maxY));

            // make sheep
            GameObject tempGameObject = (GameObject)Instantiate(sheepPrefab);

            //assign rotation to painting.
            tempGameObject.transform.rotation = orientToPlane;
            //flip the rotation or else it's backwards
            tempGameObject.transform.rotation *= Quaternion.Euler(0,180f,0);

            // move sheep to plane position?
            tempGameObject.transform.position = sheepPosition;
            Vector3 tempVector = new Vector3(0, 0.01f, 0);
            tempGameObject.transform.position += tempVector;
            
            tempGameObject.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);

            sheepArray[sheepindex] = tempGameObject;
            // sheepMaterials[sheepindex] = tempGameObject.GetComponent<MeshRenderer>().materials[4]; // this doesn't seem to work, don't know why
            sheepRotated[sheepindex] = false;
            sheepindex++;
            if(sheepindex > 19) sheepindex = 19;
        }
    }

    static public float findDistance(Vector3 object1, Vector3 object2){
        return Mathf.Pow((Mathf.Pow(object1.x - object2.x, 2.0f) + Mathf.Pow(object1.z - object2.z, 2.0f)), 0.5f);
    }

    float clamp(float n, float minn, float maxn){
        if(n < minn){
            return minn;
        }
        else if(n > maxn){
            return maxn;
        }
        else{
            return n;
        }
    }

    void rotateSheep(){
        Vector3 cameraRotation = Camera.main.transform.rotation.eulerAngles;
        for(int i=0; i<sheepindex; i++){
            GameObject thisSheep = sheepArray[i];
            Vector3 sheepRotation = thisSheep.transform.rotation.eulerAngles;
            sheepRotation.y = cameraRotation.y - 90.0f;
            thisSheep.transform.rotation = Quaternion.Euler(sheepRotation);
        }
    }


    void updateSheep(){
        // scale calculation
        float scaleSize = 0;
        if(arPlane.size.x > arPlane.size.y){
            scaleSize = 0.2f*arPlane.size.x;
        }
        else{
            scaleSize = 0.2f*arPlane.size.y;
        }


        // Color _color = new Color(100, 100, 100);
        // Color _color2 = new Color(0, 0, 0);          // unable to get materials, so I ditched using color changes

        sheepInBarn = 0;
        
        for(int i=0; i<sheepindex; i++){
            // detect if sheep are in the barn
            GameObject thisSheep = sheepArray[i];
            if(findDistance(thisSheep.transform.position, sheepHut.transform.position) < scaleSize*0.3f){
                if(!sheepRotated[i]){
                    Vector3 sheepRotation = thisSheep.transform.rotation.eulerAngles;
                    sheepRotation.x -= 90;
                    thisSheep.transform.rotation = Quaternion.Euler(sheepRotation);
                    sheepRotated[i] = true;
                }
                sheepInBarn++;
            }
            else{
                if(sheepRotated[i]){
                    Vector3 sheepRotation = thisSheep.transform.rotation.eulerAngles;
                    sheepRotation.x += 90;
                    thisSheep.transform.rotation = Quaternion.Euler(sheepRotation);
                    sheepRotated[i] = false;
                }
            }

            // move sheep if they are close to the player
            if(findDistance(thisSheep.transform.position, Camera.main.transform.position) < scaleSize){
                Vector3 sheepPosition = thisSheep.transform.position;
                Vector3 cameraPosition = Camera.main.transform.position;
                sheepPosition.x -= (cameraPosition.x - sheepPosition.x)/50.0f;
                sheepPosition.z -= (cameraPosition.z - sheepPosition.z)/50.0f;
                thisSheep.transform.position = sheepPosition;
            }
            // make sure sheep can't go out of bounds
            Vector3 sheepClamp = thisSheep.transform.position;
            sheepClamp.x = clamp(sheepClamp.x, minX, maxX);
            sheepClamp.z = clamp(sheepClamp.z, minY, maxY);
            thisSheep.transform.position = sheepClamp;

        }
        // win game condition
        if(sheepInBarn == num_sheep)
            wonGame = true;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateText();

        // rotates text to face camera
        areaText.transform.rotation = Quaternion.LookRotation(areaText.transform.position - Camera.main.transform.position);
        if(sheepindex > 0 && !wonGame){
            rotateSheep();
            updateSheep();
        }
    }
}
