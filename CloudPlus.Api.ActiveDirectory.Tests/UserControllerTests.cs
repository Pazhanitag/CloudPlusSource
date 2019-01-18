using System.Web.Http.Results;
using CloudPlus.Api.ActiveDirectory.Controllers.User;
using CloudPlus.Api.ActiveDirectory.Models.User;
using CloudPlus.Api.ActiveDirectory.Utils;
using CloudPlus.PowerShell;
using CloudPlus.Resources;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace CloudPlus.Api.ActiveDirectory.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private IConfigurationManager _configurationManager;
        private IPowerShellManager _powerShellManager;
        private IPowershellScriptLoader _powershellScriptLoader;
        private ISamAccountNameGenerator _samAccountNameGenerator;
        [SetUp]
        public void Init()
        {
            _configurationManager = MockRepository.GenerateMock<IConfigurationManager>();
            _powerShellManager = MockRepository.GenerateMock<IPowerShellManager>();
            _powershellScriptLoader = MockRepository.GenerateMock<IPowershellScriptLoader>();
            _samAccountNameGenerator = MockRepository.GenerateMock<ISamAccountNameGenerator>();
        }
        [Test]
        public void Should_return_generated_samaccountname_in_response()
        {
            const string samAccountName = "sam000_account";

            _powershellScriptLoader.Stub(x => x.LoadScript(PowershellScripts.CreateUser))
                .Return(PowershellScripts.CreateUser);
            _powerShellManager.Stub(x => x.AddParameter(Arg<string>.Is.Anything, Arg<object>.Is.Anything))
                .Return(_powerShellManager);
            _powerShellManager.Stub(x => x.ExecuteScript(PowershellScripts.CreateUser));

            _samAccountNameGenerator.Stub(x => x.GenerateSamAccountName(Arg<string>.Is.Anything))
                .Return(samAccountName);

            var userController = new UserController(_configurationManager, _powerShellManager, _powershellScriptLoader,
                _samAccountNameGenerator);

            var actionResult = userController.Post(new CreateUser());

            var response = actionResult as OkNegotiatedContentResult<UserCreated>;
            Assert.NotNull(response);
            response.Content.SamAccountName.Should().BeSameAs(samAccountName);

        }

        [Test]
        public void Should_add_8_parameters_to_be_passed_to_the_script()
        {
            _powershellScriptLoader.Stub(x => x.LoadScript(PowershellScripts.CreateUser))
                .Return(PowershellScripts.CreateUser);
            _powerShellManager.Stub(x => x.AddParameter(Arg<string>.Is.Anything, Arg<object>.Is.Anything))
                .Return(_powerShellManager);
            _powerShellManager.Stub(x => x.ExecuteScript(PowershellScripts.CreateUser));

            _samAccountNameGenerator.Stub(x => x.GenerateSamAccountName(Arg<string>.Is.Anything))
                .Return("");

            var userController = new UserController(_configurationManager, _powerShellManager, _powershellScriptLoader,
                _samAccountNameGenerator);

            userController.Post(new CreateUser());

            _powerShellManager.AssertWasCalled(x => x.AddParameter(Arg<string>.Is.Anything, Arg<object>.Is.Anything),
                opt => opt.Repeat.Times(8));
        }
    }
}
