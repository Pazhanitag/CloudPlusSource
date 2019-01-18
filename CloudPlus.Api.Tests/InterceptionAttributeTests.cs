using System;
using System.Web.Http.Controllers;
using CloudPlus.Api.Attributes;
using Microsoft.Owin;
using NUnit.Framework;
using Rhino.Mocks;

namespace CloudPlus.Api.Tests
{
    public class InterceptionAttributeTests
    {
        private const string RequestGuidFieldName = "RequestGUID";

        [Test]
        public void OnActionExecuting_PutsGuidIntoHttpContext()
        {
            //Arrange
            var owinContext = MockRepository.GenerateStub<IOwinContext>();

            var target = MockRepository.GeneratePartialMock<InterceptionAttribute>();

            target.Stub(x => x.GetOwinContext())
                .Return(owinContext);

            //Act
            target.OnActionExecuting(new HttpActionContext());

            //Assert
            owinContext.AssertWasCalled(x => x.Set(Arg<string>.Is.Equal(RequestGuidFieldName),
                Arg<Guid>.Is.NotEqual(Guid.Empty)));
        }
    }
}