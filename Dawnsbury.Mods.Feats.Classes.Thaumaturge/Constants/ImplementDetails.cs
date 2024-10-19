﻿namespace Dawnsbury.Mods.Feats.Classes.Thaumaturge.Constants
{
    public static class ImplementDetails
    {
        public static readonly string AmuletInitiateBenefitName = "Amulet's Abeyance";

        public static readonly string AmuletInitiateBenefitFlavorText = "Amulets are items carried for good luck and protection. Your amulet might be a magical diagram, a religious symbol, a preserved body part such as a rabbit's foot, or a lucky coin. Amulet implements are associated with the harrow suit of shields and the astrological signs of the bridge and the ogre.";

        public static readonly string AmuletInitiateBenefitRulesText = "{b}Trigger{/b} The target of your Exploit Vulnerability would damage you or an ally within 15 feet of you\n{b}Requirements{/b} You're holding an implement and are benefiting from Exploit Vulnerability.\n\nYou or a target ally within 15 feet gain resistance to all damage against the triggering damage. The resistance is equal to 2 + your level.";

        public static readonly string BellInitiateBenefitName = "Ring Bell";

        public static readonly string BellInitiateBenefitFlavorText = "Bells symbolize the power that sounds and emotions hold over others, soothing with one tone and startling with another. Bells, drums, finger cymbals, and other percussion instruments are most typical, but these implements can be any type of portable musical instrument that is played with one hand. Bell implements are associated with the astrological signs of the daughter and the blossom.";

        public static readonly string BellInitiateBenefitRulesText = "{b}Trigger{/b} The target of your Exploit Vulnerability makes a Strike or Casts a Spell that would affect you or one of your allies.\n{b}Requirements{/b} You are holding your bell implement, and the triggering creature is within 30 feet of you.\n\nThe piece played depends on whether the trigger was a Strike or Spell, and it applies to the triggering Strike or Spell, except where noted otherwise.\n\n{b}Distracting Cacophony{/b} The trigger is a spell. The target must succeed at a Fortitude save against your class DC or become stupefied 1 until the end of your next turn (stupefied 2 on a critical failure). The target doesn't have to attempt a flat check to avoid losing the triggering spell, but the discordant ring does lower the spell attack roll or spell DC of the triggering spell from stupefied.\n\n{b}Disrupting Harmony{/b} The trigger is a Strike. The target must succeed at a Will save against your class DC or become your choice of enfeebled 1 or clumsy 1 until the end of your next turn (enfeebled 2 or clumsy 2 on a critical failure).";

        public static readonly string ChaliceInitiateBenefitName = "Drink from the Chalice";

        public static readonly string ChaliceInitiateBenefitFlavorText = "Chalice implements are vessels that fill with liquid, associating them with healing, nourishment, and life. Your chalice might be a traditional cup or goblet, but it could also be a small amphora, a polished gourd, or even a hollowed-out skull. Chalice implements are associated with the astrological signs of the mother and the newlyweds, as well as the sea dragon.";

        public static readonly string ChaliceInitiateBenefitSipText = "{b}Sip{/b} A sip grants the drinker an amount of temporary Hit Points equal to 2 + half your level.";

        public static readonly string ChaliceInitiateBenefitDrainText = "{b}Drain{/b} {i}(healing, positive){/i} Drinking deep instead heals the drinker 3 Hit Points for each level you have. The chalice can not be drained again this encounter.";

        public static readonly string ChaliceInitiateBenefitRulesText = "{b}Frequency{/b} once per round\n{b}Requirements{/b} You are holding your chalice implement.\n\nYou drink from the liquid that slowly collects in your chalice or administer it to an adjacent ally. The drinker chooses whether to take a small sip or to drain the contents.\n\n" + ChaliceInitiateBenefitSipText + "\n\n" + ChaliceInitiateBenefitDrainText;

        public static readonly string LanternInitiateBenefitName = string.Empty;

        public static readonly string LanternInitiateBenefitFlavorText = "Lantern implements shine the light of revelation to part shadows and expose truth. You might use a common glass lantern, torch, paper lantern, or other similar light source. Lantern implements are associated with the harrow suit of stars and the astrological signs of the lantern bearer and the archer.";

        public static readonly string LanternInitiateBenefitRulesText = "While you hold your lantern, its burning light leaves secrets no place to hide. You are always Seeking passively without the need to spend an Action. You gain a +1 status bonus to Perception checks to Seek.";

        public static readonly string MirrorInitiateBenefitName = "Mirror's Reflection";

        public static readonly string MirrorInitiateBenefitFlavorText = "Mirror implements represent misdirection, illusion, and sleight of hand, bending and shifting a perspective and the way you look at things. While larger mirrors hold the same mystic connotations, thaumaturges always choose small, portable, handheld mirrors as implements so they can use them easily while adventuring. Mirror implements are associated with the harrow suit of keys, and the astrological signs of the stranger and the swallow.";

        public static readonly string MirrorInitiateBenefitRulesText = "{b}Requirements{/b} You're holding your mirror implement.\n\nYou reflect an illusory image of yourself into another unoccupied space within 15 feet that you can see. You are treated as being in both spaces until the start of your next turn. You occupy both spaces.\n\nYour mirror self mimics your actions exactly, but any effects you generate come from only one of your positions; you decide which each time you act. For example, if you made a melee Strike against a creature within reach of the reflection, you'd mime the actions of the Strike, but only the reflection would actually make the Strike. Anything that targets or would affect your reflection affects you and uses your statistics. Something that would target or affect both of you affects you only once.\n\nThe effect also ends when you fall unconscious, at which point you decide which version is truly you.";

        public static readonly string RegaliaInitiateBenefitName = "Regalia Initiate Benefit";

        public static readonly string RegaliaInitiateBenefitFlavorText = "Regalia implements represent rulership, leadership, and social connections. While they differ in shape depending on regional customs and markers used to signify authority, common regalia implements are scepters, jeweled orbs, and heraldic banners. Regalia implements are associated with the harrow suit of crowns and the astrological signs of the patriarch and the sovereign dragon.";

        public static readonly string RegaliaInitiateBenefitRulesText = "While you hold your regalia, you gain an air of authority and bolster the courage of allies who believe in you. When you are holding your regalia, you gain an inspiring aura that stokes the courage of you and all allies in a 15-foot emanation who can see you, granting them a +1 status bonus to saving throws against fear. At the end of your turn, at the same time you would reduce your frightened value by 1, you reduce the frightened value of all allies within your inspiring aura by 1. Your aura has the emotion, mental, and visual traits.";

        public static readonly string TomeInitiateBenefitName = "Tome Initiate Benefit";

        public static readonly string TomeInitiateBenefitFlavorText = "Tome implements embody lost knowledge and otherworldly insights. While a weathered book is most common, tome implements can have as many different form factors as there are ways to store knowledge, from carved clay tablets to bundles of knotted cords. Tome implements are associated with the harrow suit of books and the astrological signs of the stargazer and the underworld dragon.";

        public static readonly string TomeInitiateBenefitRulesText = "While you hold your tome, lines of text appear on the open pages, revealing useful information. While you hold your tome, you gain a +1 circumstance bonus to Exploit Vulnerability skill checks. You gain two additional trained skills, and at level 3 you gain an additional skill to raise to Expert.";

        public static readonly string WandInitiateBenefitName = "Fling Magic";

        public static readonly string WandInitiateBenefitFlavorText = "Wand implements are short, lightweight batons, usually made of wood but often incorporating other materials. Due to their association with spellcasters, wand implements are connected to magic and its practice, as well as the direction and manipulation of energy. Wand implements are associated with the astrological signs of the thrush and the sky dragon.";

        public static readonly string WandInitiateBenefitRulesText = "{b}Requirements{/b} You are holding your wand implement.\n\nYou fling magical energy at a target within 60 feet, dealing damage equal to 1d4 + your Charisma modifier to the target, with a basic Reflex save against your class DC. The damage is of the type you selected when you gained your wand implement. At 3rd level and every 2 levels thereafter, the damage increases by 1d4.\n\nYou can expend more energy than usual to boost the effect of Fling Magic, dealing d6s of damage instead of d4s. After you do so, the wand takes 1d4 rounds to recharge, during which you can't boost the wand's damage but can continue to Fling Magic normally. If you critically hit with a Strike, your wand recharges immediately as it draws in power from the clash.";

        public static readonly string WeaponInitiateBenefitName = "Implement's Interruption";

        public static readonly string WeaponInitiateBenefitFlavorText = "Weapon implements are the most direct and confrontational, representing battle, struggle, and potentially violence. You can choose only a one-handed weapon as an implement, which allows you to channel energies into your weapon as well as hold your other implements once you gain them. Weapon implements are associated with the harrow suit of hammers and the astrological signs of the rider and the swordswoman.";

        public static readonly string WeaponInitiateBenefitRulesText = "{b}Trigger{/b} The target of your Exploit Vulnerability uses a concentrate, manipulate, or move action, or leaves a square during a move action it's using.\n{b}Requirements{/b} You're holding your weapon implement and are benefiting from Exploit Vulnerability against a creature. The creature must be within your reach if you're wielding a melee weapon, or within 10 feet if you're wielding a ranged weapon.\n\nYour weapon senses a moment of weakness and guides your hand to strike down a foe. Make a Strike against the triggering creature with your weapon implement. If your attack is a critical hit, you disrupt the triggering action. This Strike doesn't count toward your multiple attack penalty, and your multiple attack penalty doesn't apply to this Strike.";
    }
}
