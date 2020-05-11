using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace Managers {
    public class SoundManager : MonoSingleton<SoundManager> {

        #region AudioFile
        [System.Serializable]
        private struct AudioFile {
            [SerializeField, Tooltip("The audio clip matching the audio type"), MustBeAssigned]
            private AudioClip audioClip;

            [SerializeField, Tooltip("The audio type matching the audio clip"), SearchableEnum]
            private AudioType audioType;

            public AudioClip AudioClip { get => audioClip; }
            public AudioType AudioType { get => audioType; }
        }
        #endregion

        [SerializeField, Tooltip("The audio output to use."), MustBeAssigned]
        private AudioSource source;

        [SerializeField, Tooltip("The list of playable audio files."), MustBeAssigned]
        private List<AudioFile> audioFiles;

        protected override void OnAwake() {
            if (audioFiles.IsNullOrEmpty()) {
                Debug.LogWarning("No audio files is loaded into sound manager!");
            }
        }

        internal void PlayAudioByType(AudioType audioType) {

            if (TryGetAudioClipToPlay(out AudioClip clipToPlay)) {
                source.PlayOneShot(clipToPlay);
            } else {
                Debug.LogWarning("No Audio File found for " + audioType.ToString());
            }

            #region Local_Function

            bool TryGetAudioClipToPlay(out AudioClip audioClip) {
                audioClip = null;

                foreach (var audioFile in audioFiles) {
                    if (audioFile.AudioType == audioType) {
                        audioClip = audioFile.AudioClip;
                        break;
                    }
                }

                return audioClip != null;
            }

            #endregion
        }
    }
}
