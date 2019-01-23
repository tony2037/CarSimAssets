using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class DataCollection : MonoBehaviour {

	//[SerializeField] private Camera CenterCamera = Camera.front;
	private Camera[] cameras;
	private Camera CenterCamera;

	[System.Serializable]
	public class Item
	{
		public float Position_X_min;
		public float Position_X_max;
		public float Position_Z_min;
		public float Position_Z_max;
		public float Rotation_Y_min;
		public float Rotation_Y_max;
		public float Step_X;
		public float Step_Z;
		public float Angle_Y;
	}
	public Item DataCollectionScript;

	[System.Serializable]
	public class Car_properties
	{
		public float Position_X;
		public float Position_Y;
		public float Position_Z;
		public float Rotation_X;
		public float Rotation_Y;
		public float Rotation_Z;
	}
	public Car_properties car_prop;

	private string directory;
	private string time_stamp;

	// Use this for initialization
	void Start () {
		// Read the Json script file
		TextAsset jsonObj = Resources.Load("Text/DataCollectionScript") as TextAsset;
		DataCollectionScript = JsonUtility.FromJson<Item>(jsonObj.text);
		//Debug.Log(DataCollectionScript.Position_X_max);

		// Get cameras
		GameObject camera_handle = GameObject.Find("Front Facing Camera");
		if(camera_handle == null)
		    Debug.Log("Cannot found the Front Facing Camera");
		else{
			CenterCamera = camera_handle.GetComponent<Camera>();
			if(CenterCamera == null)
				Debug.Log("Cannot get the Front Faceing Camera component");
		}

		// Initialize car_prop
		car_prop.Position_X = 170f;
		car_prop.Position_Y = -79.62f;
		car_prop.Position_Z = -42f;
		car_prop.Rotation_X = 0.0f;
		car_prop.Rotation_Y = 90f;
		car_prop.Rotation_Z = 0.0f;

		directory = "D:/Udacity-car-sim/self-driving-car-sim/Assets/Resources/Data";
		time_stamp = DateTime.Now.ToString ("MM-dd-HH:mm");
		//directory = directory + "/" + time_stamp;
		//Directory.CreateDirectory(directory);
		Debug.Log("Directory: " + directory);
		//var folder = Directory.CreateDirectory (directory);
		/*
		for(int i = 0; i < 5; i++){
			car_prop.Position_X += (float)i* DataCollectionScript.Step_X;
			Debug.Log("Position_X: " + car_prop.Position_X);
			set_car(car_prop);
			WriteImage(CenterCamera, directory, i.ToString());	
		}
		*/
		int image_number = 0;
		for(float p_x = DataCollectionScript.Position_X_min; p_x <= DataCollectionScript.Position_X_max; p_x += DataCollectionScript.Step_X){
			for(float p_z = DataCollectionScript.Position_Z_min; p_z <= DataCollectionScript.Position_Z_max; p_z += DataCollectionScript.Step_Z){
				for(float r_y = DataCollectionScript.Rotation_Y_min; r_y <= DataCollectionScript.Rotation_Y_max; r_y += DataCollectionScript.Angle_Y){
					car_prop.Position_X = p_x;
					car_prop.Position_Z = p_z;
					car_prop.Rotation_Y = r_y;
					Debug.Log("car_prop: " + car_prop);
					set_car(car_prop);
					WriteImage(CenterCamera, directory, image_number.ToString());
					image_number++;		
				}
			}
		}
		initialize_position();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void initialize_position(){
		transform.eulerAngles = new Vector3(0, 90, 0);
		transform.position = new Vector3(168.91f, -79.62f, -45.29f);
	}

	void set_car(Car_properties car_prop){
		transform.eulerAngles = new Vector3(car_prop.Rotation_X, car_prop.Rotation_Y, car_prop.Rotation_Z);
		transform.position = new Vector3(car_prop.Position_X, car_prop.Position_Y, car_prop.Position_Z);
	}

	void record_json(string Path,string file_name, Car_properties car_prop){
		File.WriteAllText(Path+ "/" + file_name, JsonUtility.ToJson(car_prop));
	}

	private string WriteImage (Camera camera, string directory, string image_name){
        //needed to force camera update 
		camera.Render();
		RenderTexture targetTexture = camera.targetTexture;
		RenderTexture.active = targetTexture;
		Texture2D texture2D = new Texture2D (targetTexture.width, targetTexture.height, TextureFormat.RGB24, false);
		texture2D.ReadPixels (new Rect (0, 0, targetTexture.width, targetTexture.height), 0, 0);
		texture2D.Apply ();
		byte[] image = texture2D.EncodeToJPG ();
		UnityEngine.Object.DestroyImmediate (texture2D);

		string path = Path.Combine(directory, image_name + ".jpg");
		File.WriteAllBytes (path, image);
		image = null;
		return path;
    }

}
