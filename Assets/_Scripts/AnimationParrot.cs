using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class AnimationParrot:MonoBehaviour
    {
        public void Awake()
        {
            var animationController = GetComponent<Animator>();
          /*  animationController.StopPlayback();
            animationController.playbackTime = 2f;
            animationController.StartPlayback();*/
        }
    }
}
