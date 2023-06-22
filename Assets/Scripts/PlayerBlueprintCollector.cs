using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlueprintCollector : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCollectBlueprint();
        }
    }

    void TryCollectBlueprint()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            Blueprint blueprint = hit.collider.GetComponent<Blueprint>();
            if (blueprint != null)
            {
                int blueprintID = blueprint.blueprintID;
                PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
                if (playerInventory != null)
                {
                    playerInventory.gameMechanicBlueprints[blueprintID] = true;
                    Destroy(hit.collider.gameObject); // Destruye el objeto que contiene el plano
                    Debug.Log("Se recolectó el plano con ID: " + blueprintID);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool hasBlueprintCollision = Physics.Raycast(ray, out hit, interactRange, interactLayer);

        Gizmos.color = hasBlueprintCollision ? Color.magenta : Color.green;
        Gizmos.DrawRay(ray.origin, ray.direction * interactRange);
    }
}
