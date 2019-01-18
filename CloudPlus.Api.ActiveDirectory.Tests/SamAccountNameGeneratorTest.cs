using System;
using System.Text.RegularExpressions;
using CloudPlus.Api.ActiveDirectory.Utils;
using CloudPlus.PowerShell;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;
namespace CloudPlus.Api.ActiveDirectory.Tests
{
    [TestFixture]
    public class SamAccountNameGeneratorTest
    {
        private IPowerShellManager _powerShellManager;
        private IPowershellScriptLoader _powershellScriptLoader;

        [SetUp]
        public void Init()
        {
            _powerShellManager = MockRepository.GenerateMock<IPowerShellManager>();
            _powershellScriptLoader = MockRepository.GenerateMock<IPowershellScriptLoader>();
        }

        [Test]
        public void Wehn_upn_is_null_or_empty_should_throw_argument_null_exception()
        {
            var samAccountNameGenerator = new SamAccountNameGenerator(_powerShellManager, _powershellScriptLoader);

            Assert.Throws<ArgumentNullException>(() => samAccountNameGenerator.GenerateSamAccountName(""));
        }

        [Test]
        public void Wehn_upn_is_not_in_correct_format_should_throw_format_exception()
        {
            var samAccountNameGenerator = new SamAccountNameGenerator(_powerShellManager, _powershellScriptLoader);

            Assert.Throws<FormatException>(() =>
                samAccountNameGenerator.GenerateSamAccountName("random_non_email_format_string"));
        }

        [TestCase("franklindelanoroosevelt@presidentofunitedstates.com")]
        [TestCase("fdr@potus.com")]
        public void When_upn_is_in_correct_format_should_return_string_less_than_20_characters(string upn)
        {
            _powershellScriptLoader.Stub(x => x.LoadScript(PowershellScripts.SamAccountNameAvailable)).Return(PowershellScripts.SamAccountNameAvailable);
            _powerShellManager.Stub(x => x.AddParameter(Arg<string>.Is.Anything, Arg<object>.Is.Anything));
            _powerShellManager.Stub(x => x.ExecuteScriptAndReturnFirst<bool>(PowershellScripts.SamAccountNameAvailable)).Return(true);

            var samAccountNameGenerator = new SamAccountNameGenerator(_powerShellManager, _powershellScriptLoader);

            var samAccountName = samAccountNameGenerator.GenerateSamAccountName(upn);

            samAccountName.Length.Should().BeLessThan(20, "SammAccaountName cannot be more than 20 characters");
        }

        [TestCase("franklin.delano.roosevelt@presidentofunitedstates.com")]
        [TestCase("franklin]delano]roosevelt@presidentofunitedstates.com")]
        [TestCase("franklin1]delano1]roosevelt1@presidentofunitedstates.com")]
        [TestCase("franklin1%delano1$roosevelt1@president^of&unitedstates.com")]
        public void When_upn_is_in_correct_format_should_return_only_alpha_numeric(string upn)
        {
            _powershellScriptLoader.Stub(x => x.LoadScript(PowershellScripts.SamAccountNameAvailable)).Return(PowershellScripts.SamAccountNameAvailable);
            _powerShellManager.Stub(x => x.AddParameter(Arg<string>.Is.Anything, Arg<object>.Is.Anything));
            _powerShellManager.Stub(x => x.ExecuteScriptAndReturnFirst<bool>(PowershellScripts.SamAccountNameAvailable)).Return(true);

            var samAccountNameGenerator = new SamAccountNameGenerator(_powerShellManager, _powershellScriptLoader);

            var samAccountName = samAccountNameGenerator.GenerateSamAccountName(upn);

            var alphaNumeric = new Regex("[^a-zA-Z0-9 -]");
            alphaNumeric.IsMatch(samAccountName).Should().Be(true);
        }
    }
}
