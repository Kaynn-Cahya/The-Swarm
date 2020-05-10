using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Managers.Timers {

    public class CallbackTimerManager : MonoSingleton<CallbackTimerManager> {
        private HashSet<CallbackTimer> timers;

        protected override void OnAwake() {
            timers = new HashSet<CallbackTimer>();
        }

        private void Update() {
            float deltaTime = Time.deltaTime;

            foreach (var timer in timers) {
                timer.Update(deltaTime);
            }

            RemoveActivatedTimers();

            #region Local_Function

            void RemoveActivatedTimers() {
                foreach (var timer in timers.ToArray()) {
                    if (timer.Activated) {
                        timers.Remove(timer);
                    }
                }
            }

            #endregion
        }

        internal void AddTimer(float duration, Action callback) {
            CallbackTimer callbackTimer = new CallbackTimer(duration, new List<Action>() { callback });

            timers.Add(callbackTimer);
        }

        internal void AddTimer(float duration, List<Action> callbacks) {

#if UNITY_EDITOR
            if (callbacks == null) {
                Debug.LogWarning("CallbackTimerManager.cs :: Callback list was null");
                return;
            }
#endif

            CallbackTimer callbackTimer = new CallbackTimer(duration, callbacks);

            timers.Add(callbackTimer);
        }
    }

}