using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BaikeAsp.Models
{
    public partial class BaikeContext : DbContext
    {
        public BaikeContext()
        {
        }

        public BaikeContext(DbContextOptions<BaikeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BkAdmin> BkAdmin { get; set; }
        public virtual DbSet<BkBarrage> BkBarrage { get; set; }
        public virtual DbSet<BkBrowseHistory> BkBrowseHistory { get; set; }
        public virtual DbSet<BkCollection> BkCollection { get; set; }
        public virtual DbSet<BkComments> BkComments { get; set; }
        public virtual DbSet<BkFavourite> BkFavourite { get; set; }
        public virtual DbSet<BkInteractiveVideo> BkInteractiveVideo { get; set; }
        public virtual DbSet<BkNextVideo> BkNextVideo { get; set; }
        public virtual DbSet<BkUser> BkUser { get; set; }
        public virtual DbSet<BkUserInfo> BkUserInfo { get; set; }
        public virtual DbSet<BkVideo> BkVideo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BkAdmin>(entity =>
            {
                entity.HasKey(e => e.AId)
                    .HasName("PRIMARY");

                entity.ToTable("BK_Admin");

                entity.HasIndex(e => e.Account)
                    .HasName("account_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.AId)
                    .HasColumnName("aID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasColumnName("account")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<BkBarrage>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.VideoId, e.SendTime })
                    .HasName("PRIMARY");

                entity.ToTable("BK_Barrage");

                entity.HasIndex(e => e.VideoId)
                    .HasName("BarVID_idx");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VideoId)
                    .HasColumnName("videoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SendTime)
                    .HasColumnName("sendTime")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.BType)
                    .HasColumnName("bType")
                    .HasColumnType("int(11)")
                    .HasComment("0为飘动弹幕，1为顶部，2为底部");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.VideoTime).HasColumnName("videoTime");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.BkBarrage)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("BarUID");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.BkBarrage)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("BarVID");
            });

            modelBuilder.Entity<BkBrowseHistory>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.WatchVideoId })
                    .HasName("PRIMARY");

                entity.ToTable("BK_BrowseHistory");

                entity.HasIndex(e => e.WatchVideoId)
                    .HasName("BroHisVID_idx");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WatchVideoId)
                    .HasColumnName("watchVideoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.WatchDate)
                    .HasColumnName("watchDate")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.BkBrowseHistory)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("BroHisUID");

                entity.HasOne(d => d.WatchVideo)
                    .WithMany(p => p.BkBrowseHistory)
                    .HasForeignKey(d => d.WatchVideoId)
                    .HasConstraintName("BroHisVID");
            });

            modelBuilder.Entity<BkCollection>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.FavVideoId })
                    .HasName("PRIMARY");

                entity.ToTable("BK_Collection");

                entity.HasIndex(e => e.FavVideoId)
                    .HasName("colVID_idx");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FavVideoId)
                    .HasColumnName("favVideoID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FavVideo)
                    .WithMany(p => p.BkCollection)
                    .HasForeignKey(d => d.FavVideoId)
                    .HasConstraintName("colVID");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.BkCollection)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("colUID");
            });

            modelBuilder.Entity<BkComments>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.InterVideoId, e.SendTime })
                    .HasName("PRIMARY");

                entity.ToTable("BK_Comments");

                entity.HasIndex(e => e.InterVideoId)
                    .HasName("CommVID_idx");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InterVideoId)
                    .HasColumnName("interVideoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SendTime)
                    .HasColumnName("sendTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.InterVideo)
                    .WithMany(p => p.BkComments)
                    .HasForeignKey(d => d.InterVideoId)
                    .HasConstraintName("CommVID");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.BkComments)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("CommUID");
            });

            modelBuilder.Entity<BkFavourite>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.FavUserId })
                    .HasName("PRIMARY");

                entity.ToTable("BK_Favourite");

                entity.HasIndex(e => e.FavUserId)
                    .HasName("favToUID_idx");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FavUserId)
                    .HasColumnName("favUserID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FavUser)
                    .WithMany(p => p.BkFavouriteFavUser)
                    .HasForeignKey(d => d.FavUserId)
                    .HasConstraintName("favToUID");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.BkFavouriteU)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("favUID");
            });

            modelBuilder.Entity<BkInteractiveVideo>(entity =>
            {
                entity.HasKey(e => e.InterVideoId)
                    .HasName("PRIMARY");

                entity.ToTable("BK_InteractiveVideo");

                entity.HasIndex(e => e.InitVideoId)
                    .HasName("initVID_idx");

                entity.HasIndex(e => e.UId)
                    .HasName("uID_idx");

                entity.Property(e => e.InterVideoId)
                    .HasColumnName("interVideoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CollectPoint)
                    .HasColumnName("collectPoint")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasColumnName("icon")
                    .HasColumnType("varchar(100)")
                    .HasComment("可以有一个默认图片")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.InitVideoId)
                    .HasColumnName("initVideoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Introduction)
                    .HasColumnName("introduction")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PlayVolume)
                    .HasColumnName("playVolume")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PraisePoint)
                    .HasColumnName("praisePoint")
                    .HasColumnType("int(11)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'")
                    .HasComment(@"0为封禁
1为编辑中
2为编辑完成");

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UploadTime)
                    .HasColumnName("uploadTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.VideoName)
                    .IsRequired()
                    .HasColumnName("videoName")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.InitVideo)
                    .WithMany(p => p.BkInteractiveVideo)
                    .HasForeignKey(d => d.InitVideoId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("initVID");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.BkInteractiveVideo)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("InterUID");
            });

            modelBuilder.Entity<BkNextVideo>(entity =>
            {
                entity.HasKey(e => new { e.VideoId, e.NextVideoId, e.Choice })
                    .HasName("PRIMARY");

                entity.ToTable("BK_NextVideo");

                entity.HasIndex(e => e.NextVideoId)
                    .HasName("nextVID_idx");

                entity.Property(e => e.VideoId)
                    .HasColumnName("videoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NextVideoId)
                    .HasColumnName("nextVideoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Choice)
                    .HasColumnName("choice")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.NextVideo)
                    .WithMany(p => p.BkNextVideoNextVideo)
                    .HasForeignKey(d => d.NextVideoId)
                    .HasConstraintName("nextVID");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.BkNextVideoVideo)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("nowVID");
            });

            modelBuilder.Entity<BkUser>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.ToTable("BK_User");

                entity.HasIndex(e => e.Account)
                    .HasName("account_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasColumnName("account")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<BkUserInfo>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.ToTable("BK_UserInfo");

                entity.HasIndex(e => e.NickName)
                    .HasName("nickName_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BackgroundIcon)
                    .HasColumnName("backgroundIcon")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasColumnName("icon")
                    .HasColumnType("varchar(200)")
                    .HasComment("用户有一个默认头像")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Introduction)
                    .HasColumnName("introduction")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NickName)
                    .IsRequired()
                    .HasColumnName("nickName")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'")
                    .HasComment(@"0为封禁
1为正常");

                entity.HasOne(d => d.U)
                    .WithOne(p => p.BkUserInfo)
                    .HasForeignKey<BkUserInfo>(d => d.UId)
                    .HasConstraintName("InfoUID");
            });

            modelBuilder.Entity<BkVideo>(entity =>
            {
                entity.HasKey(e => e.VideoId)
                    .HasName("PRIMARY");

                entity.ToTable("BK_Video");

                entity.HasIndex(e => e.InterVideoId)
                    .HasName("interVID_idx");

                entity.Property(e => e.VideoId)
                    .HasColumnName("videoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InterVideoId)
                    .HasColumnName("interVideoID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_bin");

                entity.Property(e => e.VideoUrl)
                    .IsRequired()
                    .HasColumnName("videoURL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.InterVideo)
                    .WithMany(p => p.BkVideo)
                    .HasForeignKey(d => d.InterVideoId)
                    .HasConstraintName("interVID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
