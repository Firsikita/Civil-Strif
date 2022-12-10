using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	[SerializeField] private float shakeDuration = 0f;
	[SerializeField] private float shakeAmount = 0.1f;
	[SerializeField] private float decreaseFactor = 1.0f;

	private Transform camTransform;

	Vector3 originalPos;

	void Awake()
	{
		Cursor.visible = false;
		if (camTransform == null)
		{
			camTransform = GetComponent<Transform>();
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	private void Update()
    {
		ShakeCamera();		
	}

    private void ShakeCamera()
    {
		if (shakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
	}

    public void StartShake()
	{
		originalPos = camTransform.localPosition;
		shakeDuration = 0.2f;
	}
}
