using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button sendButton;
    [SerializeField] Button resetButton;

    [SerializeField] Button startButton;
    [SerializeField] Button playAgainButton;

    [SerializeField] private Player player;
    [SerializeField] private Database database;
    [SerializeField] private ScoreSO score;
    [SerializeField] private CanvasManager canvasManager;

    [SerializeField] private DetectorController[] detectors;

    public List<User> playerData;
    public List<int> player1;
    public List<int> player2;
    public List<int> player3;

    private void Start()
    {
        score.ResetScore();
        ActiveDetectors(false);
        //database.SetReadyPlayer(false);
        sendButton.interactable = true;
        resetButton.interactable = true;

        startButton.interactable = false;
        playAgainButton.interactable = true;

        StartCoroutine(database.GetLastThreeUsers(UpdateLastThreeUsers));
    }

    private void Update()
    {
        UpdatePiecesData();
        database.SetScoreData(score.score);
        canvasManager.UpdatePlayersScore();
    }

    public void OnClick_SendData()
    {
        database.SetPiecesData(player.piecesList);
        //database.SetReadyPlayer(true);

        sendButton.interactable = false;
        resetButton.interactable = false;
        startButton.interactable = true;

        Debug.Log("Datos Enviados");
    }

    public void OnClick_Reset()
    {
        for (int i = 0; i < player.piecesButton.Length; i++)
        {
            player.piecesCount = 0;
            player.player1[i].SetActive(false);
            player.piecesButton[i].interactable = true;
        }

        //database.SetReadyPlayer(false);
        player.piecesList.Clear();
    }

    public void OnClick_Start()
    {
        ActivePlayerPieces();
        Invoke("ActiveDetectors", 1f);

        startButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(true);

        //database.SetScoreData(score.score);
        //canvasManager.UpdatePlayersScore();
    }

    public void OnClick_PlayAgain()
    {
        ActiveDetectors(false);
        //database.SetReadyPlayer(false);
        database.ResetPiecesData();
        player.piecesList.Clear();

        sendButton.interactable = true;
        resetButton.interactable = true;

        startButton.interactable = false;
        playAgainButton.interactable = true;

        player.piecesCount = 0;

        for (int i = 0; i < player.player1.Length; i++)
        {
            player.player1[i].SetActive(false);
            player.player2[i].SetActive(false);
            player.player3[i].SetActive(false);

            player.piecesButton[i].interactable = true;
        }

        for (int i = 0; i < detectors.Length; i++)
        { 
            detectors[i].count = new int[3];
        }

        score.ResetScore();
        startButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(false);
    }

    private void UpdateLastThreeUsers(List<User> users)
    {
        playerData = users;
    }

    public void UpdatePiecesData()
    {
        StartCoroutine(GetAndSetPiecesData(playerData[0].codeID.ToString(), pieces => { player1 = pieces; }));
        StartCoroutine(GetAndSetPiecesData(playerData[1].codeID.ToString(), pieces => { player2 = pieces; }));
        StartCoroutine(GetAndSetPiecesData(playerData[2].codeID.ToString(), pieces => { player3 = pieces; }));
    }

    private IEnumerator GetAndSetPiecesData(string userID, Action<List<int>> onComplete)
    {
        yield return database.GetPiecesData(userID, onComplete);
    }

    private void ActivePlayerPieces()
    {
        for (int i = 0; i < player2.Count; i++)
        {
            player.player2[player2[i]].SetActive(true);
            player.player3[player3[i]].SetActive(true);
        }
    }
    private void ActiveDetectors(bool active)
    {
        for (int i = 0; i < detectors.Length; i++)
        {
            detectors[i].gameObject.SetActive(active);
        }
    }

    private void ActiveDetectors()
    {
        for (int i = 0; i < detectors.Length; i++)
        {
            detectors[i].gameObject.SetActive(true);
        }
    }

}
