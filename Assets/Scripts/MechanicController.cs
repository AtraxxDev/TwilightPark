using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechanicController : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;
    public Slider progressSlider;
    public ParticleSystem sparkParticles;
    public float speedSlider;

    private bool isRepairing = false;
    private float repairProgress = 0f;
    private RaycastHit hit; // Mover la declaraci�n de la variable hit aqu�

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryStartRepair();
        }

        if (isRepairing)
        {
            UpdateRepairProgress();
        }
    }

    void TryStartRepair()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            GameMechanicState gameMechanicController = hit.collider.GetComponent<GameMechanicState>();
            if (gameMechanicController != null && !gameMechanicController.isRepaired)
            {
                StartRepair(gameMechanicController);
            }
        }
    }

    void StartRepair(GameMechanicState gameMechanicController)
    {
        isRepairing = true;
        progressSlider.gameObject.SetActive(true);
        repairProgress = progressSlider.minValue;

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
        progressSlider.gameObject.SetActive(false);
        sparkParticles.Stop();

        if (hit.collider != null)
        {
            GameMechanicState gameMechanicController = hit.collider.GetComponent<GameMechanicState>();
            if (gameMechanicController != null)
            {
                gameMechanicController.isRepaired = true;
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
    }
}
