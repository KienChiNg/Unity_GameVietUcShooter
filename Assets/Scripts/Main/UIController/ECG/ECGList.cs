using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSample
{
    public Vector2[] defaultSample;
    public Vector2[] normalSample;
    public int numT;
    public int normalTime;
    public bool sound;
}


public class ECGList 
{
    public static ECGList instance = new ECGList();

    public string[] listNames = { "Standstill", "Nhip_Xoang", "VF", "PVT", "3AVB", "AFIB", "AFLUTT", "ATACH", "IDIOV", "SRLVH", "SRRH", "SVT", "TORSADE", "PEA" };

    public WaveSample GetWaveSample(string wave)
    {
        switch(wave.ToUpper())
        {

            case "STANDSTILL":
                return getStandstill();

            case "NORMAL":
                return getNormal();

            case "PEA":
                return getPEA();

            case "VF":
                return getVF();

            case "PVT":
                return getPVT();
        }

        return getDeadLine();
    }

    private WaveSample getStandstill()
    {
        WaveSample wave = new WaveSample();
        wave.numT = 10;
        wave.normalTime = 1;
        wave.sound = false;

        Vector2[] defaultSample = new Vector2[12];
        defaultSample[0] = new Vector2(0, 0);
        defaultSample[1] = new Vector2(0.075f, 0);
        defaultSample[2] = new Vector2(0.075f, 0);
        defaultSample[3] = new Vector2(0.075f, 0);
        defaultSample[4] = new Vector2(0.075f, 0);
        defaultSample[5] = new Vector2(0.075f, 0);
        defaultSample[6] = new Vector2(0.075f, 0);
        defaultSample[7] = new Vector2(0.15f, 0);
        defaultSample[8] = new Vector2(0.15f, 0.2f);
        defaultSample[9] = new Vector2(0.05f, 0.2f);
        defaultSample[10] = new Vector2(0.1f, 0);
        defaultSample[11] = new Vector2(0.1f, 0);
        wave.defaultSample = defaultSample;

        Vector2[] normalSample = new Vector2[12];
        normalSample[0] = new Vector2(0, 0);
        normalSample[1] = new Vector2(0.075f, 0);
        normalSample[2] = new Vector2(0.075f, 0.15f);
        normalSample[3] = new Vector2(0.075f, 0.03f);
        normalSample[4] = new Vector2(0.075f, 1);
        normalSample[5] = new Vector2(0.075f, -0.5f);
        normalSample[6] = new Vector2(0.075f, -0.05f);
        normalSample[7] = new Vector2(0.15f, 0.15f);
        normalSample[8] = new Vector2(0.15f, 0.7f);
        normalSample[9] = new Vector2(0.05f, 0.65f);
        normalSample[10] = new Vector2(0.1f, 0.1f);
        normalSample[11] = new Vector2(0.1f, 0);
        wave.normalSample = normalSample;

        return wave;
    }

    private WaveSample getDeadLine()
    {
        WaveSample wave = new WaveSample();
        wave.numT = 10;
        wave.normalTime = 1;
        wave.sound = false;

        Vector2[] defaultSample = new Vector2[12];
        defaultSample[0] = new Vector2(0, 0);
        defaultSample[1] = new Vector2(0.075f, 0);
        defaultSample[2] = new Vector2(0.075f, 0);
        defaultSample[3] = new Vector2(0.075f, 0);
        defaultSample[4] = new Vector2(0.075f, 0);
        defaultSample[5] = new Vector2(0.075f, 0);
        defaultSample[6] = new Vector2(0.075f, 0);
        defaultSample[7] = new Vector2(0.15f, 0);
        defaultSample[8] = new Vector2(0.15f, 0);
        defaultSample[9] = new Vector2(0.05f, 0);
        defaultSample[10] = new Vector2(0.1f, 0);
        defaultSample[11] = new Vector2(0.1f, 0);
        wave.defaultSample = defaultSample;

        wave.normalSample = new Vector2[0];

        return wave;
    }

