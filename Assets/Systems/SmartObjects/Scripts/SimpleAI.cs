using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Navigation_UnityNavMesh))]
public class SimpleAI : MonoBehaviour
{
    [SerializeField] protected float PickInteractionInterval = 2f;
    [SerializeField] protected Navigation_UnityNavMesh Navigation;

    protected BaseInteraction CurrentInteraction = null;
    protected bool StartedPerforming = false;   

    protected float TimeUntilNextInteractionPicked = -1f;

    public int bored = 100;
    public int hungry = 100;
    public int tired = 100;
    public int cleany = 100;

    // Update is called once per frame
    void Update()
    {
        if (CurrentInteraction != null && Navigation.IsAtDestination)
        {
            if (Navigation.IsAtDestination && !StartedPerforming)
            {
                StartedPerforming = true;
                CurrentInteraction.Perform(this, OnInteractionFinished);
            }
        }
        else
        {
            TimeUntilNextInteractionPicked -= Time.deltaTime;
            // czas na wybranie interakcji
            if (TimeUntilNextInteractionPicked <= 1)
            {
                TimeUntilNextInteractionPicked = PickInteractionInterval;
                PickRandomInteraction();
            }
        }
    }
    void OnInteractionFinished(BaseInteraction interaction)
    {
        interaction.UnlockInterraction();
        CurrentInteraction = null;
        Debug.Log($"Finished{interaction.DisplayName}");
    }

    void PickRandomInteraction()
    {
        //losowy przedmiot
        int objectIndex = Random.Range(0, SmartObjectManager.Instance.RegisteredObjects.Count);
        var selectedObject = SmartObjectManager.Instance.RegisteredObjects[objectIndex];
        // losowa interakcja
        int interactionIndex = Random.Range(0, selectedObject.Interactions.Count);
        var selectedInteraction = selectedObject.Interactions[interactionIndex];
        
        //czy mozna wykonac interakcje?
        if(selectedInteraction.CanPerform())
        {
            CurrentInteraction = selectedInteraction;
            CurrentInteraction.LockInteraction();
            StartedPerforming = false;
            //poruszenie sie do celu
            Debug.Log(Navigation);
            Debug.Log(CurrentInteraction);
            if (!Navigation.SetDestination(selectedObject.InteractionPoint))
                
            {
                Debug.LogError($"Could not move to {selectedObject.name}");
                CurrentInteraction = null;
            }
            else
                Debug.Log($"Going to {CurrentInteraction.DisplayName} at {selectedObject.DisplayName}");
        }
    }
}
