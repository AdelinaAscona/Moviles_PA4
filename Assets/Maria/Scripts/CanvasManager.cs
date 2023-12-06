using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
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

    //private void UpdatePlayersScore()
    //{
    //    for (int i = 0; i < playersScore.Length; i++)
    //    {
    //        playersScore[i].text = userName;
    //    }
    //}

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
