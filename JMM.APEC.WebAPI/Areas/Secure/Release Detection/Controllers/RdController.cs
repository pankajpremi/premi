using JMM.APEC.ActionService;
using JMM.APEC.ReleaseDetection;
//using JMM.APEC.BusinessObjects;
using JMM.APEC.WebAPI.Areas.Secure.Models;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/rds")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,RELDET")]
    public class RdController : SecureApiController
    {
        IService service { get; set; }

        public RdController()
        {
            service = new Service(CurrentApiUser);
        }

        public RdController(IService service) { this.service = service; }

        private List<RdReportResultDto> ListRdReports(List<Service_ReleaseDetection> rdReportList)
        {
            var rdReports = from t in rdReportList
                             select new RdReportResultDto()
                             {
                                 ReleaseDetectionId = t.ReleaseDetectionId,
                                 GatewayId = t.GatewayId,
                                 GatewayName = t.GatewayName,
                                 FacilityId = t.FacilityId,
                                 FacilityName = t.FacilityName,
                                 AtgId = t.AtgId,
                                 AtgName = t.AtgName,
                                 Asset = new RdAssetDto
                                 {
                                     AssetId = t.Asset.AssetId,
                                     AssetLabel = t.Asset.AssetLabel,
                                     AssetProperty1Label = t.Asset.AssetProperty1Label,
                                     AssetProperty1Value = t.Asset.AssetProperty1Value,
                                     AssetProperty2Label = t.Asset.AssetProperty2Label,
                                     AssetProperty2Value = t.Asset.AssetProperty2Value,
                                     AssetTypeId = t.Asset.AssetTypeId,
                                     AssetTypeName = t.Asset.AssetTypeName
                                 },
                                 Result = new RdAssetResultDto
                                 {
                                     ReportDate = t.Result.ReportDate,
                                     ResultColor = t.Result.ResultColor,
                                     ResultId = t.Result.ResultId,
                                     ResultName = t.Result.ResultName,
                                     TestResultColor = t.Result.TestResultColor,
                                     TestResultId = t.Result.TestResultId,
                                     TestResultName = t.Result.TestResultName,
                                     TestDate = t.Result.TestDate,
                                     TestDateTimeZone = t.Result.TestDateTimeZone,
                                     TestTypeId = t.Result.TestTypeId,
                                     TestTypeLabel = t.Result.TestTypeLabel
                                 },
                                 RdStatus = new RdStatusDto
                                 {
                                     RdStatusColor = t.RdStatus.RdStatusColor,
                                     RdStatusId = t.RdStatus.RdStatusId,
                                     RdStatusName = t.RdStatus.RdStatusName
                                 },
                                 Template = new RdAssetTemplateDto
                                 {
                                     EffectiveStartDate = t.Template.EffectiveStartDate,
                                     EffectiveEndDate = t.Template.EffectiveEndDate,
                                     MethodName = t.Template.MethodName,
                                     TemplateId = t.Template.TemplateId,
                                     TemplateName = t.Template.TemplateName,
                                     Primary = t.Template.Primary
                                 }
                             };

            return rdReports.ToList();
        }

        [Route("results")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetReleaseDetectionResults([FromUri]RdBindingModel model)
        {
            List<Service_ReleaseDetection> rdResultList = null;

            if (model != null)
            {
                rdResultList = service.GetReleaseDetectionResults(model.Gateways, model.Facilities, model.Results, model.RdStatuses, model.AssetTypes, model.FromDate, model.ToDate, model.ReleaseDetectionId, 0, 0);
            }
            else
            {
                rdResultList = service.GetReleaseDetectionResults(null, null, null, null, null, null, null, null, 0, 0);
            }

            if (rdResultList != null)
            {
                var rdReports = ListRdReports(rdResultList);

                return Ok(new MetadataWrapper<RdReportResultDto>(rdReports));
            }

            return NotFound();
        }

        [Route("{rdsid:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetReleaseDetectionResultById(int? rdsId)
        {
            List<Service_ReleaseDetection> rdResultList = null;

            if (rdsId != null)
            {
                rdResultList = service.GetReleaseDetectionResults(null, null, null, null, null, null, null, rdsId, 0, 0);
            }

            if (rdResultList != null)
            {
                var rdReports = ListRdReports(rdResultList);

                return Ok(rdReports.FirstOrDefault());
            }

            return NotFound();

        }
    }
}
