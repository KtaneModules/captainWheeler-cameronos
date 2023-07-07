using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDisplays : MonoBehaviour {

	private float startTime;

	private float speedForDown = 1f;
	private float amplitude = 2f;
	private Vector3 startingPos;

	public void GoUp() {
		StartCoroutine(GoUpCoroutine());
	}

	public void GoBackDown()
	{
		StartCoroutine(ReturnCoroutine());
	}

	private IEnumerator GoUpCoroutine()
	{
		Vector3 targetPos = new Vector3(transform.localPosition.x, 0.0321f, transform.localPosition.z);
		float startTime = Time.time;
		float duration = 1f;
		//0.0087
		//bottom is -0.02 FOR DISPLAYS
		//wheel is 0.0184
		while(Time.time - startTime < duration)
		{
			float t = (Time.time - startTime) / duration;
			transform.localPosition = Vector3.Lerp(startingPos, targetPos, t);
			yield return null;
		}
		transform.localPosition=targetPos;
	}


	private IEnumerator ReturnCoroutine()
	{
		Vector3 getCurrentPos = transform.localPosition;
		Vector3 targetPos2 = new Vector3(transform.localPosition.x, -0.02f, transform.localPosition.z);
		float startTime = Time.time;
		float duration = 0.5f;
		//0.0087
		while(Time.time - startTime < duration)
		{
			float t = (Time.time - startTime) / duration;
			transform.localPosition = Vector3.Lerp(getCurrentPos, targetPos2, t);
			yield return null;
		}
		transform.localPosition=targetPos2;
	}

	// Use this for initialization
	void Start () {
		startingPos = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {

	}
}
