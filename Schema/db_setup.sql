use Raven_demo
go

print 'checking for created tables...'

if OBJECT_ID('dbo.Projects') is null
begin
	create table Projects (
		ProjectId uniqueidentifier primary key default newid(),
		CreatedDate datetime not null default getutcdate(),
		UpdatedDate datetime null,
		Title varchar(100) not null,
		Info varchar(1000) null,
		ShortCode varchar(5) not null unique,
		ImageLocation varchar(300) not null default '"~/Icons/002-raven-1.png"',
	)
	print 'created Project table'
end

if OBJECT_ID('dbo.Requirements') is null
begin
	create table Requirements (
		RequirementId uniqueidentifier primary key default newid(),
		ProjectId uniqueidentifier not null,
		CreatedDate datetime not null default getutcdate(),
		UpdatedDate datetime null,
		Title varchar(120) not null,
		Info varchar(4000) null,
		VersionIntroduced varchar(120) null,
	)
	print 'created Requirements table'
end

if OBJECT_ID('dbo.TestCases') is null
begin
	create table TestCases (
		TestCaseId uniqueidentifier primary key default newid(),
		ProjectId uniqueidentifier not null,
		RequirementId uniqueidentifier not null,
		CreatedDate datetime not null default getutcdate(),
		UpdatedDate datetime null,
		Title varchar(120) not null,
		Info varchar(4000) null
	)
	print 'created TestCases table'
end

if OBJECT_ID('dbo.TestCaseSteps') is null
begin
	create table TestCaseSteps (
		TestCaseStepId uniqueidentifier primary key default newid(),
		ProjectId uniqueidentifier not null,
		TestCaseId uniqueidentifier not null,
		Step varchar(1000) not null	
	)
	print 'created TestCaseSteps table'
end

print 'checking for created table constraints...'

if object_id('dbo.[FK_ProjectsRequirements]', 'F') is null
begin
	alter table Requirements
		add constraint FK_ProjectsRequirements
		foreign key (ProjectId) references Projects(ProjectId)
	print 'added FK_ProjectsRequirements'
end

if object_id('dbo.[FK_ProjectsTestCase]', 'F') is null
begin
	alter table TestCases
		add constraint FK_ProjectsTestCase
		foreign key (ProjectId) references Projects(ProjectId)
	print 'added FK_ProjectsTestCase'
end

if object_id('dbo.[FK_RequirementsTestCase]', 'F') is null
begin
	alter table TestCases
		add constraint FK_RequirementsTestCase
		foreign key (RequirementId) references Requirements(RequirementId)
	print 'added FK_RequirementsTestCase'
end

if object_id('dbo.[FK_TestCasesStep]', 'F') is null
begin
	alter table TestCaseSteps
		add constraint FK_TestCasesStep
		foreign key (TestCaseId) references TestCases(TestCaseId)
	print 'added FK_TestCasesStep'
end

if object_id('dbo.[FK_ProjectsTestCaseStep]', 'F') is null
begin
	alter table TestCaseSteps
		add constraint FK_ProjectsTestCaseStep
		foreign key (ProjectId) references Projects(ProjectId)
	print 'added FK_ProjectsTestCaseStep'
end

print 'checking constraints...'

alter table Requirements
	with check check constraint FK_ProjectsRequirements

alter table TestCases
	with check check constraint FK_ProjectsTestCase

alter table TestCases
	with check check constraint FK_RequirementsTestCase

alter table TestCaseSteps
	with check check constraint FK_TestCasesStep

alter table TestCaseSteps
	with check check constraint FK_ProjectsTestCaseStep

print 'schema creation complete.'
