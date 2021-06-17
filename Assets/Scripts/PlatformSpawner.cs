using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private int numPlatformsTotal = 16;
    private float horizontalDistance;
    [SerializeField] Vector2 verticalDistances;
    private float spawnHeight = 0;

    [SerializeField] Transform platformPrefab;
    private Transform[] platforms;

    private Transform cameraTransform;
    private float boundaryDistanceVertical;
    private float boundaryDistanceHorizontal;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        //making the distance boundry to equal the orthograpghic size proportional to the width/height of screen
        boundaryDistanceHorizontal = cameraTransform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height;
        //Settings horizontalDistance equal to the boundries of the screen minus by the platform dived by 2 (stops from spawning on edge)
        horizontalDistance = boundaryDistanceHorizontal - platformPrefab.GetComponent<SpriteRenderer>().bounds.size.x / 2 - 0.1f;
        //making the distance boundry to equal the orthograpghic 
        boundaryDistanceVertical = Camera.main.orthographicSize + 0.3f;


        platforms = new Transform[numPlatformsTotal]; //spawns the flatform
        for (int i = 0; i < numPlatformsTotal; i++) //loops to create a new platform everytime there is less than 16 platforms spawned
        {
            platforms[i] = Instantiate(platformPrefab, transform); //creates a new platform
        }


        NewGame();//do Newgame Method
    }
    public void NewGame()
    {
        spawnHeight = 0; 
        for (int i = 0; i < numPlatformsTotal; i++) //loops to where to spawn a platform everytime there is less than 16 platforms spawned
        {
            var platform = platforms[i]; //platform equal "platforms[i]"
            //When started dont spawn random, from there spawn randomly based on the distance of sides minus halve of platform
            float xPos = i == 0 ? 0 : Random.Range(-horizontalDistance, horizontalDistance); 
            float yPos = Random.Range(verticalDistances.x, verticalDistances.y); //Spawns the platforms randomly between the min and max heights
            platform.position = new Vector3(xPos, spawnHeight);  
            spawnHeight += yPos; //spawn platforms with new spawn height but now higher up as character progresses up in the game
        }
    }

    // Update is called once per frame
    void Update() 
    {
        for (int i = 0; i < numPlatformsTotal; i++) //loops to where to move the camera everytime there is less than 16 platforms spawned
        {
            //if the y axis of Camera minus y axis of platforms is larger than the vertical boundries
            if (cameraTransform.position.y - platforms[i].position.y > boundaryDistanceVertical)
            {
                //Spawns the platforms randomly based on the distance of sides minus halve of platform
                float xPos = Random.Range(-horizontalDistance, horizontalDistance);
                float yPos = Random.Range(verticalDistances.x, verticalDistances.y); //Spawns the platforms randomly between the min and max heights
                platforms[i].position = new Vector3(xPos, spawnHeight);
                spawnHeight += yPos; //spawn platforms with new spawn height but now higher up as character progresses up in the game
            }
        }
    }
}
