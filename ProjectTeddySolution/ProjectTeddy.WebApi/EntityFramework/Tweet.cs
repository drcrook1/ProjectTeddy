namespace ProjectTeddy.WebApi.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tweet
    {
        public Tweet()
        {
            AnnotatedTweets = new HashSet<AnnotatedTweet>();
            Conversations = new HashSet<Conversation>();
        }

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string ReplyToId { get; set; }

        [StringLength(50)]
        public string Coordinates { get; set; }

        public DateTime? CreatedOn { get; set; }

        public virtual ICollection<AnnotatedTweet> AnnotatedTweets { get; set; }

        public virtual ICollection<Conversation> Conversations { get; set; }
    }
}
