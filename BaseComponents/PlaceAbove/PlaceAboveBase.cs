using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace InscryptionMod.Abilities
{
    public abstract class PlaceAboveBase : AbilityBehaviour
    {
        public virtual bool CanPlaceAbove(PlayableCard card)
        {
            return card.CanBeSacrificed;
        }
        public abstract IEnumerator OnPrePlaceAbove(PlayableCard placeAboveCard);
        public abstract IEnumerator OnPostPlaceAbove(PlayableCard placeAboveCard);
    }
}
