using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<bool> gameMechanicBlueprints = new List<bool>();
   

    void Awake()
    {
        for (int i = 0; i < 2; i++)
        {
            gameMechanicBlueprints.Add(false);
        }
    }

}