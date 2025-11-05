using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class StatusNetworking : MonoBehaviour
{
	[Header("Settings")]
	public string pingAddress = "8.8.8.8"; // Google DNS
	public string downloadUrl = "https://speedtest.tele2.net/10MB.zip"; // File test download
	public string uploadUrl = "https://jap-tok04-fibertest.redmangoanalytics.com:8080"; // Dịch vụ echo upload
	public int uploadSizeMB = 5;

	void Start()
	{
		StartCoroutine(RunNetworkCheck());
		StartCoroutine(CheckPing());
	}

	IEnumerator RunNetworkCheck()
	{
		yield return StartCoroutine(CheckDownloadSpeed());
		yield return StartCoroutine(CheckUploadSpeed());
	}

	IEnumerator CheckPing()
	{
		while (true)
		{
			UnityEngine.Ping ping = new UnityEngine.Ping(pingAddress);
			float timeout = 5f;
			float elapsed = 0f;

			while (!ping.isDone && elapsed < timeout)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}

			if (ping.isDone)
			{
				Debug.Log($"Ping to {pingAddress}: {ping.time} ms");
			}
			else
			{
				Debug.Log($"Ping to {pingAddress} failed or timed out.");
			}

			yield return new WaitForSeconds(1);
		}
	}

	IEnumerator CheckDownloadSpeed()
	{
		Debug.Log("Checking Download Speed...");

		UnityWebRequest request = UnityWebRequest.Get(downloadUrl);
		float startTime = Time.time;

		yield return request.SendWebRequest();

		float endTime = Time.time;

		if (request.result == UnityWebRequest.Result.Success)
		{
			float duration = endTime - startTime;
			float sizeMB = request.downloadedBytes / (1024f * 1024f);
			float speedMbps = (sizeMB / duration) * 8f;
			Debug.Log($"Download Speed: {speedMbps:F2} Mbps ({sizeMB:F2} MB in {duration:F2} s)");
		}
		else
		{
			Debug.Log("Download failed: " + request.error);
		}
	}

	IEnumerator CheckUploadSpeed()
	{
		Debug.Log("Checking Upload Speed...");

		int bytesToSend = uploadSizeMB * 1024 * 1024;
		byte[] data = new byte[bytesToSend];
		for (int i = 0; i < data.Length; i++) data[i] = 1;

		UnityWebRequest request = UnityWebRequest.Put(uploadUrl, data);
		request.method = UnityWebRequest.kHttpVerbPOST;
		request.SetRequestHeader("Content-Type", "application/octet-stream");

		float startTime = Time.time;

		yield return request.SendWebRequest();

		float endTime = Time.time;

		if (request.result == UnityWebRequest.Result.Success)
		{
			float duration = endTime - startTime;
			float speedMbps = (uploadSizeMB / duration) * 8f;
			Debug.Log($"Upload Speed: {speedMbps:F2} Mbps ({uploadSizeMB} MB in {duration:F2} s)");
		}
		else
		{
			Debug.Log("Upload failed: " + request.error);
		}
	}
}
