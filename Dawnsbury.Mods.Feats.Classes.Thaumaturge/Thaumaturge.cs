﻿using Dawnsbury.Core.CharacterBuilder.AbilityScores;
using Dawnsbury.Core.CharacterBuilder;
using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.Selections.Options;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Mods.Feats.Classes.Thaumaturge.Constants;
using Dawnsbury.Mods.Feats.Classes.Thaumaturge.RegisteredComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Creatures;
using Dawnsbury.Core.Mechanics.Core;
using Dawnsbury.Mods.Feats.Classes.Thaumaturge.Utilities;
using Dawnsbury.Core.Mechanics.Treasure;
using Dawnsbury.Core.Possibilities;
using Dawnsbury.Core;
using Dawnsbury.Core.Mechanics.Targeting;
using static Dawnsbury.Core.Mechanics.Core.CalculatedNumber;
using Dawnsbury.Mods.Feats.Classes.Thaumaturge.Enums;
using Dawnsbury.Core.Mechanics.Damage;
using Dawnsbury.Core.Roller;
using Dawnsbury.Core.Creatures.Parts;
using Dawnsbury.Display;
using Dawnsbury.Core.Coroutines.Options;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Common;
using static System.Collections.Specialized.BitVector32;

namespace Dawnsbury.Mods.Feats.Classes.Thaumaturge
{
    /// <summary>
    /// The Thaumaturge class
    /// </summary>
    public static class Thaumaturge
    {
        /// <summary>
        /// Creates the Thaumaturge Feats
        /// </summary>
        /// <returns>The Enumerable of Thaumaturge Feats</returns>
        public static IEnumerable<Feat> CreateThaumaturgeFeats()
        {
            // Creates and adds the logic for the Exploit Vulnerability class feature
            Feat exploitVulnerabilityFeat = new Feat(ThaumaturgeFeatNames.ExploitVulnerability, "You know that every creature, no matter how obscure, has a weakness. By identifying and empowering the right object, you can strike down even the most resilient of monsters.", "You gain the Exploit Vulnerability action.", [], null);
            AddExploitVulnerabilityLogic(exploitVulnerabilityFeat);
            yield return exploitVulnerabilityFeat;

            // Creates and adds the logic for the Implement's Empowerment class feature
            Feat implementsEmpowermentFeat = new Feat(ThaumaturgeFeatNames.ImplementsEmpowerment, "The power of your implement can also be turned to the more common task of combat, its power adding to and amplifying the effects of runes and other magical empowerments.", "When you Strike, you can trace mystic patterns with an implement you're holding to empower the Strike, causing it to deal 2 additional damage per weapon damage die. Channeling the power requires full use of your hands. You don't gain the benefit of implement's empowerment if you are holding anything in either hand other than a single one-handed weapon, other implements, or esoterica, and you must be holding at least one implement to gain the benefit.", [], null);
            AddImplementsEmpowermentLogic(implementsEmpowermentFeat);
            yield return implementsEmpowermentFeat;

            Feat amuletImplementFeat = new Feat(ThaumaturgeFeatNames.AmuletImplement, ImplementDetails.AmuletInitiateBenefitFlavorText, "You gain the " + ImplementDetails.AmuletInitiateBenefitName + " reaction.\n\n{b}" + ImplementDetails.AmuletInitiateBenefitName + "{/b} {icon:Reaction}\n" + ImplementDetails.AmuletInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddAmuletImplementLogic(amuletImplementFeat);
            yield return amuletImplementFeat;

            Feat bellImplementFeat = new Feat(ThaumaturgeFeatNames.BellImplement, ImplementDetails.BellInitiateBenefitFlavorText, "You gain the " + ImplementDetails.BellInitiateBenefitName + " reaction.\n\n{b}" + ImplementDetails.BellInitiateBenefitName + "{/b} {icon:Reaction}\n" + ImplementDetails.BellInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddBellImplementLogic(bellImplementFeat);
            yield return bellImplementFeat;

            Feat chaliceImplementFeat = new Feat(ThaumaturgeFeatNames.ChaliceImplement, ImplementDetails.ChaliceInitiateBenefitFlavorText, "You gain the " + ImplementDetails.ChaliceInitiateBenefitName + " action.\n\n{b}" + ImplementDetails.ChaliceInitiateBenefitName + "{/b} {icon:Action}\n" + ImplementDetails.ChaliceInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddChaliceImplementLogic(chaliceImplementFeat);
            yield return chaliceImplementFeat;

            Feat lanternImplementFeat = new Feat(ThaumaturgeFeatNames.LanternImplement, ImplementDetails.LanternInitiateBenefitFlavorText, "You gain the following benefit.\n\n" + ImplementDetails.LanternInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddLanternImplementLogic(lanternImplementFeat);
            yield return lanternImplementFeat;

            Feat mirrorImplementFeat = new Feat(ThaumaturgeFeatNames.MirrorImplement, ImplementDetails.MirrorInitiateBenefitFlavorText, "You gain the " + ImplementDetails.MirrorInitiateBenefitName + " action.\n\n{b}" + ImplementDetails.MirrorInitiateBenefitName + "{/b} {icon:Action}\n" + ImplementDetails.MirrorInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddMirrorImplementLogic(mirrorImplementFeat);
            yield return mirrorImplementFeat;

            Feat regaliaImplementFeat = new Feat(ThaumaturgeFeatNames.RegaliaImplement, ImplementDetails.RegaliaInitiateBenefitFlavorText, "You gain the following benefit.\n\n" + ImplementDetails.RegaliaInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddRegaliaImplementLogic(regaliaImplementFeat);
            yield return regaliaImplementFeat;

            Feat tomeImplementFeat = new Feat(ThaumaturgeFeatNames.TomeImplement, ImplementDetails.TomeInitiateBenefitFlavorText, "You gain the following benefit.\n\n" + ImplementDetails.TomeInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddTomeImplementLogic(tomeImplementFeat);
            yield return tomeImplementFeat;

            Feat wandImplementFeat = new Feat(ThaumaturgeFeatNames.WandImplement, ImplementDetails.WandInitiateBenefitFlavorText, "You gain the " + ImplementDetails.WandInitiateBenefitName + " activity.\n\n{b}" + ImplementDetails.WandInitiateBenefitName + "{/b} {icon:TwoActions}\n" + ImplementDetails.WandInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddWandImplementLogic(wandImplementFeat);
            yield return wandImplementFeat;

            Feat weaponImplementFeat = new Feat(ThaumaturgeFeatNames.WeaponImplement, ImplementDetails.WeaponInitiateBenefitFlavorText, "You gain the " + ImplementDetails.WeaponInitiateBenefitName + " reaction.\n\n{b}" + ImplementDetails.WeaponInitiateBenefitName + "{/b} {icon:Reaction}\n" + ImplementDetails.WeaponInitiateBenefitRulesText, [ThaumaturgeTraits.Implement], null);
            AddWeaponImplementLogic(weaponImplementFeat);
            yield return weaponImplementFeat;

            //// Creates the class selection feat for the Thaumaturge
            yield return new ClassSelectionFeat(ThaumaturgeFeatNames.ThaumaturgeClass, "The world is full of the unexplainable: ancient magic, dead gods, and even stranger things. In response, you've scavenged the best parts of every magical tradition and built up a collection of esoterica—a broken holy relic here, a sprig of mistletoe there—that you can use to best any creature by exploiting their weaknesses and vulnerabilities. The mystic implement you carry is both badge and weapon, its symbolic weight helping you bargain with and subdue the supernatural. Every path to power has its restrictions and costs, but you turn them all to your advantage. You're a thaumaturge, and you work wonders.",
                ThaumaturgeTraits.Thaumaturge, new EnforcedAbilityBoost(Ability.Charisma), 8,
                [Trait.Reflex, Trait.Simple, Trait.Martial, Trait.LightArmor, Trait.MediumArmor],
                [Trait.Perception, Trait.Fortitude, Trait.Will],
                3,
                "{b}1. Esoteric Lore{/b} You become trained in a special lore skill that can used to Exploit Vulnerability. This is a charisma-based skill. {i}(You add your Charisma modifier to checks using this skill.){/i}\n\n" +
                "{b}2. Exploit Vulnerability {icon:Action}{/b}\n{b}Frequency{/b} once per round; {b}Requirements{/b} You are holding your implement\n\nSelect a creature you can see and attempt an Esoteric Lore check against a standard DC for its level. You gain the following effects until you Exploit Vulnerabilities again.\n\n{b}Success{/b} Your unarmed and weapon Strikes activate the highest weakness againt the target, even though the damage type your weapon deals doesn't change. This damage affects the target of your Exploit Vulnerability, as well as any other creatures of the exact same type, but not other creatures with the same weakness. The {b}Failure{/b} result is used if the target has no weakness or if it is better.\n{b}Failure{/b} This causes the target creature, and only the target creature, to gain a weakness against your unarmed and weapon Strikes equal to 2 + half your level.\n{b}Critical Failure{/b} You become flat-footed until the beginning of your next turn.\n\n\n\n" +
                "{b}3. First Implement{/b} Choose an implement. {i}{Will appear in an open hand at the start of combat, if no open hands it will be in your bag or on the ground. The weapon implement is the only exception.){/i}\n\n" +
                "{b}4. Implement's Empowerment{/b} When you Strike, you can trace mystic patterns with an implement you're holding to empower the Strike, causing it to deal 2 additional damage per weapon damage die. Channeling the power requires full use of your hands. You don't gain the benefit of implement's empowerment if you are holding anything in either hand other than a single one-handed weapon or other implements and you must be holding at least one implement to gain the benefit.\n\n" +
                "{b}5. Thaumaturge Feat{/b}\n\n" +
                "{b}At Higher Levels:{/b}\n" +
                "{b}Level 2{/b} Thaumaturge Feat\n" +
                "{b}Level 3{/b} General feat, skill increase, Expert in Esoteric Lore\n" +
                "{b}Level 4{/b} Thaumaturge Feat", 
                null)
                .WithOnSheet(delegate (CalculatedCharacterSheetValues sheet)
                {
                    sheet.AddFeat(exploitVulnerabilityFeat, null);
                    sheet.AddFeat(implementsEmpowermentFeat, null);
                    sheet.AddSelectionOption(new SingleFeatSelectionOption("FirstImplement", "First Implement", 1, (Feat ft) => ft.HasTrait(ThaumaturgeTraits.Implement)));
                    sheet.AddSelectionOption(new SingleFeatSelectionOption("ThaumaturgeFeat1", "Thaumaturge feat", 1, (Feat ft) => ft.HasTrait(ThaumaturgeTraits.Thaumaturge)));
                    sheet.AddAtLevel(3, delegate (CalculatedCharacterSheetValues values)
                    {
                        values.SetProficiency(Trait.Reflex, Proficiency.Expert);
                    });
                });

            TrueFeat rootToLifeFeat = new TrueFeat(ThaumaturgeFeatNames.RootToLife, 1, "Marigold, spider lily, pennyroyal—many primal traditions connect flowers and plants with the boundary between life and death, and you can leverage this association to keep an ally on this side of the line.", "You place a small plant or similar symbol on an adjacent dying creature, immediately stabilizing them; the creature is no longer dying and is instead unconscious at 0 Hit Points.\n\nIf you spend 2 actions instead of 1, you empower the act further by uttering a quick folk blessing to chase away ongoing pain, adding the auditory trait to the action. When you do so, attempt flat checks to remove each source of persistent damage affecting the target; due to the particularly effective assistance, the DC is 10 instead of the usual 15.", [Trait.Manipulate, Trait.Necromancy, Trait.Primal, ThaumaturgeTraits.Thaumaturge]);
            // TODO
            yield return rootToLifeFeat;
        }

        /// <summary>
        /// Adds the logic for the Exploit Vulnerability base class feature
        /// </summary>
        /// <param name="exploitVulnerabilityFeat">The Exploit Vulnerability feat object</param>
        public static void AddExploitVulnerabilityLogic(Feat exploitVulnerabilityFeat)
        {
            exploitVulnerabilityFeat.WithPermanentQEffect("Esoteric Lore check to focus weakness", delegate (QEffect self)
            {
                self.ProvideMainAction = (QEffect exploitVulnerabilityEffect) =>
                {
                    if (ThaumaturgeUtilities.IsCreatureWeildingImplement(self.Owner))
                    {
                        CombatAction exploitVulnerabilityAction = new CombatAction(
                            self.Owner, 
                            IllustrationName.GenericCombatManeuver, 
                            "Exploit Vulnerability", 
                            [Trait.Manipulate, ThaumaturgeTraits.Thaumaturge], 
                            "{b}Frequency{/b} once per round; {b}Requirements{/b} You are holding your implement.\nYou scour your experiences and learning to identify something that might repel your foe. You retrieve an object from your esoterica with the appropriate supernatural qualities, then use your implement to stoke the remnants of its power into a blaze. Select a creature you can see and attempt an Esoteric Lore check against a standard DC for its level, as you retrieve the right object from your esoterica and use your implement to empower it. You gain the following effects until you Exploit Vulnerabilities again.\n{b}Success{/b} Your unarmed and weapon Strikes activate the highest weakness againt the target, even though the damage type your weapon deals doesn't change. This damage affects the target of your Exploit Vulnerability, as well as any other creatures of the exact same type, but not other creatures with the same weakness. The {b}Failure{/b} result is used if the target has no weakness or if it is better.\n{b}Failure{/b} This causes the target creature, and only the target creature, to gain a weakness against your unarmed and weapon Strikes equal to 2 + half your level.\n{b}Critical Failure{/b} You become flat-footed until the beginning of your next turn.",
                            Target.Ranged(100)
                            .WithAdditionalConditionOnTargetCreature((attacker, defender) => attacker.HasEffect(ThaumaturgeQEIDs.UsedExploitVulnerability) ? Usability.NotUsable("Already Exploited Vulnerability this turn") : Usability.Usable))
                        .WithActiveRollSpecification(new ActiveRollSpecification(ThaumaturgeUtilities.RollEsotericLore, ThaumaturgeUtilities.CalculateEsotericLoreDC))
                        .WithEffectOnEachTarget(async delegate (CombatAction action, Creature attacker, Creature defender, CheckResult result)
                        {         
                            attacker.AddQEffect(new QEffect(ExpirationCondition.ExpiresAtStartOfYourTurn)
                            {
                                Id = ThaumaturgeQEIDs.UsedExploitVulnerability
                            });
                            bool skipAntithesis = false;
                            if (result >= CheckResult.Success)
                            {
                                if (defender.WeaknessAndResistance.Weaknesses.Count > 0)
                                {
                                    List<Resistance> weaknesses = ThaumaturgeUtilities.GetHighestWeaknesses(defender);
                                    Resistance weakness = weaknesses[0];
                                    if (weaknesses.Count > 1)
                                    {
                                        ChoiceButtonOption selectedWeakness = await attacker.AskForChoiceAmongButtons(IllustrationName.GenericCombatManeuver, "Which weakness would you like to exploit against all " + defender.BaseName + "?", weaknesses.Select(weakness => weakness.DamageKind.HumanizeTitleCase2()).ToArray());
                                        weakness = weaknesses[selectedWeakness.Index];
                                    }
                                    if (weakness.Value >= 2 + Math.Floor(attacker.Level / 2.0))
                                    {
                                        skipAntithesis = true;
                                        defender.AddQEffect(new QEffect(ExpirationCondition.Never)
                                        {
                                            Id = ThaumaturgeQEIDs.ExploitVulnerabilityTarget,
                                            Tag = attacker
                                        });
                                        attacker.AddQEffect(new QEffect(ExpirationCondition.Never)
                                        {
                                            Id = ThaumaturgeQEIDs.ExploitVulnerabilityWeakness,
                                            Tag = defender,
                                            AddExtraKindedDamageOnStrike = (CombatAction action, Creature damageTarget) =>
                                            {
                                                if (damageTarget == defender || damageTarget.BaseName == defender.BaseName)
                                                {
                                                    return new KindedDamage(DiceFormula.FromText("0", "Exploit Vulnerability - Weakness " + weakness.DamageKind), weakness.DamageKind);
                                                }

                                                return null;
                                            }
                                        });
                                    }
                                }
                            }
                            // Add CLear Logic on Reuse
                            if (result >= CheckResult.Failure && !skipAntithesis)
                            {
                                defender.AddQEffect(new QEffect(ExpirationCondition.Never)
                                {
                                    Id = ThaumaturgeQEIDs.ExploitVulnerabilityTarget,
                                    Tag = attacker,
                                    StateCheck = (QEffect stateCheck) =>
                                    {
                                        Creature owner = stateCheck.Owner;
                                        if (owner.HasEffect(ThaumaturgeQEIDs.ExploitVulnerabilityTarget))
                                        {
                                            if (!owner.WeaknessAndResistance.Weaknesses.Any(weakness => weakness.DamageKind == ThaumaturgeDamageKinds.PersonalAntithesis))
                                            {
                                                owner.WeaknessAndResistance.AddWeakness(ThaumaturgeDamageKinds.PersonalAntithesis, (int)(2 + Math.Floor(attacker.Level / 2.0)));
                                            }
                                        }
                                        else if(owner.WeaknessAndResistance.Weaknesses.Any(weakness => weakness.DamageKind == ThaumaturgeDamageKinds.PersonalAntithesis))
                                        {
                                            owner.WeaknessAndResistance.Weaknesses.RemoveAll(weakness => weakness.DamageKind == ThaumaturgeDamageKinds.PersonalAntithesis);
                                        }
                                    }
                                });
                                attacker.AddQEffect(new QEffect(ExpirationCondition.Never)
                                {
                                    Id = ThaumaturgeQEIDs.ExploitVulnerabilityWeakness,
                                    Tag = defender,
                                    AddExtraKindedDamageOnStrike = (CombatAction action, Creature damageTarget) =>
                                    {
                                        if (damageTarget == defender)
                                        {
                                            return new KindedDamage(DiceFormula.FromText("0", "Exploit Vulnerability - " + ThaumaturgeDamageKinds.PersonalAntithesis.HumanizeTitleCase2()), ThaumaturgeDamageKinds.PersonalAntithesis);
                                        }

                                        return null;
                                    }
                                });
                            }
                            else if (result == CheckResult.CriticalFailure)
                            {
                                QEffect flatFooted = QEffect.FlatFooted("Exploit Vulnerability");
                                flatFooted.ExpiresAt = ExpirationCondition.ExpiresAtStartOfYourTurn;
                                attacker.AddQEffect(flatFooted);
                            }
                        });

                        return new ActionPossibility(exploitVulnerabilityAction);
                    }

                    return null;
                };
            });
        }

        /// <summary>
        /// Adds the logic for the Amulet Implement feature
        /// </summary>
        /// <param name="amuletImplementFeat">The Amulet Implement feat object</param>
        public static void AddAmuletImplementLogic(Feat amuletImplementFeat)
        {
            AddImplementEnsureLogic(amuletImplementFeat);
            amuletImplementFeat.WithPermanentQEffect(ImplementDetails.AmuletInitiateBenefitName, delegate (QEffect self)
            {
                self.StartOfCombat = async (QEffect startOfCombat) =>
                {
                    Creature owner = startOfCombat.Owner;
                    Creature[] allies = owner.Battle.AllCreatures.Where(creature => creature.FriendOf(owner)).ToArray();
                    foreach (Creature ally in allies)
                    {
                        ally.AddQEffect(new QEffect(ExpirationCondition.Never)
                        {
                            YouAreDealtDamage = async (QEffect effect, Creature attacker, DamageStuff damage, Creature defender) =>
                            {
                                if (attacker.HasEffect(ThaumaturgeQEIDs.ExploitVulnerabilityTarget))
                                {
                                    QEffect exploitEffect = attacker.QEffects.First(qe => qe.Id == ThaumaturgeQEIDs.ExploitVulnerabilityTarget);
                                    if (exploitEffect.Tag != null && exploitEffect.Tag is Creature thaumaturge && thaumaturge == owner && owner.Actions.CanTakeReaction() && ThaumaturgeUtilities.IsCreatureWeildingImplement(owner) && ally.DistanceTo(owner) <= 3 && await owner.AskToUseReaction("Use " + ImplementDetails.AmuletInitiateBenefitName + " to give resistance equal to 2 + your level?"))
                                    {
                                        return new ReduceDamageModification(2 + owner.Level, "Amulet's Abeyance");
                                    }
                                }

                                return null;
                            }
                        });
                    }
                };
            });
        }

        /// <summary>
        /// Adds the logic for the Bell Implement feature
        /// </summary>
        /// <param name="bellImplementFeat">The Bell Implement feat object</param>
        public static void AddBellImplementLogic(Feat bellImplementFeat)
        {
            AddImplementEnsureLogic(bellImplementFeat);
            bellImplementFeat.WithPermanentQEffect(ImplementDetails.BellInitiateBenefitName, delegate (QEffect self)
            {
                self.StartOfCombat = async (QEffect startOfCombat) =>
                {
                    Creature owner = startOfCombat.Owner;
                    Creature[] allies = owner.Battle.AllCreatures.Where(creature => creature.FriendOf(owner)).ToArray();
                    foreach (Creature ally in allies)
                    {
                        // TODO: Manipulate
                        ally.AddQEffect(new QEffect(ExpirationCondition.Never)
                        {
                            YouAreTargeted = async (QEffect targeted, CombatAction action) =>
                            {
                                bool actionIsSpell = action.SpellInformation != null;
                                if (action.Owner.HasEffect(ThaumaturgeQEIDs.ExploitVulnerabilityTarget))
                                {
                                    QEffect exploitEffect = action.Owner.QEffects.First(qe => qe.Id == ThaumaturgeQEIDs.ExploitVulnerabilityTarget);
                                    if (exploitEffect.Tag != null && exploitEffect.Tag is Creature thaumaturge && thaumaturge == owner && owner.Actions.CanTakeReaction() && ThaumaturgeUtilities.IsCreatureWeildingImplement(owner) && owner.DistanceTo(action.Owner) <= 6 && await owner.AskToUseReaction("Use " + ImplementDetails.BellInitiateBenefitName + ": " + (actionIsSpell ? " Target makes Fortitude save or becomes stupefied?" : " Target makes Will save or becomes your choice of enfeebled or clumsy?")))
                                    {
                                        CheckResult savingThrowResult = CommonSpellEffects.RollSavingThrow(action.Owner, new CombatAction(owner, IllustrationName.GenericCombatManeuver, ImplementDetails.BellInitiateBenefitName, [Trait.Auditory, Trait.Emotion, Trait.Enchantment, Trait.Magical, Trait.Manipulate, Trait.Mental], ImplementDetails.BellInitiateBenefitRulesText, Target.Touch()), actionIsSpell ? Defense.Fortitude : Defense.Will, creature => ThaumaturgeUtilities.CalculateClassDC(owner, ThaumaturgeTraits.Thaumaturge));
                                        if (savingThrowResult <= CheckResult.Failure)
                                        {
                                            if (actionIsSpell)
                                            {
                                                QEffect debuff = QEffect.Stupefied(savingThrowResult == CheckResult.CriticalFailure ? 2 : 1);
                                                debuff.Source = owner;
                                                debuff.ExpiresAt = ExpirationCondition.ExpiresAtStartOfSourcesTurn;
                                                action.Owner.AddQEffect(debuff);
                                            }
                                            else
                                            {
                                                int debuffLevel = savingThrowResult == CheckResult.CriticalFailure ? 2 : 1;
                                                ChoiceButtonOption userResponse = await owner.AskForChoiceAmongButtons(IllustrationName.GenericCombatManeuver, "Add Enfeebled " + debuffLevel + " or Clumsy " + debuffLevel + " to " + action.Owner.Name, ["Enfeebled " + debuffLevel, "Clumsy " + debuffLevel]);
                                                QEffect debuff = (userResponse.Index == 0) ? QEffect.Enfeebled(debuffLevel) : QEffect.Clumsy(debuffLevel);
                                                debuff.Source = owner;
                                                debuff.ExpiresAt = ExpirationCondition.ExpiresAtStartOfSourcesTurn;
                                                action.Owner.AddQEffect(debuff);

                                            }
                                        }
                                    }
                                }
                            }
                        });
                    }
                };
            });
        }

        /// <summary>
        /// Adds the logic for the Chalice Implement feature
        /// </summary>
        /// <param name="chaliceImplementFeat">The Chalice Implement feat object</param>
        public static void AddChaliceImplementLogic(Feat chaliceImplementFeat)
        {
            AddImplementEnsureLogic(chaliceImplementFeat);
        }

        /// <summary>
        /// Adds the logic for the Lantern Implement feature
        /// </summary>
        /// <param name="lanternImplementFeat">The Lantern Implement feat object</param>
        public static void AddLanternImplementLogic(Feat lanternImplementFeat)
        {
            AddImplementEnsureLogic(lanternImplementFeat);
        }

        /// <summary>
        /// Adds the logic for the Mirror Implement feature
        /// </summary>
        /// <param name="mirrorImplementFeat">The Mirror Implement feat object</param>
        public static void AddMirrorImplementLogic(Feat mirrorImplementFeat)
        {
            AddImplementEnsureLogic(mirrorImplementFeat);
        }

        /// <summary>
        /// Adds the logic for the Regalia Implement feature
        /// </summary>
        /// <param name="regaliaImplementFeat">The Regalia Implement feat object</param>
        public static void AddRegaliaImplementLogic(Feat regaliaImplementFeat)
        {
            AddImplementEnsureLogic(regaliaImplementFeat);
        }

        /// <summary>
        /// Adds the logic for the Tome Implement feature
        /// </summary>
        /// <param name="tomeImplementFeat">The Tome Implement feat object</param>
        public static void AddTomeImplementLogic(Feat tomeImplementFeat)
        {
            AddImplementEnsureLogic(tomeImplementFeat);
        }

        /// <summary>
        /// Adds the logic for the Wand Implement feature
        /// </summary>
        /// <param name="wandImplementFeat">The Wand Implement feat object</param>
        public static void AddWandImplementLogic(Feat wandImplementFeat)
        {
            AddImplementEnsureLogic(wandImplementFeat);
        }

        /// <summary>
        /// Adds the logic for the Weapon Implement feature
        /// </summary>
        /// <param name="weaponImplementFeat">The Weapon Implement feat object</param>
        public static void AddWeaponImplementLogic(Feat weaponImplementFeat)
        {
            AddImplementEnsureLogic(weaponImplementFeat);
        }

        /// <summary>
        /// Adds the logic for the Implement's Empowerment base class feature
        /// </summary>
        /// <param name="implementsEmpowermentFeat">The Implement's Empowerment feat object</param>
        public static void AddImplementsEmpowermentLogic(Feat implementsEmpowermentFeat)
        {
            // Adds a permanent Bonus to Damage effect if the criteria matches
            implementsEmpowermentFeat.WithPermanentQEffect("2 additional damage per weapon damage die", delegate (QEffect self)
            {
                self.BonusToDamage = (QEffect self, CombatAction action, Creature defender) =>
                {
                    if (action.Item != null && action.Item.WeaponProperties != null && ThaumaturgeUtilities.IsCreatureWeildingImplement(self.Owner))
                    {
                        return new Bonus(2 * action.Item.WeaponProperties.DamageDieCount, BonusType.Untyped, "Implements Empowerment");
                    }

                    return null;
                };
            });
        }

        private static void AddImplementEnsureLogic(Feat implementFeat)
        {
            implementFeat.WithOnSheet((character) =>
            {
                ThaumaturgeUtilities.EnsureCorrectImplements(character);
            });
        }
    }
}
