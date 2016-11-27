
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/26/2016 19:59:37
-- Generated from EDMX file: C:\Users\jstelmach\Documents\GitHub\SNCRegistration\SNCRegistration\SNCRegistration\ViewModels\SNCRegistration.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SNCRegistration];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetRoles_RoleId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetRoles_RoleId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_FamilyMembers_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FamilyMembers] DROP CONSTRAINT [FK_FamilyMembers_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_Participants_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Participants] DROP CONSTRAINT [FK_Participants_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_Volunteers_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Volunteers] DROP CONSTRAINT [FK_Volunteers_ToTable];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[Admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Admin];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FamilyMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FamilyMembers];
GO
IF OBJECT_ID(N'[dbo].[Guardians]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Guardians];
GO
IF OBJECT_ID(N'[dbo].[LeadContacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LeadContacts];
GO
IF OBJECT_ID(N'[dbo].[Participants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Participants];
GO
IF OBJECT_ID(N'[dbo].[Volunteers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Volunteers];
GO
IF OBJECT_ID(N'[SNCRegistrationModelStoreContainer].[database_firewall_rules]', 'U') IS NOT NULL
    DROP TABLE [SNCRegistrationModelStoreContainer].[database_firewall_rules];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'FamilyMembers'
CREATE TABLE [dbo].[FamilyMembers] (
    [FamilyMemberID] int  NOT NULL,
    [FamilyMemberFirstName] nvarchar(50)  NOT NULL,
    [FamilyMemberLastName] nvarchar(50)  NOT NULL,
    [GuardianID] int  NOT NULL,
    [HealthForm] bit  NULL,
    [PhotoAck] bit  NULL,
    [AttendingCode] nvarchar(50)  NOT NULL,
    [Comments] nvarchar(50)  NULL,
    [FamilyMemberAge] int  NULL
);
GO

-- Creating table 'Guardians'
CREATE TABLE [dbo].[Guardians] (
    [GuardianID] int  NOT NULL,
    [GuardianFirstName] nvarchar(50)  NOT NULL,
    [GuardianLastName] nvarchar(50)  NOT NULL,
    [GuardianAddress] nvarchar(50)  NULL,
    [GuardianCity] nvarchar(50)  NOT NULL,
    [GuardianZip] int  NULL,
    [GuardianEmail] nvarchar(50)  NULL,
    [PacketSentDate] datetime  NULL,
    [ReceiptDate] datetime  NULL,
    [ConfirmationSentDate] datetime  NULL,
    [HealthForm] bit  NULL,
    [PhotoAck] bit  NULL,
    [Tent] bit  NULL,
    [AttendingCode] nvarchar(50)  NULL,
    [Comments] nvarchar(50)  NULL,
    [Relationship] nvarchar(50)  NULL,
    [GuardianCellPhone] nchar(10)  NULL
);
GO

-- Creating table 'LeadContacts'
CREATE TABLE [dbo].[LeadContacts] (
    [LeadContactID] int  NOT NULL,
    [BSType] nvarchar(50)  NOT NULL,
    [UnitChapterNumber] nchar(10)  NOT NULL,
    [LeadContactFirstName] nvarchar(50)  NOT NULL,
    [LeadContactLastName] nvarchar(50)  NOT NULL,
    [LeadContactAddress] nvarchar(50)  NOT NULL,
    [LeadContactCity] nvarchar(50)  NOT NULL,
    [LeadContactState] char(10)  NOT NULL,
    [LeadContactZip] nchar(10)  NOT NULL,
    [LeadContactEmail] nvarchar(50)  NOT NULL,
    [VolunteerAttendingCode] nvarchar(50)  NOT NULL,
    [SaturdayDinner] bit  NOT NULL,
    [TotalFee] decimal(18,0)  NULL,
    [Booth] nvarchar(50)  NOT NULL,
    [Comments] nvarchar(50)  NULL,
    [LeadContactShirtOrder] bit  NOT NULL,
    [LeadContactShirtSize] char(10)  NOT NULL,
    [LeadContactCellPhone] nchar(10)  NOT NULL
);
GO

-- Creating table 'Participants'
CREATE TABLE [dbo].[Participants] (
    [ParticipantID] int  NOT NULL,
    [ParticipantFirstName] nvarchar(50)  NOT NULL,
    [ParticipantLastName] nvarchar(50)  NOT NULL,
    [ParticipantAge] int  NOT NULL,
    [ParticipantSchool] nvarchar(50)  NOT NULL,
    [ParticipantTeacher] nchar(10)  NOT NULL,
    [ClassroomScouting] bit  NOT NULL,
    [HealthForm] bit  NULL,
    [PhotoAck] bit  NULL,
    [AttendingCode] nvarchar(50)  NOT NULL,
    [GuardianID] int  NULL,
    [Comments] nvarchar(50)  NULL,
    [Returning] bit  NULL
);
GO

-- Creating table 'Volunteers'
CREATE TABLE [dbo].[Volunteers] (
    [VolunteerID] int  NOT NULL,
    [VolunteerFirstName] nvarchar(50)  NOT NULL,
    [VolunteerLastName] nvarchar(50)  NOT NULL,
    [VolunteerAge] int  NOT NULL,
    [VolunteerShirtOrder] bit  NOT NULL,
    [VolunteerShirtSize] char(10)  NOT NULL,
    [VolunteerAttendingCode] nvarchar(50)  NOT NULL,
    [SaturdayDinner] bit  NOT NULL,
    [UnitChapterNumber] nchar(10)  NOT NULL,
    [Comments] nvarchar(50)  NULL,
    [LeadContactID] int  NOT NULL
);
GO

-- Creating table 'database_firewall_rules'
CREATE TABLE [dbo].[database_firewall_rules] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(128)  NOT NULL,
    [start_ip_address] varchar(45)  NOT NULL,
    [end_ip_address] varchar(45)  NOT NULL,
    [create_date] datetime  NOT NULL,
    [modify_date] datetime  NOT NULL
);
GO

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'Admins'
CREATE TABLE [dbo].[Admins] (
    [AdminID] nvarchar(50)  NOT NULL,
    [AdminUserName] nvarchar(50)  NOT NULL,
    [AdminFirstName] nvarchar(50)  NOT NULL,
    [AdminLastName] nvarchar(50)  NOT NULL,
    [AdminType] nvarchar(50)  NOT NULL,
    [AdminPWHash] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [FamilyMemberID] in table 'FamilyMembers'
ALTER TABLE [dbo].[FamilyMembers]
ADD CONSTRAINT [PK_FamilyMembers]
    PRIMARY KEY CLUSTERED ([FamilyMemberID] ASC);
GO

-- Creating primary key on [GuardianID] in table 'Guardians'
ALTER TABLE [dbo].[Guardians]
ADD CONSTRAINT [PK_Guardians]
    PRIMARY KEY CLUSTERED ([GuardianID] ASC);
GO

-- Creating primary key on [LeadContactID] in table 'LeadContacts'
ALTER TABLE [dbo].[LeadContacts]
ADD CONSTRAINT [PK_LeadContacts]
    PRIMARY KEY CLUSTERED ([LeadContactID] ASC);
GO

-- Creating primary key on [ParticipantID] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [PK_Participants]
    PRIMARY KEY CLUSTERED ([ParticipantID] ASC);
GO

-- Creating primary key on [VolunteerID] in table 'Volunteers'
ALTER TABLE [dbo].[Volunteers]
ADD CONSTRAINT [PK_Volunteers]
    PRIMARY KEY CLUSTERED ([VolunteerID] ASC);
GO

-- Creating primary key on [id], [name], [start_ip_address], [end_ip_address], [create_date], [modify_date] in table 'database_firewall_rules'
ALTER TABLE [dbo].[database_firewall_rules]
ADD CONSTRAINT [PK_database_firewall_rules]
    PRIMARY KEY CLUSTERED ([id], [name], [start_ip_address], [end_ip_address], [create_date], [modify_date] ASC);
GO

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [AdminID] in table 'Admins'
ALTER TABLE [dbo].[Admins]
ADD CONSTRAINT [PK_Admins]
    PRIMARY KEY CLUSTERED ([AdminID] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [VolunteerID] in table 'Volunteers'
ALTER TABLE [dbo].[Volunteers]
ADD CONSTRAINT [FK_Volunteers_Volunteers]
    FOREIGN KEY ([VolunteerID])
    REFERENCES [dbo].[Volunteers]
        ([VolunteerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRole]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUser]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUser'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUser]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [GuardianID] in table 'FamilyMembers'
ALTER TABLE [dbo].[FamilyMembers]
ADD CONSTRAINT [FK_FamilyMembers_ToTable]
    FOREIGN KEY ([GuardianID])
    REFERENCES [dbo].[Guardians]
        ([GuardianID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FamilyMembers_ToTable'
CREATE INDEX [IX_FK_FamilyMembers_ToTable]
ON [dbo].[FamilyMembers]
    ([GuardianID]);
GO

-- Creating foreign key on [GuardianID] in table 'Participants'
ALTER TABLE [dbo].[Participants]
ADD CONSTRAINT [FK_Participants_ToTable]
    FOREIGN KEY ([GuardianID])
    REFERENCES [dbo].[Guardians]
        ([GuardianID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Participants_ToTable'
CREATE INDEX [IX_FK_Participants_ToTable]
ON [dbo].[Participants]
    ([GuardianID]);
GO

-- Creating foreign key on [LeadContactID] in table 'Volunteers'
ALTER TABLE [dbo].[Volunteers]
ADD CONSTRAINT [FK_Volunteers_ToTable]
    FOREIGN KEY ([LeadContactID])
    REFERENCES [dbo].[LeadContacts]
        ([LeadContactID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Volunteers_ToTable'
CREATE INDEX [IX_FK_Volunteers_ToTable]
ON [dbo].[Volunteers]
    ([LeadContactID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------