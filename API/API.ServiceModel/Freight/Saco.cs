using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;
using System.Windows.Forms;

namespace WebApi.ServiceModel.Freight
{
				[Route("/freight/saco1", "Get")]
    public class Saco : IReturn<CommonResponse>
    {
    }
				public class Saco_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
								public List<Saco1> Get_Saco1_List(Saco request)
								{
												List<Saco1> Result = null;
												try
												{
																using (var db = DbConnectionFactory.OpenDbConnection())
																{
																				string strSQL = "Select Top 1 eDocumentPath From Saco1";
																				Result = db.Select<Saco1>(strSQL);
																}
												}
												catch { throw; }
												return Result;
								}
    }
}
