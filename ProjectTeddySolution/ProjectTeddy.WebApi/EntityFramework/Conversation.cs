namespace ProjectTeddy.WebApi.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Conversation
    {
        public Conversation()
        {
            AnnotatedConversations = new HashSet<AnnotatedConversation>();
            Tweets = new HashSet<Tweet>();
        }

        public int Id { get; set; }

        public virtual ICollection<AnnotatedConversation> AnnotatedConversations { get; set; }

        public virtual ICollection<Tweet> Tweets { get; set; }
    }
}
