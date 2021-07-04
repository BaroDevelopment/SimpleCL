﻿using System.Collections.Generic;
using SimpleCL.Models.Character;
using SimpleCL.Models.Skills;

namespace SimpleCL.Models.Entities
{
    public class Actor : Entity
    {
        public uint Hp { get; set; }
        public uint Mp { get; set; }
        public BadStatusFlag BadStatus { get; set; }
        public float WalkSpeed { get; set; }
        public float RunSpeed { get; set; }
        public float ZerkSpeed { get; set; }
        public ushort Angle { get; set; }
        public List<Buff> Buffs = new List<Buff>();
        public Actor(uint id) : base(id)
        {
            
        }

        public bool IsPlayer()
        {
            return TypeId2 == 1;
        }

        public bool IsNpc()
        {
            return TypeId2 == 2;
        }
        
        public enum BadStatusFlag : uint
        {
            None = 0,
            Freezing = 0x1, // Universal
            Frostbite = 0x2, // None
            ElectricShock = 0x4, // Universal
            Burn = 0x8, // Universal
            Poisoning = 0x10, // Universal
            Zombie = 0x20, // Universal
            Sleep = 0x40, // None
            Bind = 0x80, // None
            Dull = 0x100, // Purification
            Fear = 0x200, // Purification
            ShortSight = 0x400, // Purification
            Bleed = 0x800, // Purification
            Petrify = 0x1000, // None
            Darkness = 0x2000, // Purification
            Stun = 0x4000, // None
            Disease = 0x8000, // Purification
            Confusion = 0x10000, // Purification
            Decay = 0x20000, // Purification
            Weaken = 0x40000, // Purification
            Impotent = 0x80000, // Purification
            Division = 0x100000, // Purification
            Panic = 0x200000, // Purification
            Combustion = 0x400000, // Purification
            Unk01 = 0x800000,
            Hidden = 0x1000000, // Purification
            Unk02 = 0x2000000,
            Unk03 = 0x4000000,
            Unk04 = 0x8000000,
            Unk05 = 0x10000000,
            Unk06 = 0x20000000,
            Unk07 = 0x40000000,
            Unk08 = 0x80000000
        }
    }
}