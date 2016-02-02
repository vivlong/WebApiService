using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.ServiceHost;

namespace WebApi.ServiceModel.Freight
{
				[Route("/freight/view/pdf", "Get")] // pdf?TableName= & TrxNo= 
				public class ViewPDF : IReturn<CommonResponse>
				{
								public string TableName { get; set; }
								public string TrxNo { get; set; }
				}
}
