﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace TheNeolithicMod
{
    class BehaviorNotifyHerdOfDamage : EntityBehavior
    {
        float notifyRange = 10.0f;

        public BehaviorNotifyHerdOfDamage(Entity entity) : base(entity)
        {
        }

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);
            notifyRange = attributes["notifyRange"].AsFloat(10.0f);
        }

        public override void OnEntityReceiveDamage(DamageSource damageSource, float damage)
        {
            entity.World.GetNearestEntity(entity.ServerPos.XYZ, notifyRange, notifyRange, (e) =>
            {
                EntityAgent agent = e as EntityAgent;
                if (e.EntityId != entity.EntityId && agent != null && agent.Alive && agent.HerdId == (entity as EntityAgent).HerdId)
                {
                    float r = (float)agent.World.Rand.NextDouble();

                    if (r < 0.5 && !TryTriggerAoD(agent))
                    {
                        TryTriggerFlee(agent);
                    }
                    else
                    {
                        TryTriggerFlee(agent);
                    }
                }
                return false;
            });
        }

        public bool TryTriggerAoD(EntityAgent agent) => agent.GetBehavior<EntityBehaviorEmotionStates>().TryTriggerState("aggressiveondamage");

        public bool TryTriggerFlee(EntityAgent agent) => agent.GetBehavior<EntityBehaviorEmotionStates>().TryTriggerState("fleeentity");

        public override string PropertyName() => "damagenotify";
    }
}
