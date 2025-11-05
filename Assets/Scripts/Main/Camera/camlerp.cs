
using UnityEngine;

public class camlerp : MonoBehaviour {

	Transform curpos;
	public float smoothing;
	GameObject box;

	public Transform firstPos;

	public Transform ebePos;
	public Transform ebeStethoscopePos;
	public GameObject ebeBox;
	public GameObject ECGDiagramUI;
	NavMeshController controller = null;
	// Use this for initialization

	void Start () {
		curpos = transform;
		//ECGDiagramUI = GameObject.Find("ECGDiagramUI");
	}
	
	// Update is called once per frame
	void Update () {

		//lerp camera position
		if (Vector3.Distance(transform.position, curpos.position) >= 0.01f)
		{
			transform.position = Vector3.Lerp(transform.position, curpos.position, smoothing * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, curpos.rotation, smoothing * Time.deltaTime);
		}

		//return to first camera position when pressing ESCAPE
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			move(firstPos, null);
			//
			//if (controller != null)
			//{
			//	controller.MoveTo("door");
			//}
			//else
			//{
			//	controller = GameObject.Find("Player").GetComponent<NavMeshController>();
			//	if (controller)
			//	{
			//		controller.MoveTo("door");
			//	}

			//}
		}
		

	}

	public void move(Transform newpos, GameObject target){
		curpos = newpos;

		//enable click move to previous box
		if(box)
		{
			box.GetComponent<Collider>().enabled = true;
			box.GetComponent<highlight>().enabled = false;
		}


		if(target)
		{
			//disable click move to target box
			target.GetComponent<Collider>().enabled = false;
			target.GetComponent<highlight>().enabled = false;

			//disable ECGDiagramUI when move to monitor
			if (target.name.Equals("pos monitor"))
			{
				ECGDiagramUI.SetActive(false);
			}
			else
			{
				ECGDiagramUI.SetActive(true);
			}
		} else
        {
			ECGDiagramUI.SetActive(true);
		}
		
		//set new previous box
		box = target;
		//allow rotate
		GetComponent<CameraController>().enabled = true;
	}

	//moving camera to baby
	public void moveToEbe()
	{
		move(ebePos, ebeBox);
	}

	public void moveToEbeCamStethoscope()
    {
		move(ebeStethoscopePos, ebeBox);
		ECGDiagramUI.SetActive(false);
		GetComponent<CameraController>().enabled = false;
    }
}
