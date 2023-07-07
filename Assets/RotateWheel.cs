using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWheel : MonoBehaviour {

	private static int spinSpeed = 150;
	private static int direction = 0;
	private int speedToUse = spinSpeed * direction;
	private Quaternion initialRotation;

	private float startTime;
  private float duration = 30f;
	private bool spinning = true;
	private bool buttonPressed = false;

	private float speedForDown = 1f;
	private float amplitude = 2f;
	private Vector3 startingWheelPos;
	private Vector3 location;

	public void RestoreRotation() {
				transform.localRotation = initialRotation;
			}

			public void FindLocation() {
						location = transform.localPosition;
					}

			public void ChangeDirection (int catchDirect) {
				switch(catchDirect)
				{
					case 0:
					direction = -1;
					break;

					case 1:
					direction = 1;
					break;
				}
				}

			public void ChangeSpeed (int speed) {
					switch(speed)
					{
						case 0:
						spinSpeed = 40;
						speedToUse = spinSpeed * direction;
						break;

						case 1:
						spinSpeed = 160;
						speedToUse = spinSpeed * direction;
						break;

						case 2:
						spinSpeed = 380;
						speedToUse = spinSpeed * direction;
						break;
					}
				}


			public void Spin () {
					StartCoroutine(SpinCoroutine());
				}

				public void StopSpin () {
						spinning = false;
						StopCoroutine(SpinCoroutine());
					}

				public void ChangeScreens () {
						StartCoroutine(ScreenCoroutine());
					}


					public void ComeBackUp() {
						speedToUse = 400;
						buttonPressed = true;
						StartCoroutine(GoUpCoroutine());
					}

				public void GoDown() {
					speedToUse = 600;
					buttonPressed = true;
					StartCoroutine(GoDownCoroutine());
				}

private IEnumerator GoDownCoroutine()
{
	Vector3 targetPos = new Vector3(transform.localPosition.x, 0.0087f, transform.localPosition.z);
	float startTime = Time.time;
	float duration = 0.5f;
	//0.0087
	while(Time.time - startTime < duration)
	{
		float t = (Time.time - startTime) / duration;
		transform.localPosition = Vector3.Lerp(startingWheelPos, targetPos, t);
		yield return null;
	}
	transform.position=targetPos;
	buttonPressed=false;
}

private IEnumerator GoUpCoroutine()
{
    FindLocation();
    Vector3 targetPos = new Vector3(transform.localPosition.x, 0.0184f, transform.localPosition.z);
    float startTime = Time.time;
    float duration = 0.5f;

    while (Time.time - startTime < duration)
    {
        float t = (Time.time - startTime) / duration;
        transform.localPosition = Vector3.Lerp(location, startingWheelPos, t);
        yield return null;
    }

    transform.localPosition = startingWheelPos;
    buttonPressed = false;
}


private IEnumerator SpinCoroutine()
{
		float startTime = Time.time;
		while (spinning)
		{
				transform.Rotate(0, speedToUse * Time.deltaTime, 0);
				yield return null; // Wait for the next frame
		}
}

private IEnumerator ScreenCoroutine()
{
		float startTime = Time.time;
		while (Time.time - startTime < duration)
		{
				transform.Rotate(0, speedToUse * Time.deltaTime, 0);
				yield return null; // Wait for the next frame
		}
}

	void Start () {
		startingWheelPos = transform.localPosition;
		RestoreRotation();
	}

	// Update is called once per frame
	void Update () {
		//nothing
	}
}
