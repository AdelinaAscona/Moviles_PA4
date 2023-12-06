using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button sendButton;
    [SerializeField] Button resetButton;

    [SerializeField] private Player player;
    [SerializeField] private Database database;

    [SerializeField] private int[] playerID;

    public int[][] playerPieces;

    private void Start()
    {
        sendButton.interactable = true;
        resetButton.interactable = true;
    }

    public void OnClick_SendData()
    {
        UpdatePlayersID();
        database.SetPiecesData(player.piecesList);
        UpdatePiecesData();

        sendButton.interactable = false;
        ActivePlayerPieces();

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

        player.piecesList.Clear();
        database.ResetPiecesData();
        sendButton.interactable = true;
    }

    private void UpdatePiecesData()
    {
        for (int i = 0; i < playerID.Length; i++)
        {
            playerPieces[i] = database.GetPiecesData(playerID[0].ToString()).ToArray();
        }

        Debug.Log("Datos Recibidos");
    }

    public void ActivePlayerPieces()
    {
        for (int i = 1; i < player.player2.Length; i++)
        {
            player.player2[playerPieces[0][i]].SetActive(true);
            player.player3[playerPieces[1][i]].SetActive(true);
        }
    }

    private void UpdatePlayersID()
    {
        StartCoroutine(database.GetCodesID(GetPlayersID));
    }

    private void GetPlayersID(List<int> codesID)
    {
        playerID = codesID.ToArray();
    }
}
