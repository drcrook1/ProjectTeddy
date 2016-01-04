namespace ProjectTeddy.Core.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartsOfSpeech")]
    public partial class PartsOfSpeech
    {
        public PartsOfSpeech()
        {
            AnnotatedWords = new HashSet<AnnotatedWord>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string PartOfSpeech { get; set; }

        public virtual ICollection<AnnotatedWord> AnnotatedWords { get; set; }
    }
}
