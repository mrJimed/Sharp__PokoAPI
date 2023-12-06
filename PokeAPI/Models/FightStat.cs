using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Odbc;

namespace PokeAPI.Models
{
    [Table("fight_stat")]
    public class FightStat
    {
        [Column("id")]
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Column("win_poke_id")]
        public int WinPokeId { get; init; }

        [Column("lose_poke_id")]
        public int LosePokeId { get; init; }

        [Column("end_time", TypeName = "timestamp")]
        public DateTime EndTime { get; init; } = DateTime.Now;

        [Column("count_rounds")]
        public int CountRounds { get; init; }

        [Column("user_id")]
        public Guid UserId { get; set; }
    }
}
