using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Click,
        Hold,
        Pickup
    }

    public InteractionType interactionType;

    public bool canBeInteractedWith = true;
    float holdTime;

    public abstract string GetDescription();
    public abstract void Interact();
    public GameObject PickupObject() => gameObject;

    public void IncreaseHoldTime() => holdTime += Time.deltaTime;
    public void ResetHoldTime() => holdTime = 0f;

    public float GetHoldTime() => holdTime;
}
