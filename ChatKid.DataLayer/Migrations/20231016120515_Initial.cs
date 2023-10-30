using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatKid.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertising",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    title = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    start_date = table.Column<DateTime>(type: "time with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "time with time zone", nullable: true),
                    image_video_url = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    destination_url = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    clicks = table.Column<short>(type: "smallint", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1),
                    type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Advertising_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    created_at = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Channel_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Otp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    otp = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Otp_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PaymentMethod_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    type = table.Column<string>(type: "character varying", nullable: true),
                    energy = table.Column<short>(type: "smallint", nullable: true),
                    updated_time = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Service_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Subcription",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    actual_price = table.Column<decimal>(type: "numeric", nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    energy = table.Column<short>(type: "smallint", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Subcription_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypeBlog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Type_Blog_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    application_user_id = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    last_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    gmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    age = table.Column<int>(type: "integer", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "time with time zone", nullable: true),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Admin_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_Admin_AspNetUsers_application_user_id",
                        column: x => x.application_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expert",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    application_user_id = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    last_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    gmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    age = table.Column<int>(type: "integer", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "time with time zone", nullable: true),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Expert_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_Expert_AspNetUsers_application_user_id",
                        column: x => x.application_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Family",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    application_user_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    owner_mail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Family_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_Family_AspNetUsers_application_user_id",
                        column: x => x.application_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    content = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    image_url = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    created_at = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    voice_url = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1),
                    type_blog_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Blog_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Blog_Create_Admin_id_fkey",
                        column: x => x.created_by,
                        principalTable: "Admin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Blog_Type_Blog_id_fkey",
                        column: x => x.type_blog_id,
                        principalTable: "TypeBlog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Blog_Update_Admin_id_fkey",
                        column: x => x.updated_by,
                        principalTable: "Admin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    message = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    receiver = table.Column<string>(type: "text", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Notification_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Notification_admin_id_fkey",
                        column: x => x.created_by,
                        principalTable: "Admin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    avatar_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1),
                    family_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.id);
                    table.ForeignKey(
                        name: "User_family_id_fkey",
                        column: x => x.family_id,
                        principalTable: "Family",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Channel_User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Channel_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    User_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name_in_channel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Channel_User_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Channel_User_Channel_id_fkey",
                        column: x => x.Channel_id,
                        principalTable: "Channel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Channel_User_User_id_fkey",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "KidService",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    children_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("KidService_pkey", x => x.id);
                    table.ForeignKey(
                        name: "KidService_children_id_fkey",
                        column: x => x.children_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "KidService_service_id_fkey",
                        column: x => x.service_id,
                        principalTable: "Service",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ParentSubcription",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    subcription_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ParentSubcription_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ParentSubcription_parent_id_fkey",
                        column: x => x.parent_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "ParentSubcription_subcription_id_fkey",
                        column: x => x.subcription_id,
                        principalTable: "Subcription",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    total_energy = table.Column<short>(type: "smallint", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1),
                    updated_time = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Wallet_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Wallet_owner_id_fkey",
                        column: x => x.owner_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    content = table.Column<string>(type: "text", nullable: true),
                    image_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    voice_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    sent_time = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    channel_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Message_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Message_channel_user_id_fkey",
                        column: x => x.channel_user_id,
                        principalTable: "Channel_User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DiscussRoom",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    kid_service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expert_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_time = table.Column<string>(type: "character varying", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DiscussRoom_pkey", x => x.id);
                    table.ForeignKey(
                        name: "DiscussRoom_expert_id_fkey",
                        column: x => x.expert_id,
                        principalTable: "Expert",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "DiscussRoom_kid_service_id_fkey",
                        column: x => x.kid_service_id,
                        principalTable: "KidService",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    content = table.Column<string>(type: "text", nullable: true),
                    voice_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    image_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_time = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_children = table.Column<bool>(type: "boolean", nullable: true),
                    kid_service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "Question_kid_service_id_fkey",
                        column: x => x.kid_service_id,
                        principalTable: "KidService",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MoneyPayment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    parent_subcription_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_id = table.Column<Guid>(type: "uuid", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    created_time = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1),
                    method_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MoneyPayment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "MoneyPayment_method_id_fkey",
                        column: x => x.method_id,
                        principalTable: "PaymentMethod",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "MoneyPayment_parent_subcription_id_fkey",
                        column: x => x.parent_subcription_id,
                        principalTable: "ParentSubcription",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "QA",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    content = table.Column<string>(type: "text", nullable: true),
                    voice_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    image_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_time = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    discuss_room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_children = table.Column<bool>(type: "boolean", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("QA_pkey", x => x.id);
                    table.ForeignKey(
                        name: "QA_discuss_room_id_fkey",
                        column: x => x.discuss_room_id,
                        principalTable: "DiscussRoom",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    energy = table.Column<short>(type: "smallint", nullable: true),
                    type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    created_time = table.Column<DateTime>(type: "time with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    money_payment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    wallet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    kid_service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Transaction_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Transaction_kid_service_id_fkey",
                        column: x => x.kid_service_id,
                        principalTable: "KidService",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Transaction_money_payment_id_fkey",
                        column: x => x.money_payment_id,
                        principalTable: "MoneyPayment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Transaction_wallet_id_fkey",
                        column: x => x.wallet_id,
                        principalTable: "Wallet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "Admin_gmail_key",
                table: "Admin",
                column: "gmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Admin_phone_key",
                table: "Admin",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admin_application_user_id",
                table: "Admin",
                column: "application_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blog_created_by",
                table: "Blog",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_type_blog_id",
                table: "Blog",
                column: "type_blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_updated_by",
                table: "Blog",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_User_Channel_id",
                table: "Channel_User",
                column: "Channel_id");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_User_User_id",
                table: "Channel_User",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussRoom_expert_id",
                table: "DiscussRoom",
                column: "expert_id");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussRoom_kid_service_id",
                table: "DiscussRoom",
                column: "kid_service_id");

            migrationBuilder.CreateIndex(
                name: "Expert_gmail_key",
                table: "Expert",
                column: "gmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Expert_phone_key",
                table: "Expert",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expert_application_user_id",
                table: "Expert",
                column: "application_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Family_application_user_id",
                table: "Family",
                column: "application_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KidService_children_id",
                table: "KidService",
                column: "children_id");

            migrationBuilder.CreateIndex(
                name: "IX_KidService_service_id",
                table: "KidService",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Message_channel_user_id",
                table: "Message",
                column: "channel_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyPayment_method_id",
                table: "MoneyPayment",
                column: "method_id");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyPayment_parent_subcription_id",
                table: "MoneyPayment",
                column: "parent_subcription_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_created_by",
                table: "Notification",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ParentSubcription_parent_id",
                table: "ParentSubcription",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_ParentSubcription_subcription_id",
                table: "ParentSubcription",
                column: "subcription_id");

            migrationBuilder.CreateIndex(
                name: "IX_QA_discuss_room_id",
                table: "QA",
                column: "discuss_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Question_kid_service_id",
                table: "Question",
                column: "kid_service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_kid_service_id",
                table: "Transaction",
                column: "kid_service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_money_payment_id",
                table: "Transaction",
                column: "money_payment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_wallet_id",
                table: "Transaction",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_family_id",
                table: "User",
                column: "family_id");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_owner_id",
                table: "Wallet",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertising");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Otp");

            migrationBuilder.DropTable(
                name: "QA");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TypeBlog");

            migrationBuilder.DropTable(
                name: "Channel_User");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "DiscussRoom");

            migrationBuilder.DropTable(
                name: "MoneyPayment");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropTable(
                name: "Expert");

            migrationBuilder.DropTable(
                name: "KidService");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "ParentSubcription");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Subcription");

            migrationBuilder.DropTable(
                name: "Family");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
