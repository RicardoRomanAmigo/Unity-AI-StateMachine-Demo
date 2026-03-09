using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player stats
    private float moveSpeed = 5f;

    public void Move(Vector2 direction)
    {
       transform.Translate(direction * Time.deltaTime * moveSpeed); 
    }
}
