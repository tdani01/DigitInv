using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using VS_DigitInv.Items;
using VS_DigitInv.Utils;

namespace VS_DigitInv.BlockEntities
{
    internal class BlockEntityStorageLectern : BlockEntityDisplay
    {
        private readonly InventoryGeneric _inventory;
        public override InventoryBase Inventory => _inventory;
        public override string InventoryClassName => "storagelectern"; //actually a table but nvm
        public bool hasBook => !_inventory[0].Empty && _inventory[0].Itemstack?.Item is ItemStorageLedger;

        public BlockEntityStorageLectern()
        {
            _inventory = new InventoryGeneric(1, null, null);
        }

        public bool OnInteract(IPlayer byPlayer)
        {
            ItemSlot active = byPlayer.InventoryManager.ActiveHotbarSlot;
            // Case 1: Lectern has no book, and player is holding a ledger: place the ledger in the lectern
            if (!hasBook)
            {
                if (!active.Empty && active.Itemstack?.Item is ItemStorageLedger)
                {
                    if(Api.Side == EnumAppSide.Server)
                    {
                        _inventory[0].Itemstack = active.TakeOut(1);
                        active.MarkDirty();
                        _inventory[0].MarkDirty();
                        MarkDirty(true);
                    }
                    return true;
                }
                if (Api.Side == EnumAppSide.Server && byPlayer is IServerPlayer serverPlayer)
                {
                    serverPlayer.SendMessage(
                        GlobalConstants.GeneralChatGroup,
                        "This table has no storage ledger. Right-click it while holdeing a storage ledger to place one.",
                        EnumChatType.Notification);
                }
                return true;
            }

            // Case 2: Lectern HAS book & player is shift + right-clicking to remove book
            if (byPlayer.Entity.Controls.ShiftKey)
            {
                if (Api.Side == EnumAppSide.Server)
                {
                    ItemStack book = _inventory[0].TakeOut(1);
                    _inventory[0].MarkDirty();
                    MarkDirty(true);

                    if (!byPlayer.InventoryManager.TryGiveItemstack(book, true));
                    {
                        Api.World.SpawnItemEntity(book, Pos.ToVec3d().Add(0.5, 1.0, 0.5));
                    }
                }
                return true;
            }

            // Case 3: Lectern HAS book & normal right-click to open the ledger gui
        }
    }
}
