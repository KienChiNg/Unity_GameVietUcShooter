using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMemberListController : MonoBehaviour
{
    public static RoomMemberListController instance;
    public GameObject prefab;
    public GameObject listParent;
    public Text groupName;

    public UIEffectComponent openEffect;
    public UIEffectComponent closeEffect;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    //close popup function
    public void ClosePopup()
    {
        closeEffect.DisplayEffect(gameObject, false);
    }

    //open popup function
    public void OpenPopup()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        openEffect.DisplayEffect(gameObject, true);
    }

    //update UI follow list Users
    public void UpdateRoomMemberList(List<User> users)
    {
        //update amount of members
        groupName.text = users.Count + " người";

        //destroy old prefabs
        for(int i = 0; i < listParent.transform.childCount; i ++)
        {
            Destroy(listParent.transform.GetChild(i).gameObject);
        }

        //init new prefabs
        for (int i = 0; i < users.Count; i ++)
        {
            GameObject row = Instantiate(prefab, listParent.transform);

            if(i == 0)
            {
                row.transform.GetChild(0).GetComponent<Text>().text = "Giáo viên";
            } else
            {
                if (users[i].memRole == 0)
                {
                    row.transform.GetChild(0).GetComponent<Text>().text = "Bác sĩ";
                }
                else
                {
                    row.transform.GetChild(0).GetComponent<Text>().text = "Điều dưỡng";
                }
            }
            
            
            row.transform.GetChild(1).GetComponent<Text>().text = users[i].userName;
            row.transform.GetChild(2).GetComponent<Text>().text = "NTW001";
            row.transform.GetChild(3).GetComponent<Text>().text = "Bệnh viện Nhi Trung ương";

            if(i % 2 == 1)
            {
                row.GetComponent<Image>().color = new Color32(78, 195, 196, 60);
            }
        }
    }
}
