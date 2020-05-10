using System;
using System.Collections.Generic;

namespace Managers.Timers {
    internal class CallbackTimer {
        internal float Duration { get; private set; }

        internal float Current { get; private set; }

        internal bool Activated { get; private set; }

        private List<Action> callbacks;

        internal CallbackTimer(float duration, List<Action> callbacks) {
            Duration = duration;
            this.callbacks = callbacks;

            Current = 0;
            Activated = false;
        }

        internal void Update(float deltaTime) {
            if (Activated) { return; }

            Current += Duration;

            if (Current >= Duration) {
                Activated = true;
                InvokeCallbacks();
            }
        }

        private void InvokeCallbacks() {
            foreach (var callback in callbacks) {
                callback?.Invoke();
            }
        }
    }
}