﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSystem : MonoBehaviour
{
    [System.NonSerialized]
    public int spikeLevel;
    public List<Animator> spikeAnimControllers = new List<Animator>();
    public bool unleashed_spike_trigger = false;

    [SerializeField]
    public Sprite spike_down, spike_up;

    public void NewLevel(Map new_map)
    {
        spikeAnimControllers.Clear();

        spikeLevel = 0;
        foreach (KeyValuePair<Vector2, GameObject> dicEntry in new_map.spike_map)
        {
            // Add animator from the spike object
            spikeAnimControllers.Add(dicEntry.Value.GetComponent<Animator>());
        }
    }

    public void Step()
    {
        spikeLevel++;
        if (spikeLevel == 1)
        {                                           
            foreach (var _x in spikeAnimControllers)
            {
                _x.enabled = false;
                _x.gameObject.GetComponent<SpriteRenderer>().sprite = spike_up;
            }
        }
        else
        {
            spikeLevel = 0;
            foreach (var _x in spikeAnimControllers)
            {
                _x.enabled = false;
                _x.gameObject.GetComponent<SpriteRenderer>().sprite = spike_down;
            }
        }
    }

}