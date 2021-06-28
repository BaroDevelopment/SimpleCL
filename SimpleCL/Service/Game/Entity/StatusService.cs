﻿using SilkroadSecurityApi;
using SimpleCL.Enums.Common;
using SimpleCL.Enums.Event;
using SimpleCL.Model.Character;
using SimpleCL.Model.Entity;
using SimpleCL.Network;
using SimpleCL.Util.Extension;

namespace SimpleCL.Service.Game.Entity
{
    public class StatusService : Service
    {
        [PacketHandler(Opcodes.Agent.Response.ENTITY_POTION_UPDATE)]
        public void HealthChanged(Server server, Packet packet)
        {
            var uid = packet.ReadUInt();

            packet.ReadByte();
            packet.ReadByte();
            
            EntityStateEvent.Health eventType = (EntityStateEvent.Health) packet.ReadByte();
            uint newHp = 0;
            uint newMp = 0;
            PathingEntity.BadStatus badStatus = PathingEntity.BadStatus.None;
            
            switch (eventType)
            {
                case EntityStateEvent.Health.HP:
                    newHp = packet.ReadUInt();
                    break;

                case EntityStateEvent.Health.MP:
                    newMp = packet.ReadUInt();
                    break;

                case EntityStateEvent.Health.EntityHPMP:
                case EntityStateEvent.Health.HPMP:
                    newHp = packet.ReadUInt();
                    newMp = packet.ReadUInt();
                    break;
                
                case EntityStateEvent.Health.BadStatus:
                    badStatus = (PathingEntity.BadStatus) packet.ReadUInt();
                    break;
            }
            
            if (uid == LocalPlayer.Get.Uid)
            {
                LocalPlayer.Get.Hp = newHp;
                LocalPlayer.Get.Mp = newMp;
            }
        }
    }
}