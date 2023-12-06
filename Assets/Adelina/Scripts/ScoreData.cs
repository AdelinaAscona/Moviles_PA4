using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScoreData", menuName = "ScriptableObjects/ScoreData")]
public class ScoreData : ScriptableObject
{
    public int puntaje = 0;

    private bool puntajeIniciado = false;

    public void StartGame()
    {

        if (!puntajeIniciado)
        {
            puntaje = 0; // Inicia el puntaje en 0 
            puntajeIniciado = true; // Marca el puntaje como iniciado.
        }
    }

    public void UpdateScore()
    {
        
    }

    
}
