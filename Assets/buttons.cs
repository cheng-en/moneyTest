using UnityEngine;
using System.Collections;
using System;
using System.IO;
using JsonDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using OnePF;


public class buttons : MonoBehaviour {

	public UILabel testLabel;

	string id = "app_2";
	string urlLocal = "http://60.250.133.80:80/API.php?";
	//string device_id = "app_2";

	public bool leave = false;
	public bool down = true;

	public float timeAdd = 0;
	public float timeSecond = 0;

	public GameObject panel;
	public GameObject ClickA;
	public GameObject ClickB;
	public GameObject PushA;
	public GameObject PushB;
	public GameObject AdA;
	public GameObject AdB;
	public GameObject Change;
	public GameObject OperatingA;
	public GameObject OperatingB;
	public GameObject ErrorA;
	public GameObject ErrorB;
	public GameObject Exit;
	public GameObject Refresh;

	public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	//string phoneURL = Application.persistentDataPath+"/";

	public const string TestA = "testa";
	public const string TestB = "testb";
	public const string TestC = "testc";


	/// <summary>
	/// Shop is active right now
	/// </summary>
	private bool _processingPayment = false;
	/// <summary>
	/// If shop window is on screen
	/// </summary>
	private bool _showShopWindow = false;
	/// <summary>
	/// Text in the popup window
	/// </summary>



	// Use this for initialization
	void Start () {

		try{

		UIEventListener.Get(ClickA).onClick = ClickAFun;
		UIEventListener.Get(ClickB).onClick = ClickBFun;
		UIEventListener.Get(PushA).onClick = PushAFun;
		UIEventListener.Get(PushB).onClick = PushBFun;
		UIEventListener.Get(AdA).onClick = AdAFun;
		UIEventListener.Get(AdB).onClick = AdBFun;
		UIEventListener.Get(Change).onClick = ChangeFun;
		UIEventListener.Get(Exit).onClick = ExitFun;
		UIEventListener.Get(Refresh).onClick = RefreshFun;
		UIEventListener.Get(ErrorA).onClick = ErrorAFun;
		UIEventListener.Get(ErrorB).onClick = ErrorBFun;
		UIEventListener.Get(OperatingA).onClick = OperatingAFun;
		UIEventListener.Get(OperatingB).onClick = OperatingBFun;

		OpenIABEventManager.billingSupportedEvent += OnBillingSupported;
		OpenIABEventManager.billingNotSupportedEvent += OnBillingNotSupported;
		OpenIABEventManager.queryInventorySucceededEvent += OnQueryInventorySucceeded;
		OpenIABEventManager.queryInventoryFailedEvent += OnQueryInventoryFailed;
		OpenIABEventManager.purchaseSucceededEvent += OnPurchaseSucceded;
		OpenIABEventManager.purchaseFailedEvent += OnPurchaseFailed;
		OpenIABEventManager.consumePurchaseSucceededEvent += OnConsumePurchaseSucceeded;
		OpenIABEventManager.consumePurchaseFailedEvent += OnConsumePurchaseFailed;
		OpenIABEventManager.transactionRestoredEvent += OnTransactionRestored;
		OpenIABEventManager.restoreSucceededEvent += OnRestoreSucceeded;
		OpenIABEventManager.restoreFailedEvent += OnRestoreFailed;

		OpenIAB.mapSku(TestA, OpenIAB_iOS.STORE, "testa");
		OpenIAB.mapSku(TestB, OpenIAB_iOS.STORE, "testb");
		OpenIAB.mapSku(TestC, OpenIAB_iOS.STORE, "testc");

		// Map SKUs for Google Play
		OpenIAB.mapSku(TestA, OpenIAB_Android.STORE_GOOGLE, "testa");
		OpenIAB.mapSku(TestC, OpenIAB_Android.STORE_GOOGLE, "testb");
		OpenIAB.mapSku(TestB, OpenIAB_Android.STORE_GOOGLE, "testc");

		var options = new OnePF.Options();
		
		options.checkInventory = true;
		options.prefferedStoreNames = new string[] { OpenIAB_Android.STORE_YANDEX };
		options.verifyMode = OptionsVerifyMode.VERIFY_EVERYTHING;

		// Add Google Play public key
		options.storeKeys.Add(OpenIAB_Android.STORE_GOOGLE, "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlUgq4pLrryMv246bM44BfDUmpx/51RiwJtQvaiJYP18U1Ga/s5z+GKuhg+64UzCfj935sFqtuhDULQJ/MrCjUxMa24psr1YxY4FFkiV1HAzqnhoszm66x0xlnJf8C6r3K1tBYpaj6l+r9qKCjmtsp6NSNt0ubIhN1DGmpQAhCB+mIV/TA4ZZ9wB89pu+ml2Gt/CrdH4QPx/zX/W128J13lNrSBoGy0c2KUUTUWA1ZryXtQwFSaU9HVVMlW8th8l1yw2S3EYmx+StepUyDanaC/yemjNAY1FCeUngo72Qxco3dDwFWGwzcgVUkjLqISo+u1p9g1MlezVl0Wp4Kvu9IQIDAQAB");



		OpenIAB.init(options);
		
			testLabel.text = "44";

		}catch(Exception e){

			testLabel.text = "333";

			testLabel.text = e.ToString();


		}

	}

