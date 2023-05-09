using UnityEngine;

public class UISlime : MonoBehaviour
{
    private Vector3 moveUpPosition;
    private bool positionReached;

    private void Start()
    {
        // Set the position to move to
        moveUpPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f);
    }

    void Update()
    {
        // Move the slime to position
        if (!positionReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveUpPosition, 0.1f);

            if (Vector3.Distance(transform.position, moveUpPosition) < 0.01f)
            {
                positionReached = true;
            }
        }
        else // Move slime off screen and slowly rotate
        {
            moveUpPosition = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, moveUpPosition, 0.001f);
            transform.Rotate(Vector3.up * 0.01f);
        }
        
    }
}
