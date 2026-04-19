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

        // Этот метод вызывается каждый раз, когда начинается новая миссия (битва, тренировка и т.д.)
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);
            mission.AddMissionBehavior(new ImmortalMissionBehavior());
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            game.GameTextManager.LoadGameTexts();
            var prev = gameStarterObject.Models.Where(x => x is AgentStatCalculateModel).ToArray();

            InformationManager.DisplayMessage(new InformationMessage("Prev count: " + prev.Length));
            
            gameStarterObject.AddModel(new CombatAiAgentStatCalculateModel((AgentStatCalculateModel)prev[0]));
        }
    }
}