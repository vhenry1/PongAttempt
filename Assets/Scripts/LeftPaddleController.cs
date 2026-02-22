using UnityEngine;
using Unity.Netcode; // Required for IsOwner

public class LeftPaddle : PaddleController
{
    protected override float GetMovementInput()
    {
        // Only return input if this local player owns this paddle
        if (base.HasLocalControl())
        {
            return Input.GetAxis("LeftPaddle");
        }

        return 0f;
    }
}