using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static MouseController current;

    public Transform playerBody;

    public float mouseSensitivity;
    float xRotation = 0f;

    bool isPlacing;
    public Transform locationOfBuild;
    public LayerMask mouseColliderMask;

    public Transform point;

    void Start()
    {
        current = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);

        locationOfBuild.Rotate((Vector3.up * mouseX));

        if (isPlacing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, mouseColliderMask))
            {
                if (Vector3.Distance(hit.point, point.position) < .5f)
                {
                    locationOfBuild.position = point.position;
                    locationOfBuild.rotation = point.rotation;
                }
                else
                    locationOfBuild.position = hit.point;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                isPlacing = false;

                GameObject currentHeldObject = InventorySystem.current.currentHeldObject;
                currentHeldObject.GetComponent<Interactable>().canBeInteractedWith = true;
                currentHeldObject.transform.SetParent(null);
                currentHeldObject.transform.position = locationOfBuild.GetChild(0).position;
                currentHeldObject.transform.rotation = locationOfBuild.GetChild(0).rotation;
                currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
                currentHeldObject.layer = LayerMask.NameToLayer("Default");
                InventorySystem.current.currentHeldObject = null;

                Destroy(locationOfBuild.GetChild(0).gameObject);
                InventorySystem.current.Remove(InventorySystem.current.inventory[InventorySystem.current.currentSelection].data);
            }
        }
    }

    public void ActivatePlaceMode(InventoryItemData data)
    {
        isPlacing = true;
        if(locationOfBuild.childCount > 0)
            Destroy(locationOfBuild.GetChild(0).gameObject);
        Instantiate(data.highlightPrefab, locationOfBuild);
    }
}
