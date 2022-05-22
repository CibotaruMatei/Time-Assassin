using UnityEngine;
using System.Collections;

public class AnimateWorld : MonoBehaviour {

	// Use this for initialization


	private float PlaneMaxRotateAngle = 8.0f;
	private bool reverseRotation = false;

	private float rotationAngle = 0.0f;

	
	private float PlaneRotateSpeed = 0.1f;
	private float skyRotateSpeed = 1.2f;
	void Start () {

	}
	
	private float lastUpdate = 0.0f;

	// Update is called once per frame
	void Update () {
		
		bool tiltOption = GameObject.Find("GameManager").GetComponent<GameManager>().tiltOption;
		if(tiltOption && Time.time - lastUpdate > Random.Range(0.03f, 0.12f)) {
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
				// we returned to neutral state
				if(!reverseRotation) {
					PlaneMaxRotateAngle = Random.Range(PlaneMaxRotateAngle/2,PlaneMaxRotateAngle);
				}
			}
		}
		
		
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyRotateSpeed);


	}

	

		
}
