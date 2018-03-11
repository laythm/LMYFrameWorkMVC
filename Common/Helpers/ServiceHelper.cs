using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Helpers
{
    public static class ServiceHelper
    {
        public static class ServicesHelper
        {
            public static async Task<T> GET<T>(string url, Dictionary<string, string> headers = null)
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                var response = await client.ExecuteTaskAsync(request);

                return JsonConvert.DeserializeObject<T>(response.Content);
            }

            //sample

            //            ServicesHelper.POST<bool>(
            //                           Utilites.GetSettingByKey(AdigaXasa.Common.Enums.SettingEnum.SMSServiceURL),
            //                            new { Message = EditMembershipCodeSMSTemplate.GetTemplate(MembershipMobileVerification.Code), To = membershipPublicEditModel.MobileNumber
            //        },
            //                            new Dictionary<string, string>() { { "Key", Utilites.GetSettingByKey(AdigaXasa.Common.Enums.SettingEnum.SMSServiceKey)
            //    }, { "Secret", Utilites.GetSettingByKey(AdigaXasa.Common.Enums.SettingEnum.SMSServiceSecret)
            //} }
            //                            );
            public static async Task<T> POST<T>(string url, object data = null, Dictionary<string, string> headers = null)
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                if (data != null)
                    request.AddParameter("application/json", JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), ParameterType.RequestBody);

                var response = await client.ExecuteTaskAsync(request);

                return JsonConvert.DeserializeObject<T>(response.Content);
            }
        }
    }
}
