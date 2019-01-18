using CloudPlus.Database;
using CloudPlus.Enums.Notification;
using CloudPlus.Models.Notification;
using CloudPlus.Test.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CloudPlus.Services.Database.Tests.EmailNotifications.EmailTemplateService
{
    // ReSharper disable once InconsistentNaming
    public class GetEmailTemplate_Specs
    {
        private Database.EmailNotifications.EmailTemplateService _sut;
        private Dictionary<string, string> _placeholders;

        [SetUp]
        public void SetUp()
        {
            var emailTemplates = new List<Entities.EmailTemplate>
            {
                new Entities.EmailTemplate
                {
                    Id = 1,
                    Type = "WelcomeCompanyCustomer",
                    Subject = "Welcome Company Customer",
                    Template = "This is template example for company (#CompanyName#)"
                },
                new Entities.EmailTemplate
                {
                    Id = 2,
                    Type = "WelcomeUser",
                    Subject = "User Template",
                    Template = "This is template example for user (#UserLogin#)"
                }
            };

            _placeholders = new Dictionary<string, string>
            {
                { "(#CompanyName#)", "CloudPlus" },
                { "(#UserLogin#)", "user@example.com" },
                { "(#Nonexistent#)", "user@example.com" }
            };

            var emailTemplatesDbSet = MockSetGenerator.CreateMockSet(emailTemplates);
            var dbContextMock = new Mock<CldpDbContext>();
            dbContextMock.Setup(p => p.EmailTemplates).Returns(emailTemplatesDbSet);
            _sut = new Database.EmailNotifications.EmailTemplateService(dbContextMock.Object);
        }

        [Test]
        public void When_Data_Is_Provided_Should_Return_Template()
        {
            var data = new EmailTemplatePlaceholdersGeneratorModel
            {
                EmailTemplateType = EmailTemplateType.WelcomeCompanyCustomer,
                PlacehoderList = _placeholders
            };

            var template = _sut.GetEmailTemplate(data);

            template.Should().BeEquivalentTo(new EmailTemplateContentModel
            {
                Subject = "Welcome Company Customer",
                Body = "This is template example for company CloudPlus"
            });
        }

        [Test]
        public void When_Data_Is_Not_Provided_Should_Throw_Exception()
        {
            var data = new EmailTemplatePlaceholdersGeneratorModel();

            _sut.Invoking(y => y.GetEmailTemplate(data)).Should().Throw<Exception>().WithMessage("Can not find template None");
        }

        [Test]
        public void When_No_Placeholders_Are_Provided_Should_Return_Template()
        {
            var data = new EmailTemplatePlaceholdersGeneratorModel
            {
                EmailTemplateType = EmailTemplateType.WelcomeCompanyCustomer
            };

            var template = _sut.GetEmailTemplate(data);

            template.Should().BeEquivalentTo(new EmailTemplateContentModel
            {
                Subject = "Welcome Company Customer",
                Body = "This is template example for company (#CompanyName#)"
            });
        }

        [Test]
        public void When_Nonexistent_Template_Type_Is_Provided_Should_Throw_Exception()
        {
            var data = new EmailTemplatePlaceholdersGeneratorModel
            {
                EmailTemplateType = EmailTemplateType.ChangePassword,
                PlacehoderList = _placeholders
            };

            _sut.Invoking(y => y.GetEmailTemplate(data)).Should().Throw<Exception>().WithMessage("Can not find template ChangePassword");
        }
    }
}
