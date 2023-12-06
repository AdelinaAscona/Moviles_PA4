using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Database database;

    [SerializeField] private TextMeshProUGUI[] playersNickName;
    [SerializeField] private TextMeshProUGUI[] playersScore;

    private void Start()
    {
        ResetPlayersScore();
    }

    private void Update()
    {
        UpdatePlayersText();
    }

    private void UpdatePlayersText()
    {
        for (int i = 0; i < gameManager.playerData.Count; i++)
        {
            string userName = UsernameFromEmail(gameManager.playerData[i].email);
            playersNickName[i].text = userName;
        }
    }

    public void UpdatePlayersScore()
    {
        for (int i = 0; i < playersScore.Length; i++)
        {
            int playerIndex = i;
            StartCoroutine(GetAndSetPlayersScore(gameManager.playerData[i].codeID.ToString(), score => {
                playersScore[playerIndex].text = score.ToString();
            }));
        }
    }

    private IEnumerator GetAndSetPlayersScore(string userID, Action<int> onComplete)
    {
        yield return database.GetScoreData(userID, onComplete);
    }

    private void ResetPlayersScore()
    {
        for (int i = 0; i < playersScore.Length; i++)
        {
            playersScore[i].text = "0";
        }
    }

    private string UsernameFromEmail(string email)
    {
        string[] parts = email.Split('@');

        if (parts.Length > 0)
        {
            return parts[0];
        }

        return string.Empty;
    }
}
