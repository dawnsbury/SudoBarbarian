﻿using Dawnsbury.Core;
using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.FeatsDb;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.TrueFeatDb;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Creatures;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Core;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Core.Mechanics.Targeting;
using Dawnsbury.Core.Mechanics.Treasure;
using Dawnsbury.Core.Possibilities;
using Dawnsbury.Core.Roller;
using Dawnsbury.Modding;
using Dawnsbury.Mods.Feats.Classes.Barbarian.Remastered.RegisteredComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using static Dawnsbury.Core.CharacterBuilder.FeatsDb.TrueFeatDb.BarbarianFeatsDb;

namespace Dawnsbury.Mods.Feats.Classes.Barbarian.Remastered
{
    /// <summary>
    /// The Remastered Barbarian Class
    /// </summary>
    public static class BarbarianRemastered
    {
        /// <summary>
        /// Determines if the provided feat should be removed
        /// </summary>
        /// <param name="possibleFeatToRemove">The feat being considered for removal</param>
        /// <returns>True if the feat should be removed and false otherwise</returns>
        public static bool ShouldFeatBeRemoved(Feat possibleFeatToRemove)
        {
            // 2nd Level Second Wind, 4th Level Fast Movement, and all original Dragon Instincts
            return possibleFeatToRemove.FeatName == FeatName.SecondWind || possibleFeatToRemove.FeatName == FeatName.FastMovement || (possibleFeatToRemove is DragonInstinctFeat && BarbarianRemasteredFeatNames.OriginalDragonInstincts.Contains(possibleFeatToRemove.FeatName));
        }

