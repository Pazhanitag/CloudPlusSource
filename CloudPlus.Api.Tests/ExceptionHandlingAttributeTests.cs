using System;
using System.IO;
using System.Web;
using System.Web.Http.Filters;
using CloudPlus.Api.Attributes;
using Microsoft.Owin;
using NUnit.Framework;
using Rhino.Mocks;

namespace CloudPlus.Api.Tests
{
    public class ExceptionHandlingAttributeTests
    {
        [Test]
        public void OnActionExecuting_DoesNotThrowExceptionIfGUIDIsPresentInTheContext()
        {
            //Arrange
            var context = new HttpActionExecutedContext { Exception = new Exception() };
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            var owinContext = MockRepository.GenerateStub<IOwinContext>();
            owinContext.Stub(o => o.Get<Guid>("RequestGUID")).Return(Guid.NewGuid());

            var target = MockRepository.GeneratePartialMock<ExceptionHandlingAttribute>();

            target.Stub(x => x.GetOwinContext())
                .Return(owinContext);

            //Act
            target.OnException(context);

            //Assert
            Assert.DoesNotThrow(() => target.OnException(context));
        }
    }
}