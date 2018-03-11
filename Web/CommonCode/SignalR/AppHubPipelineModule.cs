
using LMYFrameWorkMVC.Web.CommonCode;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Web;

namespace LMYFrameWorkMVC.Web.CommonCode.SignalR
{
    public class MyHubPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext,
                                                IHubIncomingInvokerContext invokerContext)
        {
            // dynamic caller = invokerContext.Hub.Clients.Caller;
            //caller.ErrorHandler(exceptionContext.Error.Message);

            ErrorHelper.LogError(HttpContext.Current, exceptionContext.Error);

        }

    }
}
