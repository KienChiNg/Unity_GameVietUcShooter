using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class EventSynchronizeCharacter : xrInput, IPunObservable, IOnEventCallback
{
	public AudioSource audioSource;
	public AudioClip triggerSound;
	bool canTrigger = true;
	bool canTriggerBtnGuide = true;
	bool btnGuide = false;
	bool isShoot = false;
	public float fireRate = 0.2f;
	private float nextFireTime = 0f;
	public ParticleSystem muzzleFlash;
	public GameObject guide;
	private MainCanvas mainCanvas;
	private float timer = 1f;
	public GameObject canvasGO;
	public TMP_Text namePlayerTxt;
	public TMP_Text pingTxt;
	private int pingStream;
	private string namePlayer;
	public Transform headGun;
	public Transform footGun;
	public LineRenderer line;
	public Material materialLineHit;
	public Material materialNormal;
	public GameObject canvasBullet;
	public List<GameObject> bulletLists;
	public GameObject reload;
	public TMP_Text log;
	private int numOfBullet = 2;
	public GameObject _5G;
	public GameObject otherNk;
	private int isMin;

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(isShoot);
			stream.SendNext(namePlayer);
			stream.SendNext(pingStream);
			stream.SendNext(isMin);
		}
		else
		{
			isShoot = (bool)stream.ReceiveNext();
			namePlayer = (string)stream.ReceiveNext();
			pingStream = (int)stream.ReceiveNext();
			isMin = (int)stream.ReceiveNext();
		}
		// OVRManager.display.RecenterPose();
	}
	public override void Start()
	{
		base.Start();
		line.positionCount = 2;

		RemoveWaitingPanel();
		canvasBullet.SetActive(false);
		line.enabled = false;
		if (!GetComponent<PhotonView>().IsMine) return;
		GameRole _role1 = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];
		switch (_role1)
		{
			case GameRole.CHEST_COMPRESSION:
				namePlayer = "Player 1";
				isMin = 1;
				pingStream = PhotonNetwork.GetPing();
				break;
			case GameRole.AMBU_MASK:
				namePlayer = "Player 2";
				isMin = 2;
				pingStream = PhotonNetwork.GetPing() + UnityEngine.Random.Range(50, 70);
				break;
		}

		RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
		SendOptions sendOptions = new SendOptions { Reliability = true };

		// PhotonNetwork.RaiseEvent(PhotonEventCodes.NAME_PLAYER, namePlayer, raiseEventOptions, sendOptions);
		// PhotonNetwork.RaiseEvent(PhotonEventCodes.PING_PLAYER, PhotonNetwork.GetPing(), raiseEventOptions, sendOptions);
		canvasBullet.SetActive(true);
		// line.enabled = true;

		guide.SetActive(true);

		mainCanvas = FindObjectOfType<MainCanvas>();
		GameObject gameObjectCanvas = mainCanvas.gameObject;

		GameRole _role = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];

		switch (_role)
		{
			case GameRole.CHEST_COMPRESSION:

				gameObjectCanvas.transform.rotation = Quaternion.Euler(90, 180, -130);
				break;
			case GameRole.AMBU_MASK:
				gameObjectCanvas.transform.rotation = Quaternion.Euler(90, 0, -130); ;
				break;
		}
	}
	// [PunRPC]
	void RemoveWaitingPanel()
	{
		Launcher.instance.RemoveWaiting();
	}
	// [PunRPC]
	void DisplayNamePlayer(string name)
	{
		if (isMin == 1)
		{
			_5G.SetActive(true);
			otherNk.SetActive(false);
		}
		if (isMin == 2)
		{
			_5G.SetActive(false);
			otherNk.SetActive(true);
		}
		namePlayerTxt.text = name;
	}
	void FixedUpdate()
	{
		DisplayNamePlayer(namePlayer);
		DisplayStat(pingStream);
		// Vector3 direction = (headGun.position - footGun.position).normalized;
		// float distance = 500;
		// RaycastHit hit;
		// Vector3 endPoint = headGun.position + direction * distance;
		// if (Physics.Raycast(headGun.position, direction, out hit, distance, LayerMask.GetMask("Player")))
		// {
		// 	line.material = materialLineHit;

		// 	endPoint = hit.point;
		// }
		// else
		// {
		// 	line.material = materialNormal;
		// }

		// line.SetPosition(0, headGun.position);
		// line.SetPosition(1, endPoint);
	}
	float timeDelay = 0;
	bool isShootPlayer2 = false;
	public override void Update()
	{
		if (!GetComponent<PhotonView>().IsMine) return;

		RaycastHit hit;
		Vector3 direction = (headGun.position - footGun.position).normalized;
		float distance = 500;
		// Debug.DrawLine(headGun.position,  headGun.position + direction * distance, Color.red);

		targetDeviceRight.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue_R);
		targetDeviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue_R);

		if (primaryButtonValue_R)
		{
			if (!canTriggerBtnGuide) return;
			canTriggerBtnGuide = false;
			guide.SetActive(btnGuide);
			btnGuide = !btnGuide;
		}
		else
		{
			canTriggerBtnGuide = true;
		}

		if (triggerValue_R > 0.1f && Time.time >= nextFireTime && numOfBullet > 0)
		{
			if (!canTrigger)
			{
				return;
			}
			GameRole _role1 = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];
			switch (_role1)
			{
				case GameRole.CHEST_COMPRESSION:
					// namePlayer = "Player 1";
					Shoot();
					break;
				case GameRole.AMBU_MASK:
					// namePlayer = "Player 2";
					timeDelay = UnityEngine.Random.Range(0.4f, 1);
					isShootPlayer2 = true;
					pingStream = PhotonNetwork.GetPing() + UnityEngine.Random.Range(50, 70);
					break;
			}

			// if (Physics.Raycast(headGun.position, direction, out hit, distance, LayerMask.GetMask("Player")))
			// {
			// }
			canTrigger = false;
			nextFireTime = Time.time + fireRate;
		}
		if (isShootPlayer2)
		{
			timeDelay -= Time.deltaTime;
		}
		if (timeDelay <= 0 && isShootPlayer2)
		{
			isShootPlayer2 = false;
			Shoot();
		}

		if (triggerValue_R < 0.1f)
		{
			canTrigger = true;
		}


		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			// RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
			// SendOptions sendOptions = new SendOptions { Reliability = true };
			// if (!GetComponent<PhotonView>().IsMine) return;
			GameRole _role1 = (GameRole)PhotonNetwork.LocalPlayer.CustomProperties["gameRole"];
			switch (_role1)
			{
				case GameRole.CHEST_COMPRESSION:
					// namePlayer = "Player 1";
					pingStream = PhotonNetwork.GetPing();
					break;
				case GameRole.AMBU_MASK:
					// namePlayer = "Player 2";
					pingStream = PhotonNetwork.GetPing() + UnityEngine.Random.Range(50, 70);
					break;
			}
			// PhotonNetwork.RaiseEvent(PhotonEventCodes.PING_PLAYER, PhotonNetwork.GetPing(), raiseEventOptions, sendOptions);
			timer = 1f;
		}
	}
	void LateUpdate()
	{
		if (GetComponent<PhotonView>().IsMine)
		{
			canvasGO.SetActive(false);
			return;
		}

		if (Camera.main != null)
		{
			Vector3 direction = Camera.main.transform.position - canvasGO.transform.position;

			direction.y = 0;

			if (direction.sqrMagnitude > 0.001f)
			{
				Quaternion targetRotation = Quaternion.LookRotation(direction);
				canvasGO.transform.rotation = targetRotation * Quaternion.Euler(0, 180f, 0);
			}

		}
	}
	void Shoot()
	{
		// audioSource.PlayOneShot(triggerSound);
		EventSynchronizeManager.instance.CheckShot(pingStream);

		numOfBullet--;
		bulletLists[numOfBullet].SetActive(false);
		GetComponent<PhotonView>().RPC("PlayShootSound", RpcTarget.All);
		if (numOfBullet == 0)
			StartCoroutine(ReloadBullet());
	}
	IEnumerator ReloadBullet()
	{
		reload.SetActive(true);
		yield return new WaitForSeconds(2);
		numOfBullet = 2;
		foreach (GameObject bullet in bulletLists)
		{
			bullet.SetActive(true);
		}
		reload.SetActive(false);
	}
	[PunRPC]
	void PlayShootSound()
	{
		audioSource.PlayOneShot(triggerSound);
		muzzleFlash.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
		muzzleFlash.Play();
	}
	// [PunRPC]
	void DisplayStat(int ping)
	{
		// Debug.Log(PhotonNetwork.GetPing());
		if (ping < 100)
			pingTxt.text = $"Ping: <color=#0088FF>{ping} ms</color>";
		else
			pingTxt.text = $"Ping: <color=red>{ping} ms</color>";
	}

	public void OnEvent(EventData photonEvent)
	{
		switch (photonEvent.Code)
		{
			case PhotonEventCodes.NAME_PLAYER:
				DisplayNamePlayer((string)photonEvent.CustomData);
				break;
			case PhotonEventCodes.PING_PLAYER:
				DisplayStat((int)photonEvent.CustomData);
				// GameStatManager.instance.SetPingForPlayerWinner((int)photonEvent.CustomData);
				break;
		}
	}
	public void OnEnable()
	{

		PhotonNetwork.AddCallbackTarget(this);
	}

	public void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
	}
}
