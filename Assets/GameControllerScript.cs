using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {
public GameObject cubePrefab;
private GameObject[,] allCubes;
public Airplane airplane;
public int targetxLocation;
public int targetzLocation;
int numbCubes = 16;
int numCubes = 9;
int cargo = 10;
int cargoCapacity = 90;
float timeToAct = 0.0f;
float spawnFrequency = 1.5f;
	
	// Use this for initialization
	void Start () {
		
		timeToAct += spawnFrequency;

		//this is making the airplane and setting the limits
		airplane = new Airplane ();
		airplane.cargo = 0;
		airplane.points = 0;
		airplane.x = 0;
		airplane.z = numCubes - 1;
		allCubes = new GameObject[numbCubes, numCubes];

		//we're creating a 2D array as the grid
		for (int x = 0; x < numbCubes; x++)	{
			for (int z = 0; z < numCubes; z++) {
				allCubes [x,z] = (GameObject)Instantiate (cubePrefab, new Vector3 (x * 2 -14, z * 2 - 14, 10), Quaternion.identity);
				allCubes [x,z].GetComponent<CubeBehavior>().x = x;
				allCubes [x,z].GetComponent<CubeBehavior>().z = z;
				allCubes [x,z].GetComponent<CubeBehavior>().GameController = this;
			}
		}
		allCubes[0,numCubes -1].GetComponent<Renderer>().material.color = Color.red;
		allCubes[numbCubes - 1, 0].GetComponent<Renderer> ().material.color = Color.black;
	}

	//this is what creates the Activation of the Airplane

	public void Activate (GameObject oneCube)	{

	if (airplane.x == oneCube.GetComponent<CubeBehavior> ().x && airplane.z == oneCube.GetComponent<CubeBehavior> ().z) {
			if (airplane.isActive == false) {
				oneCube.GetComponent<Renderer> ().material.color = Color.yellow;
				airplane.isActive = true;
			} else if (airplane.isActive == true) {
				oneCube.GetComponent<Renderer> ().material.color = Color.red;
				airplane.isActive = false;
			}
		}
	}
	
	public void AutoMove (int x, int z)	{
		GameObject oneCube = allCubes[x,z];

		//this is the code that checks to see if a cube is clicked and sets that as the new x and z that it trails toward 

		if (airplane.isActive == true && (airplane.x != oneCube.GetComponent<CubeBehavior>().x || 
		                                       airplane.z != oneCube.GetComponent<CubeBehavior>().z))	{
			allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.white;

			//these four are different combinations of both the x and z changing position
			if (oneCube.GetComponent<CubeBehavior>().x != airplane.x && oneCube.GetComponent<CubeBehavior>().z != airplane.z) {
				if (oneCube.GetComponent<CubeBehavior>().x > airplane.x && oneCube.GetComponent<CubeBehavior>().z > airplane.z)	{
					airplane.x += 1;
					airplane.z += 1;
				}
				else if (oneCube.GetComponent<CubeBehavior>().x > airplane.x && oneCube.GetComponent<CubeBehavior>().z < airplane.z)	{
					airplane.x += 1;
					airplane.z -= 1;
				}
				else if (oneCube.GetComponent<CubeBehavior>().x < airplane.x && oneCube.GetComponent<CubeBehavior>().z > airplane.z)	{
					airplane.x -= 1;
					airplane.z += 1;
				}
				else if (oneCube.GetComponent<CubeBehavior>().x < airplane.x && oneCube.GetComponent<CubeBehavior>().z < airplane.z)	{
					airplane.x -= 1;
					airplane.z -= 1;
				}
				allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.yellow;
			}

			//these two are different combinations of just the x moving, the z stays complacent
			else if (oneCube.GetComponent<CubeBehavior>().x != airplane.x && oneCube.GetComponent<CubeBehavior>().z == airplane.z)	{
				if (oneCube.GetComponent<CubeBehavior>().x > airplane.x)	{
					airplane.x += 1;
				}
				else if (oneCube.GetComponent<CubeBehavior>().x < airplane.x)	{
					airplane.x -= 1;
				}
				allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.yellow;
			}

			//these two are different combinations of just the z moving, the x stays complacent
			else if (oneCube.GetComponent<CubeBehavior>().x == airplane.x && oneCube.GetComponent<CubeBehavior>().z != airplane.z)	{
				if (oneCube.GetComponent<CubeBehavior>().z < airplane.z) {
					airplane.z -= 1;
				}
				else if (oneCube.GetComponent<CubeBehavior>().z > airplane.z)	{
					airplane.z += 1;
				}
				allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.yellow;
			}
			//keeps the airplane active throughout
			airplane.isActive = true;
		}
	}

	public void setTargetLocation (int x, int z)	{
		targetxLocation = x;
		targetzLocation = z;
	}

	//this is what constitutes the movement of the airplane
	public void Move ()	{
		allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.white;
		airplane.Movement ();
		allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {

		//Time.time >= timeToAct is what allows the airplane to update every 1.5 seconds
		if (Time.time >= timeToAct)	{
			if (airplane.x == 0 && airplane.z == numCubes - 1 && airplane.cargo < cargoCapacity)	{
				airplane.cargo += cargo;
				print ("You are carrying " + airplane.cargo + " crates of cargo.");
			}
			if (airplane.isActive) {
				Move ();
			}
			//this checks to see if the airplanes at the port and dumps out the cargo; adding it to points
			if (airplane.x == numbCubes - 1 && airplane.z == 0) {
				airplane.PointTotals ();
				print ("You have " + airplane.points + " points.");
				airplane.cargo = 0;
				allCubes [numbCubes - 1, 0].GetComponent<Renderer> ().material.color = Color.yellow;
			} 
			else {
				allCubes[numbCubes - 1, 0].GetComponent<Renderer> ().material.color = Color.black;
			}
			AutoMove (targetxLocation, targetzLocation);
			timeToAct += spawnFrequency;
		}
	}

	//	this is the code that translates what movement keys do; however, keys don't control movement anymore
//	public void CheckInput ()	{
//		if (airplane.isActive == true) {
//			if (Input.GetKeyDown ("up") && airplane.z < numCubes - 1) {
//				airplane.SetDirection (0, 1);
//			}
//
//			if (Input.GetKeyDown ("down") && airplane.z > 0) {
//				airplane.SetDirection (0, -1);
//			}
//
//			if (Input.GetKeyDown ("right") && airplane.x < numbCubes - 1) {
//				airplane.SetDirection (1, 0);
//			}
//
//			if (Input.GetKeyDown ("left") && airplane.x > 0) {
//				airplane.SetDirection (-1, 0);
//			}
//		}
//	}
}