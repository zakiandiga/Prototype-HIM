using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Assertions;
using UnityEngine.Networking;

// This code was greatly inspired by Matthew Ventures' great sample code:
// http://www.mrventures.net/all-tutorials/downloading-google-sheets
public class GoogleSheetSimpleConditionalConversation : MonoBehaviour {

	public string googleSheetDocID = "1wlWYeWrr-wAScjgJxtmpGl_1R6v3ONrYK5Kg9tPPuyM";
	private string url;

	void Start() 
	{
		url = "https://docs.google.com/spreadsheets/d/" + googleSheetDocID + "/export?format=csv";

		// This line starts the download of the google sheet.
		StartCoroutine(DownloadData(AfterDownload));
	}

	internal IEnumerator DownloadData(System.Action<string> onCompleted)
	{
		yield return new WaitForEndOfFrame();

		string downloadData = null;
		using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {

			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError) {
				Debug.Log("Download Error: " + webRequest.error);
			} else {
				Debug.Log("Download success");
				//Debug.Log("Data: " + webRequest.downloadHandler.text);
				downloadData = webRequest.downloadHandler.text;
			}
		}

		onCompleted(downloadData);
	}

	public void AfterDownload( string data ) 
	{
        if ( null == data ) {
			// Display a notification that this is likely due to poor internet connectivity
			Debug.LogError( "Was not able to download data or retrieve stale data." );
        }
        else {
			List<Dictionary<string, object>> dataAsList = CSVReader_GoogleSheet.Read(data);

			StartCoroutine( ProcessData( dataAsList, AfterProcessData ) );
        }
    }

	// This is where you need to apply the spreadsheet in whatever way fits your application.
	// At this point, we have 'data' which is a List of Dictionaries where each one is a row in
	// the spreadsheet. Each key in the dictionary is a spreadsheet cell, where the "header" 
	// (the top of the spreadsheet) is the key, and the value of the cell is the value of the
	// dictionary.
	public IEnumerator ProcessData( List<Dictionary<string,object>> data, System.Action<string> onCompleted) 
    {
		// Not sure why this needs to be here, but it does to make the coroutine happy.
		yield return new WaitForEndOfFrame();

		// Create our instance of SimpleConditionalConversation with the data we loaded
		DialogueManager.scc = new SimpleConditionalConversation(data);
		DialogueManager.LoadInitialSCCState();
		
		// The code below was for a different implementation, but does have usieful information
		// about how to parse the data that CSVReader gives us.
		//Dictionary<string, int> gameState = new Dictionary<string, int>();
		//foreach(Dictionary<string, object> row in data) {
		//	// row is a row in the spreadsheet. See above for description.
		//	foreach (KeyValuePair<string, object> cell in row) {
		//		// cell.Key is the header, cell.Value is the value of the cell
		//		Debug.Log(cell.Key + ": " + cell.Value);
		//	}
		//}

		onCompleted( null );
    }

	private void AfterProcessData(string errorMessage)
	{
		if (null != errorMessage) {
			Debug.LogError("Was not able to process data: " + errorMessage);
		} else {
			// Put something here if you want to do something after all is loaded and processed
		}
	}

}