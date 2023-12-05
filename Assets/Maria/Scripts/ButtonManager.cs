using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Database database;

    public List<int> player1;
    public List<int> player2;
    public List<int> player3;

    public void OnClick_SendData()
    {
        database.SetPiecesData(player.piecesList);
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
    }

    public void OnClick_UpdateData(string userID)
    {
        //database.GetPiecesData(player.piecesList , userID);
        Debug.Log("Datos Recibidos");
    }
}
