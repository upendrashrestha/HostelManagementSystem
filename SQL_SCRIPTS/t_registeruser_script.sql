SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_RegisterUser](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NULL,
	[password] [nvarchar](50) NULL,
	[email] [nvarchar](50) NULL,
	[Create_On] [datetime] NULL,
	[staff_id] [varchar](50) foreign key references t_staff(staff_id), 
 CONSTRAINT [PK_t_RegisterUser] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
