using UnityEngine;
using System.Collections;

public class AnimateSkyScript : MonoBehaviour {

	// Use this for initialization


	float PlaneMaxRotateAngle = 3.0f;
	bool reverseRotation = false;

	float rotationAngle = 0.0f;

	
	public float RotateSpeed = 0.1f;
	public float skyRotateSpeed = 1.2f;
	void Start () {

	}
	
	public float lastUpdate = 0.0f;

	// Update is called once per frame
	void Update () {
		
		bool tiltOption = GameObject.Find("GameManager").GetComponent<GameManager>().tiltOption;
		if(tiltOption && Time.time - lastUpdate > Random.Range(0.10f, 0.40f)) {
			lastUpdate = Time.time;
			if(rotationAngle > 0.0f && rotationAngle < PlaneMaxRotateAngle) {
				if(!reverseRotation) {
					GameObject.Find("Plane").transform.Rotate(RotateSpeed,0,-RotateSpeed*0.6f);
					rotationAngle += RotateSpeed;
				}
				else {
					GameObject.Find("Plane").transform.Rotate(-RotateSpeed,0,RotateSpeed*0.6f);
					rotationAngle -= RotateSpeed;
				}
			}
			// change once
			else {
				reverseRotation = !reverseRotation;
				if(rotationAngle <= 0.0f) {
					GameObject.Find("Plane").transform.Rotate(RotateSpeed,0,-RotateSpeed*0.6f);
					rotationAngle += RotateSpeed;
				}
				else if(rotationAngle >= PlaneMaxRotateAngle) {
					GameObject.Find("Plane").transform.Rotate(-RotateSpeed,0,RotateSpeed*0.6f);
					rotationAngle -= RotateSpeed;
				}	
				if(reverseRotation) {
					PlaneMaxRotateAngle = Random.Range(-1.0f,3.0f);
				}
			}
		}
		
		
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyRotateSpeed);


	}

	

		
}
