using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class ObjectChange : MonoBehaviour
{
    [SerializeField] GameObject normalFire;
    [SerializeField] GameObject normalFire1;
    [SerializeField] GameObject normalFire2;

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.GameControllerInstance.WasEventDone(GameEventsEnum.WasAppleGiven)
            && GameController.GameControllerInstance.WasEventDone(GameEventsEnum.FlintHistory)
            && GameController.GameControllerInstance.WasEventDone(GameEventsEnum.PlantHistory))
        {
            normalFire2.SetActive(true);
            normalFire1.SetActive(false);
        }
        else

        if (GameController.GameControllerInstance.WasEventDone(GameEventsEnum.WasAppleGiven))
        {
            normalFire1.SetActive(true);
            normalFire.SetActive(false);
        }
    }
}
