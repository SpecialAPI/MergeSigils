using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using HarmonyLib;
using InscryptionMod.Abilities;
using UnityEngine;

namespace InscryptionMod.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerHand), "SelectSlotForCard")]
    public class PlayerHand_SelectSlotForCard
    {
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator rat, PlayableCard card)
        {
            while (rat.MoveNext())
            {
                object current = rat.Current;
                if(current.GetType() == (type ??= (AccessTools.TypeByName("DiskCardGame.BoardManager+<ChooseSlot>d__75") ?? AccessTools.TypeByName("DiskCardGame.BoardManager+<ChooseSlot>d__81"))))
                {
                    List<CardSlot> emptySlots = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => x.Card == null || (x.Card.GetComponentInChildren<MergeBase>()?.CanMergeWith(x.Card)).GetValueOrDefault() ||
                        (card.GetComponentInChildren<PlaceAboveBase>()?.CanPlaceAbove(x.Card)).GetValueOrDefault());
                    yield return BoardManager.Instance.ChooseSlot(emptySlots, card.Info.BloodCost <= 0);
                }
                else if(current.GetType() == (placeType ??= (AccessTools.TypeByName("DiskCardGame.PlayerHand+<PlayCardOnSlot>d__37") ?? AccessTools.TypeByName("DiskCardGame.PlayerHand+<PlayCardOnSlot>d__36"))))
                {
                    CardSlot slot = Singleton<BoardManager>.Instance.LastSelectedSlot;
                    MergeBase merge = null;
                    PlaceAboveBase placeAbove = null;
                    if (slot != null && slot.Card != null && slot.Card != null && slot.Card.GetComponentInChildren<MergeBase>() != null)
                    {
                        merge = slot.Card.GetComponentInChildren<MergeBase>();
                    }
                    if(slot != null && card?.GetComponentInChildren<PlaceAboveBase>() != null)
                    {
                        placeAbove = card?.GetComponentInChildren<PlaceAboveBase>();
                    }
                    PlayableCard slotCard = slot.Card;
                    bool hasMerge = false;
                    bool hasPlaceAbove = false;
                    /*bool willKillOriginal = false;
                    bool willKillPlaced = false;
                    bool willKillOriginalPA = false;
                    bool willKillPlacedPA = false;*/
                    void UpdateValues()
                    {
                        hasMerge = merge != null;
                        hasPlaceAbove = placeAbove != null && slotCard != null;
                    }
                    UpdateValues();
                    if (hasMerge)
                    {
                        foreach (MergeBase merge2 in slotCard.GetComponentsInChildren<MergeBase>())
                        {
                            UpdateValues();
                            if (hasMerge)// && ((!willKillOriginal || merge2 is not MergeKillOther) && (!willKillPlaced || merge2 is not MergeKillSelf)))
                            {
                                /*if(merge2 is MergeKillOther)
                                {
                                    willKillPlaced = true;
                                }
                                else if(merge2 is MergeKillSelf)
                                {
                                    willKillOriginal = true;
                                }*/
                                GlobalTriggerHandler.Instance.StackSize++;
                                yield return merge2.OnPreMerge(card);
                                GlobalTriggerHandler.Instance.StackSize--;
                            }
                        }
                    }
                    UpdateValues();
                    if (hasPlaceAbove)
                    {
                        foreach (PlaceAboveBase placeAbove2 in card.GetComponentsInChildren<PlaceAboveBase>())
                        {
                            UpdateValues();
                            if (hasPlaceAbove)// && (!willKillOriginalPA || placeAbove2 is not PlaceAboveKillSelf) && (!willKillPlacedPA || placeAbove2 is not PlaceAboveKillOther))
                            {
                                /*if (placeAbove2 is PlaceAboveKillSelf)
                                {
                                    willKillPlacedPA = true;
                                }
                                else if (placeAbove2 is PlaceAboveKillOther)
                                {
                                    willKillOriginalPA = true;
                                }*/
                                GlobalTriggerHandler.Instance.StackSize++;
                                yield return placeAbove2.OnPrePlaceAbove(slotCard);
                                GlobalTriggerHandler.Instance.StackSize--;
                            }
                        }
                    }
                    yield return current;
                    UpdateValues();
                    if (hasMerge)
                    {
                        foreach (MergeBase merge2 in slotCard.GetComponentsInChildren<MergeBase>())
                        {
                            UpdateValues();
                            if (hasMerge)// && ((!willKillOriginal || merge2 is not MergeKillOther) && (!willKillPlaced || merge2 is not MergeKillSelf)))
                            {
                                /*if (merge2 is MergeKillOther)
                                {
                                    willKillPlaced = true;
                                }
                                else if (merge2 is MergeKillSelf)
                                {
                                    willKillOriginal = true;
                                }*/
                                GlobalTriggerHandler.Instance.StackSize++;
                                yield return merge2.OnMerge(card);
                                GlobalTriggerHandler.Instance.StackSize--;
                            }
                        }
                    }
                    UpdateValues();
                    if (hasPlaceAbove)
                    {
                        foreach (PlaceAboveBase placeAbove2 in card.GetComponentsInChildren<PlaceAboveBase>())
                        {
                            UpdateValues();
                            if (hasPlaceAbove)// && ((!willKillOriginalPA || placeAbove2 is not PlaceAboveKillSelf) && (!willKillPlacedPA || placeAbove2 is not PlaceAboveKillOther)))
                            {
                                /*if (placeAbove2 is PlaceAboveKillSelf)
                                {
                                    willKillPlacedPA = true;
                                }
                                else if (placeAbove2 is PlaceAboveKillOther)
                                {
                                    willKillOriginalPA = true;
                                }*/
                                GlobalTriggerHandler.Instance.StackSize++;
                                yield return placeAbove2.OnPostPlaceAbove(slotCard);
                                GlobalTriggerHandler.Instance.StackSize--;
                            }
                        }
                    }
                }
                else
                {
                    yield return current;
                }
            }
            yield break;
        }

        public static Type type;
        public static Type placeType;
    }
}
