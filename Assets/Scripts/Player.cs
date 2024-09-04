using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float playerSpeed = 1.0f;
    [SerializeField] GameInput gameInput;

    private bool isWalking;
    private void Update()
    {
        
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movDirection = new Vector3(inputVector.x,0.0f,inputVector.y);
        transform.position += movDirection * playerSpeed * Time.deltaTime;

        isWalking = movDirection != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movDirection, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }


}
