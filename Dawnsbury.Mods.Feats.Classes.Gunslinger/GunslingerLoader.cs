﻿using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.FeatsDb;
using Dawnsbury.Modding;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dawnsbury.Mods.Feats.Classes.Gunslinger
{
    /// <summary>
    /// Loads the Guns and Gears content
    /// </summary>
    public class GunslingerLoader
    {
        /// <summary>
        /// Runs on launch and loads all the mod content
        /// </summary>
        [DawnsburyDaysModMainMethod]
        public static void LoadMod()
        {
            AddFeats(Gunslinger.CreateGunslingerFeats());
            AllFeats.All.ForEach(Gunslinger.PatchFeats);
        }

        /// <summary>
        /// Adds the provided feats via the ModManager
        /// </summary>
        /// <param name="feats">The feats to add</param>
        private static void AddFeats(IEnumerable<Feat> feats)
        {
            foreach (Feat feat in feats)
            {
                ModManager.AddFeat(feat);
            }
        }
    }
}
