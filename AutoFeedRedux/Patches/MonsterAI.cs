using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using AutoFeedRedux.Components;
using HarmonyLib;

namespace AutoFeedRedux.Patches;

public static class MonsterAIPatches
{
    
    
    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.UpdateConsumeItem))]
    static class MonsterAIUpdateConsumeItemPatch
    {
        static ItemDrop AutoFeederProcess(MonsterAI monsterAI)
        {
            return monsterAI.m_consumeTarget != null ? monsterAI.m_consumeTarget : AutoFeeder.Instance.FeedFromContainers(monsterAI);
        }
        
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGenerator)
        {
            var patchedSuccess = false;
            
            var instrs = instructions.ToList();

            var counter = 0;

            CodeInstruction LogMessage(CodeInstruction instruction)
            {
                AutoFeedRedux.Log.Debug($"IL_{counter}: Opcode: {instruction.opcode} Operand: {instruction.operand}");
                return instruction;
            }

            var ldArgInstruction = new CodeInstruction(OpCodes.Ldarg_0);
            var consumeTargetField = AccessTools.DeclaredField(typeof(MonsterAI), "m_consumeTarget");
            var findClosestMethod = AccessTools.DeclaredMethod(typeof(MonsterAI), nameof(MonsterAI.FindClosestConsumableItem));
            var feederMethod = AccessTools.Method(typeof(MonsterAIUpdateConsumeItemPatch), nameof(MonsterAIUpdateConsumeItemPatch.AutoFeederProcess));
            
            for (int i = 0; i < instrs.Count; ++i)
            {
                if (i > 5 && instrs[i-1].opcode == OpCodes.Stfld && instrs[i-1].operand.Equals(consumeTargetField) && instrs[i-2].opcode == OpCodes.Call && instrs[i-2].operand.Equals(findClosestMethod))
                {
                    yield return LogMessage(ldArgInstruction);
                    counter++;

                    //Monster AI
                    yield return LogMessage(ldArgInstruction);
                    counter++;
          
                    //Patch Calling Method
                    yield return LogMessage(new CodeInstruction(OpCodes.Call, feederMethod));
                    counter++;

                    //Save output of calling method to local variable 0
                    yield return LogMessage(new CodeInstruction(OpCodes.Stfld, consumeTargetField));
                    counter++;

                    //Original this
                    yield return LogMessage(instrs[i]);
                    counter++;

                    patchedSuccess = true;
                }
                else
                {
                    yield return LogMessage(instrs[i]);
                    counter++;
                }
            }
            
            if (!patchedSuccess)
            {
                AutoFeedRedux.Log.Error($"{nameof(Player.HaveRequirementItems)} Transpiler Failed To Patch");
                Thread.Sleep(5000);
            }
        }
    }
}