using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Net.Http.Headers;
using Microsoft.AspNet.Identity;

using MyPatchAPI.Models;

namespace MyPatchAPI.Controllers
{
    [RoutePrefix("api")]
    public class ResourceController : ApiController
    {
        [Route("VerifyLogin")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyLogin(VerifyLoginParameter param)
        {
            var adapter = new MyPatchStoredProcedureAdapter();

            var procParam = new VerifyLoginParams
            {
                UserID = param.UserID,
                Password = param.Password,
                MacAddr = param.MacAddr
            };

            var dbLogins = adapter.ExecuteStoredProcedure<VerifyLoginModel>(procParam.StoredProcedureName, procParam);
            var Logins = dbLogins.ToList<VerifyLoginModel>();
            if (Logins.Count > 0)
            {
                return Ok(Logins[0]);
            }

            return NotFound();
        }

        [Route("EmployeeList")]
        [HttpPost]
        public async Task<IHttpActionResult> EmployeeList(EmployeeListParameter param)
        {
            var adapter = new MyPatchStoredProcedureAdapter();

            var procParam = new EmployeeListParams
            {
                SupervisorID = param.SupervisorID
            };

            var dbEmployees = adapter.ExecuteStoredProcedure<EmployeeListModel>(procParam.StoredProcedureName, procParam);
            var employees = dbEmployees.ToList<EmployeeListModel>();
            if (employees.Count > 0)
            {
                return Ok(employees);
            }

            return NotFound();
        }

        [Route("AddDownloadLog")]
        [HttpPost]
        public async Task<IHttpActionResult> AddDownloadLog(AddDownloadLogParameter param)
        {
            var adapter = new MyPatchStoredProcedureAdapter();

            var procParam = new AddDownloadLogParams
            {
                Download_Date = param.DownloadDate,
                Login_ID = param.LoginID,
                Status = param.Status,
                DB_Path = param.DBPath,
                Total_Time = float.Parse(param.TotalTime),
                App_Version = param.AppVersion,
                OS_Type = param.OSType,
                OS_Version = param.OSVersion,
                MacAddr = param.MacAddr
            };

            var result = adapter.ExecuteStoredProcedure<int>(procParam.StoredProcedureName, procParam).SingleOrDefault();
            if (result > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [Route("AddUploadLog")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUploadLog(AddUploadLogParameter param)
        {
            var adapter = new MyPatchStoredProcedureAdapter();

            var procParam = new AddUploadLogParams
            {
                Upload_Date = param.UploadDate,
                Login_ID = param.LoginID,
                Status = param.Status,
                File_Path = param.FilePath,
                Total_Time = float.Parse(param.TotalTime),
                App_Version = param.AppVersion,
                OS_Type = param.OSType,
                OS_Version = param.OSVersion,
                MacAddr = param.MacAddr
            };

            var result = adapter.ExecuteStoredProcedure<int>(procParam.StoredProcedureName, procParam).SingleOrDefault();
            if (result > 0)
            {
                return Ok();
            }

            return BadRequest();
        }


        [Route("GetFileName")]
        [HttpPost]
        public async Task<IHttpActionResult> GetFileName(GetFileNameParameter param)
        {
            string userName = param.UserName;

            var outputModel = new ResourceFileNameModel();
            outputModel.UserName = userName;

            var adapter = new MyPatchStoredProcedureAdapter();

            var procParam = new GetDBFileParams
            {
                UserID = userName,
                AppID = param.AppID,
                OS = param.OS
            };

            var dbFileNames = adapter.ExecuteStoredProcedure<DBFileNameModel>(procParam.StoredProcedureName, procParam);
            outputModel.DbFileNames = dbFileNames.ToList<DBFileNameModel>();

            return Ok(outputModel);
        }

        [Route("Download")]
        [HttpPost]
        public async Task<IHttpActionResult> Download(ResourceDownloadParameter param)
        {
            if (param != null)
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Resources";

                if (param.FileName != null)
                {
                    var path = baseDir + "\\" + param.FileName;
                    var response = ResponseMessage(FileAsAttachment(path, param.FileName));

                    return response;
                }

                return NotFound();
            }

            return BadRequest();
        }

        [Route("Upload")]
        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            HttpRequestMessage request = this.Request;

            //var sw = new Stopwatch();
            //sw.Start();

            DateTime Today = DateTime.Now;

            // Collect information to insert log
            //string UploadDate = Today.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string LoginID = request.GetQueryString("loginid");
            LoginID = String.IsNullOrEmpty(LoginID) ? "anonymous" : LoginID;
            //string AppVersion = request.GetQueryString("appversion");
            //AppVersion = String.IsNullOrEmpty(AppVersion) ? "" : AppVersion;
            //string OSType = request.GetQueryString("ostype");
            //OSType = String.IsNullOrEmpty(OSType) ? "" : OSType;
            //string OSVersion = request.GetQueryString("osversion");
            //OSVersion = String.IsNullOrEmpty(OSVersion) ? "" : OSVersion;
            //string MacAddr = request.GetQueryString("macaddr");
            //MacAddr = String.IsNullOrEmpty(MacAddr) ? "" : MacAddr;

            if (!request.Content.IsMimeMultipartContent())
            {
                var error = new ResourceUploadErrorModel
                {
                    Error = "unsupported_mediatype_error",
                    Error_Description = "File Upload Failed",
                    UploadDate = Today.ToString("yyyy-MM-dd HH:mm:ss.fff")
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.BadRequest, error);
                errorResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //sw.Stop();
                //var elapsedTime = sw.Elapsed.Seconds;

                //// Insert a log
                //InsertUploadLog(UploadDate, LoginID, "Failure: " + error.Error, "", elapsedTime, AppVersion, OSType, OSVersion, MacAddr);

                return ResponseMessage(errorResponse);
            }

            var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "Resources\\Uploads";
            var provider = new MultipartFormDataStreamProvider(baseDir);

            var task = await request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        var error = new ResourceUploadErrorModel
                        {
                            Error = "internal_error",
                            Error_Description = "File Upload Failed",
                            UploadDate = Today.ToString("yyyy-MM-dd HH:mm:ss.fff")
                        };
                        var errorResponse = Request.CreateResponse(HttpStatusCode.BadRequest, error);
                        errorResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        //sw.Stop();
                        //var elapsedTime = sw.Elapsed.Seconds;

                        //// Insert a log
                        //InsertUploadLog(UploadDate, LoginID, "Failure: " + error.Error, "", elapsedTime, AppVersion, OSType, OSVersion, MacAddr);

                        return errorResponse;
                    }

                    // Generate a file name for the uploaded file

                    FileInfo fileInfo = new FileInfo(provider.FileData.First().LocalFileName);
                    string rawFileName = provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "");
                    int index = rawFileName.IndexOf('.');
                    // string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + rawFileName.Substring(0, index) + ".zip";
                    string fileName = Today.ToString("yyyyMMddHHmmss") + "_" + LoginID + ".zip";
                    string fullPath = Path.Combine(baseDir, fileName);

                    // Move the uploaded file to the destination

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }

