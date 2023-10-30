using System;
using System.Collections.Generic;
using ChatKid.DataLayer.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatKid.DataLayer.Entities;

public class KidtalkieContext : IdentityDbContext<ApplicationUser>, IDBContext
{

    public KidtalkieContext() { }

    public KidtalkieContext(DbContextOptions<KidtalkieContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public virtual DbSet<Advertising> Advertisings { get; set; }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<ChannelUser> ChannelUsers { get; set; }

    public virtual DbSet<DiscussRoom> DiscussRooms { get; set; }

    public virtual DbSet<Expert> Experts { get; set; }

    public virtual DbSet<Family> Families { get; set; }

    public virtual DbSet<KidService> KidServices { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MoneyPayment> MoneyPayments { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<ParentSubcription> ParentSubcriptions { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subcription> Subcriptions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<TypeBlog> TypeBlogs { get; set; }

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("server=35.213.131.71;port=5432;database=kidtalkie;uid=kidtalkie;password=Kidtalkie@2023;TrustServerCertificate=True; Include Error Detail=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Admin_pkey");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Gmail, "Admin_gmail_key").IsUnique();

            entity.HasIndex(e => e.Phone, "Admin_phone_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Gmail)
                .HasMaxLength(50)
                .HasColumnName("gmail");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
            entity.Property(e => e.ApplicationUserId)
                .HasColumnName("application_user_id");
            entity.Property(e => e.AvatarUrl)
                .HasColumnName("avatar_url");
        });

        modelBuilder.Entity<Advertising>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Advertising_pkey");

            entity.ToTable("Advertising");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Clicks).HasColumnName("clicks").HasDefaultValue((short)0);
            entity.Property(e => e.Title)
                .HasColumnName("title");
            entity.Property(e => e.Content)
                .HasColumnName("content");
            entity.Property(e => e.Company)
                    .HasColumnName("company");
            entity.Property(e => e.CompanyEmail)
                .HasColumnName("company_email");
            entity.Property(e => e.Price)
                .HasColumnName("price");
            entity.Property(e => e.DestinationUrl)
                .HasMaxLength(100)
                .HasColumnName("destination_url");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasColumnName("image_video_url");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
            entity.Property(e => e.Type)
                .HasColumnName("type");
            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");
            entity.HasOne(d => d.CreateAdmin).WithMany(p => p.Advertisings)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Advertising_admin_id_fkey");
        });

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Channel_pkey");
            entity.ToTable("Channel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);


            // Define the one-to-many relationship with Messages and enable cascade delete
            entity.HasMany(e => e.ChannelUsers)
                    .WithOne(e => e.Channel)
                    .HasForeignKey(e => e.ChannelId)
                    .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ChannelUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Channel_User_pkey");

            entity.ToTable("Channel_User");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ChannelId)
                .HasColumnName("Channel_id");
            entity.Property(e => e.NameInChannel)
                .HasMaxLength(50)
                .HasColumnName("name_in_channel");
            entity.Property(e => e.UserId)
                .HasColumnName("User_id");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

            entity.HasOne(d => d.Channel).WithMany(p => p.ChannelUsers)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Channel_User_Channel_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ChannelUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Channel_User_User_id_fkey");

        });

        modelBuilder.Entity<DiscussRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DiscussRoom_pkey");

            entity.ToTable("DiscussRoom");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_time");
            entity.Property(e => e.ExpertId).HasColumnName("expert_id");
            entity.Property(e => e.KidServiceId).HasColumnName("kid_service_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
            entity.Property(e => e.VoiceUrl)
                .HasColumnName("voice_url");

            entity.HasOne(d => d.Expert).WithMany(p => p.DiscussRooms)
                .HasForeignKey(d => d.ExpertId)
                .HasConstraintName("DiscussRoom_expert_id_fkey");

            entity.HasOne(d => d.KidService).WithMany(p => p.DiscussRooms)
                .HasForeignKey(d => d.KidServiceId)
                .HasConstraintName("DiscussRoom_kid_service_id_fkey");
        });

        modelBuilder.Entity<Expert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Expert_pkey");

            entity.ToTable("Expert");

            entity.HasIndex(e => e.Gmail, "Expert_gmail_key").IsUnique();

            entity.HasIndex(e => e.Phone, "Expert_phone_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Gmail)
                .HasMaxLength(50)
                .HasColumnName("gmail");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
            entity.Property(e => e.ApplicationUserId)
                .HasColumnName("application_user_id");
            entity.Property(e => e.AvatarUrl)
                .HasColumnName("avatar_url");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            
        });

        modelBuilder.Entity<Family>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Family_pkey");

            entity.ToTable("Family");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.OwnerMail)
                .HasMaxLength(50)
                .HasColumnName("owner_mail");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
            entity.Property(e => e.ApplicationUserId)
                .HasColumnName("application_user_id");

            entity.HasMany(e => e.Users)
                    .WithOne(e => e.Family)
                    .HasForeignKey(e => e.FamilyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Family_AspNetUsers_application_user_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(200)
                .HasColumnName("avatar_url");
            entity.Property(e => e.DeviceToken)
                .HasMaxLength(200)
                .HasColumnName("device_token");
            entity.Property(e => e.FamilyId).HasColumnName("family_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
            entity.Property(e => e.Gender)
                .HasColumnName("gender");
            entity.Property(e => e.IsUpdated)
                .HasColumnName("updated")
                .HasDefaultValue((short)0);

            entity.HasOne(d => d.Family).WithMany(p => p.Users)
                .HasForeignKey(d => d.FamilyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("User_family_id_fkey");
            entity.HasMany(d => d.Wallets).WithOne(p => p.Owner)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(entity => entity.Transactions).WithOne(entity => entity.CreateBy)
                .HasForeignKey(entity => entity.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

        });

        modelBuilder.Entity<KidService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("KidService_pkey");

            entity.ToTable("KidService");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ChildrenId).HasColumnName("children_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

            entity.HasOne(d => d.Children).WithMany(p => p.KidServices)
                .HasForeignKey(d => d.ChildrenId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("KidService_children_id_fkey");

            entity.HasOne(d => d.Service).WithMany(p => p.KidServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("KidService_service_id_fkey");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Message_pkey");

            entity.ToTable("Message");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ChannelUserId).HasColumnName("channel_user_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_url");
            entity.Property(e => e.SentTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sent_time");
            entity.Property(e => e.VoiceUrl)
                .HasMaxLength(200)
                .HasColumnName("voice_url");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

            entity.HasOne(d => d.ChannelUser).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChannelUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Message_channel_user_id_fkey");
        });

        modelBuilder.Entity<MoneyPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MoneyPayment_pkey");

            entity.ToTable("MoneyPayment");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_time");
            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.MethodId).HasColumnName("method_id");
            entity.Property(e => e.ParentSubcriptionId).HasColumnName("parent_subcription_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

            entity.HasOne(d => d.Method).WithMany(p => p.MoneyPayments)
                .HasForeignKey(d => d.MethodId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("MoneyPayment_method_id_fkey");

            entity.HasOne(d => d.ParentSubcription).WithMany(p => p.MoneyPayments)
                .HasForeignKey(d => d.ParentSubcriptionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("MoneyPayment_parent_subcription_id_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notification_pkey");

            entity.ToTable("Notification");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreateAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Content)
                .HasColumnName("content");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
            entity.Property(e => e.UpdateAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedBy).
            HasColumnName("created_by");

            entity.Property(e => e.Receiver)
            .HasColumnName("receiver");

            entity.HasOne(d => d.CreateAdmin)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Notification_admin_id_fkey");
        });

        modelBuilder.Entity<ParentSubcription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ParentSubcription_pkey");

            entity.ToTable("ParentSubcription");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.FamilyId).HasColumnName("family_id");
            entity.Property(e => e.SubcriptionId).HasColumnName("subcription_id");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

            entity.HasOne(d => d.Family).WithMany(p => p.ParentSubcriptions)
                .HasForeignKey(d => d.FamilyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ParentSubcription_family_id_fkey");

            entity.HasOne(d => d.Subcription).WithMany(p => p.ParentSubcriptions)
                .HasForeignKey(d => d.SubcriptionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ParentSubcription_subcription_id_fkey");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PaymentMethod_pkey");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Question");

            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_time");
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url");
            entity.Property(e => e.Answer)
                .HasColumnName("answer");
            entity.Property(e => e.KidServiceId).HasColumnName("kid_service_id");
            entity.Property(e => e.VoiceUrl)
                .HasColumnName("voice_url");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

            entity.HasOne(d => d.KidService).WithMany()
                .HasForeignKey(d => d.KidServiceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Question_kid_service_id_fkey");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Service_pkey");

            entity.ToTable("Service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Energy).HasColumnName("energy");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_time");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

        });

        modelBuilder.Entity<Subcription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Subcription_pkey");

            entity.ToTable("Subcription");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ActualPrice).HasColumnName("actual_price");
            entity.Property(e => e.Energy).HasColumnName("energy");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Transaction_pkey");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_time");
            entity.Property(e => e.Energy).HasColumnName("energy");
            entity.Property(e => e.KidServiceId).HasColumnName("kid_service_id");
            entity.Property(e => e.MoneyPaymentId).HasColumnName("money_payment_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .HasColumnName("type");
            entity.Property(e => e.WalletId).HasColumnName("wallet_id");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);
            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

            entity.HasOne(d => d.KidService).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.KidServiceId)
                .HasConstraintName("Transaction_kid_service_id_fkey");

            entity.HasOne(d => d.MoneyPayment).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.MoneyPaymentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Transaction_money_payment_id_fkey");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Transaction_wallet_id_fkey");

            entity.HasOne(d => d.CreateBy).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Transaction_user_id_fkey");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Wallet_pkey");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalEnergy).HasColumnName("total_energy");
            entity.Property(e => e.UpdatedTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_time");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

            entity.HasOne(d => d.Owner).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Wallet_owner_id_fkey");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Blog_pkey");
            entity.ToTable("Blog");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("title");

            entity.Property(e => e.Content)
                .HasMaxLength(30)
                .HasColumnName("content");

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(30)
                .HasColumnName("image_url");

            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

            entity.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by");

            entity.Property(e => e.VoiceUrl)
                .HasMaxLength(30)
                .HasColumnName("voice_url");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)1);

            entity.Property(e => e.TypeBlogId)
                .HasColumnName("type_blog_id");


            entity.HasOne(e => e.TypeBlog)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.TypeBlogId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Blog_Type_Blog_id_fkey");

            entity.HasOne(e => e.CreateAdmin)
                    .WithMany(p => p.CreatedBlogs)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Blog_Create_Admin_id_fkey");

            entity.HasOne(e => e.UpdateAdmin)
                    .WithMany(p => p.UpdatedBlogs)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Blog_Update_Admin_id_fkey");
        });

        modelBuilder.Entity<TypeBlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Type_Blog_pkey");

            entity.ToTable("TypeBlog");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url");

        });

        base.OnModelCreating(modelBuilder);


        //OnModelCreatingPartial(modelBuilder);
    }

    /*    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    */
}