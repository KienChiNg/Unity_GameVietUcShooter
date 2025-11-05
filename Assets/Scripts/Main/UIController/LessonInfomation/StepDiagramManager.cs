using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StepDiagramManager : MonoBehaviour
{
    public static StepDiagramManager instance;
    public GameObject openDiagramBtn;
    public Scrollbar scrollbar;
    public Image diagram;

    public UIEffectComponent openEffect;
    public UIEffectComponent closeEffect;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        openDiagramBtn.GetComponent<Button>().onClick.AddListener(delegate
        {
            OpenPopup();
        });

        if (PhotonNetwork.IsMasterClient)
        {
            openDiagramBtn.SetActive(true);
        } else
        {
            openDiagramBtn.SetActive(false);
        }

        Sprite img = Resources.Load<Sprite>("Sprite/Diagram/" + PhotonNetwork.CurrentRoom.CustomProperties["lessonCode"]);
        if(img != null)
        {
            diagram.GetComponent<Image>().sprite = img;
            diagram.GetComponent<Image>().preserveAspect = true;
        } else
        {
            diagram.GetComponent<Image>().sprite = null;
        }

        gameObject.SetActive(false);
    }

    public void OpenPopup()
    {
        if(!UIEffectController.instance.isGameObjectExist(gameObject))
        {
            gameObject.SetActive(true);
            openEffect.DisplayEffect(gameObject, true);
        }
    }

    public void ClosePopup()
    {
        if (!UIEffectController.instance.isGameObjectExist(gameObject))
        {
            closeEffect.DisplayEffect(gameObject, false);
        }
        
    }

}
