using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemController : MonoBehaviour
{

    // We can define function either in this class, or in other objects, to handle these inputs. 
    // Will do Friday, currently this only works to intercept input.
    // The "Level Manager" Object can take other game objects to pass input onto

    public void HandleMovement(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            print(context.ToString());
        }
    }

    public void HandlePlacement(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            print(context.ToString());
        }
    }

    public void HandleInventory(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            print(context.ToString());
        }
    }

    public void HandleExit(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            print(context.ToString());
        }
    }

    public void HandleShuffle(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            print(context.ToString());
        }
    }
}
