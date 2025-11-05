
using UnityEngine;
using UnityEngine.UI;

//this script make UI 2D follow 3D game object 
public class PlayerUI : MonoBehaviour
{

    [Tooltip("Pixel offset from the player target")]
    public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);

    [Tooltip("UI Text to display Player's Name")]
    public Text NameText;


    public GameObject _target;

    float _characterControllerHeight = 0f;

    Transform _targetTransform;

    Renderer _targetRenderer;

    Vector3 _targetPosition;

    void Awake()
    {
        //this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void setLabel(GameObject target, string name, Vector3 offset)
    {
        _target = target;
        _targetTransform = _target.GetComponent<Transform>();
        _targetRenderer = _target.GetComponent<Renderer>();
        ScreenOffset = offset;
        NameText.text = name;
    }

    void Update()
    {
        // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
        if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }

    }

    void LateUpdate()
    {

        // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
        if (_targetRenderer != null)
        {
            //this.gameObject.SetActive(_targetRenderer.isVisible);
            //Debug.Log(_targetRenderer.isVisible);
        }

        // #Critical
        // Follow the Target GameObject on screen.
        if (_targetTransform != null)
        {
            _targetPosition = _targetTransform.position;
            _targetPosition.y += _characterControllerHeight;

            this.transform.position = Camera.main.WorldToScreenPoint(_targetPosition) + ScreenOffset;
        }

    }
}