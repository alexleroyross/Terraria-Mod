using System;
using System.Collections.Generic;
using ExampleMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;

namespace ExampleMod.Projectiles
{
	// The following laser shows a channeled ability, after charging up the laser will be fired
	// Using custom drawing, dust effects, and custom collision checks for tiles
	public class WarpLaser : ModProjectile
	{
        // The maximum length for the laser
        private const int MaxHeight = 200;

        // Flag: has the laser reached its max length?
        private const bool ReachedMaxHeight = false;

        // Flag: has the laser reached its min length?
        private const bool ReachedMinHeight = false;

		//The distance charge particle from the player center
		//private const float MoveDistance = 60f;

        // The actual distance is stored in the ai0 field
        // By making a property to handle this it makes our life easier, and the accessibility more readable
        public float Distance
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        public int Height
        {
            get { return (int)projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }

        public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 2;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.hide = true;
		}

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 origin = Main.npc[projectile.owner].Center;
            float r = projectile.velocity.ToRotation() + -1.57f;

            //for (float i = (int)MoveDistance; i <= Distance; i += 10)
            //{
            Color c = Color.White;
            // origin = Main.npc[projectile.owner].Center + projectile.velocity;//i * projectile.velocity;
            origin = Main.npc[projectile.owner].TopRight + projectile.velocity;
            spriteBatch.Draw(Main.projectileTexture[projectile.type], origin - Main.screenPosition,
                new Rectangle(0, 0, 18, (Height > 200 ? 200 : Height)), c, r,
                new Vector2(28 * .5f, 26 * .5f), 1f, 0, 0);
            //}


            /*
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                if (projectile.oldPos[k] == Vector2.Zero)
                {
                    return;
                }
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + projectile.Size / 2f;
                Color color = Lighting.GetColor((int)(projectile.oldPos[k].X / 16f), (int)(projectile.oldPos[k].Y / 16f));
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, 0f, projectile.Size / 2f, 1f, SpriteEffects.None, 0f);
            }
            */
        }

        // TODO: THIS IS PROBABLY WRONG
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            Vector2 unit = projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(player.Hitbox.TopLeft(), player.Hitbox.Size(), targetHitbox.TopRight(),
                targetHitbox.TopRight() + unit * Distance, 18, ref point);
        }

		// The AI of the projectile
		public override void AI()
		{
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[projectile.owner];
            NPC npc = Main.npc[projectile.owner];
            
            Vector2 diff = player.Center - npc.TopRight;
            diff.Normalize();
            projectile.velocity = diff;
            projectile.direction = player.position.X > npc.position.X ? 1 : -1;
            projectile.netUpdate = true;
            projectile.timeLeft = 2;
            
            if (Height < MaxHeight)
            {
                if (!ReachedMaxHeight)
                {
                    Height += (int)projectile.velocity.Length();
                }
                else if (ReachedMaxHeight && !ReachedMinHeight)
                {
                    Height -= (int)projectile.velocity.Length();
                    projectile.position = npc.TopRight + projectile.velocity;
                }
                else
                {
                    projectile.position = npc.TopRight + projectile.velocity;// * MoveDistance;
                }
            }
            projectile.height = Height;
            Distance = projectile.Hitbox.Size().Length();


            /*
            int dir = projectile.direction;
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            */

            /*
            //Add lights
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
				DelegateMethods.CastLight);
                */
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = projectile.velocity;
			Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
		}
	}
}
