using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;

namespace PostSample.Models
{
    public class PostContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        //public DbSet<Profile> Profiles { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Post> Posts { get; set; }
        //PostType
        public DbSet<PostType> PostTypes { get; set; }

        public DbSet<PostComment> PostComments { get; set; }

        //public DbSet<CommentComment> CommentComments { get; set; }
        //public DbSet<UpdatePost> UpdatePosts { get; set; }
        //public DbSet<QuestionPost> QuestionPosts { get; set; }

        public DbSet<EventPost> EventPosts { get; set; }
        public DbSet<EventAttendPerson> EventAttendPersons { get; set; }
        //EventAttendType
        public DbSet<EventAttendType> EventAttendTypes { get; set; }

        public DbSet<PollPost> PollPosts { get; set; }
        public DbSet<PollAnswers> PollAnswers { get; set; }

        public DbSet<LikeStatus> LikeStatues { get; set; }

        //for self ref
        //public DbSet<FeedPost> FeedPosts { get; set; }
        //public DbSet<FeedPostComment> FeedPostComments { get; set; }

        public PostContext()
        {
            try
            {
                Database.SetInitializer<PostContext>(new PostContextInitializer());
                var testdb = this.Users.ToList();
            }
            catch (Exception exp)
            {
                string dbStr = exp.Message.ToString();
            }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //many to many relationship mapping
            modelBuilder.Entity<User>()
           .HasMany(u => u.Roles)
           .WithMany(r => r.Users)
           .Map(m =>
           {
               m.ToTable("RoleMemberships");
               m.MapLeftKey("UserName");
               m.MapRightKey("RoleName");
           });

            //modelBuilder.Entity<Profile>()
            //.HasRequired(x => x.User)
            //.WithRequiredDependent(x => x.Profile);

            //modelBuilder.Entity<User>()
            //.HasRequired(x => x.Profile)
            //.WithRequiredDependent(x => x.User);

            //one to one relationship with user mapping
            //modelBuilder.Entity<User>()
            //.HasOptional(u => u.Profile)
            //.WithMany()
            //.HasForeignKey(u => u.ProfileId);

            //one to one relationship with profile mapping
            //modelBuilder.Entity<Profile>()
            //.HasRequired(u => u.User)
            //.WithMany()
            //.HasForeignKey(u => u.UserName);

            //one to one relationship with updatepost
            //modelBuilder.Entity<UpdatePost>()
            //.HasRequired(p => p.Post)
            //.WithMany()
            //.HasForeignKey(p => p.PostId);

            //one to one relationship with QuestionPost
            //modelBuilder.Entity<QuestionPost>()
            //.HasRequired(p => p.Post)
            //.WithMany()
            //.HasForeignKey(p => p.PostId);

            //one to one relationship with EventPost
            modelBuilder.Entity<EventPost>()
            .HasRequired(p => p.Post)
            .WithMany()
            .HasForeignKey(p => p.PostId);

            //one to one relationship with PollPost
            modelBuilder.Entity<PollPost>()
            .HasRequired(p => p.Post)
            .WithMany()
            .HasForeignKey(p => p.PostId);

            //many to many relationship mapping
            modelBuilder.Entity<User>()
           .HasMany(p => p.PollAnsweres)
           .WithMany(u => u.Users)
           .Map(m =>
           {
               m.ToTable("PollAnswerUser");
               m.MapLeftKey("UserId");
               m.MapRightKey("PollAnswerId");
           });

            //many to many relationship mapping
            modelBuilder.Entity<Post>()
           .HasMany(p => p.LikeStatues)
           .WithMany(l => l.Posts)
           .Map(m =>
           {
               m.ToTable("PostLikeStatus");
               m.MapLeftKey("PostId");
               m.MapRightKey("LikeStatusId");
           });

            //many to many relationship mapping
            modelBuilder.Entity<PostComment>()
           .HasMany(p => p.LikeStatues)
           .WithMany(l => l.Comments)
           .Map(m =>
           {
               m.ToTable("PostCommentLikeStatus");
               m.MapLeftKey("CommentId");
               m.MapRightKey("LikeStatusId");
           });

            //many to many relationship mapping
            modelBuilder.Entity<LikeStatus>()
           .HasMany(l => l.Users)
           .WithMany(u => u.LikeStatuses)
           .Map(m =>
           {
               m.ToTable("UserLikeStatus");
               m.MapLeftKey("LikeStatusId");
               m.MapRightKey("UserId");
           });

           // //many to many relationship mapping
           // modelBuilder.Entity<EventAttendPerson>()
           //.HasMany(e => e.Users)
           //.WithMany(u => u.EventAttendPersons)
           //.Map(m =>
           //{
           //    m.ToTable("UserEventAttendPersons");
           //    m.MapLeftKey("EventAttendPersonId");
           //    m.MapRightKey("UserId");
           //});

            base.OnModelCreating(modelBuilder);
        }

    }

    public class PostContextInitializer : DropCreateDatabaseIfModelChanges<PostContext>
    {

        //public PostContextInitializer()
        //{
        //    //TODO HERE
        //    try
        //    {
        //        PostContext postContext = new PostContext();
        //        var testdb = postContext.Users.ToList();
        //    }
        //    catch (Exception exp)
        //    {
        //        string dbStr = exp.Message.ToString();
        //    }
        //}

        protected override void Seed(PostContext context)
        {
            //TODO HERE
            var roles = new List<Role>{
                new Role{RoleName = "Admin"},
                new Role{RoleName = "User"}               
            };

            // adding default roles
            roles.ForEach(r => context.Roles.Add(r));

            ////creating new user
            //string userName = "rasel";
            //string email = "rasel.ahmmed@regfire.com";

            //var user = new User { UserName = userName, Email = email };

            //MembershipCreateStatus createStatus;

            //string npassword = "@123456";
            //Membership.CreateUser(userName, npassword, email, null, null, true, null, out createStatus);

            //var profile = new Profile { FirstName = "Rasel", LastName = "Ahmmed", SurName = "Bappi", Address = "Dhaka", DateOfBirth = new DateTime(1985, 12, 01), MobileNumber = "012346", PhoneNumber = "012346", User = user };

            //context.Profiles.Add(profile);

            //post types
            var posttypes = new List<PostType>{
                new PostType{PostTypeName = "an update"},
                new PostType{PostTypeName = "a question"},
                new PostType{PostTypeName = "an event"} ,
                new PostType{PostTypeName = "a poll"}                
            };

            // adding default post types
            posttypes.ForEach(p => context.PostTypes.Add(p));

            //post event attend types
            var eventattends = new List<EventAttendType>{
                new EventAttendType{AttendTypeName = "Yes, I'm attending"},
                new EventAttendType{AttendTypeName = "Maybe, I might attend"},
                new EventAttendType{AttendTypeName = "No, I won't attend"}      
            };


            // adding default post event attend types
            eventattends.ForEach(e => context.EventAttendTypes.Add(e));

            //test parent child relation
            //var feed = new FeedPost { Content = "feed test", CreateDate = DateTime.Now };

            //FeedPostComment fdComment = new FeedPostComment();
            //fdComment.Content = "Comment 1";
            //fdComment.CreateDate = DateTime.Now;
            //fdComment.UserId = 1;
            //fdComment.FeedPost = feed;


            //FeedPostComment fdChild = new FeedPostComment();
            //fdChild.Content = "Comment 2";
            //fdChild.CreateDate = DateTime.Now;
            //fdComment.UserId = 2;
            //fdChild.ParentId = fdComment.CommentId;
            //fdChild.Parent = fdComment;
            //fdChild.Feed = feed;
            //List<FeedPostComment> lstFeedPostCommentChild = new List<FeedPostComment>();
            //lstFeedPostCommentChild.Add(fdChild);

            //fdComment.FeedPostCommentChilds = lstFeedPostCommentChild;



            //List<FeedPostComment> lstFeedComment = new List<FeedPostComment>();
            //lstFeedComment.Add(fdComment);
            //lstFeedComment.Add(fdChild);
            //feed.FeedPostComment = lstFeedComment;
            //context.FeedPosts.Add(feed);

            context.SaveChanges();
        }
    }


}