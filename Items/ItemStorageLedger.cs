using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.Server;

using VS_DigitInv.Network;
using VS_DigitInv.Utils;

namespace VS_DigitInv.Items
{
    internal class ItemStorageLedger : Item
    {
        public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel,
            EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
        {
            if (!firstEvent) return;
            IWorldAccessor world = byEntity.World;
            IPlayer player = (byEntity as EntityPlayer)?.Player;

            // 1. Shift + Right click on a container to link/unlink it (Toggle Link)
            if (byEntity.Controls.ShiftKey && blockSel != null)
            {
                if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is IBlockEntityContainer)
                {
                    if (world.Side == EnumAppSide.Server && player is IServerPlayer serverPlayer)
                    {
                        bool added = PosAttributeUtil.TogglePosition(slot.Itemstack.Attributes, blockSel.Position);
                        slot.MarkDirty();

                        string msg = added 
                            ? $"Linked container at {blockSel.Position}"
                            : $"Unlinked container at {blockSel.Position}";
                        serverPlayer.SendMessage(GlobalConstants.GeneralChatGroup, msg, EnumChatType.Notification);
                    }
                    handling = EnumHandHandling.PreventDefault;
                    return;
                }
            }
            base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
        }

        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {
            base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
            int count = PosAttributeUtil.GetPositions(inSlot.Itemstack.Attributes).Count;
            dsc.AppendLine($"Linked containers: {count}");
        }
    }
}