	bool VerifyDeveloperPayload(string developerPayload)
	{
		/*
         * TODO: verify that the developer payload of the purchase is correct. It will be
         * the same one that you sent when initiating the purchase.
         * 
         * WARNING: Locally generating a random string when starting a purchase and 
         * verifying it here might seem like a good approach, but this will fail in the 
         * case where the user purchases an item on one device and then uses your app on 
         * a different device, because on the other device you will not have access to the
         * random string you originally generated.
         *
         * So a good developer payload has these characteristics:
         * 
         * 1. If two different users purchase an item, the payload is different between them,
         *    so that one user's purchase can't be replayed to another user.
         * 
         * 2. The payload must be such that you can verify it even when the app wasn't the
         *    one who initiated the purchase flow (so that items purchased by the user on 
         *    one device work on other devices owned by the user).
         * 
         * Using your own server to store and verify developer payloads across app
         * installations is recommended.
         */
		return true;
	}



	private void OnBillingSupported()
	{
		Debug.Log("Billing is supported");
		OpenIAB.queryInventory(new string[] { TestC, TestB, TestA });
	}
	
	private void OnBillingNotSupported(string error)
	{
		Debug.Log("Billing not supported: " + error);
	}
	
	private void OnQueryInventorySucceeded(Inventory inventory)
	{
		Debug.Log("Query inventory succeeded: " + inventory);
		
		// Do we have the infinite ammo subscription?
		Purchase godModePurchase = inventory.GetPurchase(TestB);
		bool godModeSubscription = (godModePurchase != null && VerifyDeveloperPayload(godModePurchase.DeveloperPayload));
		Debug.Log("User " + (godModeSubscription ? "HAS" : "DOES NOT HAVE") + " god mode subscription");

		
		// Check premium skin purchase
		Purchase cowboyHatPurchase = inventory.GetPurchase(TestC);
		bool isPremiumSkin = (cowboyHatPurchase != null && VerifyDeveloperPayload(cowboyHatPurchase.DeveloperPayload));
		Debug.Log("User " + (isPremiumSkin ? "HAS" : "HAS NO") + " premium skin");

		
		// Check for delivery of expandable items. If we own some, we should consume everything immediately
		Purchase repairKitPurchase = inventory.GetPurchase(TestA);
		if (repairKitPurchase != null && VerifyDeveloperPayload(repairKitPurchase.DeveloperPayload))
			OpenIAB.consumeProduct(inventory.GetPurchase(TestA));
	}
	
	private void OnQueryInventoryFailed(string error)
	{
		Debug.Log("Query inventory failed: " + error);
	}
	
	private void OnPurchaseSucceded(Purchase purchase)
	{

		testLabel.text = "QQQ";

		Debug.Log("Purchase succeded: " + purchase.Sku + "; Payload: " + purchase.DeveloperPayload);
		if (!VerifyDeveloperPayload(purchase.DeveloperPayload))
			return;
		
		// Check what was purchased and update game
		switch (purchase.Sku)
		{
		case TestA:
			testLabel.text = "TestA  onPurchase!";
			// Consume repair kit
			OpenIAB.consumeProduct(purchase);
			break;
		case TestB:
			testLabel.text = "TestB  onPurchase!";
			OpenIAB.consumeProduct(purchase);
			break;;
		case TestC:
			testLabel.text = "TestC  onPurchase!";
			OpenIAB.consumeProduct(purchase);
			break;
		default:
			Debug.LogWarning("Unknown SKU: " + purchase.Sku);
			break;
		}
		_processingPayment = false;
	}
	
	private void OnPurchaseFailed(int errorCode, string error)
	{
		Debug.Log("Purchase failed: " + error);
		_processingPayment = false;
	}
	
	private void OnConsumePurchaseSucceeded(Purchase purchase)
	{
		Debug.Log("Consume purchase succeded: " + purchase.ToString());
		_processingPayment = false;
	}
	
	private void OnConsumePurchaseFailed(string error)
	{
		Debug.Log("Consume purchase failed: " + error);
		_processingPayment = false;
	}
	
	private void OnTransactionRestored(string sku)
	{
		Debug.Log("Transaction restored: " + sku);
	}
	
	private void OnRestoreSucceeded()
	{
		Debug.Log("Transactions restored successfully");
	}
	
	private void OnRestoreFailed(string error)
	{
		Debug.Log("Transaction restore failed: " + error);
	}









	void ClickAFun(GameObject button){

		try{
			OpenIAB.purchaseProduct(TestA);
		}catch(Exception e){

			testLabel.text = e.ToString();
		}

	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
//		// check for errors
//		if (www.error == null)
//		{
//			Debug.Log("WWW Ok!: " + www.data);
//		} else {
//			Debug.Log("WWW Error: "+ www.error);
//		} 
//
//		if(leave == true){
//
//			Application.Quit();
//		}
	}



