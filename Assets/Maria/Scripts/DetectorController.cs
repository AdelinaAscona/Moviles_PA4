using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorController : MonoBehaviour
{
    [SerializeField] private ScoreSO score;
    public SpriteRenderer spriteRenderer;

    public int[] count;

    private void Start()
    {
        count = new int[3];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1")
            count[0]++;
        else if (collision.gameObject.tag == "Player2")
            count[1]++;
        else if (collision.gameObject.tag == "Player3")
            count[2]++;

        if (count[0]+count[1]+count[2] == 3)
            spriteRenderer.enabled = true;
        else
            spriteRenderer.enabled = false;


        if (spriteRenderer.enabled == true)
        {
            for (int i = 0; i < count[0]; i++)
                score.UpdateScore(10);
        }
    }
}