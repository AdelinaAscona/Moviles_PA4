using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1" ||
            collision.gameObject.tag == "Player2" ||
            collision.gameObject.tag == "Player3")
            collision.gameObject.SetActive(false);
    }
}
