using Emik.Microtypes.Unity;
using LibRibbonPlus;
using UnityEngine;

namespace VibRibbonPlus
{
    public class Life : CacheableBehaviour
    {
        //public for debugging
        public int _xp = 0;
        public int _health = 10;
        public int _life = 1;

        internal void Start()
        {
            Find<NoteController>().OnHit += Hit;
            Find<NoteController>().OnMiss += Miss;
        }

        private void Hit(Note note)
        {
            if(_life != 4)
            _xp++;

            if (_xp >= 18)
            {
                _xp = 0;
                _health = 10;
                _life++;
            }
        }

        private void Miss(Note note)
        {
            _xp = 0;

            _health -= 1;

            if (_life == 4)
                _health = 0;

            if (_health <= 0)
            {
                _life--;
                _health = 10;
            }
        }
    }
}
