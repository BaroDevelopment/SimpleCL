﻿using System.Threading.Tasks;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Server;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Exchange;
using SimpleCL.Models.Items;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Ui;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class StallService : Service
    {
        private readonly SilkroadServer _silkroadServer;
        private StallWindow StallWindow { get; set; }

        public StallService(SilkroadServer silkroadServer)
        {
            _silkroadServer = silkroadServer;
        }

        #region Opened

        [PacketHandler(Opcode.Agent.Response.STALL_TALK)]
        public void StallOpen(Server server, Packet packet)
        {
            if (!packet.ReadBool())
            {
                return;
            }

            var playerUid = packet.ReadUInt();
            if (!Entities.AllEntities.TryGetValue(playerUid, out var entity)
                || entity is not Player player
                || player.Stall == null)
            {
                return;
            }

            player.Stall.Description = packet.ReadUnicode();
            player.Stall.Opened = packet.ReadBool();
            var mode = packet.ReadByte();

            byte slot;
            while ((slot = packet.ReadByte()) != byte.MaxValue)
            {
                var stallItem = StallItem.ParseItem(packet, _silkroadServer.Locale);
                stallItem.Slot = slot;
                var inventorySlot = packet.ReadByte();
                stallItem.Quantity = packet.ReadUShort();
                stallItem.GoldValue = packet.ReadULong();
                player.Stall.Items.Add(stallItem);
            }

            Task.Run(() =>
            {
                Program.Gui.InvokeLater(() =>
                {
                    StallWindow = new StallWindow(player);
                    StallWindow.ShowDialog();
                });
            });
        }

        #endregion

        #region Created

        [PacketHandler(Opcode.Agent.Response.STALL_ENTITY_CREATE)]
        public void StallCreate(Server server, Packet packet)
        {
            if (!Entities.AllEntities.TryGetValue(packet.ReadUInt(), out var entity) || entity is not Player player)
            {
                return;
            }

            var stall = new Stall
            {
                Title = packet.ReadUnicode(),
                PlayerUid = player.Uid
            };

            player.Stall = stall;
            player.InteractionType = Player.Interaction.OnStall;
            Program.Gui.RefreshPlayerMarker(player.Uid);
        }

        #endregion

        #region Destroyed

        [PacketHandler(Opcode.Agent.Response.STALL_ENTITY_DESTROY)]
        public void StallDestroy(Server server, Packet packet)
        {
            if (!Entities.AllEntities.TryGetValue(packet.ReadUInt(), out var entity) || entity is not Player player)
            {
                return;
            }

            player.Stall = null;
            Program.Gui.RefreshPlayerMarker(player.Uid);
        }

        #endregion

        [PacketHandler(Opcode.Agent.Response.STALL_LEAVE)]
        public void StallLeave(Server server, Packet packet)
        {
            if (!packet.ReadBool())
            {
                return;
            }

            StallWindow?.InvokeLater(() => { StallWindow?.Exit(); });
        }
    }
}