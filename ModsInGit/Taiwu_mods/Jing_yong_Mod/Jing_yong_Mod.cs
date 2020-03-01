using Harmony12;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace Jing_yong_Mod
{
    public class Settings : UnityModManager.ModSettings
    {
        public int rejuvenatedAge = 0;
        public int rejuvenationAge = 6;
        public int changedGongFaFLevel = 0;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    public static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry.ModLogger Logger;

        public static Settings setting;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            setting = Settings.Load<Settings>(modEntry);

            Logger = modEntry.Logger;

            modEntry.OnToggle = OnToggle;

            modEntry.OnGUI = OnGUI;

            //if (enabled) 
            {
                string resdir = System.IO.Path.Combine(modEntry.Path, "Data");
                Logger.Log(" resdir :" + resdir);
                BaseResourceMod.Main.registModResDir(modEntry, resdir);
            }

            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal("Box");
            GUILayout.Label("�رղ����������Ϸ����ȡ���������ݼ���");
            GUILayout.BeginVertical("Box");
            GUILayout.Label("��ǰʵװ��Ч���У�");
            GUILayout.Label("1.������Ϊ���ԣ���ϰ�á���а���ס�����ʱ�ڱ仯ʱ�������ԡ��޸�֮�ˡ���");
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }

    [HarmonyPatch(typeof(UIDate), "TrunChange")]
    public static class UIDate_TrunChange_Patch
    {
        private static void Prefix()
        {
            if (!Main.enabled) {
                return;
            }

            if (DateFile.instance.gongFaDate.ContainsKey(150006)) {
                if (DateFile.instance.actorGongFas[DateFile.instance.mianActorId].ContainsKey(150006)) {
                    if (DateFile.instance.GetActorDate(DateFile.instance.mianActorId, 14, false) == "1") {
                        DateFile.instance.AddActorFeature(DateFile.instance.mianActorId, 1001);
                    }
                }
            }
        }
    }
}
