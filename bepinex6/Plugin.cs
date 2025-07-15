using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using HarmonyLib;
using BepInEx.Unity.Mono;
using BepInEx.Configuration;
using TerminalPoolSystem;

namespace GreyHackCustomizableOutput
{
  [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
  public class Plugin : BaseUnityPlugin
  {
    public static Harmony harmony = new Harmony("mod.greyhack_customizable_output");
    public static ConfigEntry<int> allowedTagsPerLine;
    public static ConfigEntry<int> allowedTextPerLine;

    void Awake()
    {
      // Plugin startup logic
      Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

      allowedTagsPerLine = Config.Bind(
        "General",
        "AllowedTagsPerLine",
        -1,
        "Allowed Tags Per Line"
      );

      allowedTextPerLine = Config.Bind(
        "General",
        "AllowedTextPerLine",
        256,
        "Allowed Text Per Line"
      );

      harmony.PatchAll();
    }
  }

  [HarmonyPatch(typeof(TerminalListAdapter))]
  [HarmonyPatch("AddText")]
  public static class TerminalListAdapterPatch
  {
    private static List<TerminalListItemModel> GenerateTerminalLines(ref string[] lines)
    {
      List<TerminalListItemModel> list = new List<TerminalListItemModel>();

      int maxTextPerLine = Math.Max(Plugin.allowedTextPerLine.Value, 1);
      int maxTagsPerLine = Plugin.allowedTagsPerLine.Value;

      for (int i = 0; i < lines.Length; i++)
      {
        string currentLine = lines[i];
        string textWithoutTags = TerminalListAdapter.StripTagsRegexCompiled(currentLine);
        int tagCount = currentLine.Length - textWithoutTags.Length;

        bool exceedsText = textWithoutTags.Length > maxTextPerLine;
        bool exceedsTags = maxTagsPerLine > -1 && tagCount > maxTagsPerLine;

        if (exceedsText || exceedsTags)
        {
          ReadOnlySpan<char> span = currentLine.AsSpan();
          for (int j = 0; j < span.Length;)
          {
            int remaining = span.Length - j;
            int take = Math.Min(maxTextPerLine, remaining);
            list.Add(new TerminalListItemModel
            {
              line = span.Slice(j, take).ToString()
            });
            j += take;
          }
        }
        else
        {
          list.Add(new TerminalListItemModel
          {
            line = currentLine
          });
        }
      }

      // Clear to prevent following code from using the same array
      lines = new string[0];

      return list;
    }

    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
      var codeMatcher = new CodeMatcher(instructions);

      codeMatcher
        .Start()
        .MatchForward(false,
          new CodeMatch(i => i.opcode == OpCodes.Newobj
          && ((ConstructorInfo)i.operand).DeclaringType == typeof(List<TerminalListItemModel>))
        )
        .SetAndAdvance(OpCodes.Ldloca_S, 0)  // Load array (local variable 0)
        .Insert(CodeInstruction.Call(typeof(TerminalListAdapterPatch), nameof(GenerateTerminalLines)));

      return codeMatcher.InstructionEnumeration();
    }
  }
}
