namespace ProjectTeddy.Core.EntityFramework
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
        public Word(string text)
        {
            this.Text = text;
            AnnotatedWords = new HashSet<AnnotatedWord>();
        }

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public ICollection<AnnotatedWord> AnnotatedWords { get; set; }
    }
}