                    File.Move(fileInfo.FullName, fullPath);

                    //string FilePath = fullPath;

                    // Check if the compressed file is corrupt or not

                    bool result = false;
                    try
                    {
                        using (ZipArchive archive = ZipFile.Open(fullPath, ZipArchiveMode.Read))
                        {
                            result = archive.Entries.Count > 0;
                        }
                    }
                    catch (Exception)
                    {
                        result = false;
                    }

                    if (!result)
                    {
                        var error = new ResourceUploadErrorModel
                        {
                            Error = "corrupted_media_error",
                            Error_Description = "File Upload Failed",
                            UploadDate = Today.ToString("yyyy-MM-dd HH:mm:ss.fff")
                        };
                        var errorResponse = Request.CreateResponse(HttpStatusCode.BadRequest, error);
                        errorResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        // Insert a log
                        //sw.Stop();
                        //var elapsedTime = sw.Elapsed.Seconds;

                        //InsertUploadLog(UploadDate, LoginID, "Failure: " + error.Error, FilePath, elapsedTime, AppVersion, OSType, OSVersion, MacAddr);

                        return errorResponse;
                    }

                    // Insert a Log

                    //sw.Stop();
                    //int elapsed_time = sw.Elapsed.Seconds;

                    //InsertUploadLog(UploadDate, LoginID, "Success", FilePath, elapsed_time, AppVersion, OSType, OSVersion, MacAddr);

                    var success = new ResourceUploadSuccessModel
                    {
                        UploadDate = Today.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        FilePath = fullPath
                    };
                    var response = Request.CreateResponse(HttpStatusCode.OK, success);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return response;
                }
            );

            return ResponseMessage(task);
        }

        #region Helpers

        public static HttpResponseMessage FileAsAttachment(string path, string filename)
        {
            if (File.Exists(path))
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(path, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = filename;
                return result;
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public bool InsertUploadLog(string UploadDate, string LoginID, string Status, string FilePath, int ElapsedTime, string AppVersion, string OSType, string OSVersion, string MacAddr)
        {
            var procParam = new AddUploadLogParams
            {
                Upload_Date = UploadDate,
                Login_ID = LoginID,
                Status = Status,
                File_Path = FilePath,
                Total_Time = (float)ElapsedTime,
                App_Version = AppVersion,
                OS_Type = OSType,
                OS_Version = OSVersion,
                MacAddr = MacAddr
            };

            var adapter = new MyPatchStoredProcedureAdapter();
            var procResult = adapter.ExecuteStoredProcedure<int>(procParam.StoredProcedureName, procParam).SingleOrDefault();
            if (procResult > 0)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}