    private WaveSample getPEA()
    {
        WaveSample wave = new WaveSample();
        wave.numT = 10;
        wave.normalTime = 2;
        wave.sound = true;

        Vector2[] defaultSample = new Vector2[12];
        defaultSample[0] = new Vector2(0, 0);
        defaultSample[1] = new Vector2(0.075f, 0);
        defaultSample[2] = new Vector2(0.075f, 0.15f);
        defaultSample[3] = new Vector2(0.075f, 0.03f);
        defaultSample[4] = new Vector2(0.075f, 0.5f);
        defaultSample[5] = new Vector2(0.075f, -0.1f);
        defaultSample[6] = new Vector2(0.075f, 0);
        defaultSample[7] = new Vector2(0.15f, 0.15f);
        defaultSample[8] = new Vector2(0.15f, 0);
        defaultSample[9] = new Vector2(0.05f, 0);
        defaultSample[10] = new Vector2(0.1f, 0);
        defaultSample[11] = new Vector2(0.1f, 0);
        wave.defaultSample = defaultSample;

        Vector2[] normalSample = new Vector2[12];
        normalSample[0] = new Vector2(0, 0);
        normalSample[1] = new Vector2(0.075f, 0);
        normalSample[2] = new Vector2(0.075f, 0.15f);
        normalSample[3] = new Vector2(0.075f, 0.03f);
        normalSample[4] = new Vector2(0.075f, 1);
        normalSample[5] = new Vector2(0.075f, -0.5f);
        normalSample[6] = new Vector2(0.075f, -0.05f);
        normalSample[7] = new Vector2(0.15f, 0.15f);
        normalSample[8] = new Vector2(0.15f, 0.7f);
        normalSample[9] = new Vector2(0.05f, 0.65f);
        normalSample[10] = new Vector2(0.1f, 0.1f);
        normalSample[11] = new Vector2(0.1f, 0);
        wave.normalSample = normalSample;

        return wave;
    }
    private WaveSample getNormal()
    {
        WaveSample wave = new WaveSample();
        wave.numT = 10;
        wave.normalTime = 10;
        wave.sound = true;

        Vector2[] defaultSample = new Vector2[12];
        defaultSample[0] = new Vector2(0, 0);
        defaultSample[1] = new Vector2(0.075f, 0);
        defaultSample[2] = new Vector2(0.075f, 0.15f);
        defaultSample[3] = new Vector2(0.075f, 0.03f);
        defaultSample[4] = new Vector2(0.075f, 1);
        defaultSample[5] = new Vector2(0.075f, -0.5f);
        defaultSample[6] = new Vector2(0.075f, -0.05f);
        defaultSample[7] = new Vector2(0.15f, 0.15f);
        defaultSample[8] = new Vector2(0.15f, 0.7f);
        defaultSample[9] = new Vector2(0.05f, 0.65f);
        defaultSample[10] = new Vector2(0.1f, 0.1f);
        defaultSample[11] = new Vector2(0.1f, 0);
        wave.defaultSample = defaultSample;

        wave.normalSample = new Vector2[0];
        return wave;
    }

    private WaveSample getVF()
    {
        WaveSample wave = new WaveSample();
        wave.numT = 5;
        wave.normalTime = 10;
        wave.sound = true;

        Vector2[] defaultSample = new Vector2[28];
        defaultSample[0] = new Vector2(0, 0);
        defaultSample[1] = new Vector2(0.075f, 0.5f);
        defaultSample[2] = new Vector2(0.075f, -0.7f);
        defaultSample[3] = new Vector2(0.075f, 0.03f);
        defaultSample[4] = new Vector2(0.075f, -0.2f);
        defaultSample[5] = new Vector2(0.075f, 0.7f);
        defaultSample[6] = new Vector2(0.075f, -0.4f);
        defaultSample[7] = new Vector2(0.075f, 0.15f);
        defaultSample[8] = new Vector2(0.075f, -0.4f);
        defaultSample[9] = new Vector2(0.075f, 0.15f);
        defaultSample[10] = new Vector2(0.075f, -1f);
        defaultSample[11] = new Vector2(0.075f, 0.1f);
        defaultSample[12] = new Vector2(0.075f, -0.3f);
        defaultSample[13] = new Vector2(0.075f, 0.2f);
        defaultSample[14] = new Vector2(0.075f, 0.5f);
        defaultSample[15] = new Vector2(0.075f, -0.6f);
        defaultSample[16] = new Vector2(0.075f, 0.1f);
        defaultSample[17] = new Vector2(0.075f, 0.3f);
        defaultSample[18] = new Vector2(0.075f, -0.3f);
        defaultSample[19] = new Vector2(0.075f, -0.7f);
        defaultSample[20] = new Vector2(0.075f, 0.2f);
        defaultSample[21] = new Vector2(0.075f, 0.4f);
        defaultSample[22] = new Vector2(0.075f, -0.2f);
        defaultSample[23] = new Vector2(0.075f, -0.5f);
        defaultSample[24] = new Vector2(0.075f, 0.2f);
        defaultSample[25] = new Vector2(0.075f, 0.6f);
        defaultSample[26] = new Vector2(0.075f, -0.1f);
        defaultSample[27] = new Vector2(0.075f, 0.3f);

        wave.defaultSample = defaultSample;

        wave.normalSample = new Vector2[0];
        return wave;
    }

    private WaveSample getPVT()
    {
        WaveSample wave = new WaveSample();
        wave.numT = 10;
        wave.normalTime = 10;
        wave.sound = true;

        Vector2[] defaultSample = new Vector2[16];
        defaultSample[0] = new Vector2(0, 0);
        defaultSample[1] = new Vector2(0.05f, -0.4f);
        defaultSample[2] = new Vector2(0.093f, 0.4f);
        defaultSample[3] = new Vector2(0.05f, 0.5f);
        defaultSample[4] = new Vector2(0.093f, -0.5f);
        defaultSample[5] = new Vector2(0.05f, -0.6f);
        defaultSample[6] = new Vector2(0.093f, 0.7f);
        defaultSample[7] = new Vector2(0.05f, 0.8f);
        defaultSample[8] = new Vector2(0.093f, -0.7f);
        defaultSample[9] = new Vector2(0.05f, -0.8f);
        defaultSample[10] = new Vector2(0.093f, 0.5f);
        defaultSample[11] = new Vector2(0.05f, 0.6f);
        defaultSample[12] = new Vector2(0.093f, -0.5f);
        defaultSample[13] = new Vector2(0.05f, -0.6f);
        defaultSample[14] = new Vector2(0.093f, 0.3f);
        defaultSample[15] = new Vector2(0.05f, 0.4f);

        wave.defaultSample = defaultSample;

        wave.normalSample = new Vector2[0];
        return wave;
    }
}
