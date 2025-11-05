using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class SimbabyController : MonoBehaviour, IPunObservable
{
    public static SimbabyController instance;
    public Animator ani;
    public Animator dayMonitorAnimator;
   
    public Animator diencucMonitor;
    public AudioSource audioSource;
    public GameObject body, introBabyBody;
    public GameObject domdai;
    public GameObject[] thooxy;
    public GameObject datNKQ;
    public GameObject divat;
    Material[] bodySkin, introBabyBodySkin;
    public bool skinChange = false;
    public List<int> skinIds;
    int code;
    //eyes
    public Material eyeMaterial;
    public float radius = 0.08f;
    public float percentLerp = 0;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        this.gameObject.name = "Simbaby";
    }

    void Start()
    {
        //init skin of simbaby
        bodySkin = body.GetComponent<Renderer>().materials;
        introBabyBodySkin = introBabyBody.GetComponent<Renderer>().materials;
        skinIds = new List<int>();
    }

    void Update()
    {
        //enable sound when lesson start
        //if(!audioSource.isPlaying && GameStateController.instance.roomstate == 1)
        //{
        //    audioSource.Play();
        //}

        //update skin color when skinChange = true
        if(skinChange)
        {
            int finish = 0;
            for(int i = 1; i < bodySkin.Length; i ++)
            {
                //increase alpha chanel of skin to 255
                int alphaCh = ((Color32)bodySkin[i].GetColor("_Color")).a;

                if (skinIds.Contains(i) && alphaCh < 255)
                {
                    //simbaby
                    bodySkin[i].SetColor("_Color", new Color32(255, 255, 255, (byte)(alphaCh + 5)));
                    //intro baby
                    introBabyBodySkin[i].SetColor("_Color", new Color32(255, 255, 255, (byte)(alphaCh + 5)));
                } else if(!skinIds.Contains(i) && alphaCh > 0)
                {
                    //simbaby
                    bodySkin[i].SetColor("_Color", new Color32(255, 255, 255, (byte)(alphaCh - 5)));
                    //intro baby
                    introBabyBodySkin[i].SetColor("_Color", new Color32(255, 255, 255, (byte)(alphaCh - 5)));
                } else
                {
                    finish++;
                }
            }
            
            if(finish == bodySkin.Length)
            {
                skinChange = false;
            }

        }

        float eyeMaterialRadius = eyeMaterial.GetFloat("_Radius");

        if (eyeMaterialRadius != radius && percentLerp <= 1)
        {
            percentLerp += Time.deltaTime / 1f;
            eyeMaterial.SetFloat("_Radius", Mathf.Lerp(eyeMaterialRadius, radius, percentLerp));
        }
    }
    // hiển thị các trạng thái của symbaby tải về từ server 
    public void displayStateDetails(State state)
    {
        List<int> listSkin = new List<int>();

        //update skin color follow received State
        if(state.state.ContainsKey("DA"))
        {
            listSkin.Add(int.Parse(state.state["DA"].getValue()));
        }

        if(listSkin.Count > 0)
        {
            changeSymbabySkin(listSkin);
        }

        //update head state  follow received State
        if (state.state.ContainsKey("DAU"))
        {
            ani.SetInteger("dau", int.Parse(state.state["DAU"].getValue()));
            dayMonitorAnimator.SetInteger("dau", int.Parse(state.state["DAU"].getValue()));
        }

        //update phlegm follow received State 
        if(state.state.ContainsKey("DOMDAI"))
        {
            State_details domdai_detail = state.state["DOMDAI"];
            if (domdai_detail.valueType == 3 && domdai_detail.value.Equals("TRUE"))
            {
                domdai.SetActive(true);
            } else
            {
                domdai.SetActive(false);
            }
        }

        //enable/disable oxy mask follow receive State
        if(state.state.ContainsKey("OXY"))
        {
            State_details oxy_detail = state.state["OXY"];
            if (oxy_detail.valueType == 3 && oxy_detail.value.Equals("TRUE"))
            {
                for(int i = 0; i < thooxy.Length; i ++)
                {
                    thooxy[i].SetActive(true);
                }
                
            }
            else
            {
                for (int i = 0; i < thooxy.Length; i++)
                {
                    thooxy[i].SetActive(false);
                }
            }
        }

        //change voice
        if(state.state.ContainsKey("TIENGKEU"))
        {
            State_details voice = state.state["TIENGKEU"];

            switch(voice.getValue().ToUpper().Trim())
            {
                case "HO":
                    audioSource.clip = Resources.Load<AudioClip>("Sound/Mouth/HO");
                    break;
                case "NON":
                    audioSource.clip = Resources.Load<AudioClip>("Sound/Mouth/NON");
                    break;
                case "LA_HET":
                    audioSource.clip = Resources.Load<AudioClip>("Sound/Mouth/LAHET");
                    break;
                case "REN_RI":
                    audioSource.clip = Resources.Load<AudioClip>("Sound/Mouth/RENRI");
                    break;
                default:
                    audioSource.clip = null;
                    break;
            }

            audioSource.loop = true;
        }

        //change idle state if awrr khac 0
        if(state.state.ContainsKey("AWRR"))
        {
            State_details awrr_detail = state.state["AWRR"];
            
            if(awrr_detail.getValue().Equals("0"))
            {
                ani.SetBool("tho", false);
                dayMonitorAnimator.SetBool("tho", false);
            } else
            {
                ani.SetBool("tho", true);
                dayMonitorAnimator.SetBool("tho", true);
            }
        }

        //update arm, leg action
        if (state.state.ContainsKey("TAYTRAI") && state.state.ContainsKey("TAYPHAI") && state.state.ContainsKey("CHANPHAI") && state.state.ContainsKey("CHANTRAI"))
        {
            int taytrai = int.Parse(state.state["TAYTRAI"].getValue());
            int tayphai = int.Parse(state.state["TAYPHAI"].getValue());
            int chanphai = int.Parse(state.state["CHANPHAI"].getValue());
            int chantrai = int.Parse(state.state["CHANTRAI"].getValue());
            int dau = int.Parse(state.state["DAU"].getValue());

            code = SymbabyAnimationCode.GetCGCode(taytrai, tayphai, chantrai, chanphai, dau);

            SetCogiatCode(code);
        }

        //update refill duration
        if(state.state.ContainsKey("REFILL"))
        {
            string refill = state.state["REFILL"].getValue();

        }
        
    }

    //display oxy mask
    public void ShowThoOxy()
    {
        
    }
    //hidden oxy mask
    public void HiddenThoOxy()
    {
        //hide oxy mask
        for (int i = 0; i < thooxy.Length; i++)
        {
            thooxy[i].SetActive(false);
        }
    }

    //display animation when lesson is completed
    public void ShowHealthyState()
    {
        ani.SetLayerWeight(ani.GetLayerIndex("khoe"), 1);
        dayMonitorAnimator.SetLayerWeight(ani.GetLayerIndex("khoe"), 1);

        
        
    }

    //hien thi trang thai co giat
    public void SetCogiatCode(int cgcode)
    {
        if (cgcode == 0)
        {
            ani.SetInteger("cogiat", cgcode);
            dayMonitorAnimator.SetInteger("cogiat", cgcode);
            
        }
        else
        {
            ani.SetInteger("cogiat", code);
            dayMonitorAnimator.SetInteger("cogiat", code);
        }
        //ani.SetInteger("cogiat", cgcode);
    }

    public void SetArmCode(int armcode)
    {
        ani.SetInteger("tay", armcode);
        dayMonitorAnimator.SetInteger("tay", armcode);
    }

    public void SetCoGiatLayerWeight(float weight)
    {
        ani.SetLayerWeight(ani.GetLayerIndex("cogiat"), weight);
        dayMonitorAnimator.SetLayerWeight(dayMonitorAnimator.GetLayerIndex("cogiat"), weight);
  
    }

    public void ChocXuongChay()
    {
        //run straight leg animation
        ani.SetInteger("chan", 1);
        dayMonitorAnimator.SetInteger("chan", 1);
    }

    public void DatNKQ()
    {
        //run open mouth animation
        ani.SetTrigger("mieng");
        dayMonitorAnimator.SetTrigger("mieng");
    }

    //change skin color of simbaby
    public void changeSymbabySkin(List<int> skinIds)
    {
        this.skinIds = skinIds;
        skinChange = true;
    }
  
    //open and close eye 
    public void EyeState(int eye)
    {
        if(ani.GetInteger("cogiat") == 0)
        {
            ani.SetInteger("mat", eye);
        }
    }

    //co gian dong tu
    public void BanKinhDongTu(float radius)
    {
        percentLerp = 0;
        this.radius = radius;
    }

    //test PXAS
    public void TestPXAS()
    {

    }

    //set layer weight
    public void ChangeState(string name, float value)
    {
        ani.SetLayerWeight(ani.GetLayerIndex(name), value);
    }

   

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
       
    }
    
}
