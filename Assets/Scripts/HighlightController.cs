using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    void OnEnable() {
        highlightManagers.Add(this);
    }

    void OnDisable() {
        highlightManagers.Remove(this);
    }

    public static void DisableAll() {
        List<HighlightController> hms = new List<HighlightController>(highlightManagers);
        foreach(HighlightController hm in hms) {
            hm.gameObject.SetActive(false);
        }
    }

    static List<HighlightController> highlightManagers = new List<HighlightController>();
    public Position position;
}