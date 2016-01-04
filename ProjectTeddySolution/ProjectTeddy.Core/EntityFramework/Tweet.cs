namespace ProjectTeddy.Core.EntityFramework
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tweet
    {
        public Tweet()
        {
            Conversations = new HashSet<Conversation>();
            AnnotatedTweets = new HashSet<AnnotatedTweet>();
        }

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string ReplyToId { get; set; }

        [StringLength(50)]
        public string Coordinates { get; set; }

        public DateTime? CreatedOn { get; set; }

        public ICollection<Conversation> Conversations { get; set; }
        public ICollection<AnnotatedTweet> AnnotatedTweets { get; set; }
    }
}
