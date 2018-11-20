using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Items
{
	public class Cheese : ModItem
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("How can you go wrong with cheese?");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.value = 100;
            item.rare = 1;
            item.useStyle = 2;
            item.useAnimation = 17;
            item.useTime = 17;
            item.consumable = true;
            item.healLife = 50;
            item.UseSound = SoundID.Item2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock);
            recipe.SetResult(this, 99);
            recipe.AddRecipe();
            //recipe = new ModRecipe(mod);
            //recipe.AddRecipeGroup("ExampleMod:Cheese");
            //recipe.SetResult(this, 99);
            //recipe.AddRecipe();
        }

            /*
            public override bool UseItem(Item item, Player player)
            {
                if (item.healLife > 0)
                {
                    if (player.GetModPlayer<ExamplePlayer>(mod).badHeal)
                    {
                        int heal = item.healLife;
                        int damage = player.statLifeMax2 - player.statLife;
                        if (heal > damage)
                        {
                            heal = damage;
                        }
                        if (heal > 0)
                        {
                            player.AddBuff(mod.BuffType("Undead2"), 2 * heal, false);
                        }
                    }
                }
                return base.UseItem(item, player);
            }
            */
        }
}