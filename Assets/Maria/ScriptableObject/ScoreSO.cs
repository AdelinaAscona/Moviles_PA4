using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "ScriptableObject/Data/Score", order = 0)]
public class ScoreSO : ScriptableObject
{
    [SerializeField] private int _score;
    public int score => _score;

    public void UpdateScore(int num)
    {
        _score+=num;
    }

    public void ResetScore()
    {
        _score = 0;
    }
}