        /// <summary>
        /// Creates the Remastered Barbaian Feats
        /// </summary>
        /// <returns>The Enumerable of Barbarian Feats</returns>
        public static IEnumerable<Feat> CreateRemasteredBarbarianFeats()
        {
            // All Remastered Dragon Instincts
            yield return new DragonInstinctFeat(ModManager.RegisterFeatName("Adamantine dragon"), DamageKind.Bludgeoning);
            yield return new DragonInstinctFeat(ModManager.RegisterFeatName("Conspirator dragon"), DamageKind.Poison);
            yield return new DragonInstinctFeat(ModManager.RegisterFeatName("Diabolic dragon"), DamageKind.Fire);
            // TODO: When spirit damage is added ad Empureal
            //yield return new DragonInstinctFeat(ModManager.RegisterFeatName("Empureal dragon"), DamageKind.Spirit);
            yield return new DragonInstinctFeat(ModManager.RegisterFeatName("Fortune dragon"), DamageKind.Force);
            yield return new DragonInstinctFeat(ModManager.RegisterFeatName("Horned dragon"), DamageKind.Poison);
            yield return new DragonInstinctFeat(ModManager.RegisterFeatName("Mirage dragon"), DamageKind.Mental);
            yield return new DragonInstinctFeat(ModManager.RegisterFeatName("Omen dragon"), DamageKind.Mental);

            // The Giant Instict Feat
            yield return new Feat(BarbarianRemasteredFeatNames.GiantInstict, "Your rage gives you the raw power and size of a giant", "You can use weapons built for larger creature. While weilding any weapon you increase the additional damage from Rage from 2 to 6. All weapons you weild are considered large which leaves you clumsy 1 as you weild a weapon.\n\n{i}Specialization ability{/i} — At level 7, the rage bonus increases from 6 to 10 if you are weilding a large weapon..", new List<Trait>(), null).WithOnCreature(delegate (Creature cr)
            {
                cr.AddQEffect(new QEffect
                {
                    // Removed the original Rage Damage
                    StateCheck = delegate (QEffect sc)
                    {
                        QEffect qEffect = sc.Owner.QEffects.FirstOrDefault((QEffect qfId) => qfId.Id == QEffectId.Rage);
                        if (qEffect != null)
                        {
                            qEffect.YouDealDamageWithStrike = null;
                        }
                    },

                    // Checks if the weapon is a giant weapon and upgrades the damage if it is
                    AddExtraStrikeDamage = delegate (CombatAction attack, Creature defender)
                    {
                        Creature owner = attack.Owner;
                        if (owner.HasEffect(QEffectId.Rage) && BarbarianFeatsDb.DoesRageApplyToAction(attack) && attack.Item != null)
                        {
                            List<DamageKind> list = attack.Item.DetermineDamageKinds();
                            DamageKind damageTypeToUse = defender.WeaknessAndResistance.WhatDamageKindIsBestAgainstMe(list);
                            DiceFormula item3 = null;
                            if (attack.HasTrait(BarbarianRemasteredTraits.GiantWeaponTrait))
                            {
                                string agileRageDamage = (owner.Level < 7) ? "3" : "5";
                                string normalRageDamage = (owner.Level < 7) ? "6" : "10";
                                item3 = DiceFormula.FromText((attack.HasTrait(Trait.Agile)) ? agileRageDamage : normalRageDamage, (attack.HasTrait(Trait.Unarmed) || attack.HasTrait(Trait.Agile)) ? "Barbarian rage (giant & agile)" : "Barbarian rage (giant)");
                            }
                            else
                            {
                                item3 = DiceFormula.FromText((attack.HasTrait(Trait.Agile)) ? "1" : "2", (attack.HasTrait(Trait.Unarmed) || attack.HasTrait(Trait.Agile)) ? "Barbarian rage (agile)" : "Barbarian rage");
                            }
                            
                            return (item3, damageTypeToUse);
                        }

                        return null;
                    },

                    // Goes through every item Giant Instict's are holding and converts non-consumable weapons to Giant versions
                    StartOfCombat = async (QEffect qEffect) =>
                    {
                        Creature owner = qEffect.Owner;
                        foreach (Item item in owner.HeldItems.Concat(owner.CarriedItems).Where(item => DoesGiantInstictApply(item)))
                        {
                            item.Traits.Add(BarbarianRemasteredTraits.GiantWeaponTrait);
                            item.Name = "Giant " + item.Name;
                        }
                    }
                });
            });

            // Class Level 4 Feat - Scars of Steel - NOTE: Is Once per combat instead of once per day
            yield return new TrueFeat(ModManager.RegisterFeatName("Scars of Steel"), 4, "When you are struck with the mightiest of blows, you can flex your muscles to turn aside some of the damage.", "Once per day, when an opponent critically hits you with an attack that deals physical damage, you can spend a reaction to gain resistance to the triggering attack equal to your Constitution modifier plus half your level.", new Trait[] { Trait.Barbarian, Trait.Rage })
            .WithActionCost(-2).WithPermanentQEffect("After being critically hit, gain resistence to the damage.", delegate (QEffect qf)
            {
                // Checks the incoming damage and prompts for reaction if it's a crit and physical. Then applies the damage reduction
                qf.YouAreDealtDamage = async (QEffect qEffect, Creature attacker, DamageStuff damage, Creature defender) =>
                {
                    int possibleResistance = qEffect.Owner.Abilities.Constitution + (int)Math.Floor(qEffect.Owner.Level / 2.0);
                    if (damage.Kind.IsPhysical() && !qf.Owner.PersistentUsedUpResources.UsedUpActions.Contains("Scars of Steel") && damage.Power != null && damage.Power.CheckResult == CheckResult.CriticalSuccess && damage.Power.HasTrait(Trait.Attack) && await qf.Owner.Battle.AskToUseReaction(qf.Owner, "You were critically hit for a total damage of " + damage.Amount + ".\nUse Scars of Steel to gain " + possibleResistance + " damage resistence?"))
                    {
                        qf.Owner.PersistentUsedUpResources.UsedUpActions.Add("Scars of Steel");
                        return new ReduceDamageModification(possibleResistance, "Scars of Steel: You reduced " + possibleResistance + " damage from the incoming damage.");
                    }

                    return null;
                };
            });

            yield return new TrueFeat(ModManager.RegisterFeatName("Giant's Stature"), 6, "Your reach grows in size.", "Your giant melee weapon gains reach if it doesn't already have it until you stop raging.", [Trait.Barbarian, Trait.Polymorph, Trait.Primal, Trait.Rage])
                .WithActionCost(1)
                .WithPrerequisite(BarbarianRemasteredFeatNames.GiantInstict, "Must have giant instict.")
                .WithPermanentQEffect("Melee giant weapons gain reach.", delegate (QEffect qf)
                {
                    qf.ProvideMainAction = (QEffect mainAction) =>
                    {
                        return new ActionPossibility(new CombatAction(qf.Owner, IllustrationName.Rage, "Giant's Stature", [Trait.Polymorph, Trait.Primal, Trait.Rage], "Your giant melee weapon gains reach if it doesn't already have it until you stop raging.", Target.Self()
                            .WithAdditionalRestriction((Creature user) =>
                            {
                                if (!user.HasEffect(QEffectId.Rage))
                                {
                                    return "Must be raging";
                                }

                                return null;
                            }))
                            .WithActionCost(1)
                            .WithEffectOnSelf(async (Creature self) =>
                            {
                                self.AddQEffect(new QEffect("Giant's Stature", "Your giant weapon's gain reach while raging")
                                {
                                    Illustration = IllustrationName.Rage,
                                    StateCheck = async (QEffect stateCheck) =>
                                    {
                                        Creature owner = stateCheck.Owner;
                                        if (!owner.HasEffect(QEffectId.Rage))
                                        {
                                            stateCheck.ExpiresAt = ExpirationCondition.Immediately;
                                        }
                                        foreach (Item item in owner.HeldItems.Concat(owner.CarriedItems).Where(item => item.HasTrait(Trait.Melee) && item.HasTrait(BarbarianRemasteredTraits.GiantWeaponTrait) && !item.HasTrait(BarbarianRemasteredTraits.TemporaryReachTrait) && !item.HasTrait(Trait.Reach)))
                                        {
                                            item.Traits.Add(Trait.Reach);
                                            item.Traits.Add(BarbarianRemasteredTraits.TemporaryReachTrait);
                                        }
                                    },
                                    WhenExpires = (QEffect expires) =>
                                    {
                                        Creature owner = expires.Owner;
                                        foreach (Item item in owner.HeldItems.Concat(owner.CarriedItems).Where(item => item.HasTrait(BarbarianRemasteredTraits.TemporaryReachTrait)))
                                        {
                                            item.Traits.Remove(Trait.Reach);
                                            item.Traits.Remove(BarbarianRemasteredTraits.TemporaryReachTrait);
                                        }
                                    }
                                });
                            }));
                    };
                });
        }

