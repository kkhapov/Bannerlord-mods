using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CombatAI
{
    public class CombatAiAgentStatCalculateModel : AgentStatCalculateModel
    {
        private readonly AgentStatCalculateModel prev;

        public CombatAiAgentStatCalculateModel(AgentStatCalculateModel prev)
        {
            this.prev = prev;
        }

        public override void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
        {
            prev.UpdateAgentStats(agent, agentDrivenProperties);
            if (!agent.IsHuman)
                return;

            var settings = BannerlordCombatAISettings.Instance;
            if (settings == null)
            {
                // todo: do not want to spam error to Log, but we need to notify user mod do not work
                return;
            }

            if (!settings.EnableMod)
                return;

            ImprovedCombatAiUpdateStats(agent, agentDrivenProperties, settings.Difficulty);
        }
        
        private void ImprovedCombatAiUpdateStats(Agent agent, AgentDrivenProperties agentDrivenProperties, float difficulty)
        {
            var num4 = difficulty + agent.Defensiveness;
            var difficultyModifier = GetDifficultyModifier();
            
            agentDrivenProperties.AiFacingMissileWatch = difficulty * 0.06f - 0.96f;
            agentDrivenProperties.AiFlyingMissileCheckRadius = (float) (8.0 - 6.0 * difficulty);
            agentDrivenProperties.AIBlockOnDecideAbility = MBMath.Lerp(0.5f, 0.99f, MBMath.ClampFloat((float) Math.Pow(difficulty, 0.5), 0.0f, 1f));
            agentDrivenProperties.AIParryOnDecideAbility = MBMath.Lerp(0.5f, 0.95f, MBMath.ClampFloat(difficulty, 0.0f, 1f));
            agentDrivenProperties.AiTryChamberAttackOnDecide = (difficulty - 0.15f) * 0.1f;
            agentDrivenProperties.AIAttackOnParryChance = 0.08f - 0.02f * agent.Defensiveness;
            agentDrivenProperties.AiAttackOnParryTiming = 0.3f * difficulty - 0.2f;
            agentDrivenProperties.AIDecideOnAttackChance = 0.5f * agent.Defensiveness;
            agentDrivenProperties.AIParryOnAttackAbility = MBMath.ClampFloat(difficulty, 0.0f, 1f);
            agentDrivenProperties.AiKick = (difficulty > 0.4f ? 0.4f : difficulty) - 0.1f;
            agentDrivenProperties.AiAttackCalculationMaxTimeFactor = difficulty;
            agentDrivenProperties.AiDecideOnAttackWhenReceiveHitTiming = (float) (-0.25 * (1.0 - difficulty));
            agentDrivenProperties.AiDecideOnAttackContinueAction = (float) (-0.5 * (1.0 - difficulty));
            agentDrivenProperties.AiDecideOnAttackingContinue = 0.1f * difficulty;
            agentDrivenProperties.AIParryOnAttackingContinueAbility = MBMath.Lerp(0.05f, 0.95f, MBMath.ClampFloat(difficulty, 0.0f, 1f));
            agentDrivenProperties.AIDecideOnRealizeEnemyBlockingAttackAbility = MBMath.ClampFloat((float) Math.Pow(difficulty, 2.5) - 0.1f, 0.0f, 1f);
            agentDrivenProperties.AIRealizeBlockingFromIncorrectSideAbility = MBMath.ClampFloat((float) Math.Pow(difficulty, 2.5) - 0.1f, 0.0f, 1f);
            agentDrivenProperties.AiAttackingShieldDefenseChance = 0.2f + 0.3f * difficulty;
            agentDrivenProperties.AiAttackingShieldDefenseTimer = 0.3f * difficulty - 0.3f;
            agentDrivenProperties.AiRandomizedDefendDirectionChance = 1f - MathF.Pow(difficulty, 3f);
            //agentDrivenProperties.AiShooterError = 0.008f;
            agentDrivenProperties.AISetNoAttackTimerAfterBeingHitAbility = MBMath.Lerp(0.33f, 1f, difficulty);
            agentDrivenProperties.AISetNoAttackTimerAfterBeingParriedAbility = MBMath.Lerp(0.2f, 1f, difficulty * difficulty);
            agentDrivenProperties.AISetNoDefendTimerAfterHittingAbility = MBMath.Lerp(0.1f, 0.99f, difficulty * difficulty);
            agentDrivenProperties.AISetNoDefendTimerAfterParryingAbility = MBMath.Lerp(0.15f, 1f, difficulty * difficulty);
            agentDrivenProperties.AIEstimateStunDurationPrecision = 1f - MBMath.Lerp(0.2f, 1f, difficulty);
            agentDrivenProperties.AIHoldingReadyMaxDuration = MBMath.Lerp(0.25f, 0.0f, Math.Min(1f, difficulty * 2f));
            agentDrivenProperties.AIHoldingReadyVariationPercentage = difficulty;
            agentDrivenProperties.AiRaiseShieldDelayTimeBase = (float) (0.5 * difficulty - 0.75);
            agentDrivenProperties.AiUseShieldAgainstEnemyMissileProbability = 0.1f + difficulty * 0.6f + num4 * 0.2f;
            agentDrivenProperties.AiCheckApplyMovementInterval = (float) ((2.0 - difficultyModifier) * (0.05 + 0.005 * (1.1 - difficulty)));
            agentDrivenProperties.AiCheckCalculateMovementInterval = agent.HasMount || agent.IsMount ? 0.25f : (float) ((2.0 - difficultyModifier) * 0.25);
            agentDrivenProperties.AiCheckDecideSimpleBehaviorInterval = (float) ((2.0 - difficultyModifier) * (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWieldWeapon) ? 1.5 : 0.2));
            agentDrivenProperties.AiCheckDoSimpleBehaviorInterval = 2f - difficultyModifier;
            agentDrivenProperties.AiParryDecisionChangeValue = 0.05f + 0.7f * difficulty;
            agentDrivenProperties.AiDefendWithShieldDecisionChanceValue = Math.Min(2f, (float) (0.5 + difficulty + 0.6f * num4));
            agentDrivenProperties.AiMoveEnemySideTimeValue = (float) (0.5 * difficulty - 2.5);
            agentDrivenProperties.AiMinimumDistanceToContinueFactor = (float) (2.0 + 0.3f * (3.0 - difficulty));
            agentDrivenProperties.AiChargeHorsebackTargetDistFactor = (float) (1.5 * (3.0 - difficulty));
            agentDrivenProperties.AIAttackOnDecideChance = MathF.Clamp((float) (0.1 * CalculateAIAttackOnDecideMaxValue() * (3.0 - agent.Defensiveness)), 0.05f, 1f);

        }
        
        #region empty

        public override void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties,
            AgentBuildData agentBuildData)
        {
            prev.InitializeAgentStats(agent, spawnEquipment, agentDrivenProperties, agentBuildData);
        }

        public override float GetDifficultyModifier()
        {
            return prev.GetDifficultyModifier();
        }

        public override bool CanAgentRideMount(Agent agent, Agent targetMount)
        {
            return prev.CanAgentRideMount(agent, targetMount);
        }

        public override float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon)
        {
            return prev.GetWeaponDamageMultiplier(agent, weapon);
        }

        public override float GetEquipmentStealthBonus(Agent agent)
        {
            return prev.GetEquipmentStealthBonus(agent);
        }

        public override float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon)
        {
            return prev.GetSneakAttackMultiplier(agent, weapon);
        }

        public override float GetKnockBackResistance(Agent agent)
        {
            return prev.GetKnockBackResistance(agent);
        }

        public override float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid)
        {
            return prev.GetKnockDownResistance(agent, strikeType);
        }

        public override float GetDismountResistance(Agent agent)
        {
            return prev.GetDismountResistance(agent);
        }

        public override float GetBreatheHoldMaxDuration(Agent agent, float baseBreatheHoldMaxDuration)
        {
            return prev.GetBreatheHoldMaxDuration(agent, baseBreatheHoldMaxDuration);
        }
        
        #endregion
    }
}