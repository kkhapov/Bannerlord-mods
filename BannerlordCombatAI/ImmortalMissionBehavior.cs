using TaleWorlds.MountAndBlade;

namespace BannerlordCombatAI
{
    // this thing is for debug only
    public class ImmortalMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
        {
            if (victim != null)
                victim.Health += 1000;
            
            base.OnMeleeHit(attacker, victim, isCanceled, collisionData);
        }
    }
}