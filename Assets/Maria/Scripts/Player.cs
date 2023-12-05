using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject[] player1;

    public GameObject[] player2;
    public GameObject[] player3;

    public Button[] piecesButton;

    public int piecesCount;
    public List<int> piecesList;

    private void Start()
    {
        piecesCount = 0;
    }

    public void OnClick_CkechingPieces()
    {
        for (int i = 0; i < player1.Length; i++)
        {
            if (player1[i].activeSelf)
            {
                piecesCount++;
                piecesList.Add(i);
                Debug.Log("Pieza N°" + i + " - Activada");
            }
            else
                Debug.Log("Pieza N°" + i + " - Desactivada");
        }

        DesactiveButton();
    }

    private void DesactiveButton()
    {
        if (piecesCount > 3)
        {
            for (int i = 0; i < piecesButton.Length; i++)
            {
                piecesButton[i].interactable = false;
            }
        }
    }
}