        /// <summary>
        /// Patches all feats for the Barbarian Remaster
        /// </summary>
        /// <param name="feat">The feat to patch</param>
        public static void PatchFeats(Feat feat)
        {
            // Patches Intimidating Stike to be selectable by Barbarians
            if (feat.FeatName == FeatName.IntimidatingStrike)
            {
                PatchIntimidatingStrike(feat);
            }

            // Updates the 'Barbarian' class feature to include the 'Quick-Tempered' feature.
            if (feat.FeatName == FeatName.Barbarian && feat is ClassSelectionFeat classSelectionFeat)
            {
                // Add Quick-Tempered to the Barbarian
                AddQuickTemperedToClassSelection(classSelectionFeat);

                // Replace Deny Advantage and add Furious Footfalls to the Barbarian
                ReplaceDenyAdvantageWithFuriousFootfalslToClassSelection(classSelectionFeat);

                // Updates the 'Fury Instinct' sub class to match the Remaster
                classSelectionFeat.Subfeats.ForEach(subClass =>
                {
                    if (subClass.Name == "Fury Instinct")
                    {
                        UpdateFuryInstict(subClass);
                    }
                });

                // Adds the Giant Instict Sub Class
                AddGiantInstict(classSelectionFeat);

                // Updates Rage to match the Remaster
                UpdateRage(classSelectionFeat);

                // Sets up the Giant Weapon trait
                SetupGiantWeaponTrait();

                // Updates all text descriptions for the Barbarian
                UpdateAllTextDescriptions(classSelectionFeat);
            }
        }

        /// <summary>
        /// Adds the Quick Tempered base class Feature
        /// </summary>
        /// <param name="classSelectionFeat">The Barbarian Class Selection Feat</param>
        private static void AddQuickTemperedToClassSelection(ClassSelectionFeat classSelectionFeat)
        {
            // Adds the QEffect at the start of combat to prompt for a free action rage if the user is not wearing heavy armor
            classSelectionFeat.WithOnCreature(creature =>
            {
                creature.AddQEffect(new QEffect("Quick-Tempered", "Rage as a free action if you're not wearing heavy armor")
                {
                    StartOfCombat = async (QEffect qEffect) =>
                    {
                        Creature owner = qEffect.Owner;
                        if ((owner.Armor.Item == null || !owner.Armor.Item.Traits.Contains(Trait.HeavyArmor)) && await owner.Battle.AskForConfirmation(owner, IllustrationName.Rage, "Enter rage as a free action?", "Rage!"))
                        {
                            BarbarianFeatsDb.EnterRage(owner);
                        }
                    }
                });
            });
        }

