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
    public float speedSliderDecrease;

    private bool isRepairing = false;
    private bool isProgressUpdating = false;
    private float repairProgress = 0f;
    private RaycastHit hit;
    [SerializeField] private ParticleSystem sparkParticles;

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
                        Debug.Log("No tienes el plano necesario para reparar este juego mec�nico.");
                    }
                }
                else
                {
                    Debug.Log("Este juego mec�nico ya ha sido reparado.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontr� el componente GameMechanicState en el objeto golpeado.");
            }
        }
    }

    void StartRepair(GameMechanicState gameMechanicState)
    {
        isRepairing = true;
        progressSlider.gameObject.SetActive(true);
        repairProgress = progressSlider.minValue;
        isProgressUpdating = true;
        sparkParticles = hit.collider.GetComponentInChildren<ParticleSystem>();

        // Aqu� puedes realizar cualquier otra acci�n relacionada con el inicio de la reparaci�n del juego mec�nico
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
            }
            else
            {
                Debug.LogWarning("No se encontr� el componente GameMechanicState en el objeto golpeado.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontr� un objeto golpeado para finalizar la reparaci�n.");
        }

        StopSparkParticles();
    }

    void ResetRepairProgress()
    {
        progressSlider.gameObject.SetActive(false);
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
}
