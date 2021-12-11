using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;

    public float interactionDistance;

    public TMPro.TextMeshProUGUI interactionText;
    public GameObject interactionHoldGO;
    public UnityEngine.UI.Image interactionHoldProgress;

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        RaycastHit hit;

        bool successfullHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (interactable.canBeInteractedWith)
                {
                    HandleInteraction(interactable);
                    interactionText.text = interactable.GetDescription();
                    successfullHit = true;

                    interactionHoldGO.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
                }
            }
        }

        if (!successfullHit)
        {
            interactionText.text = "";
            interactionHoldGO.SetActive(false);
        }
    }

    void HandleInteraction(Interactable interactable)
    {
        KeyCode key = KeyCode.E;
        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Click:
                if (Input.GetKeyDown(key))
                    interactable.Interact();
                break;
            case Interactable.InteractionType.Hold:
                if (Input.GetKey(key))
                {
                    interactable.IncreaseHoldTime();
                    if (interactable.GetHoldTime() > 1f)
                    {
                        interactable.Interact();
                        interactable.ResetHoldTime();
                    }
                }
                else
                    interactable.ResetHoldTime();
                interactionHoldProgress.fillAmount = interactable.GetHoldTime();
                break;
            case Interactable.InteractionType.Pickup:
                if (Input.GetKeyDown(key))
                {
                    interactable.canBeInteractedWith = false;
                    interactable.Interact();
                }
                break;

            default:
                throw new System.Exception("Unsupported type of interaction");
        }
    }
}



//public float throwForce;
//public Transform holdParent;
//public GameObject currentHeldObject;

//public Transform examineParent;
//public bool isExamining = false;

//currentHeldObject = interactable.PickupObject();
//currentHeldObject.transform.SetParent(holdParent);
//currentHeldObject.transform.localPosition = Vector3.zero;
//currentHeldObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
//currentHeldObject.GetComponent<Rigidbody>().isKinematic = true;


//if (Input.GetKeyDown(KeyCode.Space))
//    Examine();


//void Examine()
//{
//    isExamining = !isExamining;

//    switch (isExamining)
//    {
//        case true:
//            currentHeldObject.transform.SetParent(examineParent);
//            currentHeldObject.transform.localPosition = Vector3.Lerp(examineParent.position, Vector3.zero, 1f);
//            currentHeldObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
//            break;
//        case false:
//            currentHeldObject.transform.SetParent(holdParent);
//            currentHeldObject.transform.localPosition = Vector3.Lerp(currentHeldObject.transform.localPosition, Vector3.zero, 1f);
//            currentHeldObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
//            break;
//    }
//}
