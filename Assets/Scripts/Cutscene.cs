using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private VideoPlayer vp;
    [SerializeField] private ScenesManager sm;


    private void Update()
    {
        if (!vp.isPlaying && vp.frame > 5) {
            sm.switchScene(1);
        }
    }
}
