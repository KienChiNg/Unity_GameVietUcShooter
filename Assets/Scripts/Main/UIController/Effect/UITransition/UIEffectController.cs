using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType { Translate, RotateAroundAxis, Scale };
public enum Axis { axisX, axisY, axisZ };

[System.Serializable]
public class Effect 
{
    public EffectType type;
    public float duration;
    public float delay;
    public float percent = 0f;
    public bool isCompleted = false;

    //property for Vector3 lerp
    public Vector3 v3Current;
    public Vector3 v3Start;
    public Vector3 v3End;

    //property for Rotate
    public Axis rotateAxis;
    public bool isClockwise;
    public int rotateTimes;

    public void Reset()
    {
        v3Current = v3Start;
        percent = 0f;
        isCompleted = false;
    }

    public Vector3 Vector3Lerp() 
    {
        percent += Time.deltaTime / duration;

        if (percent < 1f)
        {
           v3Current = Vector3.Lerp(v3Current, v3End, percent);
        }
        else
        {
            isCompleted = true;
        }

        return v3Current;
    }

    public Quaternion RotateAroundAxis()
    {
        float angle = 0;
        percent += Time.deltaTime / duration;
        Vector3 axis = Vector3.up;

        //declare axis vector
        switch(rotateAxis)
        {
            case Axis.axisX:
                axis = Vector3.right;
                break;
            case Axis.axisY:
                axis = Vector3.up;
                break;
            case Axis.axisZ:
                axis = Vector3.forward;
                break;
        }

        if (percent < 1f)
        {
            angle = (Time.deltaTime / duration) * rotateTimes * 360;
        }
        else
        {
            if(!isCompleted)
            {
                angle = (1 + (Time.deltaTime / duration) - percent) * rotateTimes * 360;
                isCompleted = true;
            } else
            {
                angle = 0;
            }
            
        }

        if (isClockwise)
        {
            return Quaternion.AngleAxis(angle, axis);
        }
        else
        {
            return Quaternion.AngleAxis(-angle, axis);
        }
    }
}


[System.Serializable]
public class UIEffect
{
    public GameObject gameObject;
    public List<Effect> effects = new List<Effect>();
    public float currentTime = 0;
    public bool finalState = true;

    public void Run()
    {
        currentTime += Time.deltaTime;

        foreach(Effect e in effects)
        {
            switch(e.type)
            {
                case EffectType.Translate:
                    if(currentTime >= e.delay)
                    {               
                        gameObject.transform.localPosition = e.Vector3Lerp();
                    }
                    break;
                case EffectType.RotateAroundAxis:
                    if (currentTime >= e.delay)
                    {
                        gameObject.transform.rotation *= e.RotateAroundAxis();
                    }
                    break;
                case EffectType.Scale:
                    if (currentTime >= e.delay)
                    {
                        gameObject.transform.localScale = e.Vector3Lerp();
                    }
                    break;
            }
        }
    }

    public bool isCompleted()
    {
        for(int i = 0; i < effects.Count; i ++)
        {
            if(!effects[i].isCompleted)
            {
                return false;
            }
        }

        if(!finalState)
        {
            gameObject.SetActive(false);
        }

        return true;
    }

    public void Reset()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Reset();
        }
    }
}


public class UIEffectController : MonoBehaviour
{
    public static UIEffectController instance;
    public List<UIEffect> effectLists;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        effectLists = new List<UIEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < effectLists.Count; i ++)
        {
            if(effectLists[i].isCompleted())
            {
                effectLists.Remove(effectLists[i]);
            } else
            {
                effectLists[i].Run();
            }
        }
    }

    public bool Run(UIEffect effect)
    {
        if(!isGameObjectExist(effect.gameObject))
        {
            effect.Reset();
            effectLists.Add(effect);
            return true;
        }

        return false;
    }

    public bool isGameObjectExist(GameObject effectGO)
    {
        for(int i = 0; i < effectLists.Count; i ++)
        {
            if(effectGO.Equals(effectLists[i].gameObject))
            {
                return true;
            }
        }
        return false;
    }

}
