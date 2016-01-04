namespace ProjectTeddy.Core.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SentenceType
    {
        public SentenceType()
        {
            AnnotatedConversations = new HashSet<AnnotatedConversation>();
            AnnotatedTweets = new HashSet<AnnotatedTweet>();
        }

        public int Id { get; set; }

        [Column("SentenceType")]
        [Required]
        [StringLength(50)]
        public string SentenceType1 { get; set; }

        public virtual ICollection<AnnotatedConversation> AnnotatedConversations { get; set; }

        public virtual ICollection<AnnotatedTweet> AnnotatedTweets { get; set; }
    }
}
