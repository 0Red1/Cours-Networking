using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineIntroController : MonoBehaviour
{
    [SerializeField] private PlayableDirector timelineIntro;
    void OnEnable()
    {
        GameIntroState.OnIntroStart += PlayTimeline;
    }

    void OnDisable()
    {
        GameIntroState.OnIntroStart -= PlayTimeline;
    }

    private void PlayTimeline()
    {
        Debug.Log("PLAYING");
        timelineIntro.Play();
    }
}
