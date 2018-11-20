using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.NPCs
{
	public class CheeseGuy : ModNPC
    {
        Random rand = new Random();
        const int NumFrames = 4;
        int JumpCoolDown = 400;
        int FrameCoolDown = 4;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cheese Guy");
			Main.npcFrameCount[npc.type] = NumFrames; // make sure to set this for your modnpcs.
        }

		public override void SetDefaults()
		{
			npc.width = 20;
			npc.height = 20;
			npc.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1.
			npc.damage = 7;
			npc.defense = 2;
			npc.lifeMax = 25;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			//npc.alpha = 175;
			//npc.color = new Color(0, 80, 255, 100);
			npc.value = 25f;
            
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// we would like this npc to spawn in the overworld.
			return (ExampleWorld.CheeseMoonActive ? SpawnCondition.OverworldNightMonster.Chance : 0f);
		}
        
		public override void AI()
        {
            npc.TargetClosest(true);
            if (JumpCoolDown <= 0 && npc.velocity.Y == 0/* && npc.oldVelocity.Y >= 0*/)
            {
                if (rand.Next(1, 3) == 1)
                    npc.velocity.X = 0f;
                npc.velocity.Y = -rand.Next(2, 17);
                JumpCoolDown = rand.Next(6, 61);
            }
            if(npc.velocity.Y < 0 && npc.velocity.X == 0)
            {
                npc.velocity.X = npc.direction * rand.Next(1, 5);
            }
            JumpCoolDown--;
		}
        
		public override void FindFrame(int frameHeight)
		{
            if (FrameCoolDown <= 0)
            {
                npc.frame.Y = npc.height * (rand.Next(1, NumFrames + 1) - 1);
                FrameCoolDown = rand.Next(2, 31);
            }
            FrameCoolDown--;
		}

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), mod.ItemType("Cheese"));
        }
    }
}
