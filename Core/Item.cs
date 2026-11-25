using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace VS_DigitInv.Core
{
    internal interface INetowrkItem
    {
        int Id { get; }
        uint c_StackSize { get; }
    }

    internal interface IStorageSource
    {
        string SourceId { get; }
        int Priority { get; }
        bool isValid(IWorldAccessor world);
    }
}
