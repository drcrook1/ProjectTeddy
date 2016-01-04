namespace ProjectTeddy.WebApi.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnnotatedTweet")]
    public partial class AnnotatedTweet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int TweetId { get; set; }

        public int SentenceTypeId { get; set; }

        public double Sentiment { get; set; }

        [Required]
        [StringLength(50)]
        public string AnnotatedBy { get; set; }

        public DateTime AnnotatedOn { get; set; }

        public virtual SentenceType SentenceType { get; set; }

        public virtual Tweet Tweet { get; set; }
    }
}
