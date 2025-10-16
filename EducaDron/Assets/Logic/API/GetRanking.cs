using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetRanking : MonoBehaviour
{
    string URLGet = "http://localhost:5062/api/users/points/ranking";

    public IEnumerator GetRankingData(System.Action<List<RankingData>> onSuccess)
    {
        UnityWebRequest req = UnityWebRequest.Get(URLGet);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en el Get" + req.error);

        }
        else
        {
            string json = req.downloadHandler.text;
            Debug.Log("Received JSON: " + json);

            RankingDataList rankingDataList = JsonUtility.FromJson<RankingDataList>(json);
                onSuccess?.Invoke(rankingDataList.dataList);

        }
    }
}

[System.Serializable]
public class RankingData
{
    public string id;
    public string username;
    public int totalPoints;
}
[System.Serializable]
public class RankingDataList
{
    public List<RankingData> dataList;
}
