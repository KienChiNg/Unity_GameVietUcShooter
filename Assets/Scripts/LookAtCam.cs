using UnityEngine;

public class LookAtCam : MonoBehaviour
{
	private void Update()
	{
		Vector3 lookPos = Camera.main.transform.position;
		lookPos.y = transform.position.y; // Giữ nguyên trục Y (chỉ xoay quanh trục Y)

		transform.LookAt(lookPos);
		transform.Rotate(0, 180, 0);
	}
}