        /// <summary>
        /// Adds the Furious Footfalls base class feature
        /// </summary>
        /// <param name="classSelectionFeat">The Barbarian Class Selection Feat</param>
        private static void ReplaceDenyAdvantageWithFuriousFootfalslToClassSelection(ClassSelectionFeat classSelectionFeat)
        {
            // Replaces Deny Advantage with Furious Footfalls as a base class feature
            classSelectionFeat.WithPermanentQEffect("+5 Status to Speed and +10 when raging", (QEffect qEffect) =>
            {
                qEffect.StartOfCombat = async (QEffect self) =>
                {
                    self.Owner.RemoveAllQEffects((QEffect qf) => qf.Id == QEffectId.DenyAdvantage);
                    if (self.Owner.Level >= 3)
                    {
                        self.Owner.AddQEffect(new QEffect("Furious Footfalls", "You gain a +5-foot status bonus to your Speed. This bonus increases to +10 feet while you're raging.")
                        {
                            BonusToAllSpeeds = (QEffect qEffect) =>
                            {
                                int speedBonus = (qEffect.Owner.HasEffect(QEffectId.Rage)) ? 2 : 1;
                                return new Bonus(speedBonus, BonusType.Status, "Furious Footfalls", true);
                            }
                        });
                    }
                };
            });
        }

        /// <summary>
        /// Adds the Giant Instict subclass to the Barbarian class
        /// </summary>
        /// <param name="classSelectionFeat">The barbarian class feat</param>
        private static void AddGiantInstict(ClassSelectionFeat classSelectionFeat)
        {
            Feat giantInstictFeat = AllFeats.All.FirstOrDefault(feat => feat.FeatName ==  BarbarianRemasteredFeatNames.GiantInstict);
            classSelectionFeat.Subfeats.Add(giantInstictFeat);
        }

        /// <summary>
        /// Updates the 'Fury Instinct' sub class to match the Remaster
        /// </summary>
        /// <param name="classSelectionFeat">The Barbarian Class Selection Feat</param>
        private static void UpdateFuryInstict(Feat furyInstictFeat)
        {
            // Updates the Rules Text and adds the updated effect
            furyInstictFeat.RulesText = "Increase the additional damage from Rage from 2 to 3. " + furyInstictFeat.RulesText.Replace("from +2 to +6.", "from +3 to +7.");
            furyInstictFeat.WithOnCreature(delegate (Creature cr)
            {
                cr.AddQEffect(new QEffect
                {
                    // ??? Not sure mostly copied from Dragon Instict
                    StateCheck = delegate (QEffect sc)
                    {
                        QEffect qEffect = sc.Owner.QEffects.FirstOrDefault((QEffect qfId) => qfId.Id == QEffectId.Rage);
                        if (qEffect != null)
                        {
                            qEffect.YouDealDamageWithStrike = null;
                        }
                    },

                    // ??? Not sure mostly copied from Dragon Instict
                    AddExtraStrikeDamage = delegate (CombatAction attack, Creature defender)
                    {
                        Creature owner = attack.Owner;
                        if (owner.HasEffect(QEffectId.Rage) && BarbarianFeatsDb.DoesRageApplyToAction(attack) && attack.Item != null)
                        {
                            string agileRangeDamage = (owner.Level < 7) ? "1" : "5";
                            string normalDamage = (owner.Level < 7) ? "3" : "7";
                            List<DamageKind> list = attack.Item.DetermineDamageKinds();
                            DamageKind damageTypeToUse = defender.WeaknessAndResistance.WhatDamageKindIsBestAgainstMe(list);
                            DiceFormula item3 = DiceFormula.FromText((attack.HasTrait(Trait.Agile)) ? agileRangeDamage : normalDamage, (attack.HasTrait(Trait.Unarmed) || attack.HasTrait(Trait.Agile)) ? "Barbarian rage (agile)" : "Barbarian rage");
                            return (item3, damageTypeToUse);
                        }

                        return null;
                    }
                });
            });
        }

        /// <summary>
        /// Updates Rage to match the Remaster
        /// NOTE: Temp HP is added every time regardless if enough time has passed
        /// </summary>
        /// <param name="classSelectionFeat">The Barbarian Class Selection Feat</param>
        private static void UpdateRage(ClassSelectionFeat classSelectionFeat)
        {
            classSelectionFeat.WithPermanentQEffect(null, (QEffect qEffect) =>
            {
                qEffect.YouAcquireQEffect = (QEffect self, QEffect effectToCheck) =>
                {
                    if (effectToCheck.Name == "Rage")
                    {
                        effectToCheck.BonusToDefenses = null;
                        effectToCheck.Description = effectToCheck.Description.Replace("You take a -1 penalty to AC.\n\n", string.Empty);
                    }
                    else if (effectToCheck.Id == QEffectId.HasRagedThisEncounter)
                    {
                        effectToCheck = null;
                    }
                    return effectToCheck;
                };
            });
        }

