using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class RightPaneTapHandler : MonoBehaviour
{
    PlayerController m_player;

    private void Start()
    {
        m_player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        GetComponent<TapGesture>().Tapped += HandleTap;
        FlickGesture fg = GetComponent<FlickGesture>();
        fg.Flicked += HandleFlick;
        fg.StateChanged += HandleFlick;
        fg.MinDistance = 1f;
        fg.FlickTime = 0.3f;
    }

    void HandleTap(object sender, System.EventArgs e)
    {
        m_player.Jump();
    }

    void HandleFlick(object sender, System.EventArgs e)
    {
        var gesture = sender as FlickGesture;

        if (gesture.State != FlickGesture.GestureState.Recognized) return;

        if (gesture.ScreenFlickVector.y < 0)
        {
            m_player.GoDownThroughPlatform();
        }
    }
}
