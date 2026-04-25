using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BannerlordCombatAI
{
    public class CombatAiSubmoduleSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            // InformationManager.DisplayMessage(new InformationMessage("Mod for forced blocking has been loaded!"));
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);
            //mission.AddMissionBehavior(new ImmortalMissionBehavior());
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            game.GameTextManager.LoadGameTexts();
            var prev = gameStarterObject.Models.Where(x => x is AgentStatCalculateModel).ToArray();

            if (prev.Length > 0)
            {
                gameStarterObject.AddModel(new CombatAiAgentStatCalculateModel((AgentStatCalculateModel)prev[0]));
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("Bannerlord Combat AI: failed to find default AgentStatCalculateModel, combat AI changes won't work!"));
            }
        }
    }
}