        /// <summary>
        /// Updates all the text descriptions for the Barbarian changes
        /// </summary>
        /// <param name="classSelectionFeat">The Barbarian Class Selection Feat</param>
        private static void UpdateAllTextDescriptions(ClassSelectionFeat classSelectionFeat)
        {
            // Updates the 'Rage' text for the combat rage button
            ModManager.RegisterActionOnEachActionPossibility(action =>
            {
                // Updates the Class Features description
                classSelectionFeat.RulesText = classSelectionFeat.RulesText.Replace("{b}2. Instinct.{/b} You select an instinct which is the source of your rage and grants you an additional power.\r\n\r\n{b}3. Barbarian feat.{/b}", "{b}2. Quick-Tempered.{/b} So long as you are able to move freely, your fury is instinctive and instantaneous. At the beginning of each encounter, you can enter rage as a free action if you are not wearing heavy armor.\r\n\r\n{b}3. Instinct.{/b} You select an instinct which is the source of your rage and grants you an additional power.\r\n\r\n{b}4. Barbarian feat.{/b}")
                .Replace("you take a -1 penalty to AC and you can't use concentrate actions.", "you can't use concentrate actions.")
                .Replace("Deny advantage {i}(you aren't flat-footed to hidden or flanking creatures of your level or lower){/i}", "Furious Footfalls {i}You gain a +5-foot status bonus to your Speed. This bonus increases to +10 feet while you're raging.{/i}");

                // Updates the 'Rage' button description in combat
                if (action.Name == "Rage")
                {
                    action.Description = action.Description.Replace("• You take a -1 penalty to AC.\n", string.Empty);
                    action.ShortDescription = action.ShortDescription.Replace(" and a penalty to AC until the end of the encounter", string.Empty);
                }
            });
        }

        /// <summary>
        /// Patches Intimidating Stike to be selectable by Barbarians
        /// </summary>
        /// <param name="intimidatingStrikeFeat">The Intimidating Strike feat</param>
        private static void PatchIntimidatingStrike(Feat intimidatingStrikeFeat)
        {
            // Adds the Barbarian trait and cycles through the Class Prerequisites that don't have Barbarian and adds it
            intimidatingStrikeFeat.Traits.Add(Trait.Barbarian);
            for (int i = 0; i < intimidatingStrikeFeat.Prerequisites.Count; i++)
            {
                Prerequisite prereq = intimidatingStrikeFeat.Prerequisites[i];
                if (prereq is ClassPrerequisite classPrerequisite)
                {
                    if (!classPrerequisite.AllowedClasses.Contains(Trait.Barbarian))
                    {
                        List<Trait> updatedAllowedClasses = classPrerequisite.AllowedClasses;
                        updatedAllowedClasses.Add(Trait.Barbarian);
                        intimidatingStrikeFeat.Prerequisites[i] = new ClassPrerequisite(updatedAllowedClasses);
                    }
                }
          
            
            }
        }

        /// <summary>
        /// Setups the logic for a creature holding a giant weapon
        /// </summary>
        private static void SetupGiantWeaponTrait()
        {
            string clumsyFromGiantWeaponTag = "Clumsy 1 from Giant Weapon";
            ModManager.RegisterActionOnEachCreature(creature =>
            {
                creature.AddQEffect(new QEffect
                {
                    StateCheck = (QEffect self) =>
                    {
                        List<Item> giantItems = self.Owner.HeldItems.Where(item => item.HasTrait(BarbarianRemasteredTraits.GiantWeaponTrait)).ToList();
                        if (giantItems.Count > 0 && self.Owner.QEffects.Count(effect => effect.Tag != null && effect.Tag.GetType() == typeof(string) && effect.Tag.Equals(clumsyFromGiantWeaponTag)) == 0)
                        {
                            self.Owner.AddQEffect(new QEffect
                            {
                                Tag = clumsyFromGiantWeaponTag
                            });
                            self.Owner.AddQEffect(QEffect.Clumsy(1));
                        }
                        else if (giantItems.Count == 0 && self.Owner.QEffects.Count(effect => effect.Tag != null && effect.Tag.GetType() == typeof(string) && effect.Tag.Equals(clumsyFromGiantWeaponTag)) > 0)
                        {
                            self.Owner.RemoveAllQEffects(effect => effect.NameWithValue == "Clumsy 1" || (effect.Tag != null && effect.Tag.GetType() == typeof(string) && effect.Tag.Equals(clumsyFromGiantWeaponTag)));
                        }
                    }
                });
            });
        }

        /// <summary>
        /// Determines if the provided item should have Giant Instict apply
        /// </summary>
        /// <param name="item">The item being checked</param>
        /// <returns>True if the item can benefit from Giant Instict and false otherwise</returns>
        private static bool DoesGiantInstictApply(Item item)
        {
            return item.HasTrait(Trait.Weapon) && !item.HasTrait(Trait.Ranged) && !item.HasTrait(Trait.Consumable);
        }
    }
}
