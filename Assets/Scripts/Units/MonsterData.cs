using Scriptable_Objects.Unit;
using UnityEngine;

namespace Units
{
    public class MonsterData : UnitData
    {
        private MonsterSO monsterSo => (MonsterSO) unitSo;
        
        public MonsterData(UnitSO unitSo) : base(unitSo)
        {
            Init();
        }
        
        protected override void Init()
        {
            currentHp = MaxHp;
            currentMp = MaxMp;
        }
        
        public override int MaxHp { get => monsterSo.MaxHp; }

        public override int MaxMp { get => monsterSo.MaxMp; }
        
        public int HitRate { get => monsterSo.HitRate; }
        
        public (int,int) Rewards { get => (monsterSo.XpReward, monsterSo.GilReward); }
    }
}