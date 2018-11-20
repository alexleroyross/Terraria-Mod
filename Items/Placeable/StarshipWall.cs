using Terraria.ModLoader;

namespace ExampleMod.Items.Placeable
{
	public class StarshipWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("An extraterrestrial construction.");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.consumable = true;
			item.createWall = mod.WallType("StarshipWall");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarshipBlock");
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}