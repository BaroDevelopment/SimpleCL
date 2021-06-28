﻿using System;
using System.Collections.Specialized;
using SimpleCL.Database;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Entity.Fortress;
using SimpleCL.Model.Entity.Fortress.Structure;
using SimpleCL.Model.Entity.Mob;
using SimpleCL.Model.Entity.Pet;

namespace SimpleCL.Model.Entity
{
    public class Entity
    {
        public readonly uint Id;
        public readonly string ServerName;
        public string Name { get; set; }
        public readonly byte TypeId1;
        public readonly byte TypeId2;
        public readonly byte TypeId3;
        public readonly byte TypeId4;
        public LocalPoint LocalPoint { get; set; }

        public WorldPoint WorldPoint => WorldPoint.FromLocal(LocalPoint);

        public Entity(uint id)
        {
            Id = id;

            if (id == uint.MaxValue)
            {
                return;
            }

            NameValueCollection data;
            if ((data = GameDatabase.Get.GetModel(id)) != null)
            {
                TypeId1 = 1;
            }
            else if ((data = GameDatabase.Get.GetItemData(id)) != null)
            {
                TypeId1 = 3;
            }
            else if ((data = GameDatabase.Get.GetTeleportBuilding(id)) != null ||
                     (data = GameDatabase.Get.GetTeleportLink(id)) != null)
            {
                TypeId1 = 4;
            }

            if (data != null)
            {
                var tid2 = TypeId1 == 3 ? "tid1" : "tid2";
                var tid3 = TypeId1 == 3 ? "tid2" : "tid3";
                var tid4 = TypeId1 == 3 ? "tid3" : "tid4";
                ServerName = data["servername"];
                Name = data["name"];
                TypeId2 = byte.Parse(data[tid2]);
                TypeId3 = byte.Parse(data[tid3]);
                TypeId4 = byte.Parse(data[tid4]);
            }
            else
            {
                Console.WriteLine("Unable to parse entity with id: " + id);
            }
        }

        public static Entity FromId(uint id)
        {
            Entity entity = new Entity(id);
            if (entity.IsSkillAoe())
            {
                return new SkillAoe(id);
            }

            if (entity.IsPathingEntity())
            {
                PathingEntity pathingEntity = new PathingEntity(id);
                if (pathingEntity.IsPlayer())
                {
                    return new Player(id);
                }

                if (pathingEntity.IsNpc())
                {
                    Npc npc = new Npc(id);
                    if (npc.IsMonster())
                    {
                        Monster monster = new Monster(id);
                        if (monster.IsFlower())
                        {
                            return new Flower(id);
                        }

                        if (monster.IsThiefCaravan())
                        {
                            return new ThiefCaravan(id);
                        }

                        if (monster.IsTraderCaravan())
                        {
                            return new TraderCaravan(id);
                        }
                        
                        return monster;
                    }

                    if (npc.IsTalk())
                    {
                        return new TalkNpc(id);
                    }

                    if (npc.IsCos())
                    {
                        Cos cos = new Cos(id);
                        if (cos.IsHorse())
                        {
                            return new Horse(id);
                        }

                        if (cos.IsTransport())
                        {
                            return new Transport(id);
                        }

                        if (cos.IsAttackPet())
                        {
                            return new AttackPet(id);
                        }

                        if (cos.IsPickPet())
                        {
                            return new PickPet(id);
                        }

                        if (cos.IsGuildGuard())
                        {
                            return new GuildGuard(id);
                        }

                        if (cos.IsQuestPet())
                        {
                            return new QuestPet(id);
                        }

                        if (cos.IsFellowPet())
                        {
                            return new FellowPet(id);
                        }
                    }

                    if (npc.IsFortressCos())
                    {
                        FortressCos fortressCos = new FortressCos(id);
                        if (fortressCos.IsPatrolGuard())
                        {
                            return new FortressPatrolGuard(id);
                        }

                        if (fortressCos.IsFortressFlag())
                        {
                            return new FortressFlag(id);
                        }

                        if (fortressCos.IsFortressMonster())
                        {
                            return new FortressMonster(id);
                        }

                        if (fortressCos.IsDefenseGuard())
                        {
                            return new FortressDefenseGuard(id);
                        }
                    }

                    if (npc.IsFortressStructure())
                    {
                        FortressStructure structure = new FortressStructure(id);
                        if (structure.IsHeart())
                        {
                            return new FortressHeart(id);
                        }

                        if (structure.IsTower())
                        {
                            return new FortressTower(id);
                        }

                        if (structure.IsGate())
                        {
                            return new FortressGate(id);
                        }

                        if (structure.IsDefenseCamp())
                        {
                            return new FortressDefenseCamp(id);
                        }

                        if (structure.IsCommandPost())
                        {
                            return new FortressCommandPost(id);
                        }

                        if (structure.IsObstacle())
                        {
                            return new FortressObstacle(id);
                        }
                    }
                }
            }

            if (entity.IsGroundItem())
            {
                return new GroundItem(id);
            }

            if (entity.IsTeleport())
            {
                return new Teleport(id);
            }

            return entity;
        }

        public bool IsSkillAoe()
        {
            return Id == uint.MaxValue;
        }

        public bool IsPathingEntity()
        {
            return TypeId1 == 1;
        }

        public bool IsGroundItem()
        {
            return TypeId1 == 3;
        }

        public bool IsTeleport()
        {
            return TypeId1 == 4;
        }

        public override string ToString()
        {
            return GetType().Name + ": " + Name + " [" + Id + "]";
        }
    }
}