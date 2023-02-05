using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperAnimation
{
    public static bool IsAnimationCurrentAnimation(Animator animator, string tagAnim, int indexLayer=0)
    {
        return animator.GetCurrentAnimatorStateInfo(indexLayer).IsTag(tagAnim);
    }

    public static bool AnimatorIsPlaying(Animator animator, string tagAnim, int indexLayer=0){
        return AnimatorIsPlaying(animator,indexLayer) && IsAnimationCurrentAnimation(animator,tagAnim,indexLayer);
    }
    
    public static bool AnimatorIsPlaying(Animator animator, int indexLayer=0){
        return animator.GetCurrentAnimatorStateInfo(indexLayer).normalizedTime % 1 < 0.95f;
    }
    
}