	void ClickBFun(GameObject button){
		OpenIAB.purchaseSubscription(TestB);


	}
	void PushAFun(GameObject button){
		OpenIAB.purchaseSubscription(TestC);

	}



	void PushBFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "PushB";
		
		string doing = "mode=3&app_id=" + id + "&device_name=" + name + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);
		
		StartCoroutine (WaitForRequest(www));
	}
	void AdAFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "AdA";
		
		string doing = "mode=6&app_id=" + id + "&advertising_id=" + name + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);

		StartCoroutine (WaitForRequest(www));
	}
	void AdBFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "AdB";
		
		string doing = "mode=6&app_id=" + id + "&advertising_id=" + name + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);

		StartCoroutine (WaitForRequest(www));
	}
	void ChangeFun(GameObject button){
		
		down = !down;
		if(down == true){
			panel.transform.localPosition = new Vector3(0,0,0);
		}else{
			panel.transform.localPosition = new Vector3(0,-550,0);

		}

	}
	void ExitFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "AdB";
		
		string doing = "mode=2&app_id=" + id + "&use_time=" + (int)timeSecond + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);

		leave = true;

		StartCoroutine (WaitForRequest(www));
	}
	void RefreshFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "AdB";
		
		string doing = "mode=2&app_id=" + id + "&use_time=" + (int)timeSecond + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);

		StartCoroutine (WaitForRequest(www));

		timeSecond = 0;
	}
	void ErrorAFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "ErrorA";
		
		string doing = "mode=5&app_id=" + id + "&function_id=0&function_name=" + name + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);

		StartCoroutine (WaitForRequest(www));
	}
	void ErrorBFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "ErrorB";
		
		string doing = "mode=5&app_id=" + id + "&function_id=0&function_name=" + name + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);

		StartCoroutine (WaitForRequest(www));
	}
	void OperatingAFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "OperatingA";
		
		string doing = "mode=7&app_id=" + id + "&function_id=0&function_name=" + name + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);

		StartCoroutine (WaitForRequest(www));
	}
	void OperatingBFun(GameObject button){
		int CurrTime = ToIntTime();
		
		string name = "OperatingB";
		
		string doing = "mode=7&app_id=" + id + "&function_id=0&function_name=" + name + "&time=" + CurrTime + "&device_id=01";
		
		string url = urlLocal + doing;
		
		Debug.Log (url);
		
		WWW www = new WWW (url);

		StartCoroutine (WaitForRequest(www));
	}
	
	// Update is called once per frame
	void Update () {
		timeSecond += Time.deltaTime ;


	}

	public static int ToIntTime() {

		DateTime aDate = DateTime.Now;

		if (aDate == DateTime.MinValue) {
			return -1;
		}
		TimeSpan span = (aDate - UnixEpoch);
		return (int)Math.Floor(span.TotalSeconds);
	}


	public void Checkfrom(){

//		string JsonFoodDataPath = "storage/emulated/0/Android/data/com.iii.demo/files/associate.txt";
//		string testPath = Application.persistentDataPath + "/associate.txt";
//
//		
//		if(!File.Exists(JsonFoodDataPath)){
//			List<object> Food = null;
//
//			//testLabel.text = "1";
//			
//		
//				Food = new List<object> ();
//				File.WriteAllText(JsonFoodDataPath, JsonConvert.SerializeObject(new{Food},Formatting.Indented));
//				
//			//testLabel.text = "2";
//				
//				Food = (JsonConvert.DeserializeObject<JObject> (File.ReadAllText(JsonFoodDataPath))["Food"] as JArray).ToObject<List<object>>();
//				
//				
//					var food = new {
//						From = id,
//					};
//					
//					Food.Add(food);
//				
//				File.WriteAllText(JsonFoodDataPath, JsonConvert.SerializeObject(new{Food},Formatting.Indented));
//				
//	
//		}else{
//		
//			//testLabel.text = "3";
//			
//			JArray ja = JsonConvert.DeserializeObject<JObject> (File.ReadAllText (JsonFoodDataPath)) ["Food"] as JArray;
//			
//			int jaLength = ja.Count-1;
//
//			string From = (string)ja[jaLength]["From"];
//
//			int CurrTime = ToIntTime();
//
//			string urlAsso ="mode=1&app_id_a="+From+"&app_id_b="+ id +"&time="+CurrTime+"&device_id=01";
//
//			string url = urlLocal + urlAsso;
//			
//			Debug.Log (url);
//			
//			WWW www = new WWW (url);
//			
//			StartCoroutine (WaitForRequest(www));
//
//
//			List<object> Food = null;
//			
//			
//			Food = new List<object> ();
//			File.WriteAllText(JsonFoodDataPath, JsonConvert.SerializeObject(new{Food},Formatting.Indented));
//			
//			
//			
//			Food = (JsonConvert.DeserializeObject<JObject> (File.ReadAllText(JsonFoodDataPath))["Food"] as JArray).ToObject<List<object>>();
//			
//		
//				var food = new {
//					From = id,
//				};
//				
//				Food.Add(food);
//			
//			File.WriteAllText(JsonFoodDataPath, JsonConvert.SerializeObject(new{Food},Formatting.Indented));
			


//		}
//
//
	}




}
