using UnityEngine;



public class Collectible : MonoBehaviour, IInteractable

{

    [SerializeField] private int skullIndex = 1; // 1 pentru ochi, 2 pentru entitate



    public void Interact()

    {

        Collect();

    }



    private void Collect()

    {

        if (EntityPhaseManager.Instance != null)

        {

            EntityPhaseManager.Instance.CollectSkull(skullIndex);

            Debug.Log("Craniu colectat: " + skullIndex);

        }

        

        gameObject.SetActive(false);

    }

}
