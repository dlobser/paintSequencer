using UnityEngine;
using System.Collections;

public class ShortStroke : Stroke {




    protected override void OnPlayStart() {
        base.OnPlayStart();
    }

    protected override void OnPlay() {
        base.OnPlay();
    }

    protected override void OnPlayFinish() {
        base.OnPlayFinish();
    }

    protected override void HandlePlayStart() {
        base.HandlePlayStart();

        this.SwitchState(StrokeState.PLAY);
    }

    protected override void HandlePlay() {
        base.HandlePlay();

        this.age += Time.deltaTime;
    }

    protected override void HandlePlayFinish() {
        base.HandlePlayFinish();


        if (lRend)
            lRend.SetWidth(0, trailWidth);

        if (trailWidth > 0.01f) {
            trailWidth *= .97f;
        } else {
            this.Reset();
        }
    }


    public virtual void traceLine() {
        root.transform.localPosition = Trail[currentPlaybackIndex];
        trailToLine(0, currentPlaybackIndex);
        currentPlaybackIndex++;
        animateAnimators((float)currentPlaybackIndex / (float)trailHead);
    }
}
