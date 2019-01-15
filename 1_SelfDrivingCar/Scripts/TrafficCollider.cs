using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TrafficCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("TrafficCollider start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		Debug.Log("Traffic Collision: " + collision.collider.name);
		if(collision.collider.name != "ColliderBottom" && collision.collider.name != "Parking"){
			transform.eulerAngles = new Vector3(0, 90, 0);
			transform.position = new Vector3(168.91f, -79.62f, -45.29f);
		}
			//SceneManager.LoadScene ("ParkingTrackAutonomous");
	}
}
