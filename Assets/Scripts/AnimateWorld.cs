using UnityEngine;
using System.Collections;

public class AnimateWorld : MonoBehaviour {

	// Use this for initialization


	float PlaneMaxRotateAngle = 7.0f;
	bool reverseRotation = false;

	float rotationAngle = 0.0f;

	
	public float PlaneRotateSpeed = 0.1f;
	public float skyRotateSpeed = 1.2f;
	void Start () {

	}
	
	public float lastUpdate = 0.0f;

	// Update is called once per frame
	void Update () {
		
		bool tiltOption = GameObject.Find("GameManager").GetComponent<GameManager>().tiltOption;
		if(tiltOption && Time.time - lastUpdate > Random.Range(0.10f, 0.40f)) {
			lastUpdate = Time.time;
			if(rotationAngle > 0 && rotationAngle < PlaneMaxRotateAngle) {
				if(!reverseRotation) {
					GameObject.Find("Plane").transform.Rotate(PlaneRotateSpeed,0,-PlaneRotateSpeed*0.6f);
					rotationAngle += PlaneRotateSpeed;
				}
				else {
					GameObject.Find("Plane").transform.Rotate(-PlaneRotateSpeed,0,PlaneRotateSpeed*0.6f);
					rotationAngle -= PlaneRotateSpeed;
				}
			}
			// change once
			else {
				reverseRotation = !reverseRotation;
				if(rotationAngle <= 0) {
					GameObject.Find("Plane").transform.Rotate(PlaneRotateSpeed,0,-PlaneRotateSpeed*0.6f);
					rotationAngle += PlaneRotateSpeed;
				}
				else if(rotationAngle >= PlaneMaxRotateAngle) {
					GameObject.Find("Plane").transform.Rotate(-PlaneRotateSpeed,0,PlaneRotateSpeed*0.6f);
					rotationAngle -= PlaneRotateSpeed;
				}	
				if(reverseRotation) {
					PlaneMaxRotateAngle = Random.Range(3.0f,7.0f);
				}
			}
		}
		
		
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyRotateSpeed);


	}

	

		
}
