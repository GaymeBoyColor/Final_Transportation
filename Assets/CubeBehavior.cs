using UnityEngine;
using System.Collections;

public class CubeBehavior : MonoBehaviour {

	public int x,z;
	public GameControllerScript GameController;

	void OnMouseDown () {

		GameObject[] allCubes = GameObject.FindGameObjectsWithTag("Clickable Cube");
		GameController.Activate (gameObject);
		GameController.setTargetLocation (x,z);
	}
}