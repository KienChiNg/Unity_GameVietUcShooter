using UnityEngine;

namespace KienChi
{
	public class CamAutoRotate : MonoBehaviour
	{
		public Vector3 rotateSpeed;

        void Update()
        {
			transform.Rotate(rotateSpeed * Time.deltaTime);
        }

    }
}
