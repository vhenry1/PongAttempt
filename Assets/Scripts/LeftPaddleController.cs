using UnityEngine;
using Unity.Netcode; // Required for IsOwner

public class LeftPaddle : PaddleController
{
    protected override float GetMovementInput()
    {
        // Only return input if this local player owns this paddle
        if (IsOwner) 
        {
            return Input.GetAxis("LeftPaddle");
        }

        // If we don't own it, return 0 so it doesn't move based on our keyboard
        return 0f;
    }
}