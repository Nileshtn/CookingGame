using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArg> OnSelectedCounterChange;
    public class OnSelectedCounterChangedEventArg : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] LayerMask counterLayermask;

    //private bool isPlayerCollition;
    private bool isWalking;
    private Vector3 lastMovementTracker;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("there is more than one player");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnIntractAction += GameInput_OnIntractAction;
    }

    private void Update()
    {
        MovementHandeller();
        InteractionHandeller();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void GameInput_OnIntractAction(object sender, System.EventArgs e)
    {
        selectedCounter?.Interact();
    }

    private void InteractionHandeller()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        if(moveDirection != Vector3.zero )
        {
            lastMovementTracker = moveDirection;
        }
        float intrationRadius = 1.5f;
        if(Physics.Raycast(transform.position, lastMovementTracker, out RaycastHit hitInfo, intrationRadius, counterLayermask))
        {
            if(hitInfo.transform.TryGetComponent(out ClearCounter clearCounter) )
            {
                if (clearCounter != selectedCounter)
                {
                    setSelectedCounter(clearCounter);
                }
            }
            else
            {
                setSelectedCounter(null);
            }
        }
        else
        { 
            setSelectedCounter(null);

        }

        //Debug.Log(selectedCounter);
    }

    private void setSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangedEventArg
        {
            selectedCounter = selectedCounter
        });

    }
    private void MovementHandeller()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movDirection = new Vector3(inputVector.x, 0.0f, inputVector.y);
        bool canMove;
        float playerRadius = 0.6f;
        float playerHeight = 1.7f;
        float playerSpeed = moveSpeed * Time.deltaTime;

        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movDirection, playerSpeed);

        if (!canMove)
        {
            Vector3 movDirX = new Vector3(movDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movDirX, playerSpeed);

            if (canMove)
            {
                movDirection = movDirX;
            }
            else
            {
                Vector3 movDirZ = new Vector3(0, 0, movDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movDirZ, playerSpeed);

                if (canMove)
                {
                    movDirection = movDirZ;
                }
                else
                {
                    //can not move any where
                }

            }
        }

        if (canMove)
        {
            transform.position += movDirection * playerSpeed;
        }

        isWalking = movDirection != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, movDirection, Time.deltaTime * rotateSpeed);
    }
}
