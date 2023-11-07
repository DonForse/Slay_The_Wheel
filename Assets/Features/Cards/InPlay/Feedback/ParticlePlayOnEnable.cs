using UnityEngine;

namespace Features.Cards.InPlay.Feedback
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlePlayOnEnable :MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
        
        private void OnEnable()
        {
            _particleSystem.Play();
        }
    }
}