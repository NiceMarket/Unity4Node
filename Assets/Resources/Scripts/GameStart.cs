using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	public const string URL = "http://127.0.0.1:3100/";
	public string UserName = "username";
	public string Password = "password";

	public Hashtable cookieHeaders = new Hashtable();

	private void SendLogin(string account, string password) {
		WWWForm form = new WWWForm();
		form.AddField("user", UserName);
		form.AddField("password", Password);
		WWW www = new WWW(URL+"login", form.data);
		StartCoroutine(WaitLogin(www));
	}

	IEnumerator WaitLogin(WWW www)
	{
		while (!www.isDone)
			yield return null;  
		
		if (string.IsNullOrEmpty(www.error))
		{
			if (www.responseHeaders.ContainsKey("SET-COOKIE"))
			{
				cookieHeaders.Clear();
				cookieHeaders.Add("COOKIE", www.responseHeaders ["SET-COOKIE"]);
				Debug.Log(www.responseHeaders ["SET-COOKIE"]);
			}

			Debug.Log(www.text);
		} else
			Debug.Log(www.error);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GSocket.Get.ReceivePotocol();
    }
    
    void OnGUI() {
		GUI.Label(new Rect(220, 100, 100, 40), "UserName");
		UserName = GUI.TextArea(new Rect(300, 100, 100, 40), UserName, 255);
		GUI.Label(new Rect(220, 150, 100, 40), "Password");
		Password = GUI.TextArea(new Rect(300, 150, 100, 40), Password, 255);

		if (GUI.Button(new Rect(100, 100, 100, 100), "Http Login")) {
			SendLogin(UserName, Password);
		}

		if (GUI.Button(new Rect(100, 300, 200, 60), "Socket.io Connect")) {
			GSocket.Get.Connect();
		}
	}
}
