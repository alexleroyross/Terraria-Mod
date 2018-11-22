using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExampleMod.Items
{
    [AutoloadEquip(EquipType.Neck)]
    public class BandOfTheShark : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Band of the Shark");
			Tooltip.SetDefault("Sharks are pretty aggressive." + 
                "\n+7 Armor Penetration" + 
                "\n+3% Damage" + 
                "\n+7% Critical Strike Chance");
		}

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = 4;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 7;

            player.magicCrit += 7;
            player.meleeCrit += 7;
            player.rangedCrit += 7;
            player.thrownCrit += 7;

            player.meleeDamage *= 1.03f;
            player.thrownDamage *= 1.03f;
            player.rangedDamage *= 1.03f;
            player.magicDamage *= 1.03f;
            player.minionDamage *= 1.03f;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SharkToothNecklace);
            recipe.AddIngredient(ItemID.Shackle);
            recipe.AddIngredient(ItemID.SharkFin);
            recipe.AddTile(TileID.SharpeningStation);
			recipe.SetResult(this);
			recipe.AddRecipe();


        }
	}
}