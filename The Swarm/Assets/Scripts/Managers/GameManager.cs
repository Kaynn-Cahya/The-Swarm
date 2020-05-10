using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    public class GameManager : MonoSingleton<GameManager> {
        public bool GameOver { get; private set; }

        protected override void OnAwake() {
            GameOver = false;
        }
    }
}
