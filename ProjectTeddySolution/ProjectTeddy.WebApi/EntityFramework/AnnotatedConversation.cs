namespace ProjectTeddy.WebApi.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnnotatedConversation")]
    public partial class AnnotatedConversation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ConversationId { get; set; }

        [Required]
        public string Response { get; set; }

        public int ResponseSentenceTypeId { get; set; }

        public double ResponseSentiment { get; set; }

        [Required]
        [StringLength(50)]
        public string AnnotatedBy { get; set; }

        public DateTime AnnotatedOn { get; set; }

        public virtual Conversation Conversation { get; set; }

        public virtual SentenceType SentenceType { get; set; }
    }
}
