using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour {

	public float topHeigth, finalHeigth, bottomHeigth, time;
	Vector3 startPos;
	float startTime, endTime;
	bool lightOff = false, canChange = true;
	
	
	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.J) && canChange)
		{
			canChange = false;
			startTime = Time.timeSinceLevelLoad;
			endTime = startTime + time;			
		}
		
		float aux;
		
		if(canChange == false)
		{
			if(lightOff == false)
			{				
				
				if(Time.timeSinceLevelLoad >= endTime)
				{
					aux = finalHeigth;
					lightOff = true;
					canChange = true;
				}			
				else
				{
					aux = Mathf.Lerp(topHeigth, finalHeigth, (Time.timeSinceLevelLoad - startTime) / time);
				}
				
				transform.position = new Vector3(transform.position.x, aux , transform.position.z);
				
			}
			
			else
			{
				if(Time.timeSinceLevelLoad >= endTime)
				{
					aux = topHeigth;
					lightOff = false;
					canChange = true;
				}		
				
				else
				{
					aux = Mathf.Lerp(finalHeigth, bottomHeigth, (Time.timeSinceLevelLoad - startTime) / time);
				}
				
				transform.position = new Vector3(transform.position.x, aux , transform.position.z);
			}
			
		}
	}
	
	
}
