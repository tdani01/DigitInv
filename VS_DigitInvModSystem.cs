using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

using VS_DigitInv.BlockEntities;
using VS_DigitInv.Core;
using VS_DigitInv.Items;
using VS_DigitInv.Models;
using VS_DigitInv.Network;
using VS_DigitInv.Utils;

using System.Linq;
using System.Collections.Generic;

namespace VS_DigitInv
{
    public class VS_DigitInvModSystem : ModSystem
    {
        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            Mod.Logger.Notification("Hello from template mod: " + api.Side);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("vs_digitinv:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("vs_digitinv:hello"));
        }
    }
}
