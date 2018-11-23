using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.NPCs
{
	//ported from my tAPI mod because I'm lazy
	public class Extraterrestrian : Hover
	{
		public Extraterrestrian()
		{
			speedY = 1f;
			accelerationY = 0.1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Extraterrestrian");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 1100;
			npc.damage = 140;
			npc.defense = 100;
			npc.knockBackResist = 0.3f;
			npc.width = 26;
			npc.height = 56;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = Item.buyPrice(0, 0, 15, 0);
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			banner = npc.type;
			bannerItem = mod.ItemType("ExtraterrestrianBanner");
		}

		public override void CustomBehavior(ref float ai)
		{
			Player player = Main.player[npc.target];
			ai += 1f;
			if (Math.Abs(npc.Center.X - player.Center.X) < 16f * 30f && Math.Abs(npc.Center.Y - player.Center.Y) < 16f * 20f)
			{
				if (ai >= 180f)
				{
					ai = -120f;
					if (Main.netMode != 1)
					{
						int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("WarpLaser"), npc.damage / 2, 0f, Main.myPlayer, player.Center.X, player.Center.Y);
					}
					npc.netUpdate = true;
				}
			}
			else if (ai > 300f)
			{
				ai = 300f;
			}
			if (ai < 0f)
			{
				if (Math.Abs(npc.velocity.X) >= 0.01f)
				{
					npc.velocity *= 0.95f;
				}
				else
				{
					npc.velocity.X = 0.01f * npc.direction;
				}
				if (ai == -60f || ai == -120f)
				{
					Main.PlaySound(SoundID.NPCDeath6, npc.position);
				}
				if (ai == -1f)
				{
					for (int k = 0; k < 255; k++)
					{
						Player target = Main.player[k];
						if (Math.Abs(npc.Center.X - target.Center.X) < 16f * 30f && Math.Abs(npc.Center.Y - target.Center.Y) < 16f * 20f)
						{
							target.AddBuff(BuffID.Cursed, 240, true);
							target.AddBuff(BuffID.Slow, 240, true);
							target.AddBuff(BuffID.Darkness, 240, true);
							if (target.FindBuffIndex(BuffID.Cursed) >= 0 || target.FindBuffIndex(BuffID.Slow) >= 0 || target.FindBuffIndex(BuffID.Darkness) >= 0)
							{
                                target.GetModPlayer<ExamplePlayer>(mod).lockTime = 60;
							}
						}
					}
				}
				if (ai == -61f)
				{
					ai = -1f;
				}
			}
			for (int k = 0; k < 2; k++)
			{
				int dust = Dust.NewDust(npc.position - new Vector2(8f, 8f), npc.width + 16, npc.height + 16, mod.DustType("Smoke"), 0f, 0f, 0, Color.Black);
				Main.dust[dust].velocity += npc.velocity * 0.25f;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 1;
			if (npc.frameCounter >= 30)
			{
				npc.rotation = Main.rand.Next(-2, 3) * (float)Math.PI / 32f;
				npc.frameCounter = 0;
			}
			npc.spriteDirection = npc.direction;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Nazar);
			}
		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.rand.Next(3) == 0)
			{
				player.AddBuff(BuffID.Cursed, 240, true);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            if (Main.LocalPlayer.GetModPlayer<ExamplePlayer>(mod).ZoneStarship)
                return SpawnCondition.OverworldDaySlime.Chance;
            return 0f;
		}
	}
}