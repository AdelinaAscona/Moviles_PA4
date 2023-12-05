using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject[] pieces;
    [SerializeField] private Button[] piecesButton;

    [SerializeField] private int piecesCount;

    private void Start()
    {
        piecesCount = 0;
    }

    public void OnClick_CkechingPieces()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i].activeSelf)
            {
                piecesCount++;
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

    public void OnClick_Reset()
    {
        for (int i = 0; i < piecesButton.Length; i++)
        {
            piecesCount = 0;
            pieces[i].SetActive(false);
            piecesButton[i].interactable = true;
        }
    }
}
