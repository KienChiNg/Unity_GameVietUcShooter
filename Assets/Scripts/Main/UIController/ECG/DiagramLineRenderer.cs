using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class DiagramLineRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    float width;
    float height;
    public float offset;
    public int numT;
    public UILineRenderer line;
    public Vector2[] sample;
    Vector2[] sampleOrigin;
    public float timeStep;
    int start;
    RectTransform rectTransform;
    bool isReady;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        isReady = false;
        sampleOrigin = (Vector2[])sample.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        if(isReady)
        {
            moveToLeft();
        } else
        {
            if (rectTransform.rect.width != 0 && rectTransform.rect.height != 0)
            {
                width = rectTransform.rect.width;
                height = rectTransform.rect.height;
                for (int i = 0; i < sample.Length; i++)
                {
                    sample[i].x *= width / numT;
                    sample[i].y *= height / 2;
                }
                start = 0;
                initBasicLine();
                isReady = true;
            }
        }
        
    }

    public void initBasicLine()
    {
        line.Points = new Vector2[sample.Length * (numT + 2)];
        for (int i = 0; i < line.Points.Length; i ++)
        {
            if(i == 0)
            {
                line.Points[i] = getNextPoint(-width / 2);
            } else
            {
                line.Points[i] = getNextPoint(line.Points[i-1].x);
            }
            
            start++;
           
        }

    }

    public Vector2 getNextPoint(float startX)
    {
        start = start % (sample.Length);
        Vector2 sampleVector = sample[start];
        if (sampleVector.x == 0)
        {
            return new Vector2(startX + offset * width / numT, sampleVector.y);
        } else
        {
            return new Vector2(startX + sampleVector.x, sampleVector.y );
        }
        
    }

    public void moveToLeft()
    {
        List<Vector2> vertices = new List<Vector2>(line.Points);

       
        if (vertices[1].x <= - width/2)
        {
            vertices.RemoveAt(0);
            vertices.Add(getNextPoint(vertices[vertices.Count - 1].x));
            start++;
        }
            

        for (int i = 0; i < vertices.Count; i ++)
        {
            vertices[i] = new Vector2(vertices[i].x - timeStep * width / numT, vertices[i].y);
        }

        line.Points = vertices.ToArray();
    }

    public void setSpo2Line(string spo2)
    {
        if(isReady)
        {
            float spo2F = 0;
            if (!float.TryParse(spo2, out spo2F))
            {
                spo2F = 0f;
            }

            float percent = spo2F / 100;

            for (int i = 0; i < sample.Length; i++)
            {
                sample[i].y = ((sampleOrigin[i].y + 1) * percent - 1) * height / 2;
            }
            start = 0;
            initBasicLine();
        }
        
    }

    public void setPressureLine(string systolicStr, string diastolicStr)
    {
        if (isReady)
        {
            int systolic = 0;
            int diastolic = 0;

            int.TryParse(systolicStr, out systolic);
            int.TryParse(diastolicStr, out diastolic);

            int average = diastolic + (systolic - diastolic) / 3;

            float systolicF = systolic / 75f;
            float diastolicF = diastolic / 75f;
            float averageF = average / 75f;

            sample[0].y = (diastolicF - 1) * height / 2;
            sample[1].y = (systolicF - 1) * height / 2;
            sample[2].y = (systolicF - 1) * height / 2;
            sample[3].y = (averageF - 1) * height / 2;
            sample[4].y = (diastolicF - 1) * height / 2;

            start = 0;
            initBasicLine();
        }
        
    }

    public void setCo2Line(string co2)
    {
        if(isReady)
        {
            float co2f;
            float.TryParse(co2, out co2f);

            if(co2f > 0)
            {
                for (int i = 0; i < sample.Length; i++)
                {
                    sample[i].y = sampleOrigin[i].y * height / 2;
                }
            } else
            {
                for(int i = 0; i < sample.Length; i ++)
                {
                    sample[i].y = 0;
                }
            }

            start = 0;
            initBasicLine();
        }
    }
}
