using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;
    [SerializeField] private Transform spawnPoint;
    public void Interact()
    {
        Debug.Log("interact !");
        Transform tomatoTransform = Instantiate(kitchenObjectsSO.prefab, spawnPoint);
        tomatoTransform.localPosition = Vector3.zero;

        Debug.Log( tomatoTransform.GetComponent<KitchenObjects>().GetKitchenObjectsSO().objName);
    }
}
