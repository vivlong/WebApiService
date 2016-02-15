using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using WebApi.ServiceModel.Tables;

namespace WebApi.ServiceModel.Freight
{
				[Route("/freight/view/pdf", "Get")] // pdf?FolderName=
				public class ViewPDF : IReturn<CommonResponse>
				{
								public string FolderName { get; set; }
				}
				public class ViewPDF_Logic
				{
								public IWebAttachPath WebAttachPath { get; set; }
								public IDbConnectionFactory DbConnectionFactory { get; set; }
								public struct TrxNoPDFName
								{
												public string TrxNo;
												public string FileName;
								}
								private List<TrxNoPDFName> tnPDF = null;
								private void SortAsFileCreationTime(ref FileInfo[] arrFi)
								{
												Array.Sort<FileInfo>(arrFi, delegate(FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
								}
								public void GetAllDirList(string strPath)
								{
												try
												{																
																if (Directory.Exists(strPath))
																{
																				DirectoryInfo di = new DirectoryInfo(strPath);
																				DirectoryInfo[] diA = di.GetDirectories();
																				for (int i = 0; i <= diA.Length - 1; i++)
																				{
																								TrxNoPDFName tnn = new TrxNoPDFName();
																								tnn.TrxNo = diA[i].Name;
																								FileInfo[] arrFi = diA[i].GetFiles();
																								if (arrFi.Length > 0)
																								{
																												SortAsFileCreationTime(ref arrFi);
																												tnn.FileName = arrFi[0].Name;
																								}
																								else
																								{
																												tnn.FileName = "";
																								}
																								tnPDF.Add(tnn);
																								GetAllDirList(diA[i].FullName);
																				}
																}																		
												}
												catch { throw; }
								}
								public object Get_List(ViewPDF request)
								{
												List<ViewPDF_Ivcr> Result = null;
												tnPDF = new List<TrxNoPDFName>();
												string strPath = "";
												try
												{
																if (!string.IsNullOrEmpty(request.FolderName))
																{
																				strPath = WebAttachPath.strAttachPath + "\\" + request.FolderName + "\\eDoc";
																				GetAllDirList(strPath);
																}
																if (tnPDF.Count > 0)
																{
																				string strTrxNos = "";
																				for (int i = 0; i <= tnPDF.Count - 1; i++)
																				{
																								strTrxNos = strTrxNos + tnPDF[i].TrxNo + ",";
																				}
																				if (strTrxNos.LastIndexOf(",").Equals(strTrxNos.Length-1)){
																								strTrxNos = strTrxNos.Substring(0,strTrxNos.Length-1);
																				}
																				using (var db = DbConnectionFactory.OpenDbConnection())
																				{
																								string strSQL = "Select TrxNo,InvoiceNo,InvoiceDate,CustomerName,InvoiceAmt From Ivcr1 Where TrxNo in (" + strTrxNos + ")";
																								Result = db.Select<ViewPDF_Ivcr>(strSQL);
																				}
																				foreach (ViewPDF_Ivcr vi in Result)
																				{
																								for(int i=0;i<= tnPDF.Count-1;i++){
																												if (tnPDF[i].TrxNo.Equals(vi.TrxNo.ToString()))
																												{
																																vi.FilePath = "./" + request.FolderName + "/eDoc/" + tnPDF[i].TrxNo + "/" + tnPDF[i].FileName;
																																break;
																												}
																								}
																				}
																}																
												}
												catch { throw; }
												return Result;
								}
				}
}
