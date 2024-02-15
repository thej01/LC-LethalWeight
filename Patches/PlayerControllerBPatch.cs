using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static LethalWeight.LethalWeightMod;

namespace LethalWeight.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        public static float logTimer = 0;
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void PreUpdate(ref float ___carryWeight, ref float ___fallValueUncapped)
        {
            // zeekerss what the fuck?
            float carryWeightReal = Mathf.Clamp(___carryWeight - 1f, 0f, 100f) * 105f;
            float extraFall = fallPerLB * carryWeightReal;
            float deltaTime = Time.deltaTime;

            //logTimer += deltaTime;

            if (logTimer > 5)
            {
                string msg = string.Format("Weight LB: {0}", carryWeightReal);
                lWeightLogger.Log(msg);

                msg = string.Format("Fall Value Uncapped:{0}", ___fallValueUncapped);
                lWeightLogger.Log(msg);

                msg = string.Format("Extra Fall Value Uncapped:{0}", extraFall);
                lWeightLogger.Log(msg);

                logTimer = 0;
            }

            if (___fallValueUncapped < 0)
                ___fallValueUncapped -= extraFall * deltaTime;
        }
    }
}
