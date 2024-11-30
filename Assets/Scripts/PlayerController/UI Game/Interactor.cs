using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Interactor : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask InteractableLayermask=8;
    public Interactable interactable;
    public Image intereactImage;
    public Sprite defaultIcon;
    public Sprite InteractIcon;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit,2,InteractableLayermask))
        {
            if(hit.collider.GetComponent<Interactable>() != false)
            {
                if(interactable == null|| interactable.ID != hit.collider.GetComponent<Interactable>().ID)
                {   
                    interactable = hit.collider.GetComponent<Interactable>();
                    
                }
                if(interactable.InteractIcon != null)
                {
                    intereactImage.sprite = interactable.InteractIcon;
                }
                else
                {
                    intereactImage.sprite = defaultIcon;
                }
                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactable.OnInteract.Invoke();
                }
            }
        }
        else
        {
            if(intereactImage.sprite != defaultIcon)
            {
                intereactImage.sprite = defaultIcon;
                Debug.Log("Null");
            }
        }
    }
}
