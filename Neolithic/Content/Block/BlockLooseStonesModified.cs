﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace TheNeolithicMod
{
    class BlockLooseStonesModified : BlockLooseStones
    {
        public readonly string[] allowedbases = new string[]
        {
            "soil",
            "gravel",
            "sand"
        };

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
            if (slot.Itemstack != null)
            {
                if (slot.Itemstack.Collectible.FirstCodePart() == "tamper")
                {
                    return true;
                }
            }
            return false;
        }

        public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
            if (slot.Itemstack != null)
            {
                if (slot.Itemstack.Collectible.FirstCodePart() == "tamper")
                {
                    return HandAnimations.Hit(world, byPlayer.Entity, secondsUsed);
                }
            }
            return false;
        }

        public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
            if (slot.Itemstack != null)
            {
                if (slot.Itemstack.Collectible.FirstCodePart() == "tamper")
                {
                    if (world.Side.IsServer())
                    {
                        Block dBlock = blockSel.Position.DownCopy().GetBlock();

                        if (!allowedbases.Contains(dBlock.FirstCodePart())) return;
                        string blockbase = dBlock.FirstCodePart() == "soil" ? "andesite" : dBlock.LastCodePart();
                        AssetLocation location = new AssetLocation("neolithicmod:3droad-" + dBlock.FirstCodePart() + "-" + LastCodePart() + "-" + blockbase + "-" + "stepping" + world.Rand.Next(1, 4));
                        Block nextBlock = location.GetBlock();
                        if (nextBlock == null) return;

                        world.PlaySoundAt(Sounds.Break, blockSel.Position.X, blockSel.Position.Y, blockSel.Position.Z);
                        world.BlockAccessor.SetBlock(0, blockSel.Position);
                        world.BlockAccessor.SetBlock(nextBlock.BlockId, blockSel.Position.DownCopy());
                    }
                    return;
                }
            }
        }
    }
}