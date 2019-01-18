using System;
using System.Collections.Generic;
using CloudPlus.AppServices.User.Consumers;
using CloudPlus.AppServices.User.Workflow.CreateUser;
using CloudPlus.Enums.User;
using CloudPlus.Models.Identity;
using CloudPlus.QueueModels.Users.Commands;
using CloudPlus.Services.Database.WorkflowActivity;
using CloudPlus.Services.Identity.User;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;
using MassTransit.TestFramework;
using Moq;
using NUnit.Framework;

namespace CloudPlus.AppServices.User.Tests.Consmers
{
    [TestFixture]
    public class CreateUserConsumerTests
    {
        private Mock<IWorkflow<ConsumeContext<ICreateUserCommand>>> _createUserWorkflowBuilderMock;
        private Mock<IWorkflowUserActivityService> _workflowActivityServiceMock;
        private Mock<IUserService> _userService;
        [SetUp]
        public void Init()
        {
            _createUserWorkflowBuilderMock = new Mock<IWorkflow<ConsumeContext<ICreateUserCommand>>>();
            _workflowActivityServiceMock = new Mock<IWorkflowUserActivityService>();
            _userService = new Mock<IUserService>();
        }

        [TearDown]
        public void TearDown()
        {
            _createUserWorkflowBuilderMock = null;
            _workflowActivityServiceMock = null;
            _userService = null;
        }

        [Test]
        public void When_email_is_null_or_empty_should_throw_argument_exception()
        {
            var createUserConsumer = CreateCreateUserConsumerInstance();

            Assert.ThrowsAsync<ArgumentException>(() =>
                createUserConsumer.Consume(new TestConsumeContext<ICreateUserCommand>(new CreateUserCommandTest())));
        }

        [Test]
        public void When_user_exists_in_db_should_throw_exception()
        {
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new UserModel());

            var createUserConsumer = CreateCreateUserConsumerInstance();

            Assert.ThrowsAsync<Exception>(() =>
                createUserConsumer.Consume(new TestConsumeContext<ICreateUserCommand>(new CreateUserCommandTest
                {
                    Email = "test@test.test"
                })), "User already exists");

        }

        [Test]
        public void When_user_is_already_being_created_should_throw_exception()
        {
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _workflowActivityServiceMock.Setup(x => x.IsUserBeingCreated(It.IsAny<string>())).Returns(true);
            var createUserConsumer = CreateCreateUserConsumerInstance();

            Assert.ThrowsAsync<Exception>(() =>
                createUserConsumer.Consume(new TestConsumeContext<ICreateUserCommand>(new CreateUserCommandTest
                {
                    Email = "test@test.test"
                })), "Create user already started");

        }

        [Test]
        public void When_all_check_passes_should_call_Executeworkflow()
        {
            _userService.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _workflowActivityServiceMock.Setup(x => x.IsUserBeingCreated(It.IsAny<string>())).Returns(false);
         
            var createUserConsumer = CreateCreateUserConsumerInstance();
            var consumeContext = new TestConsumeContext<ICreateUserCommand>(new CreateUserCommandTest
            {
                Email = "test@test.test"
            });

            createUserConsumer.Consume(consumeContext);

            _createUserWorkflowBuilderMock.Verify(x => x.Execute(consumeContext));

        }

        public ICreateUserConsumer CreateCreateUserConsumerInstance()
        {
            return new CreateUserConsumer(_createUserWorkflowBuilderMock.Object as CreateUserWorkflow, _workflowActivityServiceMock.Object, _userService.Object);
        }
    }

    public class CreateUserCommandTest : ICreateUserCommand
    {
        public string UniqueId { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public DateTime Created { get; set; }
        public DateTime Completed { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string AlternativeEmail { get; set; }
        public string Department { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string ProfilePicture { get; set; }
        public string Password { get; set; }
        public PasswordSetupMethod PasswordSetupMethod { get; set; }
        public string PasswordSetupEmail { get; set; }
        public UserStatus UserStatus { get; set; }
        public List<int> Roles { get; set; }
        public string CompanyDomain { get; set; }
        public int CustomerLegacyId { get; set; }
        public int CustomerId { get; set; }
        public bool SendPlainPasswordViaEmail { get; set; }
    }
}
