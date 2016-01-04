namespace ProjectTeddy.Core.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AnnotatorModel : DbContext
    {
        public AnnotatorModel()
            : base("name=AnnotatorModel")
        {
        }

        public virtual DbSet<AnnotatedConversation> AnnotatedConversations { get; set; }
        public virtual DbSet<AnnotatedTweet> AnnotatedTweets { get; set; }
        public virtual DbSet<AnnotatedWord> AnnotatedWords { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<PartsOfSpeech> PartsOfSpeeches { get; set; }
        public virtual DbSet<SentenceType> SentenceTypes { get; set; }
        public virtual DbSet<Tweet> Tweets { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<WordRelation> WordRelations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conversation>()
                .HasMany(e => e.AnnotatedConversations)
                .WithRequired(e => e.Conversation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Conversation>()
                .HasMany(e => e.Tweets)
                .WithMany(e => e.Conversations)
                .Map(m => m.ToTable("Junc_ConversationTweet").MapLeftKey("ConversationId").MapRightKey("TweetId"));

            modelBuilder.Entity<PartsOfSpeech>()
                .HasMany(e => e.AnnotatedWords)
                .WithRequired(e => e.PartsOfSpeech)
                .HasForeignKey(e => e.PartOfSpeechId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SentenceType>()
                .HasMany(e => e.AnnotatedConversations)
                .WithRequired(e => e.SentenceType)
                .HasForeignKey(e => e.ResponseSentenceTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SentenceType>()
                .HasMany(e => e.AnnotatedTweets)
                .WithRequired(e => e.SentenceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Word>()
                .HasMany(e => e.AnnotatedWords)
                .WithRequired(e => e.Word)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tweet>()
                .HasMany(e => e.AnnotatedTweets)
                .WithRequired(e => e.Tweet)
                .WillCascadeOnDelete(false);
        }
    }
}
