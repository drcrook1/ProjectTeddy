namespace ProjectTeddy.WebApi.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Word
    {
        public Word()
        {
            AnnotatedWords = new HashSet<AnnotatedWord>();
        }

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual ICollection<AnnotatedWord> AnnotatedWords { get; set; }
    }
}
