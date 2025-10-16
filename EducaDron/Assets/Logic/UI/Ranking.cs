using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Ranking : MonoBehaviour
{
    [SerializeField] GameObject rankPrefab;
    [SerializeField] Transform contentParent;
    [SerializeField] GetRanking rankingApi;

    [SerializeField] Sprite goldMedal;
    [SerializeField] Sprite silverMedal;
    [SerializeField] Sprite bronzeMedal;

    void Start()
    {
        /*
        List<(string name, int points)> boceto = new()
        {
            ("Alex", 540),
            ("Elise", 410),
            ("Dorian", 385),
            ("Tanwen", 200),
            ("Kael", 190),
        };
        LoadRanking(boceto);
        */

        StartCoroutine(rankingApi.GetRankingData(LoadRanking));
    }

    void LoadRanking(List<RankingData> rankingData)
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        int rank = 1;

        foreach (var player in rankingData)
        {
            var entryGO = Instantiate(rankPrefab, contentParent);

            TextMeshProUGUI rankText = entryGO.transform.Find("RankText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = entryGO.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI pointsText = entryGO.transform.Find("PointsText").GetComponent<TextMeshProUGUI>();
            
            Image medalImage = entryGO.transform.Find("MedalImage").GetComponent<Image>();

            rankText.text = rank.ToString();
            nameText.text = player.username;
            pointsText.text = $"{player.totalPoints} pts";

            rankText.color = Color.white;
            medalImage.gameObject.SetActive(false);

            switch (rank)
            {
                case 1:
                    rankText.color = Color.yellow;
                    medalImage.sprite = goldMedal;
                    medalImage.gameObject.SetActive(true);
                    break;
                case 2:
                    rankText.color = new Color(0.75f, 0.75f, 0.75f);
                    medalImage.sprite = silverMedal;
                    medalImage.gameObject.SetActive(true);
                    break;
                case 3:
                    rankText.color = new Color(0.65f, 0.5f, 0.2f);
                    medalImage.sprite = bronzeMedal;
                    medalImage.gameObject.SetActive(true);
                    break;
            }
            rank++;
        }
    }

}
