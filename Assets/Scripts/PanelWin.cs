using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelWin : MonoBehaviour
{
	public List<TMP_Text> namePlayerWin;
	public List<TMP_Text> timeShot;
	public TMP_Text winnerPing;
	public GameObject _5G;
	public GameObject otherNk;


	public void SetNamePlayerWin(int name)
	{
		if (name == 1)
		{
			_5G.SetActive(true);
			otherNk.SetActive(false);
		}
		else
		{
			_5G.SetActive(false);
			otherNk.SetActive(true);
		}
		foreach (TMP_Text namePlayer in namePlayerWin)
		{
			namePlayer.text = $"Player {name}";
		}
	}
	public void SetTimeShot(float time)
	{
		foreach (TMP_Text timeShotText in timeShot)
		{
			timeShotText.text = $"Bullet fired after<color=#1C94FC> {time:0.00}</color> seconds!";
		}
	}
	public void SetPing(float ping)
	{
		winnerPing.text = $"Ping: <color=#3D90D7> {ping}ms</color>";
	}
}
