using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTeddy.Core.EntityFramework
{
    public partial class WordRelation
    {
        [Required]
        [StringLength(50)]
        public string WordOne { get; set; }

        [Required]
        [StringLength(50)]
        public string WordTwo { get; set; }

        public int WordOneId { get; set; }

        public int WordTwoId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int PairId { get; set; }

        public int Id { get; set; }

        public double rScore { get; set; }
    }
}
