using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Timers {
    internal class CallbackTimer {
        internal float Duration { get; private set; }

        internal float Current { get; private set; }

        private List<Action> callbacks;

        internal CallbackTimer(float duration, float current, List<Action> callbacks) {
            Duration = duration;
            Current = current;
            this.callbacks = callbacks;
        }

        internal void Update(float deltaTime) {
            Current += Duration;
        }
    }
}