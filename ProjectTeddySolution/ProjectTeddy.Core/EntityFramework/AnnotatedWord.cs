namespace ProjectTeddy.Core.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AnnotatedWord
    {
        public int Id { get; set; }

        public int WordId { get; set; }

        [Required]
        [StringLength(255)]
        public string AnnotatedBy { get; set; }

        public DateTime AnnotatedOn { get; set; }

        public double Sentiment { get; set; }

        public int PartOfSpeechId { get; set; }

        public virtual PartsOfSpeech PartsOfSpeech { get; set; }

        public virtual Word Word { get; set; }
    }
}
