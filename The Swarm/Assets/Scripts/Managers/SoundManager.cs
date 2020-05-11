using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace Managers {
	public class SoundManager : MonoSingleton<SoundManager> {

		#region Local_Classes

		[System.Serializable]
		private class AudioFile {
			public AudioType AudioType;
			public AudioClip Clip;

			public bool PlayOnAwake;
			public bool IsLooping;


			[Range(0f, 1f)] public float AudioVolume = 1f;
			[HideInInspector] public AudioSource Source;
		}

		#endregion

		[SerializeField, Tooltip("Contains all the audios in the game.")] private List<AudioFile> audioFiles = default;

		private static Dictionary<AudioType, AudioFile> audioDict = new Dictionary<AudioType, AudioFile>();


		protected override void OnAwake() {
			if(audioFiles.IsNullOrEmpty()) {
				Debug.LogWarning("No audio files is loaded into sound manager!");
				return;
			}
			InitializeAudioDict();
			InitializeAudioFiles();
		}

		/// <summary>
		/// Add all the type of audios into the dictionary.
		/// </summary>
		private void InitializeAudioDict() {
			foreach(AudioFile audioFile in audioFiles) {
				audioDict.Add(audioFile.AudioType, audioFile);
			}
		}

		/// <summary>
		/// Create an audio source component for each audio clip and initialize with the audio file properties.
		/// </summary>
		private void InitializeAudioFiles() {
			foreach(var a in audioFiles) {
				a.Source = gameObject.AddComponent<AudioSource>();

				// Assign audio properties to the newly created audio source.
				a.Source.clip = a.Clip;
				a.Source.volume = a.AudioVolume;
				a.Source.loop = a.IsLooping;

				// Start playing the audio if needed.
				if(a.PlayOnAwake) {
					a.Source.Play();
				}
			}
		}

		internal void PlayAudioByType(AudioType audioType) {
			// Get audio file from dictionary.
			audioDict.TryGetValue(audioType, out AudioFile audioFile);

			// Play audio if there is such audio file of the given type.
			if(audioFile != null) {
				audioFile.Source.PlayOneShot(audioFile.Clip);
			} else {
				Debug.LogError("Audio clip for " + audioType + " has not been assigned.");
			}
		}
	}
}
