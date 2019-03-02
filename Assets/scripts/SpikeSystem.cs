using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSystem : MonoBehaviour
{
    [System.NonSerialized]
    public int spikeLevel;
    private List<Animator> spikeAnimControllers = new List<Animator>();

    [System.NonSerialized]
    public bool unleashed_spike_trigger = false;

    public void NewLevel(Map new_map)
    {
        spikeLevel = 0;
        foreach(KeyValuePair<Vector2, GameObject> dicEntry in new_map.spike_map)
        {
            // Add animator from the spike object
            spikeAnimControllers.Add(dicEntry.Value.GetComponent<Animator>());
        }
    }

    public void Step()
    {
        spikeLevel++;
        switch (spikeLevel)
        {
            case 0:
            case 1:
                // Pass
                break;
            case 2:
                foreach(var _x in spikeAnimControllers)
                {
                    _x.enabled = false;
                    _x.gameObject.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("spike1");
                }
                break;

            case 3:
                unleashed_spike_trigger = true;
                foreach(var _x in spikeAnimControllers)
                {
                    _x.enabled = true;
                    _x.SetTrigger("up_then_down");
                }
                spikeLevel = 0;

                break;
        }
    }

    private void LateUpdate()
    {
        unleashed_spike_trigger = false;
    }

}
