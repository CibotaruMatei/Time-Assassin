using UnityEngine;
using System.Collections;

public class AnimateWorld : MonoBehaviour {

	// Use this for initialization


	private float PlaneMaxRotateAngle;
	private bool reverseRotation = false;

	private float rotationAngle = 0.0f;

	GameManager gm;

	
	private float PlaneRotateSpeed;
	private float skyRotateSpeed = 1.2f;

	private float updateRate;
	void Start () {
	gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	PlaneMaxRotateAngle = Random.Range((gm.tiltPower+4)/4.0f+1,(gm.tiltPower+4)/2.0f+3);
	}


	
	private float lastUpdate = 0.0f;

	// Update is called once per frame
	void Update () {

		updateRate = (4 - (float)(gm.tiltPower)/5) * 0.12f; 
		PlaneRotateSpeed = (gm.tiltPower+4) / 4.0f * 0.05f;
		bool tiltOption = gm.tiltOption;
		if(tiltOption && Time.time - lastUpdate > Random.Range(updateRate/4, updateRate)) {
			lastUpdate = Time.time;
			if(rotationAngle > 0.0f && rotationAngle < PlaneMaxRotateAngle) {
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
					PlaneMaxRotateAngle = Random.Range((gm.tiltPower+1)/4.0f+1,(gm.tiltPower+1)/2.0f+3);
				}
			}
		}
		
		
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyRotateSpeed);


	}

	

		
}
