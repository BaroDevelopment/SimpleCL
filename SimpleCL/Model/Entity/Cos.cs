﻿namespace SimpleCL.Model.Entity
{
    public class Cos : Npc
    {
        public Cos(uint id) : base(id)
        {
        }

        public bool IsHorse()
        {
            return TypeId4 == 1;
        }

        public bool IsTransport()
        {
            return TypeId4 == 2;
        }

        public bool IsAttackPet()
        {
            return TypeId4 == 3;
        }

        public bool IsPickPet()
        {
            return TypeId4 == 4;
        }

        public bool IsGuildGuard()
        {
            return TypeId4 == 5;
        }

        public bool IsQuestPet()
        {
            return TypeId4 == 6;
        }

        public bool IsFellowPet()
        {
            return TypeId4 == 9;
        }

        public bool IsRam()
        {
            return TypeId4 == 10;
        }

        public bool IsCatapult()
        {
            return TypeId4 == 11;
        }
    }
}