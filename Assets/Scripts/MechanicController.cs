using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechanicController : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;
    public Slider progressSlider;
    public float speedSlider;
    public AudioSource repairSound;
    public AudioSource repairCompleteSound;
    public GameObject[] checkMechanics;

    private bool isRepairing = false;
    private bool isProgressUpdating = false;
    private float repairProgress = 0f;
    private RaycastHit hit;
    [SerializeField] private ParticleSystem sparkParticles;
    [SerializeField] private Light pointlightComplete;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryStartRepair();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            ResetRepairProgress();
        }

        if (isRepairing && isProgressUpdating)
        {
            UpdateRepairProgress();
        }
    }

    void TryStartRepair()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            GameMechanicState gameMechanicState = hit.collider.GetComponent<GameMechanicState>();
            if (gameMechanicState != null)
            {
                if (!gameMechanicState.isRepaired)
                {
                    int mechanicID = gameMechanicState.mechanicID;

                    PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
                    if (playerInventory.gameMechanicBlueprints[mechanicID])
                    {
                        StartRepair(gameMechanicState);
                    }
                    else
                    {
                        Debug.Log("No tienes el plano necesario para reparar este juego mecánico.");
                    }
                }
                else
                {
                    Debug.Log("Este juego mecánico ya ha sido reparado.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el componente GameMechanicState en el objeto golpeado.");
            }
        }
    }

    void StartRepair(GameMechanicState gameMechanicState)
    {
        isRepairing = true;
        progressSlider.gameObject.SetActive(true);
        repairSound.Play();
        repairProgress = progressSlider.minValue;
        isProgressUpdating = true;
        sparkParticles = hit.collider.GetComponentInChildren<ParticleSystem>();
        pointlightComplete = hit.collider.GetComponentInChildren<Light>();

        // Aquí puedes realizar cualquier otra acción relacionada con el inicio de la reparación del juego mecánico
    }

    void UpdateRepairProgress()
    {
        repairProgress += speedSlider * Time.deltaTime;
        progressSlider.value = repairProgress;

        if (repairProgress >= progressSlider.maxValue)
        {
            FinishRepair();
        }
    }

    void FinishRepair()
    {
        isRepairing = false;
        isProgressUpdating = false;
        progressSlider.gameObject.SetActive(false);

        if (hit.collider != null)
        {
            GameMechanicState gameMechanicState = hit.collider.GetComponent<GameMechanicState>();
            if (gameMechanicState != null)
            {
                gameMechanicState.isRepaired = true;
                repairCompleteSound.Play();
                pointlightComplete.enabled = true;
                // Verificar el ID y activar el objeto correspondiente
                int mechanicID = gameMechanicState.mechanicID;
                ActivateObjectByMechanicID(mechanicID);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente GameMechanicState en el objeto golpeado.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto golpeado para finalizar la reparación.");
        }

        StopSparkParticles();
    }

    void ResetRepairProgress()
    {
        progressSlider.gameObject.SetActive(false);
        repairSound.Stop();
        isProgressUpdating = false;
        repairProgress = progressSlider.minValue;
    }

    void StopSparkParticles()
    {
        if (sparkParticles != null)
        {
            sparkParticles.Stop();
        }
    }

    void ActivateObjectByMechanicID(int mechanicID)
    {
        // Aquí puedes definir la lógica para activar el objeto según el ID del juego mecánico reparado
        switch (mechanicID)
        {
            case 0:
                checkMechanics[0].SetActive(true);// Activar objeto con ID 0
                break;
            case 1:
                checkMechanics[1].SetActive(true);// Activar objeto con ID 0  // Activar objeto con ID 1
                break;
            // Agrega más casos según tus necesidades
            default:
                Debug.LogWarning("ID de juego mecánico no reconocido: " + mechanicID);
                break;
        }
    }


    void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool hasCollision = Physics.Raycast(ray, out hit, interactRange, interactLayer);

        if (hasCollision)
        {
            Gizmos.color = Color.magenta;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawRay(ray.origin, ray.direction * interactRange);
    }
}
