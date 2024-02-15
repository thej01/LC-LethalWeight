using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalWeight.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LethalWeight.Logger;

namespace LethalWeight
{
    public class Logger
    {
        internal ManualLogSource MLS;

        public string modName = "No-Name";
        public string modVersion = "No-Ver";

        public enum LogLevelConfig
        {
            None,
            Important,
            Everything
        }

        public void Init(string modGUID = "")
        {
            MLS = BepInEx.Logging.Logger.CreateLogSource(modGUID);
        }

        public bool LogLevelAllow(LogLevelConfig severity = LogLevelConfig.Important, LogLevelConfig severity2 = LogLevelConfig.Everything)
        {
            if (severity2 == LogLevelConfig.None)
                return false;

            if (severity == LogLevelConfig.Everything)
            {
                return severity2 == LogLevelConfig.Everything;
            }

            return true;
        }

        public void Log(string text = "", LogLevel level = LogLevel.Info, LogLevelConfig severity = LogLevelConfig.Important)
        {
            bool allowed = true; // ConfigValues.logLevel == null;
            /*if (!allowed)
            {
                allowed = LogLevelAllow(severity, ConfigValues.logLevel);
            }*/

            if (allowed)
            {
                string resultText = string.Format("[{0} v{1}] - {2}", modName, modVersion, text);
                MLS.Log(level, resultText);
            }
        }
    }

    [BepInPlugin(modGUID, modName, modVersion)]
    public class LethalWeightMod : BaseUnityPlugin
    {
        private const string modGUID = "thej01.lc.LethalFalling";
        private const string modName = "LethalFalling";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static LethalWeightMod Instance;

        public static Logger lWeightLogger = new Logger();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            lWeightLogger.Init(modGUID);

            lWeightLogger.modName = modName;
            lWeightLogger.modVersion = modVersion;

            lWeightLogger.Log("lWeightLogger Initialised!", LogLevel.Info, LogLevelConfig.Everything);

            lWeightLogger.Log("Patching LethalWeightMod...", LogLevel.Info, LogLevelConfig.Everything);
            harmony.PatchAll(typeof(LethalWeightMod));
            lWeightLogger.Log("Patched LethalWeightMod.", LogLevel.Info, LogLevelConfig.Everything);

            lWeightLogger.Log("Patching PlayerControllerBPatch...", LogLevel.Info, LogLevelConfig.Everything);
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            lWeightLogger.Log("Patched PlayerControllerBPatch.", LogLevel.Info, LogLevelConfig.Everything);
        }

        // how much is added to fallValueUncapped every second per LB
        public static float fallPerLB = 0.09f;
    }
}
