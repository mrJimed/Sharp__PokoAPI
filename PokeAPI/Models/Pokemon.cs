namespace PokeAPI.Models
{
    public class Pokemon : ICloneable
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int Weight { get; init; }
        public int Height { get; init; }
        public string Image { get; init; }
        public int Hp { get; set; }
        public int AttackPower { get; init; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
